using _20263recetario.DTOs.Identity;
using _20263recetario.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace _20263recetario.Controllers
{
    [ApiController]
    [Route("api/[controller]")] //ruta api account
    public class AccountsController : ControllerBase //todo controlador debe heredar de control o controlBase
        //inyeccion de dependencias (usar los metodos de las librerias)
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly IConfiguration configuration; //ir a appsetings.json
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IMapper mapper;

        public AccountsController(UserManager<ApplicationUser> userManager, IConfiguration configuration, SignInManager<ApplicationUser> signInManager, IMapper mapper)
        {//la inyeccion se deben de inicializar en el constructor
            this.userManager = userManager;
            this.configuration = configuration;
            this.signInManager = signInManager;
            this.mapper = mapper;
        }
        
        [HttpPost("register")]
        public async Task<ActionResult<AuthenticationResponseDto>> Register(UserCredentialsDto userCredentialsDto) //mappeo
        {
                var usuario = mapper.Map<ApplicationUser>
                (userCredentialsDto);
                var resultado = await userManager.CreateAsync(usuario, userCredentialsDto.Password);//usermanager ya trae la validacion y es arte de la inyeccion
                if(resultado.Succeeded)//con esto se loguea automaticamente
            {
                return await BuildToken(userCredentialsDto.Email);
            }
            else
            {
                return BadRequest(resultado.Errors);
            }
        }
        private async Task<AuthenticationResponseDto> BuildToken(String email)//hay que generar el token para que de acceso
        {
            var claims = new List<Claim>//informacion que se manda al jwt
            {
                new Claim("email", email),
                new Claim(ClaimTypes.Email, email)
            };
            //extraer la informacion de los claims
            var usuario = await userManager.FindByEmailAsync(email);
            var claimsRoles = await userManager.GetClaimsAsync(usuario!);
            var usuarioId = usuario!.Id;
            var roles = await userManager.GetRolesAsync(usuario);//obtenemos los claims que tiene y vamos recorriendolos con el foreach
            foreach (var rol in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }
            claims.AddRange(claimsRoles);

            var llave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["LlaveJWT"]!));
            var creds = new SigningCredentials(llave, SecurityAlgorithms.HmacSha256);//mecanismo de encriptacion
            var expiracion = DateTime.UtcNow.AddDays(30);

            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiracion, signingCredentials: creds);//investigar

            return new AuthenticationResponseDto
            {//mandamos llamar lo que tenemos
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expiration = expiracion,
                UserId = usuarioId
            };

        }

        [HttpDelete("renew")]//se renueva el token cadavez que hacemos un movimiento
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] //aqui vemos si esta autorizado para entrar a los diferentes lugares
        public async Task<ActionResult<AuthenticationResponseDto>> Renew() 
        {
            var emailClaim = HttpContext.User.Claims.FirstOrDefault(x => x.Type == "email");
            return await BuildToken(emailClaim!.Value);
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthenticationResponseDto>> Login(UserCredentialsDto userCredentialsDto)//contienen el email y la contrasena
        {
            var resultado = await signInManager.PasswordSignInAsync
            (userCredentialsDto.Email, userCredentialsDto.Password, isPersistent: false, lockoutOnFailure: false);

            if (resultado.Succeeded)
            {
                return await BuildToken(userCredentialsDto.Email);
            }
            else
            {
                return BadRequest("Login incorrecto");//lo hacemos asi por si uno de los campos e correcto no nos ataquen por ahi(seguridad)
            }
            //el token se guarda en el navegador.
        }

    }
}
