using Microsoft.AspNetCore.Mvc;
using CustyUpBankingApi.Services.Interfaces;
using CustyUpBankingApi.Models.Requests;

namespace CustyUpBankingApi.Controllers
{
    [ApiController]
	[Route("[controller]")]
	public class BudgetController : ControllerBase
	{
		IBudgetService _budgetService;
		private DateOnly yesterday = DateOnly.FromDateTime(DateTime.Today.AddDays(-1));
		public BudgetController(IBudgetService budgetService)
		{
			_budgetService = budgetService;
		}

		[HttpGet("GetTransactions")]
		public async Task<IActionResult> GetSpendTransactions()
		{
			return Ok(await _budgetService.GetSpendTransactions(null));
		}
		[HttpGet("GetYesterdaysTransactions")]
		public async Task<IActionResult> GetYesterdaysTransactions()
		{
			return Ok(await _budgetService.GetSpendTransactions(yesterday));
		}
		[HttpGet("InsertYesterdaysTransactions")]
		public async Task<IActionResult> InsertYesterdaysTransactions()
		{
			return Ok(await _budgetService.InsertTransactionsFromDate(yesterday));
		}
		[HttpGet("InsertTransactionsForDate")]
		public async Task<IActionResult> InsertTransactionsForDate([FromQuery]DateTime date)
		{
			return Ok(await _budgetService.InsertTransactionsFromDate(DateOnly.FromDateTime(date)));
		}
		[HttpGet("GetCurrentWeeksTransactions")]
		public async Task<IActionResult> GetCurrentWeeksTransactions()
		{
			var result = await _budgetService.GetCurrentWeeksTransactions();
			return Ok(result);
		}

		[HttpGet("GetCurrentWeekBudget")]
		public async Task<IActionResult> GetCurrentWeekBudget()
		{
			var result = await _budgetService.GetCurrentWeekBudget();
			return Ok(result);
		}

        [HttpPost("UpdateBudget")]
        public async Task<IActionResult> UpdateCurrentBudget([FromBody] List<UpdateBudgetRequest> request)
        {
            await _budgetService.UpdateCurrentBudget(request);
			return NoContent();
        }

		[HttpPost("UpdateCategory")]
		public async Task<IActionResult> UpdateCategory([FromBody] UpdateCategoryRequest request)
		{
			await _budgetService.UpdateCategory(request);

			return NoContent();
        }
    }
}