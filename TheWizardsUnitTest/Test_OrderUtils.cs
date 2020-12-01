using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheWizardsGameShop.Controllers;
using TheWizardsGameShop.Models;
using Xunit;

namespace TheWizardsUnitTest
{
    public class Test_OrderUtils
    {
        TheWizardsGameShopContext _context = new TheWizardsGameShopContext();

        Dictionary<string, string> mockOrder;

        List<Dictionary<string, string>> mockOrderDetails;

        private void InitializeOrderAndDetails()
        {
            mockOrder = new Dictionary<string, string>
            {
                {"OrderId", "1" },
                {"UserId", "26" },
                {"Total", "132.21" },
                {"CreditCardId", "18" },
                {"MailingAddressId", "20" },
                {"ShippingAddressId", "21" },
                {"OrderStatusId", "1" }
            };

            mockOrderDetails = new List<Dictionary<string, string>>()
            {
                new Dictionary<string, string>
                {
                    {"OrderId", "1" },
                    {"GameId", "1" },
                    {"Quantity", "1" },
                    {"IsDigital", "true" }
                },
                new Dictionary<string, string>
                {
                    {"OrderId", "1" },
                    {"GameId", "2" },
                    {"Quantity", "2" },
                    {"IsDigital", "false" }
                },
                new Dictionary<string, string>
                {
                    {"OrderId", "2" },
                    {"GameId", "1" },
                    {"Quantity", "2" },
                    {"IsDigital", "false" }
                }
            };
        }

        [Theory]
        [InlineData("1")]
        public void TestCalculateTotal_GivenOrderDetails_ShouldIs132dot21(string value)
        {
            // Arrange
            InitializeOrderAndDetails();

            // Act
            decimal expect = Convert.ToDecimal(mockOrder["Total"]);
            decimal result = calculateTotal(Convert.ToInt16(value), mockOrderDetails);

            // Assert
            Assert.Equal(Math.Round(expect, 2), Math.Round(result, 2));
        }

        [Theory]
        [InlineData("99")]
        public void TestOrderExist_InvalidOrderId_Rejected(string value)
        {
            int orderId = int.Parse(value);
            OrdersController controller = new OrdersController(_context);
            bool isExist = controller.OrderExists(orderId);
            Assert.Equal("False", isExist.ToString());
        }

        [Theory]
        [InlineData("99,99")]
        public void TestOrderDetailExist_InvalidOrderIdAndGameId_Rejected(string value)
        {
            int orderId = int.Parse(value.Substring(0, value.IndexOf(",")));
            int gameId = int.Parse(value.Substring(value.IndexOf(",") + 1, 
                value.Length - value.IndexOf(",") -1));
            OrderDetailsController controller = new OrderDetailsController(_context);
            bool isExist = controller.OrderExists(orderId, gameId);
            Assert.Equal("False", isExist.ToString());
        }

        private decimal calculateTotal(int orderId, List<Dictionary<string, string>> orderDetails)
        {
            List<Dictionary<string, string>> orders = orderDetails
                .Where(m => m["OrderId"] == orderId.ToString()).ToList();

            List<Game> games = new List<Game>();
            foreach(Dictionary<string, string> d in orders)
            {
                games.Add(_context.Game.FirstOrDefault(g => g.GameId == Convert.ToInt16(d["GameId"])));
            }

            decimal total = 0;

            for(int i = 0; i < games.Count; i++)
            {
                total += games[i].GamePrice * Convert.ToDecimal(orderDetails[i]["Quantity"]);
            }

            total = total * (decimal)(1 + 0.13);
            return total;
        }
    }
}
