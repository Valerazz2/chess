using Newtonsoft.Json;

namespace Chess.Model
{
    public class DeskObj
    {
        [JsonIgnore]
        public Desk Desk { get;}

        protected DeskObj(Desk getDesk)
        {
            Desk = getDesk;
        }
    }
}