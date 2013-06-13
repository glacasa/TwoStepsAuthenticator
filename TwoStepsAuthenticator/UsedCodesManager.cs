using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace TwoStepsAuthenticator
{
    internal class UsedCodesManager
    {
        private class UsedCode
        {
            public UsedCode(String secret, String code)
            {
                this.UseDate = DateTime.Now;
                this.Code = secret + code;
            }

            public DateTime UseDate { get; set; }
            public String Code { get; set; }

            public override bool Equals(object obj)
            {
                return obj.ToString().Equals(Code);
            }
            public override string ToString()
            {
                return Code;
            }
            public override int GetHashCode()
            {
                return Code.GetHashCode();
            }
        }

        private Queue<UsedCode> codes;
        private object codeLock = new object();
        private Timer cleaner;

        public UsedCodesManager()
        {
            codes = new Queue<UsedCode>();
            cleaner = new Timer(TimeSpan.FromSeconds(5).TotalMilliseconds);
            cleaner.Elapsed += cleaner_Elapsed;
            cleaner.Start();
        }

        void cleaner_Elapsed(object sender, ElapsedEventArgs e)
        {
            var timeToClean = DateTime.Now.AddSeconds(-5);
            lock (codeLock)
            {
                while (codes.Count > 0 && codes.Peek().UseDate < timeToClean)
                {
                    codes.Dequeue();
                }
            }
        }

        public void AddCode(String secret, String code)
        {
            lock (codeLock)
            {
                codes.Enqueue(new UsedCode(secret, code));
            }
        }

        public bool IsCodeUsed(String secret, String code)
        {
            lock (codeLock)
            {
                return codes.Contains(new UsedCode(secret, code));
            }
        }
    }


}
