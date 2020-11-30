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
    public class Test_Review
    {
        TheWizardsGameShopContext _context = new TheWizardsGameShopContext();
        TheWizardsUnitTestContext _testContext = new TheWizardsUnitTestContext();
        Review review;

        private void InitializeReview()
        {
            try
            {
                _context.Entry(review).State = EntityState.Detached;
            }
            catch (Exception)
            {
                // TODO: nothing...
            }

            review = new Review()
            {
                ReviewId = 1,
                UserId = 26,
                GameId = 1,
                ReviewContent = "Good game.",
                ReviewDate = System.DateTime.Now,
                IsPublished = true
            };
        }

        [Fact]
        public void TestCreate_ValidReview_ShouldSuccess()
        {
            // Arrange
            InitializeReview();

            // Act
            _testContext.Review.Add(review);

            // Assert
            _testContext.EFValidation();
        }

        [Fact]
        public async void TestCreate_InvalidReview_ShouldFail()
        {
            // Arrange
            ReviewsController controller = new ReviewsController(_context);
            InitializeReview();
            review.GameId = 99;

            try
            {
                // Act
                var result = await controller.Create(review);

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
        public void TestEdit_ValidReview_ShouldSuccess()
        {
            // Arrange
            InitializeReview();

            // Act
            _testContext.Review.Update(review);

            // Assert
            _testContext.EFValidation();
        }

        [Theory]
        [InlineData("26")]
        public async void TestEdit_InvalidReview_ShouldFail(string value)
        {
            // Arrange
            ReviewsController controller = new ReviewsController(_context);
            int userId = int.Parse(value);

            try
            {
                // Act
                Review replayReview = await _context.Review
                    .FirstOrDefaultAsync(a => a.UserId == userId);
                replayReview.GameId = 0;

                var result = await controller.Edit(replayReview.GameId, replayReview);

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
        public void TestDelete_ValidReview_ShouldSuccess()
        {
            // Arrange
            InitializeReview();

            // Act
            _testContext.Review.Remove(review);

            // Assert
            _testContext.EFValidation();
        }

        [Theory]
        [InlineData("99")]
        public async void TestDelete_InvalidReview_ShouldFail(string value)
        {
            // Arrange
            ReviewsController controller = new ReviewsController(_context);
            int gameId = int.Parse(value);

            try
            {
                // Act
                var result = await controller.DeleteConfirmed(gameId);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.Equal("System.ArgumentNullException", ex.GetType().ToString());
            }
        }
    }
}
