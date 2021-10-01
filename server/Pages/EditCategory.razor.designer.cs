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
    public partial class EditCategoryComponent : ComponentBase
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

        [Parameter]
        public dynamic category_id { get; set; }

        PersonalFinance.Models.Sql.Category _category;
        protected PersonalFinance.Models.Sql.Category category
        {
            get
            {
                return _category;
            }
            set
            {
                if (!object.Equals(_category, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "category", NewValue = value, OldValue = _category };
                    _category = value;
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
            var sqlGetCategoryBycategoryIdResult = await Sql.GetCategoryBycategoryId(category_id);
            category = sqlGetCategoryBycategoryIdResult;
        }

        protected async System.Threading.Tasks.Task Form0Submit(PersonalFinance.Models.Sql.Category args)
        {
            try
            {
                var sqlUpdateCategoryResult = await Sql.UpdateCategory(category_id, category);
                DialogService.Close(category);
            }
            catch (System.Exception sqlUpdateCategoryException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to update Category" });
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
