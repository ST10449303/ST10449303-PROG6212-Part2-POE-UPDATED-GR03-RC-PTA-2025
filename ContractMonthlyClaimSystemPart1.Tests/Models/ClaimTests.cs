using ContractMonthlyClaimSystemPart1.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractMonthlyClaimSystemPart1.Tests.Models
{
   
    public class ClaimTests
    {
        [Fact]
        public void Amount_Is_Calculated_When_Hours_And_Rate_Set()
        {
            var claim = new Claim();
            claim.Hours = 5;
            claim.HourlyRate = 200;

            Assert.Equal(1000, claim.Amount);
        }

        [Fact]
        public void DisplayTitle_Contains_LecturerName_And_Status()
        {
            var claim = new Claim
            {
                LecturerName = "John Doe",
                StaffNumber = "LEC123",
                Hours = 4,
                HourlyRate = 150,
                Status = ClaimStatus.Verified
            };

            Assert.Contains("John Doe", claim.DisplayTitle);
            Assert.Contains("LEC123", claim.DisplayTitle);
            Assert.Contains("Verified", claim.DisplayTitle);
        }
    }
}
