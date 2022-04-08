using Common;
using DataAccess.Data.Models;
using DTOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
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

        public AuthController(SignInManager<ApplicationUser> signInManager,
                               UserManager<ApplicationUser> userManager,
                               RoleManager<IdentityRole> roleManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
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
    }
}
