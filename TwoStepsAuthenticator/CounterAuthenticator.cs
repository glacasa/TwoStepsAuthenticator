using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoStepsAuthenticator
{

    /// <summary>
    /// Implementation of RFC 4226 Counter-Based One-Time Password Algorithm
    /// </summary>
    public class CounterAuthenticator : Authenticator
    {
        private readonly int WindowSize;

        public CounterAuthenticator(int windowSize = 10) {
            if (windowSize <= 0) {
                throw new ArgumentException("look-ahead window size must be positive");
            }

            this.WindowSize = windowSize;
        }

        /// <summary>
        /// Generates One-Time-Password.
        /// </summary>
        /// <param name="secret">Shared Secret</param>
        /// <param name="counter">Current Counter</param>
        /// <returns>OTP</returns>
        public string GetCode(string secret, ulong counter)
        {
            return GetCodeInternal(secret, counter);
        }

        /// <summary>
        /// Checks if the passed code is valid.
        /// </summary>
        /// <param name="secret">Shared Secret</param>
        /// <param name="code">OTP</param>
        /// <param name="counter">Current Counter Position</param>
        /// <returns>true if any code from counter to counter + WindowSize matches</returns>
        public bool CheckCode(string secret, string code, ulong counter)
        {
            ulong successfulSequenceNumber = 0uL;

            return CheckCode(secret, code, counter, out successfulSequenceNumber);
        }

        /// <summary>
        /// Checks if the passed code is valid.
        /// </summary>
        /// <param name="secret">Shared Secret</param>
        /// <param name="code">OTP</param>
        /// <param name="counter">Current Counter Position</param>
        /// <param name="usedCounter">Matching counter value if successful</param>
        /// <returns>true if any code from counter to counter + WindowSize matches</returns>
        public bool CheckCode(string secret, string code, ulong counter, out ulong usedCounter)
        {
            var codeMatch = false;
            ulong successfulSequenceNumber = 0uL;

            for (uint i = 0; i <= WindowSize; i++)
            {
                ulong checkCounter = counter + i;
                if (ConstantTimeEquals(GetCode(secret, checkCounter), code)) {
                    codeMatch = true;
                    successfulSequenceNumber = checkCounter;
                }
            }

            usedCounter = successfulSequenceNumber;
            return codeMatch;
        }
    }
}
