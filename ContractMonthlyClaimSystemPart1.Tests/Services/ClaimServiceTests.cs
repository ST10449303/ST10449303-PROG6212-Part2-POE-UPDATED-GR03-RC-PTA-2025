
using ContractMonthlyClaimSystemPart1.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace ContractMonthlyClaimSystemPart1.Tests.Services
{
  
    public class ClaimServiceTests
    {
        [Fact]
        public void CreateClaim_Adds_Claim_To_Store()
        {
            var store = new ObservableCollection<Claim>();
            var service = new ClaimService(store);

            var claim = service.CreateClaim("Jane", "LEC001", 3, 100);

            Assert.Single(store);
            Assert.Equal("Jane", claim.LecturerName);
            Assert.Equal(300, claim.Amount);
            Assert.Equal(ClaimStatus.PendingVerification, claim.Status);
        }

        [Fact]
        public void Approve_Claim_Sets_Status_Approved()
        {
            var store = new ObservableCollection<Claim>();
            var service = new ClaimService(store);
            var claim = service.CreateClaim("Sam", "LEC002", 2, 200);

            service.ApproveClaim(claim, "Manager");

            Assert.Equal(ClaimStatus.Approved, claim.Status);
            Assert.Equal("Manager", claim.ApprovedBy);
        }

        [Fact]
        public void RejectClaim_Sets_Status_Rejected()
        {
            var store = new ObservableCollection<Claim>();
            var service = new ClaimService(store);
            var claim = service.CreateClaim("Alex", "LEC003", 5, 120);

            service.RejectClaim(claim, "Incorrect hours");

            Assert.Equal(ClaimStatus.Rejected, claim.Status);
            Assert.Equal("Incorrect hours", claim.Notes);
        }

    }
}
