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
    public class Test_OrderDetail
    {
        TheWizardsGameShopContext _context = new TheWizardsGameShopContext();
        TheWizardsUnitTestContext _testContext = new TheWizardsUnitTestContext();
        OrderDetail orderDetail;

        private void InitializeOrderDetail()
        {
            try
            {
                _context.Entry(orderDetail).State = EntityState.Detached;
            }
            catch (Exception)
            {
                // TODO: nothing...
            }

            orderDetail = new OrderDetail()
            {
                OrderId = 1,
                GameId = 1,
                Quantity = 2,
                IsDigital = false
            };
        }

        [Fact]
        public void TestCreate_ValidOrderDetail_ShouldSuccess()
        {
            // Arrange
            InitializeOrderDetail();

            // Act
            _testContext.OrderDetail.Add(orderDetail);

            // Assert
            _testContext.EFValidation();
        }

        [Fact]
        public async void TestCreate_InvalidOrderDetail_ShouldFail()
        {
            // Arrange
            OrderDetailsController controller = new OrderDetailsController(_context);
            InitializeOrderDetail();
            orderDetail.GameId = 99;

            try
            {
                // Act
                var result = await controller.Create(orderDetail);

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
        public void TestEdit_ValidOrderDetail_ShouldSuccess()
        {
            // Arrange
            InitializeOrderDetail();

            // Act
            _testContext.OrderDetail.Update(orderDetail);

            // Assert
            _testContext.EFValidation();
        }

        [Fact]
        public async void TestEdit_InvalidOrderDetail_ShouldFail()
        {
            // Arrange
            OrderDetailsController controller = new OrderDetailsController(_context);

            try
            {
                // Act
                OrderDetail replayOrderDetail = await _context.OrderDetail
                    .FirstOrDefaultAsync(a => a.OrderId == 20 && a.GameId == 20);
                replayOrderDetail.GameId = 99;

                var result = await controller.Edit(replayOrderDetail.OrderId, replayOrderDetail.GameId, replayOrderDetail);

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
        public void TestDelete_ValidOrderDetail_ShouldSuccess()
        {
            // Arrange
            InitializeOrderDetail();

            // Act
            _testContext.OrderDetail.Remove(orderDetail);

            // Assert
            _testContext.EFValidation();
        }

        [Fact]
        public async void TestDelete_InvalidOrderDetail_ShouldFail()
        {
            // Arrange
            OrderDetailsController controller = new OrderDetailsController(_context);

            try
            {
                // Act
                var result = await controller.DeleteConfirmed(99, 99);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.Equal("System.InvalidOperationException", ex.GetType().ToString());
            }
        }
    }
}
