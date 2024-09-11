namespace OasisoftTask.Common
{
    public class JwtSetupData
    {
        public string SecretKey { get; set; }
        public string Issuer { get; set; }
        public TimeSpan TokenLifetime { get; set; }
    }
}
