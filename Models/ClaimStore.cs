using System.Collections.ObjectModel;
using System.Linq;

namespace ContractMonthlyClaimSystemPart1.Models
{
    public static class ClaimStore
    {
        
        private static readonly ObservableCollection<Claim> _allClaims = new ObservableCollection<Claim>();
        public static ObservableCollection<Claim> AllClaims => _allClaims;

        
        public static ObservableCollection<Claim> PendingClaims =>
            new ObservableCollection<Claim>(_allClaims.Where(c => c.Status == ClaimStatus.PendingVerification));

        public static ObservableCollection<Claim> VerifiedClaims =>
            new ObservableCollection<Claim>(_allClaims.Where(c => c.Status == ClaimStatus.Verified));

        public static ObservableCollection<Claim> ApprovedClaims =>
            new ObservableCollection<Claim>(_allClaims.Where(c => c.Status == ClaimStatus.Approved));

        public static ObservableCollection<Claim> RejectedClaims =>
            new ObservableCollection<Claim>(_allClaims.Where(c => c.Status == ClaimStatus.Rejected));

        public static ObservableCollection<Claim> GetLecturerClaims(string staffNumber) =>
            new ObservableCollection<Claim>(_allClaims.Where(c => c.StaffNumber == staffNumber));
    }
}
