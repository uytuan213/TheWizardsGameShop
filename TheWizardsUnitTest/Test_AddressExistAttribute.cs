using TheWizardsGameShop.Controllers;
using TheWizardsGameShop.Models;
using Xunit;

namespace TheWizardsUnitTest
{
    public class Test_AddressExistAttribute
    {
        TheWizardsGameShopContext _context = new TheWizardsGameShopContext();

        [Theory]
        [InlineData("1")]
        public void InvalidAddressId_Rejected(string value)
        {
            int addressId = int.Parse(value);
            AddressesController controller = new AddressesController(_context);
            bool isExist = controller.AddressExists(addressId);
            Assert.Equal("False", isExist.ToString());
        }

        [Theory]
        [InlineData("21")]
        public void ValidAddressId_Accepted(string value)
        {
            int addressId = int.Parse(value);
            AddressesController controller = new AddressesController(_context);
            bool isExist = controller.AddressExists(addressId);
            Assert.Equal("True", isExist.ToString());
        }
    }
}
