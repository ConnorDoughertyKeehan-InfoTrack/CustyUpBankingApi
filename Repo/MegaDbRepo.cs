using CustyUpBankingApi.Contexts;
using CustyUpBankingApi.Models.Entities;
using CustyUpBankingApi.Models.Requests;
using CustyUpBankingApi.Models.SpResponses;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CustyUpBankingApi.Repo
{
    public class MegaDbRepo : IMegaDbRepo
    {
        private readonly MegaDbContext _dbContext;
        public MegaDbRepo(MegaDbContext megaDbContext)
        {
            _dbContext = megaDbContext;
        }
        public async Task InsertSpendTransactions(List<SpendTransaction> spendTransactions)
        {
            await _dbContext.SpendTransactions.AddRangeAsync(spendTransactions);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<WeeklySpendingResponse>> GetWeeklySpending()
        {
            var weeklySpending = await _dbContext.Set<WeeklySpendingResponse>()
                .FromSqlRaw("EXEC WeeklySpending")
                .ToListAsync();

            return weeklySpending;
        }

        public async Task<List<LastWeekSpendResponse>> GetLastWeekSpend()
        {
            var lastWeekSpend = await _dbContext.Set<LastWeekSpendResponse>()
                .FromSqlRaw("EXEC LastWeekSpend")
                .ToListAsync();

            return lastWeekSpend;
        }

        public async Task<List<SpendTransaction>> GetTransactionsByWeek(int weekNumber)
        {
            var result = await _dbContext.SpendTransactions
                .Where(x => x.WeekNumber == weekNumber)
                .OrderByDescending(x => x.DateSpend)
                .ToListAsync();

            return result;
        }

        public async Task<List<GetWeeklyBudgetResponse>> GetBudgetByWeek(int weekNumber)
        {
            var result = await _dbContext.Set<GetWeeklyBudgetResponse>()
                .FromSqlRaw("EXEC GetWeeklyBudget @weekNumber", new SqlParameter("@weekNumber", weekNumber))
                .ToListAsync();

            return result;
        }

        public async Task<string?> GetMostAssignedCategoryByString(string description)
        {
            var response = await _dbContext.Set<GetMostAssignedCategoryByStringResponse>()
                .FromSqlRaw("EXEC GetMostAssignedCategoryByString @Description", new SqlParameter("@Description", description))
                .ToListAsync();

            var result = response.SingleOrDefault();

            return result?.Category;
        }

        public async Task UpsertBudgetItem(int weekNumber, string category, decimal amount)
        {
            var existingRow = _dbContext.Budget.Where(x => x.WeekNumber == weekNumber && x.Category == category && x.Amount == amount).SingleOrDefault();

            if (existingRow != null)
            {
                existingRow.Amount = amount;
            }
            else
            {
                var newRow = new Budget { WeekNumber = weekNumber, Category = category, Amount = amount };
                await _dbContext.Budget.AddAsync(newRow);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateCategoryBySpendTransactionId(int id, string category)
        {
            var transaction = _dbContext.SpendTransactions.Where(x => x.Id == id).Single();

            transaction.Category = category.ToString();

            await _dbContext.SaveChangesAsync();
        }
    }
}
