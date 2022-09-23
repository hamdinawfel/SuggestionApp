using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.Identity.Web;
using SuggestionAppLibrary.DataAccess;

namespace SuggestionsAppUI
{
    public static class RegisterServices
    {
        public static void ConfigureServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddRazorPages();
            builder.Services.AddServerSideBlazor();
            builder.Services.AddMemoryCache();

            builder.Services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(builder.Configuration.GetSection("AzureAdB2C"));



            builder.Services.AddSingleton<IDbConnetion, DbConnetion>();
            builder.Services.AddSingleton<ICategoryData, MongoCategoryData>();
            builder.Services.AddSingleton<IStatusData, MongoStatusData>();
            builder.Services.AddSingleton<IUserData, MongoUserData>();
            builder.Services.AddSingleton<ISuggestionData, MongoSuggestionData>();
        }
    }
}
