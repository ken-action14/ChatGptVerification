namespace VerifyChatGPTAPIUsingCsharp;
class Program
{
    static async Task Main(string[] args)
    {
        var openAIAPIService = new OpenAIAPIService();
        // await openAIAPIService.ListModelAsync();
        //await openAIAPIService.RetrieveModelAsync("gpt-3.5-turbo");
        Console.WriteLine("質問を入力してください。");
        var systemPrompt = "あなたは優秀なアシスタントです。";
        var firstQuestion = Console.ReadLine();
        if (string.IsNullOrEmpty(firstQuestion))
        {
            Console.WriteLine("質問が入力されなかったので、処理を修正します。");
        }
        else
        {
            await openAIAPIService.CreateCompletionAsync(systemPrompt, firstQuestion);
        }
    }
}

