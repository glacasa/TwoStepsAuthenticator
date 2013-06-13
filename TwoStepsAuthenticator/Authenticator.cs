using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace TwoStepsAuthenticator
{
    public class Authenticator
    {
        private static Lazy<UsedCodesManager> usedCodes = new Lazy<UsedCodesManager>();

        public string GenerateKey()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
            var random = new Random();
            var keyChars = new char[16];
            for (int i = 0; i < 16; i++)
            {
                keyChars[i] = chars[random.Next(chars.Length)];
            }
            return new String(keyChars);
        }

        public string GetCode(string secret)
        {
            return GetCode(secret, DateTime.Now);
        }

        public string GetCode(string secret, DateTime date)
        {
            var key = Base32Encoding.ToBytes(secret);
            for (int i = secret.Length; i < key.Length; i++)
            {
                key[i] = 0;
            }

            TimeSpan ts = (date.ToUniversalTime() - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc));
            var interval = (long)ts.TotalSeconds / 30;

            long chlg = interval;
            byte[] challenge = new byte[8];
            for (int j = 7; j >= 0; j--)
            {
                challenge[j] = (byte)((int)chlg & 0xff);
                chlg >>= 8;
            }

            HMACSHA1 mac = new HMACSHA1(key);
            var hash = mac.ComputeHash(challenge);

            int offset = hash[hash.Length - 1] & 0xf;

            int truncatedHash = 0;
            for (int j = 0; j < 4; j++)
            {
                truncatedHash <<= 8;
                truncatedHash |= hash[offset + j];
            }

            truncatedHash &= 0x7FFFFFFF;
            truncatedHash %= 1000000;

            string code = truncatedHash.ToString();
            return code.PadLeft(6, '0');
        }

        public bool CheckCode(string secret, string code)
        {
            if (usedCodes.Value.IsCodeUsed(secret, code))
                return false;

            var baseTime = DateTime.Now;
            for (int i = -2; i <= 1; i++)
            {
                var checkTime = baseTime.AddSeconds(30 * i);
                if (GetCode(secret, checkTime) == code)
                {
                    usedCodes.Value.AddCode(secret, code);
                    return true;
                }
            }

            return false;
        }



    }
}
