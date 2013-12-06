namespace TwoStepsAuthenticator.UnitTests
{
    internal class MockUsedCodesManager : IUsedCodesManager {
        public ulong? LastChallenge { get; private set; }
        public string LastCode { get; private set; }

        public void AddCode(ulong challenge, string code) {
            this.LastChallenge = challenge;
            this.LastCode = code;
        }

        public bool IsCodeUsed(ulong challenge, string code) {
            return false;
        }
    }
}