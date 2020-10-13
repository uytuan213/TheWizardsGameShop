using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace TheWizardsUnitTest
{
    public class Test_PhoneValidationAttribute
    {
        [Theory]
        [InlineData("2261231234")]
        [InlineData("226123-1234")]
        [InlineData("226(123)1234")]
        public void Text10Digits_Acceptable(string value)
        {
            Assert.True(Phone_TryFormat(ref value));
        }

        [Theory]
        [InlineData("226123123")]
        [InlineData("234")]
        [InlineData("23-1234")]
        public void TextLessThan10digits_shouldNotBeAccepted(string value)
        {
            Assert.False(Phone_TryFormat(ref value));
        }

        [Theory]
        [InlineData("22612312345")]
        [InlineData("(226) 123-12345")]
        public void TextMoreThan10digits_shouldNotBeAccepted(string value)
        {
            Assert.False(Phone_TryFormat(ref value));
        }

        public static Boolean Phone_TryFormat(ref string phone)
        {
            string newPhone = "";
            foreach (char character in phone)
            {
                if (char.IsDigit(character))
                {
                    newPhone += character;
                }
            }
            if (newPhone.Length == 10)
            {
                phone = newPhone.Insert(4, "-").Insert(7, "-");
                return true;
            }
            else return false;
        }
    }
}
