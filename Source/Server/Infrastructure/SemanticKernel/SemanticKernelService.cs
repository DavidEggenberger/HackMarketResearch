using Microsoft.Extensions.Configuration;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Memory;
using Server.Features.MarketResearches;
using System;
using System.Collections.Generic;
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
                .AddOpenAIChatCompletion("gpt-4o-mini", configuration["LearningContentsConfiguration:OpenAIApiKey"]);

            kernel = kernelBuilder.Build();
        }

        public async Task<string> Chat(Guid marketResearchId, string userMessage)
        {
            var chat = kernel.GetRequiredService<IChatCompletionService>();

            var chatHistory = new ChatHistory("Du bist ein hilfsbereiter chatbot");
            chatHistory.AddUserMessage(userMessage);

            var messageContent = await chat.GetChatMessageContentAsync(chatHistory, kernel: kernel);

            return messageContent.ToString();
        }
    }
}
