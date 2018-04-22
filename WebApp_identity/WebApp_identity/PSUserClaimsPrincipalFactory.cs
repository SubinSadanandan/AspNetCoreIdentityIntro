using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebApp_identity
{
    public class PSUserClaimsPrincipalFactory:UserClaimsPrincipalFactory<PSUser>
    {
        private UserManager<PSUser> _usermanager;
        private IOptions<IdentityOptions> _options;

        public PSUserClaimsPrincipalFactory(UserManager<PSUser> userManager,IOptions<IdentityOptions> options)
            :base(userManager,options)
        {
            _usermanager = UserManager;
            _options = options;
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(PSUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);
            identity.AddClaim(new Claim("locale", user.Locale));
            return identity;
        }
    }
}
