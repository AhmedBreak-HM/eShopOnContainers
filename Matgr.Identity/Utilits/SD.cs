using Duende.IdentityServer.Models;
using Duende.IdentityServer;

namespace Matgr.Identity.Utilits
{
    public static class SD
    {
        public const string Admin = "Admin";
        public const string Customer = "Customer";

        public static IEnumerable<IdentityResource> IdentityResources =>
            new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Email(),
                new IdentityResources.Profile(),
                new IdentityResource("role", new[] { "role" })
    };
        public static IEnumerable<ApiScope> ApiScopes =>
            new List<ApiScope>()
            {
                new ApiScope("client","Matgr Server"),
                new ApiScope("matgr","Matgr Server"),

                new ApiScope(name:"read", displayName: "Read your data."),
                new ApiScope(name: "write", displayName: "Write your data"),
                new ApiScope(name:"delete", displayName: "Delete your data")
            };

        public static IEnumerable<Client> Clients =>
            new List<Client>()
            {
                new Client
                {
                   ClientId="client",
                    ClientSecrets= { new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.ClientCredentials,

                    AlwaysIncludeUserClaimsInIdToken = true,


                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                         IdentityServerConstants.StandardScopes.Email,
                         "client",
                         "role"
                    }

                },
                 new Client
                {
                    ClientId = "matgr",
                    ClientSecrets= { new Secret("secret".Sha256())},
                    AllowedGrantTypes = GrantTypes.Code,

                   AlwaysIncludeUserClaimsInIdToken = true,

                    RedirectUris = { "https://localhost:7187/signin-oidc" },
                    PostLogoutRedirectUris={ "https://localhost:7187/signout-callback-oidc" },
                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                         IdentityServerConstants.StandardScopes.Email,
                         "matgr",
                         "role"
                    }
                }

            };
    }
}
