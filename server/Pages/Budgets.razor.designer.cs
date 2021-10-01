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
    public partial class BudgetsComponent : ComponentBase
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
        protected RadzenDataGrid<PersonalFinance.Models.Sql.Budget> grid0;
        protected RadzenDataGrid<PersonalFinance.Models.Sql.SpendingThisMonth> datagrid0;

        IEnumerable<PersonalFinance.Models.Sql.Budget> _getBudgetsResult;
        protected IEnumerable<PersonalFinance.Models.Sql.Budget> getBudgetsResult
        {
            get
            {
                return _getBudgetsResult;
            }
            set
            {
                if (!object.Equals(_getBudgetsResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getBudgetsResult", NewValue = value, OldValue = _getBudgetsResult };
                    _getBudgetsResult = value;
                    OnPropertyChanged(args);
                    Reload();
                }
            }
        }

        IEnumerable<PersonalFinance.Models.Sql.SpendingThisMonth> _getSpendingThisMonthResult;
        protected IEnumerable<PersonalFinance.Models.Sql.SpendingThisMonth> getSpendingThisMonthResult
        {
            get
            {
                return _getSpendingThisMonthResult;
            }
            set
            {
                if (!object.Equals(_getSpendingThisMonthResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getSpendingThisMonthResult", NewValue = value, OldValue = _getSpendingThisMonthResult };
                    _getSpendingThisMonthResult = value;
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
            var sqlGetBudgetsResult = await Sql.GetBudgets(new Query() { Expand = "Category" });
            getBudgetsResult = sqlGetBudgetsResult;

            var sqlGetSpendingThisMonthsResult = await Sql.GetSpendingThisMonths(new Query() { OrderBy = $"budgeted desc" });
            getSpendingThisMonthResult = sqlGetSpendingThisMonthsResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddBudget>("Add Budget", null);
            await grid0.Reload();

            await datagrid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(PersonalFinance.Models.Sql.Budget args)
        {
            var dialogResult = await DialogService.OpenAsync<EditBudget>("Edit Budget", new Dictionary<string, object>() { {"budget_id", args.budget_id} });
            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Datagrid0RowSelect(PersonalFinance.Models.Sql.SpendingThisMonth args)
        {
            var dialogResult = await DialogService.OpenAsync<EditBudget>("Edit Budget", new Dictionary<string, object>() { {"budget_id", args.budget_id} });
            await InvokeAsync(() => { StateHasChanged(); });
        }
    }
}
