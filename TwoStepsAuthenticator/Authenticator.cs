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
        private static readonly RNGCryptoServiceProvider Random = new RNGCryptoServiceProvider();    // Is Thread-Safe
        private static readonly int KeyLength = 16;
        private static readonly string AvailableKeyChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";
        private readonly Func<DateTime> NowFunc;

        public Authenticator(Func<DateTime> nowFunc = null) {
            this.NowFunc = (nowFunc == null) ? () => DateTime.Now : nowFunc;
        }

        public string GenerateKey()
        {
            var keyChars = new char[KeyLength];
            for (int i = 0; i < keyChars.Length; i++)
            {
                keyChars[i] = AvailableKeyChars[RandomInt(AvailableKeyChars.Length)];
            }
            return new String(keyChars);
        }

        public string GetCode(string secret)
        {
            return GetCode(secret, NowFunc());
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
            for (int j = 7; j >= 0; j--) {
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

            var baseTime = NowFunc();

            // We need to do this in constant time
            var codeMatch = false;
            for (int i = -2; i <= 1; i++)
            {
                var checkTime = baseTime.AddSeconds(30 * i);
                if (ConstantTimeEquals(GetCode(secret, checkTime), code))
                {
                    codeMatch = true;
                    usedCodes.Value.AddCode(secret, code);
                }
            }

            return codeMatch;
        }

        private bool ConstantTimeEquals(string a, string b) {
            uint diff = (uint)a.Length ^ (uint)b.Length;

            for (int i = 0; i < a.Length && i < b.Length; i++) {
                diff |= (uint)a[i] ^ (uint)b[i];
            }

            return diff == 0;
        }

        private int RandomInt(int max) {
            var randomBytes = new byte[4];
            Random.GetBytes(randomBytes);

            return Math.Abs((int)BitConverter.ToUInt32(randomBytes, 0) % max);
        }


    }
}
