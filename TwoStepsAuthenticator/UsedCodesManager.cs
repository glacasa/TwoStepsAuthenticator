using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace TwoStepsAuthenticator
{
    /// <summary>
    /// Local, thread-save used codes manager implementation
    /// </summary>
    public class UsedCodesManager : IUsedCodesManager
    {
        internal sealed class UsedCode
        {
            public UsedCode(ulong challenge, String code)
            {
                this.UseDate = DateTime.Now;
                this.Code = code;
                this.ChallengeValue = challenge;
            }

            internal DateTime UseDate { get; private set; }
            internal ulong ChallengeValue { get; private set; }
            internal String Code { get; private set; }

            public override bool Equals(object obj)
            {
                if (Object.ReferenceEquals(this, obj)) {
                    return true;
                }

                var other = obj as UsedCode;
                return (other != null) ? this.Code.Equals(other.Code) && this.ChallengeValue.Equals(other.ChallengeValue) : false;
            }
            public override string ToString()
            {
                return String.Format("{0}: {1}", ChallengeValue, Code);
            }
            public override int GetHashCode()
            {
                return Code.GetHashCode() + ChallengeValue.GetHashCode() * 17;
            }
        }

        private readonly Queue<UsedCode> codes;
        private readonly System.Threading.ReaderWriterLock rwlock = new System.Threading.ReaderWriterLock();
        private readonly TimeSpan lockingTimeout = TimeSpan.FromSeconds(5);
        private readonly Timer cleaner;

        public UsedCodesManager()
        {
            codes = new Queue<UsedCode>();
            cleaner = new Timer(TimeSpan.FromMinutes(5).TotalMilliseconds);
            cleaner.Elapsed += cleaner_Elapsed;
            cleaner.Start();
        }

        void cleaner_Elapsed(object sender, ElapsedEventArgs e)
        {
            var timeToClean = DateTime.Now.AddMinutes(-5);

            try 
            {
                rwlock.AcquireWriterLock(lockingTimeout);

                while (codes.Count > 0 && codes.Peek().UseDate < timeToClean) {
                    codes.Dequeue();
                }
            } 
            finally 
            {
                rwlock.ReleaseWriterLock();
            }
        }

        public void AddCode(ulong challenge, String code)
        {
            try {
                rwlock.AcquireWriterLock(lockingTimeout);

                codes.Enqueue(new UsedCode(challenge, code));
            } 
            finally 
            {
                rwlock.ReleaseWriterLock();
            }
        }

        public bool IsCodeUsed(ulong challenge, String code)
        {
            try 
            {
                rwlock.AcquireReaderLock(lockingTimeout);

                return codes.Contains(new UsedCode(challenge, code));
            } 
            finally 
            {
                rwlock.ReleaseReaderLock();
            }
        }
    }


}
