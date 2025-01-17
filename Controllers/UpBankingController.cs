using Microsoft.AspNetCore.Mvc;
using CustyUpBankingApi.Services.Interfaces;

namespace CustyUpBankingApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UpBankingController : ControllerBase
    {
        IUpBankingService _upBankingService;
        public UpBankingController(IUpBankingService upBankingService)
        {
            _upBankingService = upBankingService;
        }

        [HttpGet("TestUpBank")]
        public async Task<IActionResult> PingUpBank()
        {
            return Ok(await _upBankingService.Test());
        }

        [HttpGet("GetTransactions")]
        public async Task<IActionResult> GetTransactions()
        {
            return Ok(await _upBankingService.GetTransactions(null));
        }
    }
}
