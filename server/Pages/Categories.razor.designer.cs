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
    public partial class CategoriesComponent : ComponentBase
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
        protected RadzenDataGrid<PersonalFinance.Models.Sql.Category> grid0;

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

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            await Load();
        }
        protected async System.Threading.Tasks.Task Load()
        {
            if (string.IsNullOrEmpty(search)) {
                search = "";
            }

            var sqlGetCategoriesResult = await Sql.GetCategories(new Query() { Filter = $@"i => i.name.Contains(@0)", FilterParameters = new object[] { search } });
            getCategoriesResult = sqlGetCategoriesResult;
        }

        protected async System.Threading.Tasks.Task Button0Click(MouseEventArgs args)
        {
            var dialogResult = await DialogService.OpenAsync<AddCategory>("Add Category", null);
            await grid0.Reload();

            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task Splitbutton0Click(RadzenSplitButtonItem args)
        {
            if (args?.Value == "csv")
            {
                await Sql.ExportCategoriesToCSV(new Query() { Filter = $@"{grid0.Query.Filter}", OrderBy = $"{grid0.Query.OrderBy}", Expand = "", Select = "category_id,name" }, $"Categories");

            }

            if (args == null || args.Value == "xlsx")
            {
                await Sql.ExportCategoriesToExcel(new Query() { Filter = $@"{grid0.Query.Filter}", OrderBy = $"{grid0.Query.OrderBy}", Expand = "", Select = "category_id,name" }, $"Categories");

            }
        }

        protected async System.Threading.Tasks.Task Grid0RowSelect(PersonalFinance.Models.Sql.Category args)
        {
            var dialogResult = await DialogService.OpenAsync<EditCategory>("Edit Category", new Dictionary<string, object>() { {"category_id", args.category_id} });
            await InvokeAsync(() => { StateHasChanged(); });
        }

        protected async System.Threading.Tasks.Task GridDeleteButtonClick(MouseEventArgs args, dynamic data)
        {
            try
            {
                if (await DialogService.Confirm("Are you sure you want to delete this record?") == true)
                {
                    var sqlDeleteCategoryResult = await Sql.DeleteCategory(data.category_id);
                    if (sqlDeleteCategoryResult != null)
                    {
                        await grid0.Reload();
                    }
                }
            }
            catch (System.Exception sqlDeleteCategoryException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to delete Category" });
            }
        }
    }
}
