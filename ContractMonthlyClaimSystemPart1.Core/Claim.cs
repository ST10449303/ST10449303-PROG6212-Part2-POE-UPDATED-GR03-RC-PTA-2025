namespace ContractMonthlyClaimSystemPart1.Core
{

    public class Claim
    {
        public string LecturerName { get; set; }
        public string StaffNumber { get; set; }
        public decimal Hours { get; set; }
        public decimal HourlyRate { get; set; }
        public decimal Amount => Hours * HourlyRate;
        public ClaimStatus Status { get; set; }
        public string ApprovedBy { get; set; }
        public string Notes { get; set; }

        public DateTime SubmittedAt { get; set; }
        public string SubmittedBy { get; set; }

        public string DisplayTitle => $"{LecturerName} [{StaffNumber}] — {Hours}h @ {HourlyRate:C} — {Status}";
    }

    public enum ClaimStatus
    {
        PendingVerification,
        Verified,
        Approved,
        Rejected
    }
}

