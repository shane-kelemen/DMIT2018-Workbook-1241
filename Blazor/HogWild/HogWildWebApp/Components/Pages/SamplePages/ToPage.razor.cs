using HogWildWebApp.LocalViewModels;
using HogWildWebApp.Persistance;
using Microsoft.AspNetCore.Components;

namespace HogWildWebApp.Components.Pages.SamplePages
{
    public partial class ToPage
    {
        #region Fields
        private EmployeeView employee;
        #endregion

        #region Properties
        [Inject]
        AppState AppState { get; set; }
        #endregion

        protected override void OnInitialized()
        {
            base.OnInitialized();

            if (AppState.EmployeeView == null) return;

            employee = AppState.EmployeeView;
        }
    }
}
