using API_SGHSS.DTOs.AuthDTOs;
using API_SGHSS.Models;
using API_SGHSS.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace API_SGHSS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthController(ITokenService tokenService, UserManager<User> userManager,
                    RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }
        
        /// <summary>
        /// Cria uma nova Role.
        /// </summary>
        /// <remarks>
        /// Exemplo de request
        ///     
        ///     POST api/Auth/CreateRole
        ///     {
        ///         "RoleName": Paciente
        ///     }
        /// </remarks>
        /// <param name="roleName">Nome da Role.</param>
        /// <returns>Retorna um 201 Created ou 400 BadRequest e o nome da Role.</returns>
        [HttpPost]
        [Route("CreateRole")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            var roleExist = await _roleManager.RoleExistsAsync(roleName);

            if (!roleExist)
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
                if(roleResult.Succeeded)
                {
                    return StatusCode(StatusCodes.Status201Created,
                        new ResponseDTO { Status = "Success", Message = $"Role {roleName} added successfully" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new ResponseDTO { Status = "Error", Message = $"Issue adding the new {roleName} role" });
                }

            }
            return StatusCode(StatusCodes.Status400BadRequest,
                new ResponseDTO { Status = "Error", Message = "Role already exist." });
        }

        /// <summary>
        /// Vincula um Usuario a uma Role existente.
        /// </summary>
        /// <remarks>
        /// Exemplo de request
        /// 
        ///     POST api/Auth/AddUserToRole
        ///     {
        ///         "Email": carlos@gmail.com,
        ///         "RoleName": Paciente
        ///     }
        /// </remarks>
        /// <param name="email">Email do Usuario.</param>
        /// <param name="roleName">Nome da Role.</param>
        /// <returns>Retorna um 201 Created ou 400 BadRequest e Email do Usuario e a RoleName</returns>
        [HttpPost]
        [Route("AddUserToRole")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddUserToRole(string email, string roleName)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if(user != null)
            {
                var result = await _userManager.AddToRoleAsync(user, roleName);
                if (result.Succeeded)
                {
                    return StatusCode(StatusCodes.Status201Created,
                        new ResponseDTO { Status = "Success", Message = $"User {user.Email} added to the {roleName} role" });
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest,
                        new ResponseDTO { Status = "Error", Message = $"Unable to add user {user.Email} to the {roleName} role" });
                }
            }
            return BadRequest(new { error = "Unable to find user" });
        }

        /// <summary>
        /// Faz o login de um Usuario.
        /// </summary>
        /// <remarks>
        /// Exemplo de request:
        /// 
        ///     POST api/Auth/login
        ///     {
        ///         "UserName": carlos,
        ///         "Password": Teste#2025
        ///     }
        /// </remarks>
        /// <param name="loginDTO">Objeto LoginDTO</param>
        /// <returns>Retorna um 200 OK ou 401 Unauthorized, um Token e um Refresh Token</returns>
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var user = await _userManager.FindByNameAsync(loginDTO.UserName!);

            if (user is not null && await _userManager.CheckPasswordAsync(user, loginDTO.Password!))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim(ClaimTypes.Email, user.Email!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var generateToken = _tokenService.GenerateAccessToken(authClaims, _configuration);

                var refresToken = _tokenService.GenerateRefreshToken();

                _ = int.TryParse(_configuration["JWT:RefreshTokenValidtyInMinutes"], out int refreshTokenValidtyInMinutes);

                user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(refreshTokenValidtyInMinutes);

                user.RefreshToken = refresToken;

                await _userManager.UpdateAsync(user);

                return Ok(new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(generateToken),
                    RefreshToken = refresToken,
                    Expiration = generateToken.ValidTo
                });
            }

            return Unauthorized();
        }

        /// <summary>
        /// Cria um novo Usuario.
        /// </summary>
        /// <remarks>
        /// Exemplo de request
        /// 
        ///     POST api/Auth/Register
        ///     {
        ///         "UserName": maria,
        ///         "Email": maria@gmail.com,
        ///         "Password": Teste#2025
        ///     }
        /// </remarks>
        /// <param name="registerDTO">Objeto RegistroDTO</param>
        /// <returns>Retorna um 201 Created ou 500 InternalServerError e uma RespostaDTO</returns>
        [HttpPost]
        [Route("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register(RegisterDTO registerDTO)
        {
            var userExists = await _userManager.FindByNameAsync(registerDTO.UserName!);

            if (userExists != null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ResponseDTO { Status = "Error", Message = "usuario ja existe" });
            }

            User user = new User()
            {
                Email = registerDTO.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = registerDTO.UserName
            };

            var result = await _userManager.CreateAsync(user, registerDTO.Password!);

            if (!result.Succeeded)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    new ResponseDTO { Status = "Error", Message = "Criação do usuario falhou" });
            }

            return Ok(new ResponseDTO { Status = "Success", Message = "Criado o usuario" });
        }

        /// <summary>
        /// Retorna um novo Token e Refresh Token atualizado.
        /// </summary>
        /// <remarks>
        /// Exemplo de request
        /// 
        ///     POST api/Auth/Refresh-Token
        ///     {
        ///         "acessToken": eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IndlbGxpbmd0b24iLCJlbWFpbCI6IndlbGxpbmd0b25AZ21haWwuY29tIiwianRpIjoiNGQzNjc4YWYtZWNmMC00NjhhLWI1MDMtMDkyOTU5NWVhN2U3Iiwicm9sZSI6IkFkbWluIiwibmJmIjoxNzY0NzkzODE3LCJleHAiOjE3NjQ3OTQ3MTcsImlhdCI6MTc2NDc5MzgxNywiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NzI2OSIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjcyNjkifQ.jV6Z7zUbZnUxU4sM7lhPWeC6kg9e0gwkOuj-E_KJ-_8,
        ///         "refreshToken": 2+sRbbgNNy63UGZxkBCI77m/mDhakEd8R8+taqEvuZuZg24KSgAy0i0tOD7IJY6QxruO4+WlIXEHiv+DPrP/CA==,
        ///         "expiration": 2025-12-03T20:45:17Z
        ///     }
        /// </remarks>
        /// <param name="tokenDTO">Objeto TokenDTO</param>
        /// <returns>Retorna um 201 Created ou 400 BadRequest e um novo token e um Refresh Token</returns>
        /// <exception cref="ArgumentNullException"></exception>
        [HttpPost]
        [Route("refresh-token")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RefreshToken(TokenDTO tokenDTO)
        {
            if (tokenDTO is null)
                return BadRequest("Solicitação de cliente inválida");

            string? accessToken = tokenDTO.AcessToken ?? throw new ArgumentNullException(nameof(tokenDTO));

            string? refresToken = tokenDTO.RefreshToken ?? throw new ArgumentNullException(nameof(tokenDTO));

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken, _configuration);

            if (principal == null)
                return BadRequest("acesso a token/refresh token invalido");

            string userName = principal.Identity!.Name!;
            
            var user = await _userManager.FindByNameAsync(userName);

            if (user is null || user.RefreshToken != refresToken
                || user.RefreshTokenExpiryTime <= DateTime.Now)
                return BadRequest("acesso a token/refresh token invalido");

            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims.ToList(), _configuration);

            var newRefreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;

            await _userManager.UpdateAsync(user);

            return new ObjectResult(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                RefreshToken = newRefreshToken
            });
        }

        /// <summary>
        /// Remove o Refresh Token que esta registrado no Banco de dados.
        /// </summary>
        /// <remarks>
        /// Exemplo de request
        /// 
        ///     POST api/Auth/revoke/{username}
        ///     {
        ///         "UserName": carlos
        ///     }
        /// </remarks>
        /// <param name="userName">Nome do usuario para remover o Refresh Token</param>
        /// <returns>Retorna um 204 NoContent ou um 400 BadRequest</returns>
        [HttpPost]
        [Route("revoke/{userName}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Revoke(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null) return BadRequest("Nome do usuario invalido");

            user.RefreshToken = null;

            await _userManager.UpdateAsync(user);

            return NoContent();
        }
    }
}
