namespace SuggestionsAppUI.Pages
{
    public partial class Profile
    {
        private UserModel loggedInUser;
        private List<SuggestionModel> submistions;
        private List<SuggestionModel> approved;
        private List<SuggestionModel> archived;
        private List<SuggestionModel> pending;
        private List<SuggestionModel> rejected;
        protected async override Task OnInitializedAsync()
        {
            loggedInUser = await authProvider.GetUserFromAuth(UserData);
            var results = await SuggestionData.GetUsersSuggestions(loggedInUser.Id);
            if (results != null && loggedInUser != null)
            {
                submistions = results.OrderByDescending(c => c.DateCreated).ToList();
                approved = submistions.Where(s => s.ApprovedForRelease && s.Archived == false && s.Rejected).ToList();
                archived = submistions.Where(s => s.Archived && s.Rejected == false).ToList();
                rejected = submistions.Where(s => s.Rejected && s.Archived == false).ToList();
            }
        }

        protected void ClosePage()
        {
            navManager.NavigateTo("/");
        }
    }
}