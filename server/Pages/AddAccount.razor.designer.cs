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
    public partial class AddAccountComponent : ComponentBase
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

        PersonalFinance.Models.Sql.Account _account;
        protected PersonalFinance.Models.Sql.Account account
        {
            get
            {
                return _account;
            }
            set
            {
                if (!object.Equals(_account, value))
                {
                    var args = new PropertyChangedEventArgs(){ Name = "account", NewValue = value, OldValue = _account };
                    _account = value;
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
            account = new PersonalFinance.Models.Sql.Account(){};
        }

        protected async System.Threading.Tasks.Task Form0Submit(PersonalFinance.Models.Sql.Account args)
        {
            try
            {
                var sqlCreateAccountResult = await Sql.CreateAccount(account);
                DialogService.Close(account);
            }
            catch (System.Exception sqlCreateAccountException)
            {
                NotificationService.Notify(new NotificationMessage(){ Severity = NotificationSeverity.Error,Summary = $"Error",Detail = $"Unable to create new Account!" });
            }
        }

        protected async System.Threading.Tasks.Task Button2Click(MouseEventArgs args)
        {
            DialogService.Close(null);
        }
    }
}
