namespace Matgr.Identity.IdentityConfig
{
    internal sealed class ClientConfig
    {
        public string? ClientId { get; set; }
        public List<SecretConfig>? ClientSecrets { get; set; }
        public List<string>? AllowedGrantTypes { get; set; }
        public List<string>? AllowedScopes { get; set; }
        public List<string>? RedirectUris { get; set; }
        public List<string>? PostLogoutRedirectUris { get; set; }
    }
}
