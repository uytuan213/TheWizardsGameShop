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
    public class Test_Rating
    {
        TheWizardsGameShopContext _context = new TheWizardsGameShopContext();
        TheWizardsUnitTestContext _testContext = new TheWizardsUnitTestContext();
        Rating rating;

        private void InitializeRating()
        {
            try
            {
                _context.Entry(rating).State = EntityState.Detached;
            }
            catch (Exception)
            {
                // TODO: nothing...
            }

            rating = new Rating()
            {
                Rate = 3,
                UserId = 26,
                GameId = 1
            };
        }

        [Fact]
        public void TestCreate_ValidRating_ShouldSuccess()
        {
            // Arrange
            InitializeRating();

            // Act
            _testContext.Rating.Add(rating);

            // Assert
<<<<<<< HEAD
            _testContext.EFValidation();
=======
            //_context.EFValidation();
>>>>>>> abb706896c0972368724a52409ae80583b5c947c
        }

        [Fact]
        public async void TestCreate_InvalidRating_ShouldFail()
        {
            // Arrange
            RatingsController controller = new RatingsController(_context);
            InitializeRating();
            rating.GameId = 99;

            try
            {
                // Act
                var result = await controller.Create(rating.GameId, Convert.ToInt16(rating.Rate));

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
        public void TestEdit_ValidRating_ShouldSuccess()
        {
            // Arrange
            InitializeRating();

            // Act
            _testContext.Rating.Update(rating);

            // Assert
<<<<<<< HEAD
            _testContext.EFValidation();
=======
            //_context.EFValidation();
>>>>>>> abb706896c0972368724a52409ae80583b5c947c
        }

        [Theory]
        [InlineData("26")]
        public async void TestEdit_InvalidRating_ShouldFail(string value)
        {
            // Arrange
            RatingsController controller = new RatingsController(_context);
            int userId = int.Parse(value);

            // Act
            Rating replayRating = await _context.Rating
                .FirstOrDefaultAsync(a => a.UserId == userId);
            replayRating.GameId = 0;

            try
            {
                var result = await controller.Edit(replayRating.GameId, Convert.ToInt16(rating.Rate));

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
        public void TestDelete_ValidRating_ShouldSuccess()
        {
            // Arrange
            InitializeRating();

            // Act
            _testContext.Rating.Remove(rating);

            // Assert
<<<<<<< HEAD
            _testContext.EFValidation();
=======
            //_context.EFValidation();
>>>>>>> abb706896c0972368724a52409ae80583b5c947c
        }

        [Theory]
        [InlineData("99")]
        public async void TestDelete_InvalidRating_ShouldFail(string value)
        {
            // Arrange
            RatingsController controller = new RatingsController(_context);
            int gameId = int.Parse(value);

            try
            {
                // Act
                var result = await controller.DeleteConfirmed(gameId);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.Equal("System.ArgumentException", ex.GetType().ToString());
            }
        }
    }
}
