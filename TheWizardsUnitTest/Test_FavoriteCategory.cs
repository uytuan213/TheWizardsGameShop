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
    public class Test_FavoriteCategory
    {
        TheWizardsGameShopContext _context = new TheWizardsGameShopContext();
        TheWizardsUnitTestContext _testContext = new TheWizardsUnitTestContext();
        FavoriteCategory favoriteCategory;

        private void InitializeFavoriteCategory()
        {
            try
            {
                _context.Entry(favoriteCategory).State = EntityState.Detached;
            }
            catch (Exception)
            {
                // TODO: nothing...
            }

            favoriteCategory = new FavoriteCategory()
            {
                UserId = 26,
                GameCategoryId = 1
            };
        }

        [Fact]
        public void TestCreate_ValidFavoriteCategory_ShouldSuccess()
        {
            // Arrange
            InitializeFavoriteCategory();

            // Act
            _testContext.FavoriteCategory.Add(favoriteCategory);

            // Assert
<<<<<<< HEAD
            _testContext.EFValidation();
=======
            //_context.EFValidation();
>>>>>>> abb706896c0972368724a52409ae80583b5c947c
        }

        [Fact]
        public async void TestCreate_InvalidFavoriteCategory_ShouldFail()
        {
            // Arrange
            FavoriteCategoriesController controller = new FavoriteCategoriesController(_context);
            InitializeFavoriteCategory();
            favoriteCategory.UserId = 99;
            favoriteCategory.GameCategoryId = 99;

            try
            {
                // Act
                var result = await controller.Create(favoriteCategory);

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
        public void TestEdit_ValidFavoriteCategory_ShouldSuccess()
        {
            // Arrange
            InitializeFavoriteCategory();

            // Act
            _testContext.FavoriteCategory.Update(favoriteCategory);

            // Assert
<<<<<<< HEAD
            _testContext.EFValidation();
=======
            //_context.EFValidation();
>>>>>>> abb706896c0972368724a52409ae80583b5c947c
        }

        [Theory]
        [InlineData("26")]
        public async void TestEdit_InvalidFavoriteCategory_ShouldFail(string value)
        {
            // Arrange
            FavoriteCategoriesController controller = new FavoriteCategoriesController(_context);
            int userId = int.Parse(value);

            // Act
            FavoriteCategory replayCategory = await _context.FavoriteCategory
                .FirstOrDefaultAsync(a => a.UserId == userId);
            replayCategory.GameCategoryId = 0;

            try
            {
                var result = await controller.Edit(replayCategory.UserId, replayCategory);

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
        public void TestDelete_ValidFavoriteCategory_ShouldSuccess()
        {
            // Arrange
            InitializeFavoriteCategory();

            // Act
            _testContext.FavoriteCategory.Remove(favoriteCategory);

            // Assert
<<<<<<< HEAD
            _testContext.EFValidation();
=======
            //_context.EFValidation();
>>>>>>> abb706896c0972368724a52409ae80583b5c947c
        }

        [Theory]
        [InlineData("99,99")]
        public async void TestDelete_InvalidFavoriteCategory_ShouldFail(string value)
        {
            // Arrange
            FavoriteCategoriesController controller = new FavoriteCategoriesController(_context);
            int userId = int.Parse(value.Substring(0, value.IndexOf(",")));
            int categoryId = int.Parse(value.Substring(value.IndexOf(",") + 1,
                value.Length - value.IndexOf(",") - 1));

            try
            {
                // Act
                var result = await controller.DeleteConfirmed(userId, categoryId);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.Equal("System.ArgumentNullException", ex.GetType().ToString());
            }
        }
    }
}
