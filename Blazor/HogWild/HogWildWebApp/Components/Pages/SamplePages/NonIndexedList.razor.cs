using HogWildSystem.ViewModels;

namespace HogWildWebApp.Components.Pages.SamplePages
{
    public partial class NonIndexedList
    {
        #region Fields
        protected List<CustomerEditView> Customers { get; set; } = new List<CustomerEditView>();

        private string CustomerName { get; set; }
        #endregion

        private void RemoveCustomer(int customerID)
        {
            var selectedItem = Customers
                                .FirstOrDefault(x => x.CustomerID == customerID);
            if (selectedItem != null)
            {
                Customers.Remove(selectedItem);
            }
        }

        private async Task AddCustomerToList()
        {
            int maxID = Customers.Count == 0 ?
                            1 : Customers.Max(x => x.CustomerID) + 1;

            Customers.Add(new CustomerEditView()
            {
                CustomerID = maxID,
                FirstName = CustomerName
            });

            await InvokeAsync(StateHasChanged);
        }

    }
}

