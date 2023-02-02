using Chess.Model;
using Newtonsoft.Json;

namespace Net.Json
{
    public class JsonDeskConverter : JsonConverterGeneric<Desk>
    {
        protected override void WriteJson(JsonWriter writer, Desk value, JsonSerializer serializer)
        {
            //serializer.Serialize(writer, "Desk");
            writer.WriteStartArray();
            foreach(var piece in value.GetAllPiece())
            {
                serializer.Serialize(writer, piece.GetPieceType());
            }
            writer.WriteEndArray();
        }

        protected override Desk ReadJson(JsonReader reader, Desk value, JsonSerializer serializer)
        {
            throw new System.NotImplementedException();
        }
    }
}