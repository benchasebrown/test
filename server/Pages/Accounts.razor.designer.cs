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
    public partial class AccountsComponent : ComponentBase
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
        protected RadzenDataGrid<PersonalFinance.Models.Sql.Account> grid0;

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

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            await Load();
        }
        protected async System.Threading.Tasks.Task Load()
        {
            if (string.IsNullOrEmpty(search)) {
                search = "";
            }

            var sqlGetAccountsResult = await Sql.GetAccounts(new Query() { Filter = $@"i => i.name.Contains(@0) || i.description.Contains(@1)", FilterParameters = new object[] { search, search } });
            getAccountsResult = sqlGetAccountsResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddAccount>("Add Account", null);
            await grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Splitbutton0Click(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await Sql.ExportAccountsToCSV(new Query() { Filter = $@"{grid0.Query.Filter}", OrderBy = $"{grid0.Query.OrderBy}", Expand = "", Select = "account_id,name,description" }, $"Accounts");

            }

            if (args == null || args.Value == "xlsx")
            {
                await Sql.ExportAccountsToExcel(new Query() { Filter = $@"{grid0.Query.Filter}", OrderBy = $"{grid0.Query.OrderBy}", Expand = "", Select = "account_id,name,description" }, $"Accounts");

            }
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(PersonalFinance.Models.Sql.Account args)
        {
            var dialogResult = await DialogService.OpenAsync<EditAccount>("Edit Account", new Dictionary<string, object>() { {"account_id", args.account_id} });
            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var sqlDeleteAccountResult = await Sql.DeleteAccount(data.account_id);
                    if (sqlDeleteAccountResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (System.Exception sqlDeleteAccountException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to delete Account" });
            }
        }
    }
}
