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
    public partial class TransactionsComponent : ComponentBase
    {
        [Parameter(CaptureUnmatchedValues = true)]
        public IReadOnlyDictionary<string, dynamic> Attributes { get; set; }

        public void Reload()
        {
            InvokeAsync(StateHasChanged);
        }

        public void OnPropertyChanged(PropertyChangedEventArgs args)
        {
            Change(args).Wait();
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
        protected RadzenDataGrid<PersonalFinance.Models.Sql.Balance> datagrid0;
        protected RadzenDataGrid<PersonalFinance.Models.Sql.Transaction> grid0;

        string _search;
        protected string search
        {
            get
            {
                return _search;
            }
            set
            {
                if (!object.Equals(_search, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "search", NewValue = value, OldValue = _search };
                    _search = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<PersonalFinance.Models.Sql.Transaction> _getTransactionsResult;
        protected IEnumerable<PersonalFinance.Models.Sql.Transaction> getTransactionsResult
        {
            get
            {
                return _getTransactionsResult;
            }
            set
            {
                if (!object.Equals(_getTransactionsResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getTransactionsResult", NewValue = value, OldValue = _getTransactionsResult };
                    _getTransactionsResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<PersonalFinance.Models.Sql.Balance> _getBalancesResult;
        protected IEnumerable<PersonalFinance.Models.Sql.Balance> getBalancesResult
        {
            get
            {
                return _getBalancesResult;
            }
            set
            {
                if (!object.Equals(_getBalancesResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getBalancesResult", NewValue = value, OldValue = _getBalancesResult };
                    _getBalancesResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            await Load();
        }
        protected async System.Threading.Tasks.Task Change(PropertyChangedEventArgs args)
        {

        }

        protected async System.Threading.Tasks.Task Load()
        {
            if (string.IsNullOrEmpty(search)) {
                search = "";
            }

            var sqlGetTransactionsResult = await Sql.GetTransactions(new Query() { Filter = $@"i => i.description.Contains(@0)", FilterParameters = new object[] { search }, OrderBy = $"date desc", Expand = "Account,Category,Payee,Status" });
            getTransactionsResult = sqlGetTransactionsResult;

            var sqlGetBalancesResult = await Sql.GetBalances();
            getBalancesResult = sqlGetBalancesResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddTransaction>("Add Transaction", null);
            await grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Splitbutton0Click(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await Sql.ExportTransactionsToCSV(new Query() { Filter = $@"{grid0.Query.Filter}", OrderBy = $"{grid0.Query.OrderBy}", Expand = "Account,Category,Payee,Status", Select = "transaction_id,Account.name as Accountname,date,checkNumber,Category.name as Categoryname,Payee.name as Payeename,description,amount,Status.name as Statusname" }, $"Transactions");

            }

            if (args == null || args.Value == "xlsx")
            {
                await Sql.ExportTransactionsToExcel(new Query() { Filter = $@"{grid0.Query.Filter}", OrderBy = $"{grid0.Query.OrderBy}", Expand = "Account,Category,Payee,Status", Select = "transaction_id,Account.name as Accountname,date,checkNumber,Category.name as Categoryname,Payee.name as Payeename,description,amount,Status.name as Statusname" }, $"Transactions");

            }
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(PersonalFinance.Models.Sql.Transaction args)
        {
            var dialogResult = await DialogService.OpenAsync<EditTransaction>("Edit Transaction", new Dictionary<string, object>() { {"transaction_id", args.transaction_id} });
            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var sqlDeleteTransactionResult = await Sql.DeleteTransaction(data.transaction_id);
                    if (sqlDeleteTransactionResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (System.Exception sqlDeleteTransactionException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to delete Transaction" });
            }
        }
    }
}
