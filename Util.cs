using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace user_service
{
    public static class Util
    {
        public static string FormatToJsonBody(object o, JsonNamingPolicy namingPolicy = null, List<JsonConverter> converters = null)
        {
            return JsonSerializer.Serialize(o, GetCommonJsonSerilzeOptions(namingPolicy, converters));
        }

        public static JsonSerializerOptions GetCommonJsonSerilzeOptions(JsonNamingPolicy namingPolicy = null, List<JsonConverter> converters = null)
        {
            JsonSerializerOptions options = new JsonSerializerOptions
            {
                IgnoreNullValues = true,
                AllowTrailingCommas = true,
                PropertyNameCaseInsensitive = true,
                WriteIndented = true,
                PropertyNamingPolicy = namingPolicy ?? JsonNamingPolicy.CamelCase,
                DictionaryKeyPolicy = namingPolicy ?? JsonNamingPolicy.CamelCase,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };
            converters?.ForEach(c => options.Converters.Add(c));
            return options;
        }
    }
}
