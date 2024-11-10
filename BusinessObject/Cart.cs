using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject
{
    public class Cart
    {
        public List<CartItem> KoiFishItems { get; set; } = new List<CartItem>();
        public List<CartItem> ProductItems { get; set; } = new List<CartItem>();

        public int TotalQuantity
        {
            get
            {
                return KoiFishItems.Sum(k => k.Quantity) + ProductItems.Sum(p => p.Quantity);
            }
        }

        public double TotalPrice
        {
            get
            {
                double koiFishTotal = KoiFishItems.Sum(k => k.Price * k.Quantity);
                double productTotal = ProductItems.Sum(p => p.Price * p.Quantity);
                return koiFishTotal + productTotal;
            }
        }
    }
}
