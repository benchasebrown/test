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
    public partial class BillsComponent : ComponentBase
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
        protected RadzenDataGrid<PersonalFinance.Models.Sql.Bill> grid0;

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

        IEnumerable<PersonalFinance.Models.Sql.Bill> _getBillsResult;
        protected IEnumerable<PersonalFinance.Models.Sql.Bill> getBillsResult
        {
            get
            {
                return _getBillsResult;
            }
            set
            {
                if (!object.Equals(_getBillsResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getBillsResult", NewValue = value, OldValue = _getBillsResult };
                    _getBillsResult = value;
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

            var sqlGetBillsResult = await Sql.GetBills(new Query() { Filter = $@"i => i.name.Contains(@0) || i.description.Contains(@1)", FilterParameters = new object[] { search, search }, Expand = "Payee,Category,Status" });
            getBillsResult = sqlGetBillsResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddBill>("Add Bill", null);
            await grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Splitbutton0Click(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await Sql.ExportBillsToCSV(new Query() { Filter = $@"{grid0.Query.Filter}", OrderBy = $"{grid0.Query.OrderBy}", Expand = "Payee,Category,Status", Select = "bill_id,name,description,Payee.name as Payeename,Category.name as Categoryname,amount,due,Status.name as Statusname" }, $"Bills");

            }

            if (args == null || args.Value == "xlsx")
            {
                await Sql.ExportBillsToExcel(new Query() { Filter = $@"{grid0.Query.Filter}", OrderBy = $"{grid0.Query.OrderBy}", Expand = "Payee,Category,Status", Select = "bill_id,name,description,Payee.name as Payeename,Category.name as Categoryname,amount,due,Status.name as Statusname" }, $"Bills");

            }
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(PersonalFinance.Models.Sql.Bill args)
        {
            var dialogResult = await DialogService.OpenAsync<EditBill>("Edit Bill", new Dictionary<string, object>() { {"bill_id", args.bill_id} });
            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var sqlDeleteBillResult = await Sql.DeleteBill(data.bill_id);
                    if (sqlDeleteBillResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (System.Exception sqlDeleteBillException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to delete Bill" });
            }
        }
    }
}
