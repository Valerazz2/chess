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
            
            writer.WritePropertyName("BlackCapturedPieces");
            writer.WriteStartArray();
            foreach (var pieceClone in value.BlackPlayer.capturedPieces.List)
            {
                serializer.Serialize(writer, pieceClone.PieceType.ToChar()  + pieceClone.Count.Value.ToString());
            }
            writer.WriteEndArray();
            writer.WritePropertyName("WhiteCapturedPieces");
            writer.WriteStartArray();
            foreach (var pieceClone in value.WhitePlayer.capturedPieces.List)
            {
                serializer.Serialize(writer, 
                    pieceClone.PieceType.ToChar()  + pieceClone.Count.Value.ToString() + pieceClone.Color.ToChar());
            }
            writer.WriteEndArray();
            
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
                        while (true)
                        {
                            reader.Read();
                            var pieceStr = (string)reader.Value;
                            if (string.IsNullOrEmpty(pieceStr)) break;
                            
                            var pieceType = PieceTypeEx.FromChar(pieceStr[0]);
                            var color = pieceStr[2] == 'w' ? ChessColor.White : ChessColor.Black;
                            var pieceClone = new PieceClone(color, pieceType);
                            pieceClone.Count.Value = pieceStr[1] - '0';
                            value.BlackPlayer.capturedPieces.Add(pieceClone);
                        }
                        reader.Read();
                        break;
                    case "WhiteCapturedPieces" :
                        while (true)
                        {
                            reader.Read();
                            var pieceStr = (string)reader.Value;
                            if (string.IsNullOrEmpty(pieceStr)) break;
                            
                            var pieceType = PieceTypeEx.FromChar(pieceStr[0]);
                            var color = pieceStr[2] == 'w' ? ChessColor.White : ChessColor.Black;
                            var pieceClone = new PieceClone(color, pieceType);
                            pieceClone.Count.Value = pieceStr[1] - '0';
                            value.WhitePlayer.capturedPieces.Add(pieceClone);
                        }
                        reader.Read();
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
                            
                            var piece = pieceType.GetNewPieceByType(value);
                            piece.Color = color;
                            piece.Square = square;
                            value.Pieces.Add(piece);
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
        private void AddArrayToList(ObservableList<PieceClone> playerPieces, ObservableList<PieceClone> deserializedPieces)
        {
            foreach (var piece in deserializedPieces.List)
            {
                playerPieces.Add(piece);
            }
        }
            
    }
}