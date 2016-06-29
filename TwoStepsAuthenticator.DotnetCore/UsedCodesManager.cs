using System;
using System.Collections.Generic;

namespace TwoStepsAuthenticator
{
    public class UsedCodesManager : IUsedCodesManager
    {
        public void AddCode(long timestamp, string code, object user)
        {
            throw new NotImplementedException();
        }

        public bool IsCodeUsed(long timestamp, string code, object user)
        {
            throw new NotImplementedException();
        }
    }


}
