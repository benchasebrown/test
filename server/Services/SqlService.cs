using Radzen;
using System;
using System.Web;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Data;
using System.Text.Encodings.Web;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Components;
using PersonalFinance.Data;

namespace PersonalFinance
{
    public partial class SqlService
    {
        SqlContext Context
        {
           get
           {
             return this.context;
           }
        }

        private readonly SqlContext context;
        private readonly NavigationManager navigationManager;

        public SqlService(SqlContext context, NavigationManager navigationManager)
        {
            this.context = context;
            this.navigationManager = navigationManager;
        }

        public void Reset() => Context.ChangeTracker.Entries().Where(e => e.Entity != null).ToList().ForEach(e => e.State = EntityState.Detached);

        public async Task ExportAccountsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/sql/accounts/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/sql/accounts/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportAccountsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/sql/accounts/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/sql/accounts/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnAccountsRead(ref IQueryable<Models.Sql.Account> items);

        public async Task<IQueryable<Models.Sql.Account>> GetAccounts(Query query = null)
        {
            var items = Context.Accounts.AsQueryable();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnAccountsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnAccountCreated(Models.Sql.Account item);
        partial void OnAfterAccountCreated(Models.Sql.Account item);

        public async Task<Models.Sql.Account> CreateAccount(Models.Sql.Account account)
        {
            OnAccountCreated(account);

            Context.Accounts.Add(account);
            Context.SaveChanges();

            OnAfterAccountCreated(account);

            return account;
        }
        public async Task ExportBalancesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/sql/balances/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/sql/balances/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportBalancesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/sql/balances/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/sql/balances/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnBalancesRead(ref IQueryable<Models.Sql.Balance> items);

        public async Task<IQueryable<Models.Sql.Balance>> GetBalances(Query query = null)
        {
            var items = Context.Balances.AsQueryable();
            items = items.AsNoTracking();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnBalancesRead(ref items);

            return await Task.FromResult(items);
        }
        public async Task ExportBillsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/sql/bills/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/sql/bills/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportBillsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/sql/bills/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/sql/bills/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnBillsRead(ref IQueryable<Models.Sql.Bill> items);

        public async Task<IQueryable<Models.Sql.Bill>> GetBills(Query query = null)
        {
            var items = Context.Bills.AsQueryable();

            items = items.Include(i => i.Payee);

            items = items.Include(i => i.Category);

            items = items.Include(i => i.Status);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnBillsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnBillCreated(Models.Sql.Bill item);
        partial void OnAfterBillCreated(Models.Sql.Bill item);

        public async Task<Models.Sql.Bill> CreateBill(Models.Sql.Bill bill)
        {
            OnBillCreated(bill);

            Context.Bills.Add(bill);
            Context.SaveChanges();

            OnAfterBillCreated(bill);

            return bill;
        }
        public async Task ExportBudgetsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/sql/budgets/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/sql/budgets/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportBudgetsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/sql/budgets/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/sql/budgets/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnBudgetsRead(ref IQueryable<Models.Sql.Budget> items);

        public async Task<IQueryable<Models.Sql.Budget>> GetBudgets(Query query = null)
        {
            var items = Context.Budgets.AsQueryable();

            items = items.Include(i => i.Category);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnBudgetsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnBudgetCreated(Models.Sql.Budget item);
        partial void OnAfterBudgetCreated(Models.Sql.Budget item);

        public async Task<Models.Sql.Budget> CreateBudget(Models.Sql.Budget budget)
        {
            OnBudgetCreated(budget);

            Context.Budgets.Add(budget);
            Context.SaveChanges();

            OnAfterBudgetCreated(budget);

            return budget;
        }
        public async Task ExportCategoriesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/sql/categories/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/sql/categories/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportCategoriesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/sql/categories/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/sql/categories/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnCategoriesRead(ref IQueryable<Models.Sql.Category> items);

        public async Task<IQueryable<Models.Sql.Category>> GetCategories(Query query = null)
        {
            var items = Context.Categories.AsQueryable();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnCategoriesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnCategoryCreated(Models.Sql.Category item);
        partial void OnAfterCategoryCreated(Models.Sql.Category item);

        public async Task<Models.Sql.Category> CreateCategory(Models.Sql.Category category)
        {
            OnCategoryCreated(category);

            Context.Categories.Add(category);
            Context.SaveChanges();

            OnAfterCategoryCreated(category);

            return category;
        }
        public async Task ExportDefaultCategoriesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/sql/defaultcategories/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/sql/defaultcategories/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportDefaultCategoriesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/sql/defaultcategories/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/sql/defaultcategories/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnDefaultCategoriesRead(ref IQueryable<Models.Sql.DefaultCategory> items);

        public async Task<IQueryable<Models.Sql.DefaultCategory>> GetDefaultCategories(Query query = null)
        {
            var items = Context.DefaultCategories.AsQueryable();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnDefaultCategoriesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnDefaultCategoryCreated(Models.Sql.DefaultCategory item);
        partial void OnAfterDefaultCategoryCreated(Models.Sql.DefaultCategory item);

        public async Task<Models.Sql.DefaultCategory> CreateDefaultCategory(Models.Sql.DefaultCategory defaultCategory)
        {
            OnDefaultCategoryCreated(defaultCategory);

            Context.DefaultCategories.Add(defaultCategory);
            Context.SaveChanges();

            OnAfterDefaultCategoryCreated(defaultCategory);

            return defaultCategory;
        }
        public async Task ExportPayeesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/sql/payees/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/sql/payees/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportPayeesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/sql/payees/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/sql/payees/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnPayeesRead(ref IQueryable<Models.Sql.Payee> items);

        public async Task<IQueryable<Models.Sql.Payee>> GetPayees(Query query = null)
        {
            var items = Context.Payees.AsQueryable();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnPayeesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnPayeeCreated(Models.Sql.Payee item);
        partial void OnAfterPayeeCreated(Models.Sql.Payee item);

        public async Task<Models.Sql.Payee> CreatePayee(Models.Sql.Payee payee)
        {
            OnPayeeCreated(payee);

            Context.Payees.Add(payee);
            Context.SaveChanges();

            OnAfterPayeeCreated(payee);

            return payee;
        }
        public async Task ExportRecurringTransactionsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/sql/recurringtransactions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/sql/recurringtransactions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportRecurringTransactionsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/sql/recurringtransactions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/sql/recurringtransactions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnRecurringTransactionsRead(ref IQueryable<Models.Sql.RecurringTransaction> items);

        public async Task<IQueryable<Models.Sql.RecurringTransaction>> GetRecurringTransactions(Query query = null)
        {
            var items = Context.RecurringTransactions.AsQueryable();

            items = items.Include(i => i.Payee);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnRecurringTransactionsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnRecurringTransactionCreated(Models.Sql.RecurringTransaction item);
        partial void OnAfterRecurringTransactionCreated(Models.Sql.RecurringTransaction item);

        public async Task<Models.Sql.RecurringTransaction> CreateRecurringTransaction(Models.Sql.RecurringTransaction recurringTransaction)
        {
            OnRecurringTransactionCreated(recurringTransaction);

            Context.RecurringTransactions.Add(recurringTransaction);
            Context.SaveChanges();

            OnAfterRecurringTransactionCreated(recurringTransaction);

            return recurringTransaction;
        }
        public async Task ExportSpendingThisMonthsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/sql/spendingthismonths/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/sql/spendingthismonths/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportSpendingThisMonthsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/sql/spendingthismonths/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/sql/spendingthismonths/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnSpendingThisMonthsRead(ref IQueryable<Models.Sql.SpendingThisMonth> items);

        public async Task<IQueryable<Models.Sql.SpendingThisMonth>> GetSpendingThisMonths(Query query = null)
        {
            var items = Context.SpendingThisMonths.AsQueryable();
            items = items.AsNoTracking();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnSpendingThisMonthsRead(ref items);

            return await Task.FromResult(items);
        }
        public async Task ExportStatusesToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/sql/statuses/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/sql/statuses/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportStatusesToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/sql/statuses/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/sql/statuses/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnStatusesRead(ref IQueryable<Models.Sql.Status> items);

        public async Task<IQueryable<Models.Sql.Status>> GetStatuses(Query query = null)
        {
            var items = Context.Statuses.AsQueryable();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnStatusesRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnStatusCreated(Models.Sql.Status item);
        partial void OnAfterStatusCreated(Models.Sql.Status item);

        public async Task<Models.Sql.Status> CreateStatus(Models.Sql.Status status)
        {
            OnStatusCreated(status);

            Context.Statuses.Add(status);
            Context.SaveChanges();

            OnAfterStatusCreated(status);

            return status;
        }
        public async Task ExportSummaryCategoriesThisMonthsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/sql/summarycategoriesthismonths/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/sql/summarycategoriesthismonths/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportSummaryCategoriesThisMonthsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/sql/summarycategoriesthismonths/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/sql/summarycategoriesthismonths/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnSummaryCategoriesThisMonthsRead(ref IQueryable<Models.Sql.SummaryCategoriesThisMonth> items);

        public async Task<IQueryable<Models.Sql.SummaryCategoriesThisMonth>> GetSummaryCategoriesThisMonths(Query query = null)
        {
            var items = Context.SummaryCategoriesThisMonths.AsQueryable();
            items = items.AsNoTracking();

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnSummaryCategoriesThisMonthsRead(ref items);

            return await Task.FromResult(items);
        }
        public async Task ExportTransactionsToExcel(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/sql/transactions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/sql/transactions/excel(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        public async Task ExportTransactionsToCSV(Query query = null, string fileName = null)
        {
            navigationManager.NavigateTo(query != null ? query.ToUrl($"export/sql/transactions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')") : $"export/sql/transactions/csv(fileName='{(!string.IsNullOrEmpty(fileName) ? UrlEncoder.Default.Encode(fileName) : "Export")}')", true);
        }

        partial void OnTransactionsRead(ref IQueryable<Models.Sql.Transaction> items);

        public async Task<IQueryable<Models.Sql.Transaction>> GetTransactions(Query query = null)
        {
            var items = Context.Transactions.AsQueryable();

            items = items.Include(i => i.Account);

            items = items.Include(i => i.Category);

            items = items.Include(i => i.Payee);

            items = items.Include(i => i.Status);

            if (query != null)
            {
                if (!string.IsNullOrEmpty(query.Expand))
                {
                    var propertiesToExpand = query.Expand.Split(',');
                    foreach(var p in propertiesToExpand)
                    {
                        items = items.Include(p);
                    }
                }

                if (!string.IsNullOrEmpty(query.Filter))
                {
                    if (query.FilterParameters != null)
                    {
                        items = items.Where(query.Filter, query.FilterParameters);
                    }
                    else
                    {
                        items = items.Where(query.Filter);
                    }
                }

                if (!string.IsNullOrEmpty(query.OrderBy))
                {
                    items = items.OrderBy(query.OrderBy);
                }

                if (query.Skip.HasValue)
                {
                    items = items.Skip(query.Skip.Value);
                }

                if (query.Top.HasValue)
                {
                    items = items.Take(query.Top.Value);
                }
            }

            OnTransactionsRead(ref items);

            return await Task.FromResult(items);
        }

        partial void OnTransactionCreated(Models.Sql.Transaction item);
        partial void OnAfterTransactionCreated(Models.Sql.Transaction item);

        public async Task<Models.Sql.Transaction> CreateTransaction(Models.Sql.Transaction transaction)
        {
            OnTransactionCreated(transaction);

            Context.Transactions.Add(transaction);
            Context.SaveChanges();

            OnAfterTransactionCreated(transaction);

            return transaction;
        }

        partial void OnAccountDeleted(Models.Sql.Account item);
        partial void OnAfterAccountDeleted(Models.Sql.Account item);

        public async Task<Models.Sql.Account> DeleteAccount(int? accountId)
        {
            var itemToDelete = Context.Accounts
                              .Where(i => i.account_id == accountId)
                              .Include(i => i.Transactions)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnAccountDeleted(itemToDelete);

            Context.Accounts.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterAccountDeleted(itemToDelete);

            return itemToDelete;
        }

        partial void OnAccountGet(Models.Sql.Account item);

        public async Task<Models.Sql.Account> GetAccountByaccountId(int? accountId)
        {
            var items = Context.Accounts
                              .AsNoTracking()
                              .Where(i => i.account_id == accountId);

            var itemToReturn = items.FirstOrDefault();

            OnAccountGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        public async Task<Models.Sql.Account> CancelAccountChanges(Models.Sql.Account item)
        {
            var entityToCancel = Context.Entry(item);
            entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
            entityToCancel.State = EntityState.Unchanged;

            return item;
        }

        partial void OnAccountUpdated(Models.Sql.Account item);
        partial void OnAfterAccountUpdated(Models.Sql.Account item);

        public async Task<Models.Sql.Account> UpdateAccount(int? accountId, Models.Sql.Account account)
        {
            OnAccountUpdated(account);

            var itemToUpdate = Context.Accounts
                              .Where(i => i.account_id == accountId)
                              .FirstOrDefault();
            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(account);
            entryToUpdate.State = EntityState.Modified;
            Context.SaveChanges();       

            OnAfterAccountUpdated(account);

            return account;
        }

        partial void OnBillDeleted(Models.Sql.Bill item);
        partial void OnAfterBillDeleted(Models.Sql.Bill item);

        public async Task<Models.Sql.Bill> DeleteBill(int? billId)
        {
            var itemToDelete = Context.Bills
                              .Where(i => i.bill_id == billId)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnBillDeleted(itemToDelete);

            Context.Bills.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterBillDeleted(itemToDelete);

            return itemToDelete;
        }

        partial void OnBillGet(Models.Sql.Bill item);

        public async Task<Models.Sql.Bill> GetBillBybillId(int? billId)
        {
            var items = Context.Bills
                              .AsNoTracking()
                              .Where(i => i.bill_id == billId);

            items = items.Include(i => i.Payee);

            items = items.Include(i => i.Category);

            items = items.Include(i => i.Status);

            var itemToReturn = items.FirstOrDefault();

            OnBillGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        public async Task<Models.Sql.Bill> CancelBillChanges(Models.Sql.Bill item)
        {
            var entityToCancel = Context.Entry(item);
            entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
            entityToCancel.State = EntityState.Unchanged;

            return item;
        }

        partial void OnBillUpdated(Models.Sql.Bill item);
        partial void OnAfterBillUpdated(Models.Sql.Bill item);

        public async Task<Models.Sql.Bill> UpdateBill(int? billId, Models.Sql.Bill bill)
        {
            OnBillUpdated(bill);

            var itemToUpdate = Context.Bills
                              .Where(i => i.bill_id == billId)
                              .FirstOrDefault();
            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(bill);
            entryToUpdate.State = EntityState.Modified;
            Context.SaveChanges();       

            OnAfterBillUpdated(bill);

            return bill;
        }

        partial void OnBudgetDeleted(Models.Sql.Budget item);
        partial void OnAfterBudgetDeleted(Models.Sql.Budget item);

        public async Task<Models.Sql.Budget> DeleteBudget(int? budgetId)
        {
            var itemToDelete = Context.Budgets
                              .Where(i => i.budget_id == budgetId)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnBudgetDeleted(itemToDelete);

            Context.Budgets.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterBudgetDeleted(itemToDelete);

            return itemToDelete;
        }

        partial void OnBudgetGet(Models.Sql.Budget item);

        public async Task<Models.Sql.Budget> GetBudgetBybudgetId(int? budgetId)
        {
            var items = Context.Budgets
                              .AsNoTracking()
                              .Where(i => i.budget_id == budgetId);

            items = items.Include(i => i.Category);

            var itemToReturn = items.FirstOrDefault();

            OnBudgetGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        public async Task<Models.Sql.Budget> CancelBudgetChanges(Models.Sql.Budget item)
        {
            var entityToCancel = Context.Entry(item);
            entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
            entityToCancel.State = EntityState.Unchanged;

            return item;
        }

        partial void OnBudgetUpdated(Models.Sql.Budget item);
        partial void OnAfterBudgetUpdated(Models.Sql.Budget item);

        public async Task<Models.Sql.Budget> UpdateBudget(int? budgetId, Models.Sql.Budget budget)
        {
            OnBudgetUpdated(budget);

            var itemToUpdate = Context.Budgets
                              .Where(i => i.budget_id == budgetId)
                              .FirstOrDefault();
            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(budget);
            entryToUpdate.State = EntityState.Modified;
            Context.SaveChanges();       

            OnAfterBudgetUpdated(budget);

            return budget;
        }

        partial void OnCategoryDeleted(Models.Sql.Category item);
        partial void OnAfterCategoryDeleted(Models.Sql.Category item);

        public async Task<Models.Sql.Category> DeleteCategory(int? categoryId)
        {
            var itemToDelete = Context.Categories
                              .Where(i => i.category_id == categoryId)
                              .Include(i => i.Transactions)
                              .Include(i => i.Bills)
                              .Include(i => i.Budgets)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnCategoryDeleted(itemToDelete);

            Context.Categories.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterCategoryDeleted(itemToDelete);

            return itemToDelete;
        }

        partial void OnCategoryGet(Models.Sql.Category item);

        public async Task<Models.Sql.Category> GetCategoryBycategoryId(int? categoryId)
        {
            var items = Context.Categories
                              .AsNoTracking()
                              .Where(i => i.category_id == categoryId);

            var itemToReturn = items.FirstOrDefault();

            OnCategoryGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        public async Task<Models.Sql.Category> CancelCategoryChanges(Models.Sql.Category item)
        {
            var entityToCancel = Context.Entry(item);
            entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
            entityToCancel.State = EntityState.Unchanged;

            return item;
        }

        partial void OnCategoryUpdated(Models.Sql.Category item);
        partial void OnAfterCategoryUpdated(Models.Sql.Category item);

        public async Task<Models.Sql.Category> UpdateCategory(int? categoryId, Models.Sql.Category category)
        {
            OnCategoryUpdated(category);

            var itemToUpdate = Context.Categories
                              .Where(i => i.category_id == categoryId)
                              .FirstOrDefault();
            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(category);
            entryToUpdate.State = EntityState.Modified;
            Context.SaveChanges();       

            OnAfterCategoryUpdated(category);

            return category;
        }

        partial void OnDefaultCategoryDeleted(Models.Sql.DefaultCategory item);
        partial void OnAfterDefaultCategoryDeleted(Models.Sql.DefaultCategory item);

        public async Task<Models.Sql.DefaultCategory> DeleteDefaultCategory(int? payeeId)
        {
            var itemToDelete = Context.DefaultCategories
                              .Where(i => i.payee_id == payeeId)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnDefaultCategoryDeleted(itemToDelete);

            Context.DefaultCategories.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterDefaultCategoryDeleted(itemToDelete);

            return itemToDelete;
        }

        partial void OnDefaultCategoryGet(Models.Sql.DefaultCategory item);

        public async Task<Models.Sql.DefaultCategory> GetDefaultCategoryBypayeeId(int? payeeId)
        {
            var items = Context.DefaultCategories
                              .AsNoTracking()
                              .Where(i => i.payee_id == payeeId);

            var itemToReturn = items.FirstOrDefault();

            OnDefaultCategoryGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        public async Task<Models.Sql.DefaultCategory> CancelDefaultCategoryChanges(Models.Sql.DefaultCategory item)
        {
            var entityToCancel = Context.Entry(item);
            entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
            entityToCancel.State = EntityState.Unchanged;

            return item;
        }

        partial void OnDefaultCategoryUpdated(Models.Sql.DefaultCategory item);
        partial void OnAfterDefaultCategoryUpdated(Models.Sql.DefaultCategory item);

        public async Task<Models.Sql.DefaultCategory> UpdateDefaultCategory(int? payeeId, Models.Sql.DefaultCategory defaultCategory)
        {
            OnDefaultCategoryUpdated(defaultCategory);

            var itemToUpdate = Context.DefaultCategories
                              .Where(i => i.payee_id == payeeId)
                              .FirstOrDefault();
            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(defaultCategory);
            entryToUpdate.State = EntityState.Modified;
            Context.SaveChanges();       

            OnAfterDefaultCategoryUpdated(defaultCategory);

            return defaultCategory;
        }

        partial void OnPayeeDeleted(Models.Sql.Payee item);
        partial void OnAfterPayeeDeleted(Models.Sql.Payee item);

        public async Task<Models.Sql.Payee> DeletePayee(int? payeeId)
        {
            var itemToDelete = Context.Payees
                              .Where(i => i.payee_id == payeeId)
                              .Include(i => i.Transactions)
                              .Include(i => i.Bills)
                              .Include(i => i.RecurringTransactions)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnPayeeDeleted(itemToDelete);

            Context.Payees.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterPayeeDeleted(itemToDelete);

            return itemToDelete;
        }

        partial void OnPayeeGet(Models.Sql.Payee item);

        public async Task<Models.Sql.Payee> GetPayeeBypayeeId(int? payeeId)
        {
            var items = Context.Payees
                              .AsNoTracking()
                              .Where(i => i.payee_id == payeeId);

            var itemToReturn = items.FirstOrDefault();

            OnPayeeGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        public async Task<Models.Sql.Payee> CancelPayeeChanges(Models.Sql.Payee item)
        {
            var entityToCancel = Context.Entry(item);
            entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
            entityToCancel.State = EntityState.Unchanged;

            return item;
        }

        partial void OnPayeeUpdated(Models.Sql.Payee item);
        partial void OnAfterPayeeUpdated(Models.Sql.Payee item);

        public async Task<Models.Sql.Payee> UpdatePayee(int? payeeId, Models.Sql.Payee payee)
        {
            OnPayeeUpdated(payee);

            var itemToUpdate = Context.Payees
                              .Where(i => i.payee_id == payeeId)
                              .FirstOrDefault();
            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(payee);
            entryToUpdate.State = EntityState.Modified;
            Context.SaveChanges();       

            OnAfterPayeeUpdated(payee);

            return payee;
        }

        partial void OnRecurringTransactionDeleted(Models.Sql.RecurringTransaction item);
        partial void OnAfterRecurringTransactionDeleted(Models.Sql.RecurringTransaction item);

        public async Task<Models.Sql.RecurringTransaction> DeleteRecurringTransaction(int? billId)
        {
            var itemToDelete = Context.RecurringTransactions
                              .Where(i => i.bill_id == billId)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnRecurringTransactionDeleted(itemToDelete);

            Context.RecurringTransactions.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterRecurringTransactionDeleted(itemToDelete);

            return itemToDelete;
        }

        partial void OnRecurringTransactionGet(Models.Sql.RecurringTransaction item);

        public async Task<Models.Sql.RecurringTransaction> GetRecurringTransactionBybillId(int? billId)
        {
            var items = Context.RecurringTransactions
                              .AsNoTracking()
                              .Where(i => i.bill_id == billId);

            items = items.Include(i => i.Payee);

            var itemToReturn = items.FirstOrDefault();

            OnRecurringTransactionGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        public async Task<Models.Sql.RecurringTransaction> CancelRecurringTransactionChanges(Models.Sql.RecurringTransaction item)
        {
            var entityToCancel = Context.Entry(item);
            entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
            entityToCancel.State = EntityState.Unchanged;

            return item;
        }

        partial void OnRecurringTransactionUpdated(Models.Sql.RecurringTransaction item);
        partial void OnAfterRecurringTransactionUpdated(Models.Sql.RecurringTransaction item);

        public async Task<Models.Sql.RecurringTransaction> UpdateRecurringTransaction(int? billId, Models.Sql.RecurringTransaction recurringTransaction)
        {
            OnRecurringTransactionUpdated(recurringTransaction);

            var itemToUpdate = Context.RecurringTransactions
                              .Where(i => i.bill_id == billId)
                              .FirstOrDefault();
            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(recurringTransaction);
            entryToUpdate.State = EntityState.Modified;
            Context.SaveChanges();       

            OnAfterRecurringTransactionUpdated(recurringTransaction);

            return recurringTransaction;
        }

        partial void OnStatusDeleted(Models.Sql.Status item);
        partial void OnAfterStatusDeleted(Models.Sql.Status item);

        public async Task<Models.Sql.Status> DeleteStatus(int? statusId)
        {
            var itemToDelete = Context.Statuses
                              .Where(i => i.status_id == statusId)
                              .Include(i => i.Transactions)
                              .Include(i => i.Bills)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnStatusDeleted(itemToDelete);

            Context.Statuses.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterStatusDeleted(itemToDelete);

            return itemToDelete;
        }

        partial void OnStatusGet(Models.Sql.Status item);

        public async Task<Models.Sql.Status> GetStatusBystatusId(int? statusId)
        {
            var items = Context.Statuses
                              .AsNoTracking()
                              .Where(i => i.status_id == statusId);

            var itemToReturn = items.FirstOrDefault();

            OnStatusGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        public async Task<Models.Sql.Status> CancelStatusChanges(Models.Sql.Status item)
        {
            var entityToCancel = Context.Entry(item);
            entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
            entityToCancel.State = EntityState.Unchanged;

            return item;
        }

        partial void OnStatusUpdated(Models.Sql.Status item);
        partial void OnAfterStatusUpdated(Models.Sql.Status item);

        public async Task<Models.Sql.Status> UpdateStatus(int? statusId, Models.Sql.Status status)
        {
            OnStatusUpdated(status);

            var itemToUpdate = Context.Statuses
                              .Where(i => i.status_id == statusId)
                              .FirstOrDefault();
            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(status);
            entryToUpdate.State = EntityState.Modified;
            Context.SaveChanges();       

            OnAfterStatusUpdated(status);

            return status;
        }

        partial void OnTransactionDeleted(Models.Sql.Transaction item);
        partial void OnAfterTransactionDeleted(Models.Sql.Transaction item);

        public async Task<Models.Sql.Transaction> DeleteTransaction(int? transactionId)
        {
            var itemToDelete = Context.Transactions
                              .Where(i => i.transaction_id == transactionId)
                              .FirstOrDefault();

            if (itemToDelete == null)
            {
               throw new Exception("Item no longer available");
            }

            OnTransactionDeleted(itemToDelete);

            Context.Transactions.Remove(itemToDelete);

            try
            {
                Context.SaveChanges();
            }
            catch
            {
                Context.Entry(itemToDelete).State = EntityState.Unchanged;
                throw;
            }

            OnAfterTransactionDeleted(itemToDelete);

            return itemToDelete;
        }

        partial void OnTransactionGet(Models.Sql.Transaction item);

        public async Task<Models.Sql.Transaction> GetTransactionBytransactionId(int? transactionId)
        {
            var items = Context.Transactions
                              .AsNoTracking()
                              .Where(i => i.transaction_id == transactionId);

            items = items.Include(i => i.Account);

            items = items.Include(i => i.Category);

            items = items.Include(i => i.Payee);

            items = items.Include(i => i.Status);

            var itemToReturn = items.FirstOrDefault();

            OnTransactionGet(itemToReturn);

            return await Task.FromResult(itemToReturn);
        }

        public async Task<Models.Sql.Transaction> CancelTransactionChanges(Models.Sql.Transaction item)
        {
            var entityToCancel = Context.Entry(item);
            entityToCancel.CurrentValues.SetValues(entityToCancel.OriginalValues);
            entityToCancel.State = EntityState.Unchanged;

            return item;
        }

        partial void OnTransactionUpdated(Models.Sql.Transaction item);
        partial void OnAfterTransactionUpdated(Models.Sql.Transaction item);

        public async Task<Models.Sql.Transaction> UpdateTransaction(int? transactionId, Models.Sql.Transaction transaction)
        {
            OnTransactionUpdated(transaction);

            var itemToUpdate = Context.Transactions
                              .Where(i => i.transaction_id == transactionId)
                              .FirstOrDefault();
            if (itemToUpdate == null)
            {
               throw new Exception("Item no longer available");
            }

            var entryToUpdate = Context.Entry(itemToUpdate);
            entryToUpdate.CurrentValues.SetValues(transaction);
            entryToUpdate.State = EntityState.Modified;
            Context.SaveChanges();       

            OnAfterTransactionUpdated(transaction);

            return transaction;
        }
    }
}
