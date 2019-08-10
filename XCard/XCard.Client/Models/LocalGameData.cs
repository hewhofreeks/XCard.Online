using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace XCard.Client.Models
{
    public class LocalGameData
    {
        public string Name { get; set; }

        public Guid SessionID { get; set; }

        public DateTime AddedOn { get; set; }
    }
}
