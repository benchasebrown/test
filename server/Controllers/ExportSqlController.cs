using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PersonalFinance.Data;

namespace PersonalFinance
{
    public partial class ExportSqlController : ExportController
    {
        private readonly SqlContext context;

        public ExportSqlController(SqlContext context)
        {
            this.context = context;
        }
        [HttpGet("/export/Sql/accounts/csv")]
        [HttpGet("/export/Sql/accounts/csv(fileName='{fileName}')")]
        public FileStreamResult ExportAccountsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(context.Accounts, Request.Query), fileName);
        }

        [HttpGet("/export/Sql/accounts/excel")]
        [HttpGet("/export/Sql/accounts/excel(fileName='{fileName}')")]
        public FileStreamResult ExportAccountsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(context.Accounts, Request.Query), fileName);
        }
        [HttpGet("/export/Sql/balances/csv")]
        [HttpGet("/export/Sql/balances/csv(fileName='{fileName}')")]
        public FileStreamResult ExportBalancesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(context.Balances, Request.Query), fileName);
        }

        [HttpGet("/export/Sql/balances/excel")]
        [HttpGet("/export/Sql/balances/excel(fileName='{fileName}')")]
        public FileStreamResult ExportBalancesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(context.Balances, Request.Query), fileName);
        }
        [HttpGet("/export/Sql/bills/csv")]
        [HttpGet("/export/Sql/bills/csv(fileName='{fileName}')")]
        public FileStreamResult ExportBillsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(context.Bills, Request.Query), fileName);
        }

        [HttpGet("/export/Sql/bills/excel")]
        [HttpGet("/export/Sql/bills/excel(fileName='{fileName}')")]
        public FileStreamResult ExportBillsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(context.Bills, Request.Query), fileName);
        }
        [HttpGet("/export/Sql/budgets/csv")]
        [HttpGet("/export/Sql/budgets/csv(fileName='{fileName}')")]
        public FileStreamResult ExportBudgetsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(context.Budgets, Request.Query), fileName);
        }

        [HttpGet("/export/Sql/budgets/excel")]
        [HttpGet("/export/Sql/budgets/excel(fileName='{fileName}')")]
        public FileStreamResult ExportBudgetsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(context.Budgets, Request.Query), fileName);
        }
        [HttpGet("/export/Sql/categories/csv")]
        [HttpGet("/export/Sql/categories/csv(fileName='{fileName}')")]
        public FileStreamResult ExportCategoriesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(context.Categories, Request.Query), fileName);
        }

        [HttpGet("/export/Sql/categories/excel")]
        [HttpGet("/export/Sql/categories/excel(fileName='{fileName}')")]
        public FileStreamResult ExportCategoriesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(context.Categories, Request.Query), fileName);
        }
        [HttpGet("/export/Sql/defaultcategories/csv")]
        [HttpGet("/export/Sql/defaultcategories/csv(fileName='{fileName}')")]
        public FileStreamResult ExportDefaultCategoriesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(context.DefaultCategories, Request.Query), fileName);
        }

        [HttpGet("/export/Sql/defaultcategories/excel")]
        [HttpGet("/export/Sql/defaultcategories/excel(fileName='{fileName}')")]
        public FileStreamResult ExportDefaultCategoriesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(context.DefaultCategories, Request.Query), fileName);
        }
        [HttpGet("/export/Sql/payees/csv")]
        [HttpGet("/export/Sql/payees/csv(fileName='{fileName}')")]
        public FileStreamResult ExportPayeesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(context.Payees, Request.Query), fileName);
        }

        [HttpGet("/export/Sql/payees/excel")]
        [HttpGet("/export/Sql/payees/excel(fileName='{fileName}')")]
        public FileStreamResult ExportPayeesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(context.Payees, Request.Query), fileName);
        }
        [HttpGet("/export/Sql/recurringtransactions/csv")]
        [HttpGet("/export/Sql/recurringtransactions/csv(fileName='{fileName}')")]
        public FileStreamResult ExportRecurringTransactionsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(context.RecurringTransactions, Request.Query), fileName);
        }

        [HttpGet("/export/Sql/recurringtransactions/excel")]
        [HttpGet("/export/Sql/recurringtransactions/excel(fileName='{fileName}')")]
        public FileStreamResult ExportRecurringTransactionsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(context.RecurringTransactions, Request.Query), fileName);
        }
        [HttpGet("/export/Sql/spendingthismonths/csv")]
        [HttpGet("/export/Sql/spendingthismonths/csv(fileName='{fileName}')")]
        public FileStreamResult ExportSpendingThisMonthsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(context.SpendingThisMonths, Request.Query), fileName);
        }

        [HttpGet("/export/Sql/spendingthismonths/excel")]
        [HttpGet("/export/Sql/spendingthismonths/excel(fileName='{fileName}')")]
        public FileStreamResult ExportSpendingThisMonthsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(context.SpendingThisMonths, Request.Query), fileName);
        }
        [HttpGet("/export/Sql/statuses/csv")]
        [HttpGet("/export/Sql/statuses/csv(fileName='{fileName}')")]
        public FileStreamResult ExportStatusesToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(context.Statuses, Request.Query), fileName);
        }

        [HttpGet("/export/Sql/statuses/excel")]
        [HttpGet("/export/Sql/statuses/excel(fileName='{fileName}')")]
        public FileStreamResult ExportStatusesToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(context.Statuses, Request.Query), fileName);
        }
        [HttpGet("/export/Sql/summarycategoriesthismonths/csv")]
        [HttpGet("/export/Sql/summarycategoriesthismonths/csv(fileName='{fileName}')")]
        public FileStreamResult ExportSummaryCategoriesThisMonthsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(context.SummaryCategoriesThisMonths, Request.Query), fileName);
        }

        [HttpGet("/export/Sql/summarycategoriesthismonths/excel")]
        [HttpGet("/export/Sql/summarycategoriesthismonths/excel(fileName='{fileName}')")]
        public FileStreamResult ExportSummaryCategoriesThisMonthsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(context.SummaryCategoriesThisMonths, Request.Query), fileName);
        }
        [HttpGet("/export/Sql/transactions/csv")]
        [HttpGet("/export/Sql/transactions/csv(fileName='{fileName}')")]
        public FileStreamResult ExportTransactionsToCSV(string fileName = null)
        {
            return ToCSV(ApplyQuery(context.Transactions, Request.Query), fileName);
        }

        [HttpGet("/export/Sql/transactions/excel")]
        [HttpGet("/export/Sql/transactions/excel(fileName='{fileName}')")]
        public FileStreamResult ExportTransactionsToExcel(string fileName = null)
        {
            return ToExcel(ApplyQuery(context.Transactions, Request.Query), fileName);
        }
    }
}
