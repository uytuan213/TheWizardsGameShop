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
    public class Test_WizardsUser
    {
        TheWizardsGameShopContext _context = new TheWizardsGameShopContext();
        TheWizardsUnitTestContext _testContext = new TheWizardsUnitTestContext();
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
        public void TestCreate_ValidUser_ShouldSuccess()
        {
            // Arrange
            InitializeUser();

            // Act
            _testContext.TestWizardsUser.Add(user);

            // Assert
            _testContext.EFValidation();
        }

        [Theory]
        [InlineData("")]
        [InlineData("User")]
        [InlineData("User_name_over_twenty_length")]
        [InlineData("TestUser@!")]
        public void TestCreate_InvalidUserName_ShouldFail(string value)
        {
            // Arrange
            InitializeUser();
            user.UserName = value;

            // Act
            _testContext.TestWizardsUser.Add(user);

            // Assert
            Assert.ThrowsAny<Exception>(() => _testContext.EFValidation());
        }

        [Theory]
        [InlineData("")]
        [InlineData("Password")]
        [InlineData("password1")]
        public void TestCreate_InvalidPassword_ShouldFail(string value)
        {
            // Arrange
            InitializeUser();
            user.PasswordHash = value;

            // Act
            _testContext.TestWizardsUser.Add(user);

            // Assert
            Assert.ThrowsAny<Exception>(() => _testContext.EFValidation());
        }

        [Theory]
        [InlineData("")]
        [InlineData("123123123")]
        [InlineData("12312312345")]
        [InlineData("123-123-123")]
        public void TestCreate_InvalidPhone_ShouldFail(string value)
        {
            // Arrange
            InitializeUser();
            user.Phone = value;

            // Act
            _testContext.TestWizardsUser.Add(user);

            // Assert
            Assert.ThrowsAny<Exception>(() => _testContext.EFValidation());
        }

        [Theory]
        [InlineData("")]
        [InlineData("@hotmail.com")]
        [InlineData("test@.com")]
        [InlineData("test@hotmail.")]
        public void TestCreate_InvalidEmail_ShouldFail(string value)
        {
            // Arrange
            InitializeUser();
            user.Email = value;

            // Act
            _testContext.TestWizardsUser.Add(user);

            // Assert
            Assert.ThrowsAny<Exception>(() => _testContext.EFValidation());
        }

        [Fact]
        public void TestEdit_ValidUser_ShouldSuccess()
        {
            // Arrange
            InitializeUser();

            // Act
            _testContext.TestWizardsUser.Update(user);

            // Assert
            _testContext.EFValidation();
        }

        [Theory]
        [InlineData("26")]
        public async void TestEdit_InvalidUser_ShouldFail(string value)
        {
            // Arrange
            UsersController controller = new UsersController(_context);
            int userId = int.Parse(value);

            // Act
            WizardsUser replayUser = await _context.WizardsUser
                .FirstOrDefaultAsync(a => a.UserId == userId);
            replayUser.Phone = "";
            replayUser.Email = "";

            try
            {
                var result = await controller.Edit(replayUser.UserId, replayUser);

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
                Assert.Equal("Xunit.Sdk.IsTypeException", ex.GetType().ToString());
            }
        }

        [Fact]
        public void TestDelete_ValidUser_ShouldSuccess()
        {
            // Arrange
            InitializeUser();

            // Act
            _testContext.TestWizardsUser.Remove(user);

            // Assert
            _testContext.EFValidation();
        }

        [Theory]
        [InlineData("99")]
        public async void TestDelete_InvalidUser_ShouldFail(string value)
        {
            // Arrange
            UsersController controller = new UsersController(_context);
            int userId = int.Parse(value);

            try
            {
                // Act
                var result = await controller.DeleteConfirmed(userId);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.Equal("System.ArgumentNullException", ex.GetType().ToString());
            }
        }
    }
}
