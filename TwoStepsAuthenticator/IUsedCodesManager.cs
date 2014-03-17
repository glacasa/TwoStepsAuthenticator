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
        /// <param name="user">The user</param>
        void AddCode(long timestamp, string code, object user);

        /// <summary>
        /// Checks if code was previously used.
        /// </summary>
        /// <param name="challenge">Used Challenge</param>
        /// <param name="code">Used Code</param>
        /// <param name="user">The user</param>
        /// <returns>True if the user as already used the code</returns>
        bool IsCodeUsed(long timestamp, string code, object user);
    }
}
