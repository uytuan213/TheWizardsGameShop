using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using TheWizardsGameShop.Controllers;
using TheWizardsGameShop.Models;
using Xunit;

namespace TheWizardsUnitTest
{
    public class Test_WizardsOrder
    {
        TheWizardsGameShopContext _context = new TheWizardsGameShopContext();
        TheWizardsUnitTestContext _testContext = new TheWizardsUnitTestContext();
        WizardsOrder order;

        private void InitializeOrder()
        {
            try
            {
                _context.Entry(order).State = EntityState.Detached;
            }
            catch (Exception)
            {
                // TODO: nothing...
            }

            order = new WizardsOrder()
            {
                OrderId = 1,
                UserId = 26,
                Total = 117m,
                CreditCardId = 18,
                MailingAddressId = 20,
                ShippingAddressId = 21,
                OrderStatusId = 1
            };
        }

        [Fact]
        public void TestCreate_ValidOrder_ShouldSuccess()
        {
            // Arrange
            InitializeOrder();

            // Act
            _testContext.WizardsOrder.Add(order);

            // Assert
            _testContext.EFValidation();
        }

        [Fact]
        public async void TestCreate_InvalidOrder_ShouldFail()
        {
            // Arrange
            OrdersController controller = new OrdersController(_context);
            InitializeOrder();
            order.UserId = 99;

            try
            {
                // Act
                var result = await controller.Create(order);

                // Assert
                Assert.IsType<ViewResult>(result);
                ViewResult viewResult = (ViewResult)result;
                Assert.NotNull(viewResult.ViewData.ModelState);
                Assert.NotEmpty(viewResult.ViewData.ModelState.Keys);

                foreach (string item in viewResult.ViewData.ModelState.Keys)
                {
                    Assert.Equal("", item);
                }
            }
            catch (Exception ex)
            {
                Assert.Equal("Microsoft.EntityFrameworkCore.DbUpdateException", ex.GetType().ToString());
            }
        }

        [Fact]
        public void TestEdit_ValidOrder_ShouldSuccess()
        {
            // Arrange
            InitializeOrder();

            // Act
            _testContext.WizardsOrder.Update(order);

            // Assert
            _testContext.EFValidation();
        }

        [Theory]
        [InlineData("99")]
        public async void TestEdit_InvalidOrder_ShouldFail(string value)
        {
            // Arrange
            OrdersController controller = new OrdersController(_context);
            int userId = int.Parse(value);

            try
            {
                // Act
                WizardsOrder replayOrder = await _context.WizardsOrder
                    .FirstOrDefaultAsync(o => o.UserId == userId);
                replayOrder.OrderStatusId = 9;

                var result = await controller.Edit(replayOrder.OrderId, replayOrder);

                // Assert
                Assert.IsType<ViewResult>(result);
                ViewResult viewResult = (ViewResult)result;
                Assert.NotNull(viewResult.ViewData.ModelState);
                Assert.NotEmpty(viewResult.ViewData.ModelState.Keys);

                foreach (string item in viewResult.ViewData.ModelState.Keys)
                {
                    Assert.Equal("", item);
                }
            }
            catch (Exception ex)
            {
                Assert.Equal("System.NullReferenceException", ex.GetType().ToString());
            }
        }

        [Fact]
        public void TestDelete_ValidOrder_ShouldSuccess()
        {
            // Arrange
            InitializeOrder();

            // Act
            _testContext.WizardsOrder.Remove(order);

            // Assert
            _testContext.EFValidation();
        }

        [Theory]
        [InlineData("99")]
        public async void TestDelete_InvalidOrder_ShouldFail(string value)
        {
            // Arrange
            OrdersController controller = new OrdersController(_context);
            int orderId = int.Parse(value);

            try
            {
                // Act
                var result = await controller.DeleteConfirmed(orderId);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.Equal("System.ArgumentNullException", ex.GetType().ToString());
            }
        }
    }
}
