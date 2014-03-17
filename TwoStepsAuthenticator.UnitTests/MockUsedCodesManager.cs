namespace TwoStepsAuthenticator.UnitTests
{
    internal class MockUsedCodesManager : IUsedCodesManager {
        public long? LastChallenge { get; private set; }
        public string LastCode { get; private set; }

        public void AddCode(long challenge, string code, object user) {
            this.LastChallenge = challenge;
            this.LastCode = code;
        }

        public bool IsCodeUsed(long challenge, string code, object user) {
            return false;
        }
    }
}