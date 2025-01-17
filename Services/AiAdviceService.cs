using CsvHelper.Configuration;
using CsvHelper;
using CustyUpBankingApi.Models.Entities;
using CustyUpBankingApi.Repo;
using OpenAI.Interfaces;
using OpenAI.ObjectModels.RequestModels;
using System.Globalization;
using CustyUpBankingApi.Services.Interfaces;

namespace CustyUpBankingApi.Services
{
    public class AiAdviceService : IAiAdviceService
    {
        IOpenAIService _openAiService;
        IMegaDbRepo _megaDbRepo;

        public AiAdviceService(IOpenAIService openAiService, IMegaDbRepo megaDbRepo)
        {
            _openAiService = openAiService;
            _megaDbRepo = megaDbRepo;
        }

        public async Task<string> GetAdviceOnFinance()
        {
            string systemPrompt = $"""
                You are a financial advice giver, you will be given a csv synopsis of my spending and give me kind helpful synopsis on it.

                Try to respond factually but also very kindly as if I overspend I might feel sensitive about it.
                """;

            string fromUserString = $"""
                <WeeklySpendingHistory>{await GetWeeklySpending()}</WeeklySpendingHistory>
                <LastWeekSpending>{await GetLastWeekSpend()}</LastWeekSpending>
                """;
            var completionResult = await _openAiService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
            {
                Messages = new List<ChatMessage>
                {
                    ChatMessage.FromSystem(systemPrompt),
                    ChatMessage.FromUser(fromUserString)
                },
                Model = OpenAI.ObjectModels.Models.Gpt_4o,
            });

            return completionResult.Choices.First().Message.Content;
        }

        private async Task<string> GetWeeklySpending()
        {
            var weeklySpending = await _megaDbRepo.GetWeeklySpending();
            using (var writer = new StringWriter())
            using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                csv.WriteRecords(weeklySpending);
                return writer.ToString();
            }
        }

        private async Task<string> GetLastWeekSpend()
        {
            var lastWeekSpend = await _megaDbRepo.GetLastWeekSpend();
            using (var writer = new StringWriter())
            using (var csv = new CsvWriter(writer, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                csv.WriteRecords(lastWeekSpend);
                return writer.ToString();
            }
        }
    }
}
