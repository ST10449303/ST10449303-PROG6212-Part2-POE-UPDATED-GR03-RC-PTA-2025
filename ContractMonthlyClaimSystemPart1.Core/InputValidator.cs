using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractMonthlyClaimSystemPart1.Core
{
   
    public static class InputValidator
    {
        /// <summary>
        /// Validate hours and rate input strings.
        /// Hours must be > 0. Rate must be >= 0.
        /// Uses InvariantCulture so tests are stable regardless of machine locale.
        /// </summary>
        /// <param name="hoursText">input for hours</param>
        /// <param name="rateText">input for hourly rate</param>
        /// <param name="hours">parsed hours (out)</param>
        /// <param name="rate">parsed rate (out)</param>
        /// <param name="error">error message (out) or null when ok</param>
        /// <returns>true when both parsed and valid</returns>
        public static bool ValidateHoursAndRate(string hoursText, string rateText,
            out decimal hours, out decimal rate, out string error)
        {
            hours = 0m;
            rate = 0m;
            error = null;

            if (string.IsNullOrWhiteSpace(hoursText))
            {
                error = "Invalid hours.";
                return false;
            }

            if (!decimal.TryParse(hoursText.Trim(), NumberStyles.Number | NumberStyles.AllowDecimalPoint,
                                  CultureInfo.InvariantCulture, out hours))
            {
                error = "Invalid hours.";
                return false;
            }

            if (hours <= 0m)
            {
                error = "Invalid hours.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(rateText))
            {
                error = "Invalid rate.";
                return false;
            }

            if (!decimal.TryParse(rateText.Trim(), NumberStyles.Number | NumberStyles.AllowDecimalPoint,
                                  CultureInfo.InvariantCulture, out rate))
            {
                error = "Invalid rate.";
                return false;
            }

            if (rate < 0m)
            {
                error = "Invalid rate.";
                return false;
            }

            
            error = null;
            return true;
        }
    }
}
