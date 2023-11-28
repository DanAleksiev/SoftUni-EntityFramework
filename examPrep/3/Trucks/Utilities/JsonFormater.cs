using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Invoices.Extentions
    {
    public static class JsonFormater
        {
        public static string SerializeToJson<T>(this T obj)
            {
            var jsonSerializer = new JsonSerializerSettings()
                {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
                Converters = new List<JsonConverter>()
                    {
                    new StringEnumConverter()
                    }
                };

            string result = JsonConvert.SerializeObject(obj, jsonSerializer);
            return result;
            }

        public static T DeserializeFromJson<T>(this string json)
            {
            var jsonSerializer = new JsonSerializerSettings()
                {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented,
                };

            T result = JsonConvert.DeserializeObject<T>(json, jsonSerializer);
            return result;
            }
        }
    }
