using System;
using System.Globalization;
using System.Windows.Data;
using ContractMonthlyClaimSystemPart1.Models;

namespace ContractMonthlyClaimSystemPart1.Converters
{
    public class StatusToProgressConverter : IValueConverter
    {
        // Converts ClaimStatus to a numeric value for ProgressBar
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is ClaimStatus)) return 0;

            ClaimStatus status = (ClaimStatus)value;

            // C# 7.3 compatible switch
            switch (status)
            {
                case ClaimStatus.PendingVerification:
                    return 1;
                case ClaimStatus.Verified:
                    return 2;
                case ClaimStatus.Approved:
                    return 3;
                case ClaimStatus.Rejected:
                    return 0;
                default:
                    return 0;
            }
        }

        // Not needed for ProgressBar
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
