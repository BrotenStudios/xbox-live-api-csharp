using Newtonsoft.Json;

namespace System
{
    internal static class JsonSerialization
    {
#if !WINDOWS_UWP
        internal static T FromJson<T>(string jsonInput)
        {
            return JsonConvert.DeserializeObject<T>(jsonInput);
        }

        internal static string ToJson<T>(T input)
        {
            return JsonConvert.SerializeObject(input);
        }
#endif
    }
}
