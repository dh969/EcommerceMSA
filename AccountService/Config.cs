
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccountService
{

    public static class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources() =>
       new List<IdentityResource> { new IdentityResources.OpenId(),
                                        new IdentityResources.Profile()};
        public static IEnumerable<ApiResource> GetApis() =>
            new List<ApiResource>{
                new ApiResource{
                    Name="ApiOne",
                    Scopes={"ApiOne"}
                },
                new ApiResource
                {
                    Name="ApiTwo",
                    Scopes={"ApiTwo"}
                }
            };
        public static IEnumerable<Client> GetClients() =>
            new List<Client> { new Client
            {
                ClientId="Client_Id",
                ClientSecrets={new Secret("client_secret".ToSha256())},
                AllowedGrantTypes=GrantTypes.ClientCredentials,
                AllowedScopes={"ApiOne"}
            } ,
            new Client{
            ClientId="client_id_mvc",
            ClientSecrets={new Secret("client_secret_mvc".ToSha256()) }
            ,AllowedGrantTypes=GrantTypes.Code,
                AllowedScopes =
                {
                    "ApiOne","ApiTwo",IdentityServerConstants.StandardScopes.OpenId,IdentityServerConstants.StandardScopes.Profile
                },
                RedirectUris={ "https://www.google.com/"},
                AlwaysIncludeUserClaimsInIdToken=true,
                RequireConsent=false
            },
            new Client
            {
                ClientId="angular",
                AllowedGrantTypes=GrantTypes.Code,
                RequirePkce=true,
                RequireClientSecret=false,
                 AlwaysIncludeUserClaimsInIdToken=true,
                RedirectUris={"http://localhost:4200"},
                PostLogoutRedirectUris={"http://localhost:4200"},
                AllowedCorsOrigins={ "http://localhost:4200" },
                AllowedScopes =
                { "ApiOne","ApiTwo",IdentityServerConstants.StandardScopes.OpenId,IdentityServerConstants.StandardScopes.Profile

                },RequireConsent=false,
              //  AccessTokenLifetime=1,
                AllowAccessTokensViaBrowser=true,
            }
};
        public static List<ApiScope> Scopes = new List<ApiScope>
{
    new ApiScope { Name = "ApiOne" },
    new ApiScope { Name = "ApiTwo" },
    new ApiScope(IdentityServerConstants.StandardScopes.OpenId),
    new ApiScope(IdentityServerConstants.StandardScopes.Profile)
    // Add other scopes as needed
};

    }
}
