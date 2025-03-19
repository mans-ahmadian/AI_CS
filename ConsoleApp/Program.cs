using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace ConsoleApp
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = Kernel.CreateBuilder();
            builder.AddOllamaChatCompletion("PHI4:Latest", new Uri("http://localhost:11434"));

            var kernel = builder.Build();
            var chatService = kernel.GetRequiredService<IChatCompletionService>();

            var history = new ChatHistory();
            history.AddSystemMessage("You are a helpful assistant.");
            string? userInput;
            do
            {
                // Collect user input
                Console.Write("User: ");
                userInput = Console.ReadLine();
                if(string.IsNullOrEmpty(userInput))
                {
                    break;
                }
                // Add user input
                history.AddUserMessage(userInput);


                Console.Write("Bot: ");
                string botResponce = "";
                await foreach (var response in chatService.GetStreamingChatMessageContentsAsync(history))
                {
                    Console.Write(response.Content);
                    botResponce += response.Content;
                }
                Console.WriteLine();
                history.AddAssistantMessage(botResponce ?? string.Empty);

            } while (true);
        }
    };
}


