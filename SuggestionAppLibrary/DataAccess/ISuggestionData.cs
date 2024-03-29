﻿namespace SuggestionAppLibrary.DataAccess;

public interface ISuggestionData
{
    Task<List<SuggestionModel>> GetAllSuggestions();
    Task<List<SuggestionModel>> GetAllApprovedSuggestion();
    Task<SuggestionModel> GetSuggestion(string id);
    Task<List<SuggestionModel>> GetUsersSuggestions(string userId);
    Task<List<SuggestionModel>> GetAllSuggestionWaitingForApproval();
    Task UpdateSuggestion(SuggestionModel suggestion);
    Task UpvoteSuggestion(string suggestionId, string userId);
    Task CreateSuggestion(SuggestionModel suggestion);
}