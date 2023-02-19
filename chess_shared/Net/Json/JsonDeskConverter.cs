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
            foreach(var piece in value.Pieces.List)
            {
                serializer.Serialize(writer, piece.Color.ToChar().ToString() + piece.GetPieceType().ToChar() + piece.Square.GetRef() + piece.WasMoved.ToString()[0]);
            }
            writer.WriteEndArray();
            
            writer.WritePropertyName("BlackCapturedPieces");
            SerializeCapturedPieces(writer, serializer, value.BlackPlayer);
            
            writer.WritePropertyName("WhiteCapturedPieces");
            SerializeCapturedPieces(writer, serializer, value.WhitePlayer);
            
            writer.WritePropertyName(nameof(value.Move));
            serializer.Serialize(writer, value.Move);
            writer.WriteEndObject();
        }

        protected override Desk ReadJson(JsonReader reader, Desk value, JsonSerializer serializer)
        {
            reader.Read();
            value.Pieces.Clear();
            while (reader.Value != null)
            {
                var name = (string)reader.Value;
                reader.Read();
                switch (name)
                {
                    case "BlackCapturedPieces" :
                        AddCapturedPiecesToPlayer(value.BlackPlayer, reader);
                        break;
                    case "WhiteCapturedPieces" :
                        AddCapturedPiecesToPlayer(value.WhitePlayer, reader);
                        break;
                    case nameof(value.Move) :
                        value.Move = serializer.Deserialize<ChessColor>(reader);
                        reader.Read();
                        break;
                    case nameof(value.Pieces) :
                        while (true)
                        {
                            reader.Read();
                            var pieceStr = (string)reader.Value;
                            if (string.IsNullOrEmpty(pieceStr)) break;
                            
                            var color = pieceStr[0] == 'w' ? ChessColor.White : ChessColor.Black;
                            var pieceType = PieceTypeEx.FromChar(pieceStr[1]);
                            var square = value.GetSquareAt(pieceStr[2] + pieceStr[3].ToString());
                            var wasMoved = pieceStr[4] == 't';
                            var piece = value.AddPiece(color, pieceType, square);
                            piece.WasMoved = wasMoved;
                        }
                        reader.Read();
                        break;
                    default:
                        reader.Skip();
                        break;
                }
            }
            return value;
        }

        private static void AddCapturedPiecesToPlayer(Player player, JsonReader reader)
        {
            while (true)
            {
                reader.Read();
                var pieceStr = (string)reader.Value;
                if (string.IsNullOrEmpty(pieceStr)) break;
                var pieceType = PieceTypeEx.FromChar(pieceStr[0]);
                var color = pieceStr[2] == 'w' ? ChessColor.White : ChessColor.Black;
                var pieceClone = new PieceClone(color, pieceType);
                pieceClone.Count.Value = pieceStr[1] - '0';
                player.capturedPieces.Add(pieceClone);
            }
            reader.Read();
        }

        private static void SerializeCapturedPieces(JsonWriter writer, JsonSerializer serializer, Player player)
        {
            writer.WriteStartArray();
            foreach (var pieceClone in player.capturedPieces.List)
            {
                serializer.Serialize(writer, pieceClone.PieceType.ToChar()  + pieceClone.Count.Value.ToString() + pieceClone.Color.ToChar());
            }
            writer.WriteEndArray();
        }
        
    }
}