using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Memory;
using Server.Features.MarketResearches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Server.Infrastructure.SemanticKernel
{
    public class SemanticKernelService
    {
        private readonly Kernel kernel;

        public SemanticKernelService(IConfiguration configuration)
        {
            var kernelBuilder = Kernel
                .CreateBuilder()
                .AddOpenAIChatCompletion("gpt-4o-mini", configuration["LearningContentsConfiguration:OpenAIApiKey"])
                .AddInMemoryVectorStore();

            kernel = kernelBuilder.Build();
        }

        public async Task<ChatMessage> Chat(MarketResearch marketReseach, string userMessage)
        {
            var chat = kernel.GetRequiredService<IChatCompletionService>();

            ChatHistory chatHistory = null;
            ChatMessageContent messageContent = null;
            if (string.IsNullOrEmpty(marketReseach.ProductDescription))
            {
                var chatHistoryString = @"
                    Du bist ein hilfsbereiter chatbot. Das Ziel ist es den Benutzern bei der Marktanalyse zu helfen.
                    Als erstes sollst du abklären was für ein Art von Produkt oder Idee die Nutzenden vermarkten wollen. Bitte halte dich kurz und knapp. Die Zielgruppe ist dabei noch nicht wichtig. Falls du dir sehr sicher bist das Produkt genau zu verstehen bitte antworte im folgenden JSON Format:
                    {
                        ""Produkt"" : ""Kurze Beschreibung der Produktidee""
                    }
                    Falls du nicht sicher bist was für ein produkt sich der user vorstellt darfst du ihn gerne genauere Fragen stellen";

                chatHistory = new ChatHistory(chatHistoryString);

                chatHistory.AddUserMessage(userMessage);

                messageContent = await chat.GetChatMessageContentAsync(chatHistory, kernel: kernel);

                try
                {
                    var deserialized = JsonSerializer.Deserialize<ResultProduktBeschreibung>(messageContent.ToString());
                    marketReseach.ProductDescription = deserialized.Produkt;

                    return new ChatMessage { IsSystem = true, Text = messageContent.ToString() };
                }
                catch (Exception ex)
                {
                    return new ChatMessage { IsSystem = true, Text = messageContent.ToString() };
                }
            }
            else
            {
                var chatHistoryString = @$"
                    Du bist ein hilfsbereiter chatbot. Das Ziel ist es den Benutzern bei der Zielmarksuche für ihr Produkt zu helfen. Das ist ihr Produkt {marketReseach.ProductDescription};
                    Bitte suche Nischenmärkte für das Produkt und halte dich dabei bite kurz und knapp. Bitte gebe das resultat höchstens drei im folgenden JSON zurück:
                    {{
                      ""Vorschläge"": [
                        {{
                          ""Markt"": ""Fitness Gadgets""
                        }},
                        {{
                          ""Markt"": ""Nachhaltige Mode""
                        }},
                        {{
                          ""Markt"": ""Smart Home Geräte""
                        }}
                      ]
                    }}";

                chatHistory = new ChatHistory(chatHistoryString);

                chatHistory.AddUserMessage(userMessage);

                messageContent = await chat.GetChatMessageContentAsync(chatHistory, kernel: kernel);

                try
                {
                    var deserialized = JsonSerializer.Deserialize<List<ResultMarketProposal>>(messageContent.ToString());
                    
                }
                catch (Exception ex)
                {

                }
            }

            return null;
        }
    }
    public class ResultProduktBeschreibung
    {
        public string Produkt { get; set; }
    }

    public class NischenMärkte
    {
        public List<ResultMarketProposal> Vorschläge { get; set; }
    }

    public class ResultMarketProposal
    {
        public string Markt { get; set; }
    }
}
