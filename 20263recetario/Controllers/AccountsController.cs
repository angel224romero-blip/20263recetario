using _20263recetario.DTOs.Identity;
using _20263recetario.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace _20263recetario.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration; //ir a appsetings.json
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IMapper mapper;

        public AccountsController(UserManager<ApplicationUser> userManager, IConfiguration configuration, SignInManager<ApplicationUser> signInManager, IMapper mapper)
        {
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
            this.mapper = mapper;
        }
        
        [HttpPost("register")]
        public async Task<ActionResult<AuthenticationResponseDto>> Register(UserCredentialsDto userCredentialsDto)
        {
                var usuario = mapper.Map<ApplicationUser>
                (userCredentialsDto);
                var resultado = await userManager.CreateAsync(usuario, userCredentialsDto.Password);
                if(resultado.Succeeded)
            {
                return await BuildToken(userCredentialsDto.Email);
            }
            else
            {
                return BadRequest(resultado.Errors);
            }
        }
        private async Task<AuthenticationResponseDto> BuildToken(String email)
        {
            var claims = new List<Claim>
            {
                new Claim("email", email),
                new Claim(ClaimTypes.Email, email)
            };

            var usuario = await userManager.FindByEmailAsync(email);
            var claimsRoles = await userManager.GetClaimsAsync(usuario);
            var usuarioId = usuario!.Id;
            var roles = await userManager.GetRolesAsync(usuario);
            foreach (var rol in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }
            claims.AddRange(claimsRoles);

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["LlaveJWT"]!));
            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);
            var expiracion = DateTime.UtcNow.AddDays(30);

            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiracion, signingCredentials: creds);

            return new AuthenticationResponseDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expiration = expiracion,
                UserId = usuarioId
            };

        }

    }
}
