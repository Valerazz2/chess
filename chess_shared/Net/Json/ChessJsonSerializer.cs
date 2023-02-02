using System.Collections.Generic;
using Net.Json;
using Newtonsoft.Json;

namespace chess_shared.Net
{

    public class ChessJsonSerializer
    {
        public static readonly JsonSerializerSettings Settings_ = new JsonSerializerSettings
            {TypeNameHandling = TypeNameHandling.Auto};
        
        public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings(){
            Converters = new List<JsonConverter>()
            {
                new JsonPieceConverter()
            },    
            Formatting = Formatting.Indented,
            TypeNameHandling = TypeNameHandling.Auto
        };

        public static TObjType DeserializeObj<TObjType>(string args)
        {
            return JsonConvert.DeserializeObject<TObjType>(args, Settings);
        }
        
        public static void DeserializeObj<TObjType>(string args, TObjType tagret)
        {
            JsonConvert.PopulateObject(args, tagret);
        }

        public static string SerializeObj(object obj)
        {
            return JsonConvert.SerializeObject(obj, Formatting.Indented, Settings);
        }
    }
}