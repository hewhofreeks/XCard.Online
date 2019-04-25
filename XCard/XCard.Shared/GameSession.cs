using System;
using System.Collections.Generic;
using System.Text;

namespace XCard.Shared
{
    public class GameSession
    {
        public Guid SessionID { get; set; }

        public GameUser Host { get; set; }

        public List<GameUser> CurrentUsers { get; set; }
    }
}
