using Microsoft.AspNetCore.Mvc;
using CustyUpBankingApi.Services.Interfaces;

namespace CustyUpBankingApi.Controllers
{
    [ApiController]
	[Route("[controller]")]
	public class AiAdviceController : ControllerBase
	{
		IAiAdviceService _aiAdviceService;
		private DateOnly yesterday = DateOnly.FromDateTime(DateTime.Today.AddDays(-1));
		public AiAdviceController(IAiAdviceService aiAdviceService)
		{
            _aiAdviceService = aiAdviceService;
		}

		[HttpGet("GetAdviceOnSpending")]
		public async Task<IActionResult> GetAdviceOnSpending()
		{
			return Ok(await _aiAdviceService.GetAdviceOnFinance());
		}
	}
}