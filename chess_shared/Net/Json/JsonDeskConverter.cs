using System.Net.NetworkInformation;
using Chess.Model;
using Model;
using Newtonsoft.Json;

namespace Net.Json
{
    public class JsonDeskConverter : JsonConverterGeneric<Desk>
    {
        protected override void WriteJson(JsonWriter writer, Desk value, JsonSerializer serializer)
        {
            writer.WriteStartObject();
            writer.WritePropertyName(nameof(value.Pieces));
            writer.WriteStartArray();
            foreach(var piece in value.GetAllPiece())
            {
                serializer.Serialize(writer, piece.Color.ToChar().ToString() + piece.GetPieceType().ToChar() + piece.Square.GetRef());
            }
            writer.WriteEndArray();
            writer.WritePropertyName(nameof(value.BlackPlayer));
            serializer.Serialize(writer, value.BlackPlayer);
            writer.WritePropertyName(nameof(value.WhitePlayer));
            serializer.Serialize(writer, value.WhitePlayer);
            writer.WritePropertyName(nameof(value.Move));
            serializer.Serialize(writer, value.Move);
            writer.WriteEndObject();
        }

        protected override Desk ReadJson(JsonReader reader, Desk value, JsonSerializer serializer)
        {
            reader.Read();
            value.Pieces = new ObservableList<Piece>();
            while (reader.Value != null)
            {
                var name = (string)reader.Value;
                reader.Read();
                switch (name)
                {
                    case nameof(value.BlackPlayer) :
                        value.BlackPlayer = serializer.Deserialize<Player>(reader);
                        break;
                    case nameof(value.WhitePlayer) :
                        value.WhitePlayer = serializer.Deserialize<Player>(reader);
                        break;
                    case nameof(value.Move) :
                        value.Move = serializer.Deserialize<ChessColor>(reader);
                        break;
                    case nameof(value.Pieces) :
                        while (true)
                        {
                            reader.Read();
                            var pieceStr = (string)reader.Value;
                            if (string.IsNullOrEmpty(pieceStr)) break;
                            
                            var color = pieceStr[0] == 'w' ? ChessColor.White : ChessColor.Black;
                            var pieceType = PieceTypeEx.GetPieceTypeByChar(pieceStr[1]);
                            var square = value.GetSquareAt(pieceStr[2] + pieceStr[3].ToString());
                            
                            var piece = pieceType.GetNewPieceByType(value);
                            piece.Color = color;
                            piece.Square = square;
                            value.Pieces.Add(piece);
                        }
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }

            return value;
        }
    }
}