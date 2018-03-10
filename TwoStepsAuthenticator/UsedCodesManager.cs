using System;
using System.Collections.Generic;
using System.Threading;

namespace TwoStepsAuthenticator
{
    public class UsedCodesManager : IUsedCodesManager
    {
        internal sealed class UsedCode
        {
            public UsedCode(long timestamp, String code, object user)
            {
                this.UseDate = DateTime.Now;
                this.Code = code;
                this.Timestamp = timestamp;
                this.User = user;
            }

            internal DateTime UseDate { get; private set; }
            internal long Timestamp { get; private set; }
            internal String Code { get; private set; }
            internal object User { get; private set; }

            public override bool Equals(object obj)
            {
                if (Object.ReferenceEquals(this, obj))
                {
                    return true;
                }

                var other = obj as UsedCode;
                return (other != null) && this.Code.Equals(other.Code) && this.Timestamp.Equals(other.Timestamp) && this.User.Equals(other.User);
            }
            public override string ToString()
            {
                return String.Format("{0}: {1}", Timestamp, Code);
            }
            public override int GetHashCode()
            {
                return Code.GetHashCode() + (Timestamp.GetHashCode() + User.GetHashCode() * 17) * 17;
            }
        }

        private readonly Queue<UsedCode> codes;
        private readonly System.Threading.ReaderWriterLockSlim rwlock = new System.Threading.ReaderWriterLockSlim();
        private readonly Timer cleaner;

        public UsedCodesManager()
        {
            codes = new Queue<UsedCode>();
            var delay = (int)TimeSpan.FromMinutes(5).TotalMilliseconds;
            cleaner = new Timer(cleaner_Elapsed, null, delay, delay);
        }

        void cleaner_Elapsed(object state)
        {
            var timeToClean = DateTime.Now.AddMinutes(-5);

            try
            {
                rwlock.EnterWriteLock();

                while (codes.Count > 0 && codes.Peek().UseDate < timeToClean)
                {
                    codes.Dequeue();
                }
            }
            finally
            {
                rwlock.ExitWriteLock();
            }
        }

        public void AddCode(long timestamp, String code, object user)
        {
            try
            {
                rwlock.EnterWriteLock();

                codes.Enqueue(new UsedCode(timestamp, code, user));
            }
            finally
            {
                rwlock.ExitWriteLock();
            }
        }

        public bool IsCodeUsed(long timestamp, String code, object user)
        {
            try
            {
                rwlock.EnterWriteLock();

                return codes.Contains(new UsedCode(timestamp, code, user));
            }
            finally
            {
                rwlock.ExitWriteLock();
            }
        }
    }


}
