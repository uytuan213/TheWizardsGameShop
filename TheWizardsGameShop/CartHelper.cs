using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TheWizardsGameShop
{
    public static class CartHelper
    {
        private const string SESSION_CART = "cart";
        public static List<CartItem> getCartFromSession(Controller ctr)
        {
            var str = ctr.HttpContext.Session.GetString(SESSION_CART);
            return  str != null ?
                    JsonConvert.DeserializeObject<List<CartItem>>(str) :
                    new List<CartItem>();
        }

        public static int countItemsInCart(Controller ctr)
        {
            var str = ctr.HttpContext.Session.GetString(SESSION_CART);
            var cart = str != null ?
                    JsonConvert.DeserializeObject<List<CartItem>>(str) :
                    new List<CartItem>();
            return cart.Sum(c => c.Quantity);
        }
    }
}
