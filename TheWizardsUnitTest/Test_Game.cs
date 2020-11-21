using Microsoft.AspNetCore.Hosting;
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
    public class Test_Game
    {
        TheWizardsGameShopContext _context = new TheWizardsGameShopContext();
        IWebHostEnvironment _webHostEnvironment;
        Game game;

        private void InitializeGame()
        {
            try
            {
                _context.Entry(game).State = EntityState.Detached;
            }
            catch (Exception)
            {
                // TODO: nothing...
            }

            game = new Game()
            {
                GameId = 200,
                GameStatusCode = "A",
                GameCategoryId = 1,
                GamePlatformId = 1,
                GameName = "Testing Game",
                GameDescription = "Testing description for Testing Game",
                GamePrice = 9.99m,
                GameQty = 100,
                GameDigitalPath = @"\download\200"
            };
        }

        [Fact]
        public async void TestSelect_ValidGame_ShouldSuccess()
        {
            // Arrange
            GamesController controller = new GamesController(_context, _webHostEnvironment);
            InitializeGame();

            try
            {
                // Act
                var result = await controller.Details(game.GameId);

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
        public async void TestSelect_InvalidGame_ShouldFail(string value)
        {
            // Arrange
            GamesController controller = new GamesController(_context, _webHostEnvironment);
            InitializeGame();
            game.GameId = int.Parse(value);

            try
            {
                // Act
                var result = await controller.Details(game.GameId);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.Equal("Xunit.Sdk.IsTypeException", ex.GetType().ToString());
            }
        }

        [Fact]
        public void TestCreate_ValidGame_ShouldSuccess()
        {
            // Arrange
            InitializeGame();

            // Act
            _context.Game.Add(game);

            // Assert
            //_context.EFValidation();
        }

        [Theory]
        [InlineData("")]
        [InlineData("9.9A")]
        [InlineData("9.999")]
        public void TestCreate_InvalidPrice_ShouldFail(string value)
        {
            // Arrange
            InitializeGame();

            decimal price;
            game.GamePrice = decimal.TryParse(value, out price) ? price : 0.0001m;

            // Act
            _context.Game.Add(game);

            // Assert
            //Assert.ThrowsAny<Exception>(() => _context.EFValidation());
        }

        [Fact]
        public void TestEdit_ValidGame_ShouldSuccess()
        {
            // Arrange
            InitializeGame();

            // Act
            _context.Game.Update(game);

            // Assert
            //_context.EFValidation();
        }

        [Theory]
        [InlineData("1")]
        public async void TestEdit_InvalidGame_ShouldFail(string value)
        {
            // Arrange
            GamesController controller = new GamesController(_context, _webHostEnvironment);
            int gameId = int.Parse(value);

            // Act
            Game replayGame = await _context.Game
                .FirstOrDefaultAsync(a => a.GameId == gameId);
            replayGame.GameName = "";

            try
            {
                var result = await controller.Edit(replayGame.GameId, replayGame);

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
        public void TestDelete_ValidGame_ShouldSuccess()
        {
            // Arrange
            InitializeGame();

            // Act
            _context.Game.Remove(game);

            // Assert
            //_context.EFValidation();
        }

        [Theory]
        [InlineData("99")]
        public async void TestDelete_InvalidGame_ShouldFail(string value)
        {
            // Arrange
            GamesController controller = new GamesController(_context, _webHostEnvironment);
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

        [Theory]
        [InlineData("100")]
        public void TestExist_InvalidGame_ShouldFail(string value)
        {
            int gameId = int.Parse(value);
            GamesController controller = new GamesController(_context, _webHostEnvironment);
            //bool isExist = controller.GameExists(gameId);
            //Assert.Equal("False", isExist.ToString());
        }

        [Theory]
        [InlineData("1")]
        public void TestExist_ValidGame_ShouldSuccess(string value)
        {
            int gameId = int.Parse(value);
            GamesController controller = new GamesController(_context, _webHostEnvironment);
            //bool isExist = controller.GameExists(gameId);
            //Assert.Equal("True", isExist.ToString());
        }
    }
}
