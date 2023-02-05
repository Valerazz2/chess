using Newtonsoft.Json;

namespace Chess.Model
{
    public class DeskObj
    {
        public Desk Desk { get;}

        public DeskObj(Desk getDesk)
        {
            Desk = getDesk;
        }
    }
}