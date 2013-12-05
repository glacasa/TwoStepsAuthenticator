using System;

namespace TwoStepsAuthenticator {

    /// <summary>
    /// Manages used code to prevent repeated use of a code.
    /// </summary>
    interface IUsedCodesManager {

        /// <summary>
        /// Adds secret/code pair.
        /// </summary>
        /// <param name="secret"></param>
        /// <param name="code"></param>
        void AddCode(string secret, string code);

        /// <summary>
        /// Checks if code was previously used.
        /// </summary>
        /// <param name="secret"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        bool IsCodeUsed(string secret, string code);
    }
}
