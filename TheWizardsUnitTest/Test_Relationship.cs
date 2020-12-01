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
    public class Test_Relationship
    {
        TheWizardsGameShopContext _context = new TheWizardsGameShopContext();
        TheWizardsUnitTestContext _testContext = new TheWizardsUnitTestContext();
        Relationship relationship;

        private void InitializeRelationship()
        {
            try
            {
                _context.Entry(relationship).State = EntityState.Detached;
            }
            catch (Exception)
            {
                // TODO: nothing...
            }

            relationship = new Relationship()
            {
                Sender = 26,
                Receiver = 27,
                IsAccepted = true,
                IsFamily = false
            };
        }

        [Fact]
        public void TestCreate_ValidRelationship_ShouldSuccess()
        {
            // Arrange
            InitializeRelationship();

            // Act
            _testContext.Relationship.Add(relationship);

            // Assert
            _testContext.EFValidation();
        }

        [Fact]
        public async void TestCreate_InvalidRelationship_ShouldFail()
        {
            // Arrange
            RelationshipsController controller = new RelationshipsController(_context);
            InitializeRelationship();
            relationship.Receiver = 99;

            try
            {
                // Act
                var result = await controller.Create(relationship);

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
        public void TestEdit_ValidRelationship_ShouldSuccess()
        {
            // Arrange
            InitializeRelationship();

            // Act
            _testContext.Relationship.Update(relationship);

            // Assert
            _testContext.EFValidation();
        }

        [Fact]
        public async void TestEdit_InvalidRelationship_ShouldFail()
        {
            // Arrange
            RelationshipsController controller = new RelationshipsController(_context);

            try
            {
                // Act
                Relationship replayRelationship = await _context.Relationship
                    .FirstOrDefaultAsync(r => r.Sender == 1 && r.Receiver == 2);
                replayRelationship.Receiver = 99;

                var result = await controller.Edit(replayRelationship.Sender, replayRelationship);

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
        public void TestDelete_ValidRelationship_ShouldSuccess()
        {
            // Arrange
            InitializeRelationship();

            // Act
            _testContext.Relationship.Remove(relationship);

            // Assert
            _testContext.EFValidation();
        }

        [Fact]
        public async void TestDelete_InvalidRelationship_ShouldFail()
        {
            // Arrange
            RelationshipsController controller = new RelationshipsController(_context);

            try
            {
                // Act
                var result = await controller.DeleteConfirmed(99);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.Equal("System.ArgumentException", ex.GetType().ToString());
            }
        }
    }
}
