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
    public partial class EditBillComponent : ComponentBase
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
        public dynamic bill_id { get; set; }

        PersonalFinance.Models.Sql.Bill _bill;
        protected PersonalFinance.Models.Sql.Bill bill
        {
            get
            {
                return _bill;
            }
            set
            {
                if (!object.Equals(_bill, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "bill", NewValue = value, OldValue = _bill };
                    _bill = value;
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

        protected override async System.Threading.Tasks.Task OnInitializedAsync()
        {
            await Load();
        }
        protected async System.Threading.Tasks.Task Load()
        {
            var sqlGetBillBybillIdResult = await Sql.GetBillBybillId(bill_id);
            bill = sqlGetBillBybillIdResult;

            var sqlGetPayeesResult = await Sql.GetPayees();
            getPayeesResult = sqlGetPayeesResult;

            var sqlGetCategoriesResult = await Sql.GetCategories();
            getCategoriesResult = sqlGetCategoriesResult;

            var sqlGetStatusesResult = await Sql.GetStatuses();
            getStatusesResult = sqlGetStatusesResult;
        }

        protected async System.Threading.Tasks.Task Form0Submit(PersonalFinance.Models.Sql.Bill args)
        {
            try
            {
                var sqlUpdateBillResult = await Sql.UpdateBill(bill_id, bill);
                DialogService.Close(bill);
            }
            catch (System.Exception sqlUpdateBillException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to update Bill" });
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
