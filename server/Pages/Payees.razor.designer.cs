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
    public partial class PayeesComponent : ComponentBase
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
        protected RadzenDataGrid<PersonalFinance.Models.Sql.Payee> grid0;

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

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            await Load();
        }
        protected async System.Threading.Tasks.Task Load()
        {
            if (string.IsNullOrEmpty(search)) {
                search = "";
            }

            var sqlGetPayeesResult = await Sql.GetPayees(new Query() { Filter = $@"i => i.name.Contains(@0)", FilterParameters = new object[] { search } });
            getPayeesResult = sqlGetPayeesResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddPayee>("Add Payee", null);
            await grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Splitbutton0Click(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await Sql.ExportPayeesToCSV(new Query() { Filter = $@"{grid0.Query.Filter}", OrderBy = $"{grid0.Query.OrderBy}", Expand = "", Select = "payee_id,name" }, $"Payees");

            }

            if (args == null || args.Value == "xlsx")
            {
                await Sql.ExportPayeesToExcel(new Query() { Filter = $@"{grid0.Query.Filter}", OrderBy = $"{grid0.Query.OrderBy}", Expand = "", Select = "payee_id,name" }, $"Payees");

            }
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(PersonalFinance.Models.Sql.Payee args)
        {
            var dialogResult = await DialogService.OpenAsync<EditPayee>("Edit Payee", new Dictionary<string, object>() { {"payee_id", args.payee_id} });
            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var sqlDeletePayeeResult = await Sql.DeletePayee(data.payee_id);
                    if (sqlDeletePayeeResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (System.Exception sqlDeletePayeeException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to delete Payee" });
            }
        }
    }
}
