using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http.Headers;
using CustyUpBankingApi.Models.UpBanking;
using CustyUpBankingApi.Services.Interfaces;

namespace CustyUpBankingApi.Services
{
    public class UpBankingService : IUpBankingService
    {
        private readonly string apiKey;
        private readonly string baseUrl = "https://api.up.com.au/api/v1/";

        public UpBankingService(IConfiguration configuration)
        {
            apiKey = configuration["UpBankingApiToken"];
        }

        public async Task<bool> Test()
        {
            string url = "util/ping";

            using HttpClient client = CreateUpHttpClient();

            var response = await client.GetAsync(url);

            return response.IsSuccessStatusCode;
        }

        public async Task<UpGetTransactionResponse> GetTransactions(DateOnly? date)
        {
            //This has quite a few filters baked into it, may want to extract them into the function
            string url = "transactions?page[size]=30";
            if (date != null)
            {
                DateOnly nextDate = date.Value.AddDays(1);
                string dateFilterString = $"&filter[since]={date.Value.ToString("yyyy-MM-dd")}T00%3A00%3A00%2B10%3A00&filter[until]={nextDate.ToString("yyyy-MM-dd")}T00%3A00%3A00%2B10%3A00";
                url = url + dateFilterString;
            }

            using HttpClient client = CreateUpHttpClient();

            var response = await client.GetFromJsonAsync<UpGetTransactionResponse>(url); //<List<UpTransaction>>

            //Throw an exception in the case response is null
            if (response == null) throw new NullReferenceException("Failed to deseralize UpGetTransactionResponse from UpBank");

            return response;
        }

        private HttpClient CreateUpHttpClient()
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(baseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            return client;
        }
    }
}
