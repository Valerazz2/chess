using System;

namespace Chess.Server
{
    public abstract class News
    {
        public string ID = Guid.NewGuid().ToString();
    }
}