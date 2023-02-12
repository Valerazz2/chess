using System.Collections.Generic;
using Net.Json;
using Newtonsoft.Json;

namespace chess_shared.Net
{

    public class ChessJsonSerializer
    {
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings(){
            Converters = new List<JsonConverter>()
            {
                new JsonDeskConverter()
            },
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.Auto
        };

        public static TObjType Deserialize<TObjType>(string args)
        {
            return JsonConvert.DeserializeObject<TObjType>(args, Settings);
        }
        
        public static void Populate<TObjType>(string args, TObjType target)
        {
            JsonConvert.PopulateObject(args, target, Settings);
        }

        public static string SerializeObj(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented, Settings);
        }
    }
}