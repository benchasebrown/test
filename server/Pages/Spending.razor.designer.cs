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
    public partial class SpendingComponent : ComponentBase
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
        protected RadzenDataGrid<PersonalFinance.Models.Sql.SummaryCategoriesThisMonth> datagrid0;

        IEnumerable<PersonalFinance.Models.Sql.SummaryCategoriesThisMonth> _getSummariesResult;
        protected IEnumerable<PersonalFinance.Models.Sql.SummaryCategoriesThisMonth> getSummariesResult
        {
            get
            {
                return _getSummariesResult;
            }
            set
            {
                if (!object.Equals(_getSummariesResult, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "getSummariesResult", NewValue = value, OldValue = _getSummariesResult };
                    _getSummariesResult = value;
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
            var sqlGetSummaryCategoriesThisMonthsResult = await Sql.GetSummaryCategoriesThisMonths(new Query() { OrderBy = $"total asc" });
            getSummariesResult = sqlGetSummaryCategoriesThisMonthsResult;
        }
    }
}
