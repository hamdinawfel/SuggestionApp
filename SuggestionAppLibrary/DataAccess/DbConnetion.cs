using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace SuggestionAppLibrary.DataAccess
{
    public class DbConnetion : IDbConnetion
    {
        private readonly IConfiguration _config;
        private readonly IMongoDatabase _db;
        private string _connectionId = "MongoDB";

        public string DbName { get; private set; }
        public string CategoryCollectionName { get; private set; } = "categories";
        public string StatusCollectionName { get; private set; } = "statuses";
        public string UserCollectionName { get; private set; } = "users";
        public string SuggestionsCollectionName { get; private set; } = "suggestions";

        public MongoClient Client { get; private set; }
        public IMongoCollection<CategoryModel> CategoryCollection { get; set; }
        public IMongoCollection<SuggestionModel> StatusCollection { get; set; }
        public IMongoCollection<UserModel> UserCollection { get; set; }
        public IMongoCollection<SuggestionModel> SuggestionCollection { get; set; }

        public DbConnetion(IConfiguration config)
        {
            _config = config;
            Client = new MongoClient(_config.GetConnectionString(_connectionId));
            DbName = _config["DatabaseName"];
            _db = Client.GetDatabase(DbName);

            CategoryCollection = _db.GetCollection<CategoryModel>(CategoryCollectionName);
            StatusCollection = _db.GetCollection<SuggestionModel>(StatusCollectionName);
            UserCollection = _db.GetCollection<UserModel>(UserCollectionName);
            SuggestionCollection = _db.GetCollection<SuggestionModel>(SuggestionsCollectionName);

        }
    }

}
