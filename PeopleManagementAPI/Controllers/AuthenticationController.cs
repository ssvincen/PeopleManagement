using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PeopleManagement_BI;
using PeopleManagement_BO;
using PeopleManagement_Utility;
using System.IdentityModel.Tokens.Jwt;

namespace PeopleManagementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController(IConfiguration configuration, ILogger<AuthenticationController> logger,
        IUserRepository userRepository) : ControllerBase
    {
        private readonly IConfiguration _configuration = configuration;
        private readonly ILogger<AuthenticationController> _logger = logger;
        private readonly IUserRepository _userRepository = userRepository
            ?? throw new ArgumentNullException(nameof(userRepository));

        [AllowAnonymous]
        [HttpPost("Register")]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            _logger.LogInformation("Register called");
            if (string.IsNullOrEmpty(request.EmailAddress) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest();
            }
            var existingUser = await _userRepository.FindUserByUsernameAsync(request.EmailAddress);
            if (existingUser != null)
                return Conflict("User already exists.");

            Random _random = new();
            string emailOTP = Guid.NewGuid().ToString();
            string otp = _random.Next(0, 99999).ToString();

            var result = await _userRepository.AddUserAsync(new UserModel
            {
                FirstName = request.FirstName,
                Surname = request.Surname,
                MobileNumber = request.MobileNumber,
                EmailAddress = request.EmailAddress,
                Password = Crypto.PasswordHash(request.Password),
                EmailOTP = emailOTP,
                OTP = otp
            });

            if (result.IsSuccess)
            {
                // send email
                // send otp via sms
                _logger.LogInformation("Register succeeded");
                return Ok("User successfully created");
            }
            return StatusCode(StatusCodes.Status500InternalServerError, $"Failed to create user: {result.Message}");

        }

        [AllowAnonymous]
        [HttpPost("Login")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(LoginResponse))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            _logger.LogInformation("Login called");
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
            {
                return BadRequest();
            }
            var user = await _userRepository.FindUserByUsernameAsync(request.Username);

            if (user == null || !Crypto.ComparePassword(user.PasswordHash, Crypto.PasswordHash(request.Password)).IsSuccess)
                return Unauthorized();

            JwtSecurityToken token = JwtHelper.GenerateJwt(request.Username, _configuration);
            var refreshToken = JwtHelper.GenerateRefreshToken();
            DateTime refreshtokenExpiry = DateTime.UtcNow.AddHours(1);
            await _userRepository.UpdateUserRefreshTokenAsync(user.EmailAddress, refreshToken, refreshtokenExpiry);

            _logger.LogInformation("Login succeeded");

            return Ok(new LoginResponse
            {
                JwtToken = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo,
                RefreshToken = refreshToken
            });
        }


        [HttpPost("Refresh")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Refresh([FromBody] RefreshModel model)
        {
            _logger.LogInformation("Refresh called");
            var principal = JwtHelper.GetPrincipalFromExpiredToken(model.AccessToken, _configuration);

            if (principal?.Identity?.Name is null)
                return Unauthorized();

            var user = await _userRepository.FindUserByUsernameAsync(principal.Identity.Name);
            if (user is null || user.RefreshToken != model.RefreshToken || user.RefreshTokenExpiryTime < DateTime.UtcNow)
                return Unauthorized();

            var token = JwtHelper.GenerateJwt(principal.Identity.Name, _configuration);
            _logger.LogInformation("Refresh succeeded");

            return Ok(new LoginResponse
            {
                JwtToken = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo,
                RefreshToken = model.RefreshToken
            });
        }


        [Authorize]
        [HttpDelete("Revoke")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Revoke()
        {
            _logger.LogInformation("Revoke called");
            var username = HttpContext.User.Identity?.Name;
            if (username is null)
                return Unauthorized();

            var user = await _userRepository.FindUserByUsernameAsync(username);
            if (user is null)
                return Unauthorized();

            DateTime refreshtokenExpiry = DateTime.UtcNow.AddHours(1);
            await _userRepository.UpdateUserRefreshTokenAsync(user.EmailAddress, null, refreshtokenExpiry);
            _logger.LogInformation("Revoke succeeded");
            return Ok();
        }
    }
}
