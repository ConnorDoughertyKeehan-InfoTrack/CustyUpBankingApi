using CustyUpBankingApi.Models.Entities;
using CustyUpBankingApi.Models.Requests;
using CustyUpBankingApi.Models.SpResponses;
using Microsoft.EntityFrameworkCore;

namespace CustyUpBankingApi.Contexts
{
    public class MegaDbContext : DbContext
    {
        public MegaDbContext(DbContextOptions<MegaDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WeeklySpendingResponse>().HasNoKey();
            modelBuilder.Entity<LastWeekSpendResponse>().HasNoKey();
            modelBuilder.Entity<GetWeeklyBudgetResponse>().HasNoKey();
            modelBuilder.Entity<GetMostAssignedCategoryByStringResponse>().HasNoKey();
        }
        // Define your DbSets (tables) here
        public DbSet<SpendTransaction> SpendTransactions { get; set; }
        public DbSet<Budget> Budget {  get; set; }
    }
}
