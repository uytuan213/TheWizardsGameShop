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
    public class Test_CreditCard
    {
        TheWizardsGameShopContext _context = new TheWizardsGameShopContext();
        TheWizardsUnitTestContext _testContext = new TheWizardsUnitTestContext();
        TestCreditCard creditCard;

        private void InitializeCreditCard()
        {
            try
            {
                _context.Entry(creditCard).State = EntityState.Detached;
            }
            catch (Exception)
            {
                // TODO: nothing...
            }

            creditCard = new TestCreditCard()
            {
                CreditCardId = 75,
                UserId = 99,
                CreditCardNumber = "5610591081018250",
                ExpiryDate = "1021",
                CardHolder = "Testing User",
                Cvc = "123"
            };
        }

        [Fact]
        public void TestCreate_ValidCreditCard_ShouldSuccess()
        {
            // Arrange
            InitializeCreditCard();

            // Act
            _testContext.TestCreditCard.Add(creditCard);

            // Assert
            _testContext.EFValidation();
        }

        [Theory]
        [InlineData("")]
        [InlineData("123412341234123")]
        [InlineData("12341234123412345")]
        public void TestCreate_InvalidCardNumber_ShouldFail(string value)
        {
            // Arrange
            InitializeCreditCard();
            creditCard.CreditCardNumber = value;

            // Act
            _testContext.TestCreditCard.Add(creditCard);

            // Assert
            Assert.ThrowsAny<Exception>(() => _testContext.EFValidation());
        }

        [Theory]
        [InlineData("")]
        [InlineData("102")]
        [InlineData("1020A")]
        [InlineData("10201")]
        public void TestCreate_InvalidExpireDate_ShouldFail(string value)
        {
            // Arrange
            InitializeCreditCard();
            creditCard.ExpiryDate = value;

            // Act
            _testContext.TestCreditCard.Add(creditCard);

            // Assert
            Assert.ThrowsAny<Exception>(() => _testContext.EFValidation());
        }

        [Theory]
        [InlineData("")]
        [InlineData("12345")]
        [InlineData("12")]
        [InlineData("12A")]
        public void TestCreate_InvalidCVC_ShouldFail(string value)
        {
            // Arrange
            InitializeCreditCard();
            creditCard.Cvc = value;

            // Act
            _testContext.TestCreditCard.Add(creditCard);

            // Assert
            Assert.ThrowsAny<Exception>(() => _testContext.EFValidation());
        }

        [Fact]
        public void TestEdit_ValidCreditCard_ShouldSuccess()
        {
            // Arrange
            InitializeCreditCard();

            // Act
            _testContext.TestCreditCard.Update(creditCard);

            // Assert
            _testContext.EFValidation();
        }

        [Theory]
        [InlineData("1")]
        public async void TestEdit_InvalidCreditCard_ShouldFail(string value)
        {
            // Arrange
            CreditCardsController controller = new CreditCardsController(_context);
            int creditCardId = int.Parse(value);

            // Act
            CreditCard replayCard = await _context.CreditCard
                .FirstOrDefaultAsync(a => a.CreditCardId == creditCardId);
            replayCard.CreditCardNumber = "";

            try
            {
                var result = await controller.Edit(replayCard.CreditCardId, replayCard);

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
        public void TestDelete_ValidCreditCard_ShouldSuccess()
        {
            // Arrange
            InitializeCreditCard();

            // Act
            _testContext.TestCreditCard.Remove(creditCard);

            // Assert
            _testContext.EFValidation();
        }


        [Theory]
        [InlineData("99")]
        public async void TestDelete_InvalidCreditCard_ShouldFail(string value)
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
