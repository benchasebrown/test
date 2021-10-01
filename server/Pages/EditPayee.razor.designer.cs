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
    public partial class EditPayeeComponent : ComponentBase
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
        public dynamic payee_id { get; set; }

        PersonalFinance.Models.Sql.Payee _payee;
        protected PersonalFinance.Models.Sql.Payee payee
        {
            get
            {
                return _payee;
            }
            set
            {
                if (!object.Equals(_payee, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "payee", NewValue = value, OldValue = _payee };
                    _payee = value;
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
            var sqlGetPayeeBypayeeIdResult = await Sql.GetPayeeBypayeeId(payee_id);
            payee = sqlGetPayeeBypayeeIdResult;
        }

        protected async System.Threading.Tasks.Task Form0Submit(PersonalFinance.Models.Sql.Payee args)
        {
            try
            {
                var sqlUpdatePayeeResult = await Sql.UpdatePayee(payee_id, payee);
                DialogService.Close(payee);
            }
            catch (System.Exception sqlUpdatePayeeException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to update Payee" });
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
