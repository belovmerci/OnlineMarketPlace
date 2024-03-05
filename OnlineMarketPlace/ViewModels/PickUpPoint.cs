using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMarketPlace.ViewModels
{
    public class PickUpPoint
    {
        public int Id { get; set; }
        // not null
        public string Country { get; set; }
        public string City { get; set; }
        // not null
        public string Street { get; set; }
        public string StreetNumber { get; set; }
        public string RoomNumber { get; set; }
        public string PostIndex { get; set; }
        public decimal Rating { get; set; }
    }
}
