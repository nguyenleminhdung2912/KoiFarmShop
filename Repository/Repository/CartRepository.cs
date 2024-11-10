using BusinessObject;
using Microsoft.AspNetCore.Http;
using Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Repository.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private const string CartSessionKey = "Cart";

        public CartRepository(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Cart GetCart()
        {
            var session = _httpContextAccessor.HttpContext.Session;
            Cart cart = session.Get<Cart>(CartSessionKey);
            if (cart == null)
            {
                cart = new Cart();
                SaveCart(cart);
            }
            return cart;
        }

        public void SaveCart(Cart cart)
        {
            _httpContextAccessor.HttpContext.Session.Set(CartSessionKey, cart);
        }

        public void AddKoiFish(KoiFish koiFish, int quantity)
        {
            var cart = GetCart();
            var existingItem = cart.KoiFishItems.FirstOrDefault(k => k.Id == koiFish.KoiFishId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.KoiFishItems.Add(new CartItem
                {
                    Id = koiFish.KoiFishId,
                    Name = koiFish.Name,
                    Image = koiFish.ImageData,
                    Price = (double)koiFish.Price,
                    Quantity = quantity,
                    ItemType = "KoiFish"
                });
            }

            SaveCart(cart);
        }

        public void AddProduct(Product product, int quantity)
        {
            var cart = GetCart();
            var existingItem = cart.ProductItems.FirstOrDefault(p => p.Id == product.ProductId);

            if (existingItem != null)
            {
                existingItem.Quantity += quantity;
            }
            else
            {
                cart.ProductItems.Add(new CartItem
                {
                    Id = product.ProductId,
                    Name = product.Name,
                    Image = product.ImageData,
                    Price = (double)product.Price,
                    Quantity = quantity,
                    ItemType = "Product"
                });
            }

            SaveCart(cart);
        }

        public void UpdateQuantity(long id, string itemType, int quantity)
        {
            var cart = GetCart();
            if (itemType == "KoiFish")
            {
                var item = cart.KoiFishItems.FirstOrDefault(k => k.Id == id);
                if (item != null) item.Quantity = quantity;
            }
            else
            {
                var item = cart.ProductItems.FirstOrDefault(p => p.Id == id);
                if (item != null) item.Quantity = quantity;
            }
            SaveCart(cart);
        }

        public void RemoveItem(long id, string itemType)
        {
            var cart = GetCart();
            if (itemType == "KoiFish")
            {
                cart.KoiFishItems.RemoveAll(k => k.Id == id);
            }
            else
            {
                cart.ProductItems.RemoveAll(p => p.Id == id);
            }
            SaveCart(cart);
        }

        public void AddKoiFish(Task<KoiFish?> koiFish, int quantity)
        {
            throw new NotImplementedException();
        }
    }
}
