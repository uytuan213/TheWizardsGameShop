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
    public class Test_FavoritePlatform
    {
        TheWizardsGameShopContext _context = new TheWizardsGameShopContext();
        TheWizardsUnitTestContext _testContext = new TheWizardsUnitTestContext();
        FavoritePlatform favoritePlatform;

        private void InitializeFavoritePlatform()
        {
            try
            {
                _context.Entry(favoritePlatform).State = EntityState.Detached;
            }
            catch (Exception)
            {
                // TODO: nothing...
            }

            favoritePlatform = new FavoritePlatform()
            {
                UserId = 26,
                PlatformId = 1
            };
        }

        [Fact]
        public void TestCreate_ValidFavoritePlatform_ShouldSuccess()
        {
            // Arrange
            InitializeFavoritePlatform();

            // Act
            _testContext.FavoritePlatform.Add(favoritePlatform);

            // Assert
            _testContext.EFValidation();
        }

        [Fact]
        public async void TestCreate_InvalidFavoritePlatform_ShouldFail()
        {
            // Arrange
            FavoritePlatformsController controller = new FavoritePlatformsController(_context);
            InitializeFavoritePlatform();
            favoritePlatform.UserId = 99;
            favoritePlatform.PlatformId = 99;

            try
            {
                // Act
                var result = await controller.Create(favoritePlatform);

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
        public void TestEdit_ValidFavoritePlatform_ShouldSuccess()
        {
            // Arrange
            InitializeFavoritePlatform();

            // Act
            _testContext.FavoritePlatform.Update(favoritePlatform);

            // Assert
            _testContext.EFValidation();
        }

        [Theory]
        [InlineData("26")]
        public async void TestEdit_InvalidFavoritePlatform_ShouldFail(string value)
        {
            // Arrange
            FavoritePlatformsController controller = new FavoritePlatformsController(_context);
            int userId = int.Parse(value);

            // Act
            FavoritePlatform replayPlatform = await _context.FavoritePlatform
                .FirstOrDefaultAsync(a => a.UserId == userId);
            replayPlatform.PlatformId = 0;

            try
            {
                var result = await controller.Edit(replayPlatform.UserId, replayPlatform);

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
                Assert.Equal("System.InvalidOperationException", ex.GetType().ToString());
            }
        }

        [Fact]
        public void TestDelete_ValidFavoritePlatform_ShouldSuccess()
        {
            // Arrange
            InitializeFavoritePlatform();

            // Act
            _testContext.FavoritePlatform.Remove(favoritePlatform);

            // Assert
            _testContext.EFValidation();
        }

        [Theory]
        [InlineData("99,99")]
        public async void TestDelete_InvalidFavoritePlatform_ShouldFail(string value)
        {
            // Arrange
            FavoritePlatformsController controller = new FavoritePlatformsController(_context);
            int userId = int.Parse(value.Substring(0, value.IndexOf(",")));
            int platformId = int.Parse(value.Substring(value.IndexOf(",") + 1,
                value.Length - value.IndexOf(",") - 1));

            try
            {
                // Act
                var result = await controller.DeleteConfirmed(userId, platformId);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.Equal("System.ArgumentNullException", ex.GetType().ToString());
            }
        }
    }
}
