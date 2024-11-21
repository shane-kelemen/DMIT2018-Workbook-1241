using HogWildWebApp.LocalViewModels;
using HogWildWebApp.Persistance;
using Microsoft.AspNetCore.Components;

namespace HogWildWebApp.Components.Pages.SamplePages
{
    public partial class FromPage
    {
        #region Fields
        private EmployeeView employee;
        #endregion

        #region Properties
        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        AppState AppState { get; set; }
        #endregion

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            employee = new EmployeeView()
            {
                FirstName = "Tony",
                LastName = "Stark",
                Age = 50
            };

            await InvokeAsync(StateHasChanged);
        }

        private void SendToPage()
        {
            AppState.EmployeeView = employee;
            NavigationManager.NavigateTo("/SamplePages/ToPage");
        }
    }
}
