using AutoMapper;
using CustyUpBankingApi.Extensions;
using CustyUpBankingApi.Models.Entities;
using CustyUpBankingApi.Models.Enums;
using CustyUpBankingApi.Models.Requests;
using CustyUpBankingApi.Models.SpResponses;
using CustyUpBankingApi.Models.UpBanking;
using CustyUpBankingApi.Repo;
using CustyUpBankingApi.Services.Interfaces;
using System;

namespace CustyUpBankingApi.Services
{
    public class BudgetService : IBudgetService
	{
		IMapper _mapper;
		IUpBankingService _upBankingService;
		IAiService _aiService;
		IMegaDbRepo _megaDbRepo;
		private int currentWeekNumber = DateTime.Now.GetWeekNumber();

        public BudgetService(IMapper mapper, IUpBankingService upBankingService, IAiService aiService, IMegaDbRepo megaDbRepo)
		{
			_mapper = mapper;
			_upBankingService = upBankingService;
			_aiService = aiService;
			_megaDbRepo = megaDbRepo;
		}

		public async Task<List<SpendTransaction>> GetCurrentWeeksTransactions()
		{
			var result = await _megaDbRepo.GetTransactionsByWeek(currentWeekNumber);

			return result;
		}

        public async Task<List<GetWeeklyBudgetResponse>> GetCurrentWeekBudget()
        {
            var result = await _megaDbRepo.GetBudgetByWeek(currentWeekNumber);

            return result;
        }

        public async Task UpdateCurrentBudget(List<UpdateBudgetRequest> request)
		{
			foreach(var budgetItem in request)
			{
				if(budgetItem.Amount != null && Enum.TryParse(typeof(Categories), budgetItem.Category, true, out _))
				{
					await _megaDbRepo.UpsertBudgetItem(currentWeekNumber, budgetItem.Category, budgetItem.Amount.Value);
                }
			}
        }

        public async Task UpdateCategory(UpdateCategoryRequest request)
        {
			await _megaDbRepo.UpdateCategoryBySpendTransactionId(request.Id, request.Category);
        }

        public async Task<List<SpendTransaction>> InsertTransactionsFromDate(DateOnly date)
		{
			var transactions = await GetSpendTransactions(date);
			await _megaDbRepo.InsertSpendTransactions(transactions);
			return transactions;
		}

		public async Task<List<SpendTransaction>> GetSpendTransactions(DateOnly? date)
		{
			var upBankingTrans = (await _upBankingService.GetTransactions(date)).Data;

			//This logic removes unnecessary transactions
			upBankingTrans = FilterTransactions(upBankingTrans);
			List<SpendTransaction> spendTransactions = _mapper.Map<List<SpendTransaction>>(upBankingTrans);

			foreach (var spendTransaction in spendTransactions)
			{
				var getCategoryFromDescriptionResponse = await GetCategory(spendTransaction.TransactionDescription, spendTransaction.TransactionType);
				spendTransaction.Category = getCategoryFromDescriptionResponse.Category;
				spendTransaction.CategorizedByAi = getCategoryFromDescriptionResponse.CategorizedByAi;

				AlterTransaction(spendTransaction);
			}

			return spendTransactions;
		}

		private void AlterTransaction(SpendTransaction spendTransaction)
		{
			//This function is meant to alter Transactions in place. For example:
			//If you pay the entire rent but have your roomate then pay you, you might want to alter the transaction amount.
			if (spendTransaction.TransactionDescription == "Rent")
				spendTransaction.Amount = 375;
		}

		private List<UpTransaction> FilterTransactions(List<UpTransaction> transactions)
		{
			//Remove all payments to account
			transactions.RemoveAll(t => t.Attributes.Amount.ValueInBaseUnits >= 0);

			//Remove all transfer transactions
			transactions.RemoveAll(x => x.Attributes.TransactionType == "Transfer");

			//This logic removes the pending $1 transactions, NSW Transport creates a new transaction for the full amount
			string NswTransportString = "Transport for NSW";
			transactions.RemoveAll(t => (t.Attributes.Description == NswTransportString && t.Attributes.SettledAt == null));

			return transactions;
		}

		//Get Category from common cases, in unknown cases ask GPT
		private async Task<GetCategoryFromDescriptionResponse> GetCategory(string description, string TransactionType)
		{
			var getCategoryFromDescriptionResponse = new GetCategoryFromDescriptionResponse()
			{
				Category = Categories.FailedToCategorize.ToString(),
				CategorizedByAi = false
			};

			var mostPreviouslyAssignedCategory = await _megaDbRepo.GetMostAssignedCategoryByString(description);

			//If it's been assigned before it will show up in the DB. This should prevent it from ever getting to AI
			//as long as you've categorized it a single time.
			if (mostPreviouslyAssignedCategory != null)
				return new GetCategoryFromDescriptionResponse { Category = mostPreviouslyAssignedCategory, CategorizedByAi = false };

			//If can't find cases then get from AI
			try { 
				var category = await _aiService.GetCategoryFromTransaction(description);
				return new GetCategoryFromDescriptionResponse() { Category = category.ToString(), CategorizedByAi = true };
			}
			//If AI budget runs out or GPT is down just ignore it, not actually very important with the above categorization techniques.
			catch {
				return getCategoryFromDescriptionResponse;
			}
		}

		private class GetCategoryFromDescriptionResponse
		{
			public required string Category { get; set; }
			public bool CategorizedByAi { get; set; }
		}
	}
}
