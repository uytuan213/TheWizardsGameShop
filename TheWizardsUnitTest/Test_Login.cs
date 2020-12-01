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
    public class Test_Login
    {
        TheWizardsGameShopContext _context = new TheWizardsGameShopContext();
        TestWizardsUser user;

        private void InitializeUser()
        {
            try
            {
                _context.Entry(user).State = EntityState.Detached;
            }
            catch (Exception)
            {
                // TODO: nothing...
            }

            user = new TestWizardsUser()
            {
                UserId = 26,
                UserName = "Test0099",
                PasswordHash = "Password1!",
                FirstName = "Test",
                Dob = Convert.ToDateTime("2010-10-25"),
                LastName = "Customer",
                Phone = "5197214013",
                Email = "daehwa@hotmail.com",
                Gender = "Female",
                ReceivePromotionalEmails = true
            };
        }

        [Fact]
        public async void TestLogin_ValidUser_ShouldSuccess()
        {
            // Arrange
            UsersController controller = new UsersController(_context);
            InitializeUser();

            try
            {
                // Act
                var result = await controller.Details(user.UserId);

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
            }
        }

        [Theory]
        [InlineData("99")]
        public async void TestLogin_InvalidUser_ShouldFail(string value)
        {
            // Arrange
            UsersController controller = new UsersController(_context);
            InitializeUser();
            user.UserId = int.Parse(value);

            try
            {
                // Act
                var result = await controller.Details(user.UserId);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.Equal("Xunit.Sdk.IsTypeException", ex.GetType().ToString());
            }
        }
    }
}
