using System;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Encodings;

namespace VerifyChatGPTAPIUsingCsharp
{
    /// <summary>
    /// Jsonシリアライズのオプション設定クラスです。
    /// </summary>
    public static class JsonSerializerOptionsProvider
    {
        /// <summary>
        /// デフォルト設定
        /// </summary>
        public static JsonSerializerOptions Default { get; } = new()
        {
            WriteIndented = true
        };

    }
}

