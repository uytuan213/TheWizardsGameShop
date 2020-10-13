using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Xunit;

namespace TheWizardsUnitTest
{
    public class Test_DateNotInFutureAttribute
    {
        public class AccessTo_ValidationAttribute : DateNotInFutureAttribute
        {
            public ValidationResult Run_IsValid(object value)
            {
                return IsValid(value, new ValidationContext(new { DisplayName = "The Wizards" }));
            }
        }

        AccessTo_ValidationAttribute validationInstance = new AccessTo_ValidationAttribute();
            
        [Fact]
        public void Today_Acceptable()
        {
            Assert.Equal(ValidationResult.Success, validationInstance.Run_IsValid(DateTime.Today));
        }
    
        [Fact]
        public void DateInFuture_Unacceptable()
        {
            Assert.NotEqual(ValidationResult.Success, validationInstance.Run_IsValid(DateTime.Now.AddDays(7)));
        }

        [Fact]
        public void DateInPast_Acceptable()
        {
            Assert.Equal(ValidationResult.Success, validationInstance.Run_IsValid(DateTime.Now.AddDays(-7)));
        }



    }
}
