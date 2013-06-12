using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TwoStepsAuthenticator
{
    public class TwoStepsAuthenticator
    {
        //private static final int PASS_CODE_LENGTH = 6;
        ///** Default passcode timeout period (in seconds) */
        //private static final int INTERVAL = 30;
        HMACSHA1 mac;

        public void GetPinCode(string key, DateTime date)
        {            
            TimeSpan ts = (date.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            var interval = (long)ts.TotalSeconds / 30;

            var challengeBytes = BitConverter.GetBytes(interval);
            // OK jusque là



            mac = new HMACSHA1(Base32Encoding.ToBytes(key));

            var hash = mac.ComputeHash(challengeBytes);
            //var hash = mac.TransformFinalBlock(challengeBytes);
            //int offset = hash[hash.Length - 1] & 0xF;
            //int truncatedHash = hashToInt(hash, offset) & 0x7FFFFFFF;
        }



        private int hashToInt(byte[] bytes, int start)
        {
            return BitConverter.ToInt32(bytes, start);
        }
    }
}
