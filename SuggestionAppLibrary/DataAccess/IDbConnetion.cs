using MongoDB.Driver;

namespace SuggestionAppLibrary.DataAccess;

public interface IDbConnetion
{
    string DbName { get; }
    string CategoryCollectionName { get; }
    string StatusCollectionName { get; }
    string UserCollectionName { get; }
    string SuggestionsCollectionName { get; }
    MongoClient Client { get; }
    IMongoCollection<CategoryModel> CategoryCollection { get; set; }
    IMongoCollection<StatusModel> StatusCollection { get; set; }
    IMongoCollection<UserModel> UserCollection { get; set; }
    IMongoCollection<SuggestionModel> SuggestionCollection { get; set; }
}