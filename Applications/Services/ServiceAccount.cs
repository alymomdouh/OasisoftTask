using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using OasisoftTask.Applications.Dtos.Account;
using OasisoftTask.Applications.IServices;
using OasisoftTask.Common;
using OasisoftTask.Core.DomainModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace OasisoftTask.Applications.Services
{
    public class ServiceAccount : IServiceAccount
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly JwtSetupData _jwt;
        public ServiceAccount(
            UserManager<ApplicationUser> userManager,
            RoleManager<ApplicationRole> roleManager,
            SignInManager<ApplicationUser> signInManager,
            JwtSetupData jwt
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _jwt = jwt;
        }

        public async Task<UserResult> Login(LoginDto loginDto)
        {
            var user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            {
                throw new Exception("Not Found User");
            }
            var Result = await _signInManager.PasswordSignInAsync(user.UserName, loginDto.Password, false, false);
            if (!Result.Succeeded)
            {
                throw new Exception("Invalid User Name OR Password");
            }
            if (Result.IsLockedOut)
            {
                throw new Exception("LockedAccount");
            }
            var jwtSecurityToken = await CreateJwtToken(user);

            var Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            return new UserResult(user.Name, user.Email, user.UserName, Token, user.Id);
        }

        public async Task<UserResult> Register(RegisterDto model)
        {
            var checkMail = await _userManager.FindByEmailAsync(model.Email);
            if (checkMail != null)
            {
                throw new Exception("EmailDuplicated");
            }
            ApplicationUser user = new ApplicationUser(model.Email, model.UserName, model.Name);
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded == false)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"{errors}");
            }
            result = await _userManager.AddToRoleAsync(user, Constants.SuperAdminRole);// as example
            if (result.Succeeded == false)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new Exception($"{errors}");
            }
            // will rediect to login 
            return new UserResult(user.Name, user.Email, user.UserName, "", user.Id);
        }

        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var roleClaims = new List<Claim>();   // i not use Claims for this small task
            var roles = await _userManager.GetRolesAsync(user);

            var claims = new[]
           {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sid, user.Id.ToString()),
                new Claim("SecurityStamp", await _userManager.GetSecurityStampAsync(user))

            };
            //.Union(await _userManager.GetClaimsAsync(user));

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.SecretKey));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                issuer: _jwt.Issuer,
                claims: claims,
                expires: DateTime.UtcNow.Add(_jwt.TokenLifetime),
                signingCredentials: signingCredentials);
        }

    }
}
