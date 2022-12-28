using Chess.Model;
using Chess.Server;

namespace Net
{
    public class EnemyFigureMoved : News
    {
        public string MovedFrom;
        public string MovedTo;
    }
}