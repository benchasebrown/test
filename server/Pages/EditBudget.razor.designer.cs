﻿using System;
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
    public partial class EditBudgetComponent : ComponentBase
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
        public dynamic budget_id { get; set; }

        PersonalFinance.Models.Sql.Budget _budget;
        protected PersonalFinance.Models.Sql.Budget budget
        {
            get
            {
                return _budget;
            }
            set
            {
                if (!object.Equals(_budget, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "budget", NewValue = value, OldValue = _budget };
                    _budget = value;
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
            var sqlGetBudgetBybudgetIdResult = await Sql.GetBudgetBybudgetId(budget_id);
            budget = sqlGetBudgetBybudgetIdResult;

            var sqlGetCategoriesResult = await Sql.GetCategories(new Query() { OrderBy = $"name asc" });
            getCategoriesResult = sqlGetCategoriesResult;
        }

        protected async System.Threading.Tasks.Task Form0Submit(PersonalFinance.Models.Sql.Budget args)
        {
            try
            {
                var sqlUpdateBudgetResult = await Sql.UpdateBudget(budget_id, budget);
                DialogService.Close(budget);
            }
            catch (System.Exception sqlUpdateBudgetException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to update Budget" });
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
