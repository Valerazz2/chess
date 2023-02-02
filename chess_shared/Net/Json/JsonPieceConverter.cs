using System;
using Chess.Model;
using Newtonsoft.Json;

namespace Net.Json
{
    public class JsonPieceConverter : JsonConverterGeneric<Piece>
    {
        protected override void WriteJson(JsonWriter writer, Piece value, JsonSerializer serializer)
        {
            var str = value.Color.ToChar().ToString() + value.GetPieceType().ToChar() + value.Square.GetRef();
            serializer.Serialize(writer, str);
        }

        protected override Piece ReadJson(JsonReader reader, Piece value, JsonSerializer serializer)
        {
            throw new System.NotImplementedException();
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(Piece).IsAssignableFrom(objectType);
        }
    }
}