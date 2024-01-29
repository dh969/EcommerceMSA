using IdentityServer4.Services;
using System;
using System.Threading.Tasks;
using IdentityServer4.Models;

using Microsoft.AspNetCore.Identity;
using IdentityServer4.Extensions;
using System.Linq;
using AccountService.Entities;
using System.Security.Claims;

namespace AccountService
{


  
        public class IdentityProfileService : IProfileService
        {

            private readonly IUserClaimsPrincipalFactory<AppUser> _claimsFactory;
            private readonly UserManager<AppUser> _userManager;

            public IdentityProfileService(IUserClaimsPrincipalFactory<AppUser> claimsFactory, UserManager<AppUser> userManager)
            {
                _claimsFactory = claimsFactory;
                _userManager = userManager;
            }

            public async Task GetProfileDataAsync(ProfileDataRequestContext context)
            {
                var sub = context.Subject.GetSubjectId();
                var user = await _userManager.FindByIdAsync(sub);
                if (user == null)
                {
                    throw new ArgumentException("");
                }

                var principal = await _claimsFactory.CreateAsync(user);
                var claims = principal.Claims.ToList();
                claims.Add(new Claim(ClaimTypes.Name,user.DisplayName));
            claims.Add(new Claim(ClaimTypes.Email, user.Email));
            //Add more claims like this
            //claims.Add(new System.Security.Claims.Claim("MyProfileID", user.Id));

            context.IssuedClaims = claims;
            }

            public async Task IsActiveAsync(IsActiveContext context)
            {
                var sub = context.Subject.GetSubjectId();
                var user = await _userManager.FindByIdAsync(sub);
                context.IsActive = user != null;
            }
        }

    }

