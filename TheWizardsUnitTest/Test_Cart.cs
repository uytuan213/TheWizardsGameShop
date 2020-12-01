using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using TheWizardsGameShop;
using TheWizardsGameShop.Controllers;
using TheWizardsGameShop.Models;
using Xunit;

namespace TheWizardsUnitTest
{
    public class Test_Cart
    {
        TheWizardsGameShopContext _context = new TheWizardsGameShopContext();
        List<CartItem> cartItems;

        private void InitializeCartItems()
        {
            cartItems = new List<CartItem>()
            {
                new CartItem()
                {
                    Game = new Game() { GameId = 1, GamePrice = 59 },
                    Quantity = 1,
                    IsDigital = true
                },
                new CartItem()
                { 
                    Game = new Game() { GameId = 2, GamePrice = 29 },
                    Quantity = 2,
                    IsDigital = false
                }
            };
        }

        [Fact]
        public void TestTotalPrice_GivenGameItems_ShouldEqual()
        {
            // Arrange
            InitializeCartItems();
            CartItem item1 = cartItems[0];
            CartItem item2 = cartItems[1];
            CartsController controller = new CartsController(_context);
            Game game1 = _context.Game.FirstOrDefault(g => g.GameId == cartItems[0].Game.GameId);
            Game game2 = _context.Game.FirstOrDefault(g => g.GameId == cartItems[1].Game.GameId);

            // Act
            decimal expect = Math.Round(item1.Game.GamePrice * item1.Quantity + item2.Game.GamePrice * item2.Quantity);
            decimal result = Math.Round(game1.GamePrice * item1.Quantity + game2.GamePrice * item2.Quantity);

            // Assert
            Assert.Equal(expect, result);
        }
    }
}
