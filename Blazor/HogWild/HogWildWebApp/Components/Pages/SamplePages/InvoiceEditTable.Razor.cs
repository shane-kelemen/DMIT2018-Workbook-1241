﻿// ***********************************************************************
// Assembly         : HogWildWebApp
// Author           : jamesthompson
// Created          : 03-21-2024
//
// Last Modified By : jamesthompson
// Last Modified On : 03-21-2024
// ***********************************************************************
// <copyright file="InvoiceEdit.Razor.cs" company="HogWildWebApp">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using HogWildSystem.BLL;
using HogWildSystem.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using HogWildWebApp.HelperClasses;

namespace HogWildWebApp.Components.Pages.SamplePages
{
    /// <summary>
    /// Class InvoiceEdit.
    /// Implements the <see cref="ComponentBase" />
    /// </summary>
    /// <seealso cref="ComponentBase" />
    public partial class InvoiceEditTable
    {
        #region Field
        // The description
        /// <summary>
        /// The description
        /// </summary>
        private string description;

        // The category id        
        /// <summary>
        /// The category identifier
        /// </summary>
        private int categoryID;

        // The part Categories        
        /// <summary>
        /// The part categories
        /// </summary>
        private List<LookupView> partCategories;

        // The parts
        /// <summary>
        /// The parts
        /// </summary>
        private List<PartView> parts = new List<PartView>();

        // The invoice        
        /// <summary>
        /// The invoice
        /// </summary>
        private InvoiceView invoice = new();

        //  feedback message        
        /// <summary>
        /// The feedback message
        /// </summary>
        private string feedbackMessage;

        //  error messasge        
        /// <summary>
        /// The error message
        /// </summary>
        private string errorMessage;

        //  has feedback
        /// <summary>
        /// Gets a value indicating whether this instance has feedback.
        /// </summary>
        /// <value><c>true</c> if this instance has feedback; otherwise, <c>false</c>.</value>
        private bool hasFeedback => !string.IsNullOrWhiteSpace(feedbackMessage);

        //  has errors
        /// <summary>
        /// Gets a value indicating whether this instance has error.
        /// </summary>
        /// <value><c>true</c> if this instance has error; otherwise, <c>false</c>.</value>
        private bool hasError => !string.IsNullOrWhiteSpace(errorMessage);

        //  has error details
        /// <summary>
        /// The error details
        /// </summary>
        private List<string> errorDetails = new();

        /// <summary>
        /// The show dialog
        /// </summary>
        private bool showDialog = false;
        /// <summary>
        /// The dialog message
        /// </summary>
        private string dialogMessage = string.Empty;
        /// <summary>
        /// The should invoice line be remove
        /// </summary>
        private bool shouldInvoiceLineBeRemove = false;
        /// <summary>
        /// The dialog completion source
        /// </summary>
        private TaskCompletionSource<bool?> dialogCompletionSource;


        /// <summary>
        /// Shows the dialog.
        /// </summary>
        private async Task ShowDialog()
        {
            dialogMessage = "Do you wish to close the invoice editor?";
            showDialog = true;
        }
        #endregion

        #region Validation
        //// the edit context
        private EditContext editContext;

        ////  used to store the validation message
       private ValidationMessageStore messageStore;


        #endregion

        #region Properties
        //  The navigation manager.
        /// <summary>
        /// Gets or sets the navigation manager.
        /// </summary>
        /// <value>The navigation manager.</value>
        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        //  The invoice service.
        /// <summary>
        /// Gets or sets the invoice service.
        /// </summary>
        /// <value>The invoice service.</value>
        [Inject]
        protected InvoiceService InvoiceService { get; set; }

        //  The part service.
        /// <summary>
        /// Gets or sets the part service.
        /// </summary>
        /// <value>The part service.</value>
        [Inject]
        protected PartService PartService { get; set; }

        //  The category lookup service.
        /// <summary>
        /// Gets or sets the category lookup service.
        /// </summary>
        /// <value>The category lookup service.</value>
        [Inject]
        protected CategoryLookupService CategoryLookupService { get; set; }


        //  The invoice identifier.
        /// <summary>
        /// Gets or sets the invoice identifier.
        /// </summary>
        /// <value>The invoice identifier.</value>
        [Parameter] public int InvoiceID { get; set; }

        //  The customer identifier.
        /// <summary>
        /// Gets or sets the customer identifier.
        /// </summary>
        /// <value>The customer identifier.</value>
        [Parameter] public int CustomerID { get; set; }

        //  The employee identifier.
        /// <summary>
        /// Gets or sets the employee identifier.
        /// </summary>
        /// <value>The employee identifier.</value>
        [Parameter] public int EmployeeID { get; set; }
        #endregion


        /// <summary>
        /// On initialized as an asynchronous operation.
        /// </summary>
        /// <returns>A Task representing the asynchronous operation.</returns>
        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();
            try
            {
                //  edit context needs to be setup after data has been initialized
                //  setup of the edit context to make use of the payment type property
                editContext = new EditContext(invoice.InvoiceLines);

                //  set the validation to use the HandleValidationRequest event
                editContext.OnValidationRequested += HandleValidationRequested;

                //  setup the message store to track any validation messages
                messageStore = new ValidationMessageStore(editContext);

                //  this event will fire each time the data in a property has change.
                editContext.OnFieldChanged += EditContext_OnFieldChanged;
                //  get categories
                partCategories = CategoryLookupService.GetLookups("Part Categories");

                //  get the invoice
                invoice = InvoiceService.GetInvoice(InvoiceID, CustomerID, EmployeeID);

                //  reset the error detail list
                errorDetails.Clear();

                //  reset the error message to an empty string
                errorMessage = string.Empty;

                //  reset feedback message to an empty string
                feedbackMessage = String.Empty;

                await InvokeAsync(StateHasChanged);
            }
            catch (ArgumentNullException ex)
            {
                errorMessage = BlazorHelperClass.GetInnerException(ex).Message;
            }
            catch (ArgumentException ex)
            {
                errorMessage = BlazorHelperClass.GetInnerException(ex).Message;
            }
            catch (AggregateException ex)
            {
                //  have a collection of errors
                //  each error should be place into a separate line
                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    errorMessage = $"{errorMessage}{Environment.NewLine}";
                }

                errorMessage = $"{errorMessage}Unable to search for invoice";
                foreach (var error in ex.InnerExceptions)
                {
                    errorDetails.Add(error.Message);
                }
            }
        }

        // Handles the validation requested when the form is validated (e.g., on submit).
        private void HandleValidationRequested(object sender, ValidationRequestedEventArgs e)
        {
            // Clear the message store to remove any existing validation errors.
            messageStore?.Clear();

            // Iterate through each invoice line to perform custom validations.
            foreach (var invoiceLines in invoice.InvoiceLines)
            {
                // Custom validation: Ensure Quantity is greater than 0.
                if (invoiceLines.Quantity <= 0)
                {
                    // Add a validation message for the Quantity field.
                    messageStore.Add(new FieldIdentifier(invoiceLines, nameof(invoiceLines.Quantity)),
                        "Quantity must be greater than 0.");
                }

                // Custom validation: Ensure Price is greater than 0.
                if (invoiceLines.Price <= 0)
                {
                    // Add a validation message for the Price field.
                    messageStore.Add(new FieldIdentifier(invoiceLines, nameof(invoiceLines.Price)),
                        "Price must be greater than 0.");
                }
            }

            // Notify the EditContext that the validation state has changed.
            editContext.NotifyValidationStateChanged();

            // Trigger UI updates to reflect the validation messages.
            InvokeAsync(StateHasChanged);
        }

        // Handles the field-changed event of the EditContext.
        private void EditContext_OnFieldChanged(object sender, FieldChangedEventArgs e)
        {
            // Clear previous validation messages for the specific field that changed.
            messageStore?.Clear(e.FieldIdentifier);

            // Perform custom validation for Quantity.
            // Check if the field belongs to an InvoiceLineView object using the `is` operator with pattern matching.
            // If true, the `line` variable is assigned as a strongly-typed reference to the Model.
            // Then, verify that the field being validated is the Quantity field.
            if (e.FieldIdentifier.Model is InvoiceLineView line && e.FieldIdentifier.FieldName == nameof(line.Quantity))
            {
                if (line.Quantity <= 0)
                {
                    // Add a validation message for the Quantity field.
                    messageStore?.Add(e.FieldIdentifier, "Quantity must be greater than 0.");
                }
            }

            // Perform custom validation for Price.
            // Check if the field belongs to an InvoiceLineView object using the `is` operator with pattern matching.
            // If true, the `line2` variable is assigned as a strongly-typed reference to the Model.
            // Then, verify that the field being validated is the Price field.
            if (e.FieldIdentifier.Model is InvoiceLineView line2 && e.FieldIdentifier.FieldName == nameof(line2.Price))
            {
                if (line2.Price <= 0)
                {
                    // Add a validation message for the Price field.
                    messageStore?.Add(e.FieldIdentifier, "Price must be greater than 0.");
                }
            }

            // Notify the EditContext that the validation state has changed.
            editContext.NotifyValidationStateChanged();

            // Trigger UI updates to reflect the validation messages.
            InvokeAsync(StateHasChanged);
        }

        // Manually notifies the EditContext that a field has changed.
        private void NotifyFieldChanged(InvoiceLineView line, string propertyName)
        {
            // Create a FieldIdentifier for the specified field.
            var fieldIdentifier = new FieldIdentifier(line, propertyName);

            // Notify the EditContext that the field has changed.
            editContext.NotifyFieldChanged(fieldIdentifier);

            // Optionally trigger validation for the entire form (if needed).
            editContext.Validate();
        }

        // Synchronizes the Quantity value between the UI and the model.
        private void SyncQuantity(object? inputValue, InvoiceLineView line)
        {
            // Ensure the input value is valid and parse it as an integer.
            if (inputValue is not null && int.TryParse(inputValue.ToString(), out int newQuantity))
            {
                // Update the Quantity field in the model.
                line.Quantity = newQuantity;

                // Notify the EditContext about the field change and trigger validation.
                NotifyFieldChanged(line, nameof(line.Quantity));
            }
        }

        // Synchronizes the Price value between the UI and the model.
        private void SyncPrice(object? inputValue, InvoiceLineView line)
        {
            // Ensure the input value is valid and parse it as a decimal.
            if (inputValue is not null && decimal.TryParse(inputValue.ToString(), out decimal newPrice))
            {
                // Update the Price field in the model.
                line.Price = newPrice;

                // Notify the EditContext about the field change and trigger validation.
                NotifyFieldChanged(line, nameof(line.Price));
            }
        }


        ///// <summary>
        ///// Searches this instance.
        ///// </summary>
        ///// <exception cref="System.ArgumentException">Please provide either a category and/or description</exception>
        /// <summary>
        /// Searches the parts.
        /// </summary>
        /// <exception cref="System.ArgumentException">Please provide either a category and/or description</exception>
        private async Task SearchParts()
        {
            try
            {
                //  reset the error detail list
                errorDetails.Clear();

                //  reset the error message to an empty string
                errorMessage = string.Empty;

                //  reset feedback message to an empty string
                feedbackMessage = String.Empty;

                //  clear the part list before we do our search
                parts.Clear();

                if (categoryID == 0 && string.IsNullOrWhiteSpace(description))
                {
                    throw new ArgumentException("Please provide either a category and/or description");
                }

                //  search for our parts
                List<int> existingPartIDs =
                    invoice.InvoiceLines
                    .Select(x => x.PartID)
                    .ToList();
                ;
                parts = PartService.GetParts(categoryID, description, existingPartIDs);
                await InvokeAsync(StateHasChanged);

                if (parts.Count() > 0)
                {
                    feedbackMessage = "Search for part(s) was successful";
                }
                else
                {
                    feedbackMessage = "No part were found for your search criteria";
                }
            }
            //  Your Catch Code Below
            //  code here
            catch (ArgumentNullException ex)
            {
                errorMessage = BlazorHelperClass.GetInnerException(ex).Message;
            }
            catch (ArgumentException ex)
            {
                errorMessage = BlazorHelperClass.GetInnerException(ex).Message;
            }
            catch (AggregateException ex)
            {
                //  have a collection of errors
                //  each error should be place into a separate line
                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    errorMessage = $"{errorMessage}{Environment.NewLine}";
                }
                errorMessage = $"{errorMessage}Unable to search for part";
                foreach (var error in ex.InnerExceptions)
                {
                    errorDetails.Add(error.Message);
                }
            }
        }

        //  Add part to invoice line and remove part from part list
        /// <summary>
        /// Adds the part.
        /// </summary>
        /// <param name="partID">The part identifier.</param>
        private async Task AddPart(int partID)
        {
            PartView part = PartService.GetPart(partID);
            InvoiceLineView invoiceLine = new InvoiceLineView();
            invoiceLine.PartID = partID;
            invoiceLine.Description = part.Description;
            invoiceLine.Price = part.Price;
            invoiceLine.Taxable = part.Taxable;
            invoiceLine.Quantity = 0;
            invoice.InvoiceLines.Add(invoiceLine);

            //  remove the current part from the part list.
            parts.Remove(parts
                .Where(x => x.PartID == partID)
                .FirstOrDefault());
            await InvokeAsync(StateHasChanged);
        }

        //  delete invoice line
        /// <summary>
        /// Deletes the invoice line.
        /// </summary>
        /// <param name="partID">The part identifier.</param>
        private async Task DeleteInvoiceLine(int partID)
        {
            shouldInvoiceLineBeRemove = false;
            InvoiceLineView invoiceLine = invoice.InvoiceLines
                .Where(x => x.PartID == partID)
                .Select(x => x).FirstOrDefault();

            // Initialize the TaskCompletionSource
            dialogCompletionSource = new TaskCompletionSource<bool?>();
            dialogMessage = $"Are you sure that you wish to remove {invoiceLine.Description}?";
            showDialog = true;
            bool? results = await ShowDialogAsync();

            //  return results can be either "Ok" or "Cancel"
            if ((bool)results)
            {
                //  remove invoice line
                //  second half of the "Simple List to List"                
                invoice.InvoiceLines.Remove(invoiceLine);
                //  update search results incase the part we removed
                //  is part of the search results
                if (categoryID > 0 || !string.IsNullOrEmpty(description))
                {
                    await SearchParts();
                }
                UpdateSubtotalAndTax();
                await InvokeAsync(StateHasChanged);
            }
        }

        //  update subtotal and tax
        /// <summary>
        /// Updates the subtotal and tax.
        /// </summary>
        private void UpdateSubtotalAndTax()
        {
            invoice.SubTotal = invoice.InvoiceLines
                .Where(x => !x.RemoveFromViewFlag)
                .Sum(x => x.Quantity * x.Price);
            invoice.Tax = invoice.InvoiceLines
                .Where(x => !x.RemoveFromViewFlag)
                .Sum(x => x.Taxable ? x.Quantity * x.Price * 0.05m : 0);
        }

        /// <summary>
        /// Saves this instance.
        /// </summary>
        private async Task Save()
        {
            try
            {
                bool isNewInvoice = false;
                //  reset the error detail list
                errorDetails.Clear();

                //  reset the error message to an empty string
                errorMessage = string.Empty;

                //  reset feedback message to an empty string
                feedbackMessage = String.Empty;

                //SAVE NEEDS TO BE CODED
                isNewInvoice = invoice.InvoiceID == 0;
                invoice = InvoiceService.Save(invoice);
                InvoiceID = invoice.InvoiceID;
                feedbackMessage = isNewInvoice
                    ? $"New Invoice No {invoice.InvoiceID} was created"
                    : $"Invoice No {invoice.InvoiceID} was updated";
                await InvokeAsync(StateHasChanged);
            }
            //  Your Catch Code Below
            //  code here
            catch (ArgumentNullException ex)
            {
                errorMessage = BlazorHelperClass.GetInnerException(ex).Message;
            }
            catch (ArgumentException ex)
            {
                errorMessage = BlazorHelperClass.GetInnerException(ex).Message;
            }
            catch (AggregateException ex)
            {
                //  have a collection of errors
                //  each error should be place into a separate line
                if (!string.IsNullOrWhiteSpace(errorMessage))
                {
                    errorMessage = $"{errorMessage}{Environment.NewLine}";
                }
                errorMessage = $"{errorMessage}Unable to save invoice";
                foreach (var error in ex.InnerExceptions)
                {
                    errorDetails.Add(error.Message);
                }
            }
        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        private async Task Close()
        {
            // Initialize the TaskCompletionSource
            dialogCompletionSource = new TaskCompletionSource<bool?>();
            dialogMessage = "Do you wish to close the invoice editor?";
            showDialog = true;
            bool? results = await ShowDialogAsync();
            if ((bool)results)
            {
                NavigationManager.NavigateTo($"/SamplePages/CustomerEdit/{CustomerID}");
            }
        }

        /// <summary>
        /// Show dialog as an asynchronous operation.
        /// </summary>
        /// <returns>A Task&lt;System.Boolean&gt; representing the asynchronous operation.</returns>
        private async Task<bool?> ShowDialogAsync()
        {
            // Initialize the TaskCompletionSource
            dialogCompletionSource = new TaskCompletionSource<bool?>();

            // Wait for the dialog to be closed and return the result
            return await dialogCompletionSource.Task;
        }

        /// <summary>
        /// Simples the dialog result.
        /// </summary>
        /// <param name="result">if set to <c>true</c> [result].</param>
        private void SimpleDialogResult(bool? result)
        {
            showDialog = false;
            dialogCompletionSource.SetResult(result);
        }
    }
}
