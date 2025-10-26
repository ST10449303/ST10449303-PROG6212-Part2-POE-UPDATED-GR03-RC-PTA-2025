using System;
using System.ComponentModel;
using System.IO;

namespace ContractMonthlyClaimSystemPart1.Models
{
    public enum ClaimStatus
    {
        PendingVerification,
        Verified,
        Approved,
        Rejected
    }

    public class Claim : INotifyPropertyChanged
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        private string _lecturerName = string.Empty;
        public string LecturerName
        {
            get => _lecturerName;
            set
            {
                if (_lecturerName != value)
                {
                    _lecturerName = value;
                    OnPropertyChanged(nameof(LecturerName));
                    OnPropertyChanged(nameof(DisplayTitle));
                }
            }
        }

        private string _staffNumber = string.Empty;
        public string StaffNumber
        {
            get => _staffNumber;
            set
            {
                if (_staffNumber != value)
                {
                    _staffNumber = value;
                    OnPropertyChanged(nameof(StaffNumber));
                    OnPropertyChanged(nameof(DisplayTitle));
                }
            }
        }

        private decimal _hours;
        public decimal Hours
        {
            get => _hours;
            set
            {
                if (_hours != value)
                {
                    _hours = value;
                    UpdateAmount();
                    OnPropertyChanged(nameof(Hours));
                    OnPropertyChanged(nameof(DisplayTitle));
                }
            }
        }

        private decimal _hourlyRate;
        public decimal HourlyRate
        {
            get => _hourlyRate;
            set
            {
                if (_hourlyRate != value)
                {
                    _hourlyRate = value;
                    UpdateAmount();
                    OnPropertyChanged(nameof(HourlyRate));
                    OnPropertyChanged(nameof(DisplayTitle));
                }
            }
        }

        private decimal _amount;
        public decimal Amount
        {
            get => _amount;
            private set
            {
                if (_amount != value)
                {
                    _amount = value;
                    OnPropertyChanged(nameof(Amount));
                }
            }
        }

        private ClaimStatus _status = ClaimStatus.PendingVerification;
        public ClaimStatus Status
        {
            get => _status;
            set
            {
                if (_status != value)
                {
                    _status = value;
                    OnPropertyChanged(nameof(Status));
                    OnPropertyChanged(nameof(DisplayTitle));
                }
            }
        }

        public DateTime SubmittedAt { get; set; } = DateTime.Now;
        public string Notes { get; set; } = string.Empty;

        public string SubmittedBy { get; set; } = string.Empty;
        public string VerifiedBy { get; set; } = string.Empty;
        public string ApprovedBy { get; set; } = string.Empty;

       
        private string _uploadedFilePath = string.Empty;
        public string UploadedFilePath
        {
            get => _uploadedFilePath;
            set
            {
                if (_uploadedFilePath != value)
                {
                    _uploadedFilePath = value;
                    OnPropertyChanged(nameof(UploadedFilePath));
                    OnPropertyChanged(nameof(FileName));
                }
            }
        }

        
        public string FileName =>
            string.IsNullOrEmpty(UploadedFilePath)
                ? "No File Uploaded"
                : Path.GetFileName(UploadedFilePath);

   
        public string DisplayTitle =>
            $"{LecturerName} [{StaffNumber}] — {Hours}h @ {HourlyRate:C} — {Status}";

        private void UpdateAmount()
        {
           
            if (Hours < 0 || HourlyRate < 0)
                Amount = 0;
            else
                Amount = Math.Round(Hours * HourlyRate, 2);

            OnPropertyChanged(nameof(Amount));
            OnPropertyChanged(nameof(DisplayTitle));
        }

        public event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
