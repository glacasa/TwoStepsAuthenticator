using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwoStepsAuthenticator.TestApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var auth = new TwoStepsAuthenticator();
            auth.GetPinCode("JBSWY3DPEHPK3PXP", new DateTime(2013, 6, 12, 18, 0, 0));
        }
    }
}
