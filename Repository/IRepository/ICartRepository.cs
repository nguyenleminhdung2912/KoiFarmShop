using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.IRepository
{
    public interface ICartRepository
    {
        Cart GetCart();
        void SaveCart(Cart cart);
        List<CartItem> GetCartItems();
        void AddKoiFish(KoiFish koiFish, int quantity);
        void AddProduct(Product product, int quantity);
        void UpdateQuantity(long id, string itemType, int quantity);
        void RemoveItem(long id, string itemType); 
        void ClearCart();
    }
}
