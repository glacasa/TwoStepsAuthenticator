using System;

namespace TwoStepsAuthenticator {

    /// <summary>
    /// Manages used code to prevent repeated use of a code.
    /// </summary>
    public interface IUsedCodesManager {

        /// <summary>
        /// Adds secret/code pair.
        /// </summary>
        /// <param name="challenge">Used Challenge</param>
        /// <param name="code">Used Code</param>
        void AddCode(long timestamp, string code);

        /// <summary>
        /// Checks if code was previously used.
        /// </summary>
        /// <param name="challenge">Used Challenge</param>
        /// <param name="code">Used Code</param>
        /// <returns></returns>
        bool IsCodeUsed(long timestamp, string code);
    }
}
