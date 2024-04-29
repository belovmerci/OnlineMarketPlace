using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineMarketPlace
{
    public class Order
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public DateTime CreationDate { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerContactInfo { get; set; }

        // Additional properties for related entities
        public List<Product> OrderItems { get; set; }
        public List<PickUpPoint> PickUpPoints { get; set; }
    }
}