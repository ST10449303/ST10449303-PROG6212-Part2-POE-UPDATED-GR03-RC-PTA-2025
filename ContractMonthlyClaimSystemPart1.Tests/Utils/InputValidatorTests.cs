using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ContractMonthlyClaimSystemPart1.Core;
using System.Threading.Tasks;


namespace ContractMonthlyClaimSystemPart1.Tests.Utils
{
    
    public class InputValidatorTests
    {
        [Fact]
        public void ReturnsFalse_When_Invalid_Hours()
        {
            var ok = InputValidator.ValidateHoursAndRate("abc", "100", out var _, out var _, out var err);

            Assert.False(ok);
            Assert.Equal("Invalid hours.", err);
        }

        [Fact]
        public void ReturnsTrue_When_Valid_Numbers()
        {
            var ok = InputValidator.ValidateHoursAndRate("5", "200", out var hours, out var rate, out var err);

            Assert.True(ok);
            Assert.Equal(5, hours);
            Assert.Equal(200, rate);
            Assert.Null(err);
        }
    }
}
