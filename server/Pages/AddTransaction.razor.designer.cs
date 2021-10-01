using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using PersonalFinance.Models.Sql;
using Microsoft.EntityFrameworkCore;

namespace PersonalFinance.Pages
{
    public partial class AddTransactionComponent : ComponentBase
    {
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
        }

        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager UriHelper { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected SqlService Sql { get; set; }

        IEnumerable<PersonalFinance.Models.Sql.Account> _getAccountsResult;
        protected IEnumerable<PersonalFinance.Models.Sql.Account> getAccountsResult
        {
            get
            {
                return _getAccountsResult;
            }
            set
            {
                if (!object.Equals(_getAccountsResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getAccountsResult", NewValue = value, OldValue = _getAccountsResult };
                    _getAccountsResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<PersonalFinance.Models.Sql.Category> _getCategoriesResult;
        protected IEnumerable<PersonalFinance.Models.Sql.Category> getCategoriesResult
        {
            get
            {
                return _getCategoriesResult;
            }
            set
            {
                if (!object.Equals(_getCategoriesResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getCategoriesResult", NewValue = value, OldValue = _getCategoriesResult };
                    _getCategoriesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<PersonalFinance.Models.Sql.Payee> _getPayeesResult;
        protected IEnumerable<PersonalFinance.Models.Sql.Payee> getPayeesResult
        {
            get
            {
                return _getPayeesResult;
            }
            set
            {
                if (!object.Equals(_getPayeesResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getPayeesResult", NewValue = value, OldValue = _getPayeesResult };
                    _getPayeesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<PersonalFinance.Models.Sql.Status> _getStatusesResult;
        protected IEnumerable<PersonalFinance.Models.Sql.Status> getStatusesResult
        {
            get
            {
                return _getStatusesResult;
            }
            set
            {
                if (!object.Equals(_getStatusesResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getStatusesResult", NewValue = value, OldValue = _getStatusesResult };
                    _getStatusesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        PersonalFinance.Models.Sql.Transaction _transaction;
        protected PersonalFinance.Models.Sql.Transaction transaction
        {
            get
            {
                return _transaction;
            }
            set
            {
                if (!object.Equals(_transaction, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "transaction", NewValue = value, OldValue = _transaction };
                    _transaction = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            await Load();
        }
        protected async System.Threading.Tasks.Task Load()
        {
            var sqlGetAccountsResult = await Sql.GetAccounts();
            getAccountsResult = sqlGetAccountsResult;

            var sqlGetCategoriesResult = await Sql.GetCategories(new Query() { OrderBy = $"name asc" });
            getCategoriesResult = sqlGetCategoriesResult;

            var sqlGetPayeesResult = await Sql.GetPayees(new Query() { OrderBy = $"name asc" });
            getPayeesResult = sqlGetPayeesResult;

            var sqlGetStatusesResult = await Sql.GetStatuses();
            getStatusesResult = sqlGetStatusesResult;

            transaction = new PersonalFinance.Models.Sql.Transaction(){};
        }

        protected async System.Threading.Tasks.Task Form0Submit(PersonalFinance.Models.Sql.Transaction args)
        {
            try
            {
                var sqlCreateTransactionResult = await Sql.CreateTransaction(transaction);
                DialogService.Close(transaction);
            }
            catch (System.Exception sqlCreateTransactionException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to create new Transaction!" });
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
