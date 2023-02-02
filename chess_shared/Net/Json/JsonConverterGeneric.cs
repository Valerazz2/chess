using System;
using chess_shared.Net;
using Newtonsoft.Json;

namespace Net.Json
{
    /// <summary>
    /// JsonConverter extension with generic type
    /// </summary>
    public abstract class JsonConverterGeneric<T> : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var t = (T) value;
            WriteJson(writer, t, serializer);
        }

        protected abstract void WriteJson(JsonWriter writer, T value, JsonSerializer serializer);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            return ReadJson(reader, (T) existingValue, serializer);
        }

        protected abstract T ReadJson(JsonReader reader, T value, JsonSerializer serializer);

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(T);
        }
    }
}