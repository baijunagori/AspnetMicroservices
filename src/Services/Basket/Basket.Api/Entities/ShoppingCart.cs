using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Basket.Api.Entities
{
    public class ShoppingCart
    {
        public string UserName { get; set; }
        public List<ShoppingCartItem> Items { get; set; } = new List<ShoppingCartItem>();

        public ShoppingCart()
        {

        }

        public ShoppingCart(string userName)
        {
            UserName = userName;
        }

        public decimal TotalPrice
        {
            get
            {
                return Items.Sum(i => i.Price * i.Quantity);
                //decimal totalPrice = 0;
                //foreach (var item in Items)
                //{
                //    totalPrice += item.Price * item.Quantity;
                //}
                //return totalPrice;
            }
        }
    }
}
