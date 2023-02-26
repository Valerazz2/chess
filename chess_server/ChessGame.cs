namespace Chess.Model
{
   public class ChessGame
   {
      public readonly Desk Desk;
      public ServerPlayer? PlayerWhite;
      public ServerPlayer? PlayerBlack;

      public ChessGame()
      {
         Desk = new Desk();
         Desk.CreateMap();
      }
      
   }
}
