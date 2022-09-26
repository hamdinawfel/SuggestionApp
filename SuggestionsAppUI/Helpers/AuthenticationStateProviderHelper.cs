using Microsoft.AspNetCore.Components.Authorization;
using SuggestionAppLibrary.DataAccess;

namespace SuggestionsAppUI.Helpers
{
    public static class AuthenticationStateProviderHelper
    {
        public static async Task<UserModel> GetUserFromAuth(
            this AuthenticationStateProvider provider, IUserData userData)
        {
            var authState = await provider.GetAuthenticationStateAsync();
            string objectId = authState.User.Claims.FirstOrDefault(c => c.Type.Contains("objectidentifier"))?.Value;
            return await userData.GetUserFromAuthentication(objectId);
        }
    }
}
