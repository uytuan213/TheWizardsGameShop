using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TheWizardsGameShop.Controllers;
using TheWizardsGameShop.Models;
using Xunit;

namespace TheWizardsUnitTest
{
    public class Test_Address
    {
        TheWizardsGameShopContext _context = new TheWizardsGameShopContext();
        Address address;

        private void InitializeAddress()
        {
            try
            {
                _context.Entry(address).State = EntityState.Detached;
            }
            catch (Exception)
            {
                // TODO: nothing...
            }

            address = new Address()
            {
                AddressId = 100,
                UserId = 99,
                Street1 = "123 King Street",
                Street2 = "",
                City = "Waterloo",
                ProvinceCode = "ON",
                PostalCode = "N2A 2K5",
                AddressTypeId = 3
            };
        }

        [Fact]
        public void TestCreate_ValidAddress_ShouldSuccess()
        {
            // Arrange
            InitializeAddress();

            // Act
            _context.Address.Add(address);

            // Assert
            _context.EFValidation();
        }

        [Theory]
        [InlineData("")]
        public void TestCreate_InvalidProvinceCode_ShouldFail(string value)
        {
            // Arrange
            InitializeAddress();
            address.ProvinceCode = value;

            // Act
            _context.Address.Add(address);

            // Assert
            Assert.ThrowsAny<Exception>(() => _context.EFValidation());
        }

        [Theory]
        [InlineData("")]
        [InlineData("N2A 1K")]
        [InlineData("12345")]
        [InlineData("ABC 1K1")]
        public void TestCreate_InvalidPostalCode_ShouldFail(string value)
        {
            // Arrange
            InitializeAddress();
            address.PostalCode = value;

            // Act
            _context.Address.Add(address);

            // Assert
            Assert.ThrowsAny<Exception>(() => _context.EFValidation());
        }

        [Fact]
        public void TestEdit_ValidAddress_ShouldSuccess()
        {
            // Arrange
            InitializeAddress();

            // Act
            _context.Address.Update(address);

            // Assert
            _context.EFValidation();
        }

        [Theory]
        [InlineData("11")]
        public async void TestEdit_InvalidAddress_ShouldFail(string value)
        {
            // Arrange
            AddressesController controller = new AddressesController(_context);
            int addressId = int.Parse(value);

            // Act
            Address replayAddress = await _context.Address
                .FirstOrDefaultAsync(a => a.AddressId == addressId);
            replayAddress.ProvinceCode = "green";

            try
            {
                var result = await controller.Edit(replayAddress.AddressId, replayAddress);

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
        public void TestDelete_ValidAddress_ShouldSuccess()
        {
            // Arrange
            InitializeAddress();

            // Act
            _context.Address.Remove(address);

            // Assert
            _context.EFValidation();
        }

        [Theory]
        [InlineData("99")]
        public async void TestDelete_InvalidAddress_ShouldFail(string value)
        {
            // Arrange
            AddressesController controller = new AddressesController(_context);
            int addressId = int.Parse(value);

            try
            {
                // Act
                var result = await controller.DeleteConfirmed(addressId);
            }
            // Assert
            catch (Exception ex)
            {
                Assert.Equal("System.ArgumentNullException", ex.GetType().ToString());
            }
        }


        [Theory]
        [InlineData("3")]
        public void TestExist_InvalidAddress_ShouldFail(string value)
        {
            int addressId = int.Parse(value);
            AddressesController controller = new AddressesController(_context);
            bool isExist = controller.AddressExists(addressId);
            Assert.Equal("False", isExist.ToString());
        }

        [Theory]
        [InlineData("11")]
        public void TestExist_ValidAddress_ShouldSuccess(string value)
        {
            int addressId = int.Parse(value);
            AddressesController controller = new AddressesController(_context);
            bool isExist = controller.AddressExists(addressId);
            Assert.Equal("True", isExist.ToString());
        }
    }
}
