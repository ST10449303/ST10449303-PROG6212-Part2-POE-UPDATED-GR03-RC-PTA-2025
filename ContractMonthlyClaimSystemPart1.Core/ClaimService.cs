using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ContractMonthlyClaimSystemPart1.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractMonthlyClaimSystemPart1.Core
{
    
        
        public class ClaimService
        {
            private readonly ObservableCollection<Claim> _store;

            public ClaimService(ObservableCollection<Claim> store)
            {
                _store = store ?? throw new ArgumentNullException(nameof(store));
            }

           
            public Claim CreateClaim(string lecturerName, string staffNumber, decimal hours, decimal hourlyRate)
            {
                if (string.IsNullOrWhiteSpace(lecturerName))
                    throw new ArgumentException("Lecturer name is required.");
                if (string.IsNullOrWhiteSpace(staffNumber))
                    throw new ArgumentException("Staff number is required.");
                if (hours <= 0)
                    throw new ArgumentException("Hours must be greater than zero.");
                if (hourlyRate <= 0)
                    throw new ArgumentException("Hourly rate must be greater than zero.");

                var claim = new Claim
                {
                    LecturerName = lecturerName,
                    StaffNumber = staffNumber,
                    Hours = hours,
                    HourlyRate = hourlyRate,
                    SubmittedAt = DateTime.Now,
                    Status = ClaimStatus.PendingVerification,
                    SubmittedBy = lecturerName
                };

                _store.Add(claim);
                return claim;
            }

            
            public void ApproveClaim(Claim claim, string approverName)
            {
                if (claim == null)
                    throw new ArgumentNullException(nameof(claim));
                if (string.IsNullOrWhiteSpace(approverName))
                    throw new ArgumentException("Approver name required.");

                claim.Status = ClaimStatus.Approved;
                claim.ApprovedBy = approverName;
            }

            
            public void RejectClaim(Claim claim, string reason)
            {
                if (claim == null)
                    throw new ArgumentNullException(nameof(claim));

                claim.Status = ClaimStatus.Rejected;
                claim.Notes = reason ?? "No reason provided.";
            }

           
            public ObservableCollection<Claim> GetPendingClaims()
            {
                return new ObservableCollection<Claim>(
                    _store.Where(c => c.Status == ClaimStatus.PendingVerification)
                );
            }
        }
    }

