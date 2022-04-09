using Common;
using DataAccess.Data.Models;
using DTOS;
using HotelAppAPI.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HotelAppAPI.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]

    public class AuthController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly APISettings aPISettings;

        public AuthController(SignInManager<ApplicationUser> signInManager,
                               UserManager<ApplicationUser> userManager,
                               RoleManager<IdentityRole> roleManager,
                               IOptions<APISettings> options)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
            aPISettings = options.Value;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp([FromBody] UserForRegisterDTO userForRegisterDTO)
        {
            if (userForRegisterDTO == null || !ModelState.IsValid)
            {
                return BadRequest();
            }
            var user = new ApplicationUser
            {
                UserName = userForRegisterDTO.Email,
                Email = userForRegisterDTO.Email,
                Name = userForRegisterDTO.Name,
                PhoneNumber = userForRegisterDTO.PhoneNo,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, userForRegisterDTO.Password);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(new RegisterationResponseDTO
                {
                    Errors = errors,
                    IsRegisterationSuccessful = false
                });
            }
            var roleResult = await userManager.AddToRoleAsync(user, SD.Role_Customer);
            if (!roleResult.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return BadRequest(new RegisterationResponseDTO
                {
                    Errors = errors,
                    IsRegisterationSuccessful = false
                });
            }
            return StatusCode(201);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn([FromBody] UserForLoginDTO userForLoginDTO)
        {
            var result = await signInManager.PasswordSignInAsync(userForLoginDTO.UserName,
                userForLoginDTO.Password, false, false);

            if (result.Succeeded)
            {
                var user = await userManager.FindByNameAsync(userForLoginDTO.UserName);
                if (user == null)
                {
                    return Unauthorized(new AuthenticationResponseDTO
                    {
                        IsAuthSuccessful = false,
                        ErrorMessage = "Invalid Authentication"
                    });
                }

                //everything is valid and we need to login the user
                var signingCredentials = GetSigningCredentials();
                var claims = await GetClaims(user);
                var tokenOptions = new JwtSecurityToken(
                   issuer: aPISettings.ValidIssuer,
                   audience: aPISettings.ValidAudience,
                   claims: claims,
                   expires: DateTime.Now.AddDays(30),
                   signingCredentials: signingCredentials
                   );
                var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
                return Ok(new AuthenticationResponseDTO
                {
                    IsAuthSuccessful = true,
                    Token = token,
                    UserForReturnDTO = new UserForReturnDTO
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Email = user.Email,
                        PhoneNo = user.PhoneNumber
                    }

                });
            }
            else
            {
                return Unauthorized(new AuthenticationResponseDTO
                {
                    IsAuthSuccessful = false,
                    ErrorMessage = "Invalid Authentication"
                });
            }
        }

        private SigningCredentials GetSigningCredentials()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8
                                                                .GetBytes(aPISettings.SecretKey));
            return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        }

        private async Task<List<Claim>> GetClaims(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("Id", user.Id),
            };
            var roles = await userManager.GetRolesAsync(
                                        await userManager.FindByEmailAsync(user.Email)
                                        );
            roles.ToList().ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));
            return claims;
        }
    }
}
