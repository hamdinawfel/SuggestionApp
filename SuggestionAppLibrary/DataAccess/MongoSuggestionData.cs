using Microsoft.Extensions.Caching.Memory;
namespace SuggestionAppLibrary.DataAccess
{
    public class MongoSuggestionData : ISuggestionData
    {
        private readonly IDbConnetion _db;
        private readonly IMongoCollection<SuggestionModel> _suggestions;
        private readonly IUserData _userData;
        private readonly IMemoryCache _cache;

        private const string CacheName = "SuggestionData";

        public MongoSuggestionData(IDbConnetion db, IUserData userData, IMemoryCache cache )
        {
            _db = db;
            _userData = userData;
            _suggestions = _db.SuggestionCollection;
            _cache = cache;
        }

        public async Task<List<SuggestionModel>> GetAllSuggestions()
        {
            var output = _cache.Get<List<SuggestionModel>>(CacheName);
            if (output == null)
            {
                var results = await _suggestions.FindAsync(s =>s.Archived == false);
                output = results.ToList();

                _cache.Set(CacheName, output, TimeSpan.FromMinutes(1));
            }
            return output;
        }
        public async Task<List<SuggestionModel>> GetAllApprovedSuggestion()
        {
            var output = await GetAllSuggestions();
            return output.Where(s=>s.ApprovedForRelease).ToList();
        }
        public async Task<SuggestionModel> GetSuggestion(string id)
        {
            var results = await _suggestions.FindAsync(s => s.Id == id);
            return results.FirstOrDefault();
        }
        public async Task<List<SuggestionModel>> GetAllSuggestionWaitingForApproval()
        {
            var output = await GetAllSuggestions();
            return output.Where(s => s.ApprovedForRelease == false && s.Rejected == false).ToList();
        }
        public async Task UpdateSuggestion(SuggestionModel suggestion)
        {
            await _suggestions.ReplaceOneAsync(s => s.Id == suggestion.Id,suggestion);
            _cache.Remove(CacheName);
        }
        public async Task UpvoteSuggestion(string suggestionId, string userId)
        {
            var client = _db.Client;
            using var session = await client.StartSessionAsync();
            session.StartTransaction();
            try
            {
                var db = client.GetDatabase(_db.DbName);
                var suggestionInTransaction = db.GetCollection<SuggestionModel>(_db.SuggestionsCollectionName);
                var suggestion = (await suggestionInTransaction.FindAsync(s=>s.Id == suggestionId)).First();

                bool isVoted = suggestion.UserVotes.Add(userId);
                if (!isVoted)
                {
                    suggestion.UserVotes.Remove(userId);
                }

                await suggestionInTransaction.ReplaceOneAsync(s => s.Id == suggestionId, suggestion);

                var userInTransaction = db.GetCollection<UserModel>(_db.UserCollectionName);
                var user = await _userData.GetUserAsync(suggestion.Author.Id);

                if (isVoted)
                {
                    user.VotedOnSuggestions.Add(new BasicSuggestionModel(suggestion));
                }
                else
                {
                    var suggestionToRemove = user.VotedOnSuggestions.Where(s => s.Id == suggestionId).First();
                    user.VotedOnSuggestions.Remove(suggestionToRemove);
                }

                await userInTransaction.ReplaceOneAsync(u => u.Id == userId, user);
                await session.CommitTransactionAsync();
                _cache.Remove(CacheName);

            }
            catch (Exception ex)
            {
                await session.AbortTransactionAsync();
                throw;
            }
        }
        public async Task CreateSuggestion(SuggestionModel suggestion)
        {
            var client = _db.Client;
            using var session = await client.StartSessionAsync();
            session.StartTransaction();
            try
            {
                var db = client.GetDatabase(_db.DbName);
                var suggestionInTransaction = db.GetCollection<SuggestionModel>(_db.SuggestionsCollectionName);
                await suggestionInTransaction.InsertOneAsync(suggestion);

                var userInTransaction = db.GetCollection<UserModel>(_db.UserCollectionName);
                var user = await _userData.GetUserAsync(suggestion.Author.Id);

                user.VotedOnSuggestions.Add(new BasicSuggestionModel(suggestion));
                await userInTransaction.ReplaceOneAsync(u => u.Id == user.Id, user);
                await session.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await session.AbortTransactionAsync();
                throw;
            }
        }
    }
}
