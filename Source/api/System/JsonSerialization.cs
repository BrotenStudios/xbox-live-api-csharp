using Newtonsoft.Json;

namespace Microsoft.Xbox.Services
{
    internal static class JsonSerialization
    {
        internal static T FromJson<T>(string jsonInput)
        {
            return JsonConvert.DeserializeObject<T>(jsonInput);
        }

        internal static string ToJson<T>(T input)
        {
            return JsonConvert.SerializeObject(input);
        }
    }
}
