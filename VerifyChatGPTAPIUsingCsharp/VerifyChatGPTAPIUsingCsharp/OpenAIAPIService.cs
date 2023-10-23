using System;
using System.Text.Json;
using OpenAI.GPT3;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels;
using OpenAI.GPT3.ObjectModels.RequestModels;

namespace VerifyChatGPTAPIUsingCsharp
{
    /// <summary>
    /// OpenAIのAPIに接続するサービスクラス
    /// </summary>
    public sealed class OpenAIAPIService
    {
        // APIを記載する
        const string APIKey = "";　
        private readonly OpenAIService _openAIService;
        public OpenAIAPIService()
        {
            _openAIService = new OpenAIService(new OpenAiOptions
            {
                ApiKey = APIKey
            });
        }

        #region ListModelAsync
        /// <summary>
        /// モデル一覧を取得します
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async ValueTask ListModelAsync(CancellationToken cancellationToken = default)
        {
            var response = await _openAIService.Models.ListModel(cancellationToken);
            if (!response.Successful)
            {
                Console.WriteLine($"Error:{response.Error?.Message}");
            }
            var ids = response.Models.Select(x => x.Id).OrderBy(static x => x);
            Console.WriteLine(string.Join("\n", ids));
        }
        #endregion

        #region RetrieveModelAsync
        /// <summary>
        /// モデルを取得します。
        /// </summary>
        /// <param name="model">モデル名</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async ValueTask RetrieveModelAsync(string model, CancellationToken cancellationToken = default)
        {
            var response = await _openAIService.Models.RetrieveModel(model, cancellationToken);
            if (!response.Successful)
            {
                Console.WriteLine($"Error:{response.Error?.Message}");
            }

            var options = JsonSerializerOptionsProvider.Default;
            var json = JsonSerializer.Serialize(response, options);
            Console.WriteLine(json);
        }
        #endregion

        #region CreateCompletionAsync
        /// <summary>
        /// ChatGptから会話形式で質問の回答を受け取ります。
        /// </summary>
        /// <param name="systemPrompt">どう振る舞ってほしいかの指示</param>
        /// <param name="question">質問内容</param>
        /// <param name="messages">チャットのメッセージ履歴</param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async ValueTask CreateCompletionAsync(string systemPrompt, string question, IList<ChatMessage>? messages = null, CancellationToken cancellationToken = default)
        {
            messages ??= new List<ChatMessage>
            {
                ChatMessage.FromSystem(systemPrompt),
                ChatMessage.FromUser(question)
            };

            var response = await _openAIService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
            {
                Messages = messages,
                Model = Models.ChatGpt3_5Turbo,
                N = 1, //返答するパターン数を指定できる
                User = Guid.NewGuid().ToString()
            }, cancellationToken: cancellationToken);

            if (!response.Successful)
            {
                Console.WriteLine($"Error:{response.Error?.Message}");
            }

            var choice = response.Choices.First();
            var content = choice.Message.Content;
            messages.Add(ChatMessage.FromAssistant(content));

            Console.WriteLine(content);
            var input = Console.ReadLine();

            if (string.IsNullOrEmpty(input)) return;
            messages.Add(ChatMessage.FromUser(input));

            // 会話形式にするため、再帰的に呼び出します。
            await CreateCompletionAsync(systemPrompt, question, messages, cancellationToken);
        }
        #endregion
    }
}

