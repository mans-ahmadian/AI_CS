using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace ConsoleApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var client = new HttpClient
            {
                Timeout = TimeSpan.FromMinutes(60), // Increase as needed
                BaseAddress = new Uri("http://localhost:11434")
            };
            var builder = Kernel.CreateBuilder();
            builder.AddOllamaChatCompletion("PHI4:Latest",  httpClient: client);

            var kernel = builder.Build();
            var chatService = kernel.GetRequiredService<IChatCompletionService>();

            var history = new ChatHistory();
            history.AddSystemMessage("You are a helpful assistant.");
            string userMessage = "why the sky is blue, present detail description in around 3000 words";
            history.AddUserMessage(userMessage);

            var response = chatService.GetChatMessageContentAsync(history).GetAwaiter().GetResult();

            Console.WriteLine($"Bot: {response.Content}");

            history.AddMessage(response.Role, response.Content ?? string.Empty);

        }
    }
}
