using CustyUpBankingApi.Models.Enums;
using CustyUpBankingApi.Services.Interfaces;
using OpenAI.Interfaces;
using OpenAI.Managers;
using OpenAI.ObjectModels.RequestModels;
using System.Text;

namespace CustyUpBankingApi.Services
{
    public class AiService : IAiService
    {
        IOpenAIService _openAiService;

        public AiService(IOpenAIService openAiService)
        {
            _openAiService = openAiService;
        }

        public async Task<Categories> GetCategoryFromTransaction(string description)
        {
            string systemPrompt = $"""
                You are a transaction categorizer, you will be provided a description string and you must match it to one of the these categories
                <Categories>{GetCategoriesEnumAsString()}</Categories>

                You must only respond with the word of the category as it will be directly mapped. If you are unsure make your best guess.

                If you are really unsure set it to BigBuys Category.
                """;

            var completionResult = await _openAiService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
            {
                Messages = new List<ChatMessage>
                {
                    ChatMessage.FromSystem(systemPrompt),
                    ChatMessage.FromUser(description)
                },
                Model = OpenAI.ObjectModels.Models.Gpt_4o,
            });

            var category = Categories.FailedToCategorize;

            Enum.TryParse(completionResult.Choices.First().Message.Content, out category);

            return category;
        }

        private string GetCategoriesEnumAsString()
        {
            var enumValues = Enum.GetValues(typeof(Categories)).Cast<Categories>();

            // Convert enum values to strings and join them with commas
            string categoriesString = string.Join(", ", enumValues.Select(e => e.ToString()));

            return categoriesString;
        }
    }
}
