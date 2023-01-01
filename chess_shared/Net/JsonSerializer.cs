using Newtonsoft.Json;

namespace chess_shared.Net
{

    public class JsonSerializer
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
            {TypeNameHandling = TypeNameHandling.Auto};

        public static TObjType DeserializeObj<TObjType>(string args)
        {
            return JsonConvert.DeserializeObject<TObjType>(args, Settings);
        }

        public static string SerializeObj(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented, Settings);
        }
    }
}