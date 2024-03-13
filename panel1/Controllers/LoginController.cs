using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Panel1.Classes;
using Panel.Models;
using Microsoft.AspNetCore.Cors;
using System.Security.Claims;

namespace Panel1.Controllers
{
    //[EnableCors("ReactConnection")]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly Connection _connection;

        public LoginController(IConfiguration configuration, Connection connection)
        {
            _config = configuration;
            _connection = connection;
        }

        //private Users AuthenticateUser(Users user)
        //{
        //    Users authenticatedUser = null;
        //    string hashedPassword = DecryptPassword.HashPassword(user.password);

        //    string query = $"SELECT * FROM Users_mst WHERE username = '{user.username}' AND password = '{hashedPassword}'";
        //    DataTable resultTable = _connection.ExecuteQueryWithResult(query);

        //    if (resultTable.Rows.Count > 0)
        //    {
        //        authenticatedUser = new Users { username = resultTable.Rows[0]["username"].ToString() };
        //    }

        //    return authenticatedUser;
        //}

        //private string GenerateToken(Users users)
        //{
        //    var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        //    var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        //    var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"], null,
        //        expires: DateTime.Now.AddMinutes(60),
        //        signingCredentials: credentials);
        //    return new JwtSecurityTokenHandler().WriteToken(token);
        //}
        private string GenerateToken(Users users)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var issuedAt = DateTime.UtcNow;
            var localIssuedAt = TimeZoneInfo.ConvertTimeFromUtc(issuedAt, TimeZoneInfo.Local);

            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                new List<Claim>
                {
            new Claim(ClaimTypes.NameIdentifier, users.User_ID.ToString()),
            new Claim(ClaimTypes.Name, users.username),
            new Claim("iat", new DateTimeOffset(localIssuedAt).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer)
                },
                expires: localIssuedAt.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        //      private string GenerateToken(Users users)
        //      {
        //          var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        //          var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        //          //var issuedAt = DateTime.Now;
        //          var issuedAt = DateTime.UtcNow;
        //          var token = new JwtSecurityToken(
        //              _config["Jwt:Issuer"],
        //              _config["Jwt:Audience"],
        //              new List<Claim>
        //              {
        //          new Claim(ClaimTypes.NameIdentifier, users.User_ID.ToString()),
        //          new Claim(ClaimTypes.Name, users.username),
        //          new Claim("iat", new DateTimeOffset(issuedAt).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer)
        //              },
        ////expires: issuedAt.AddMinutes(60),
        //expires: issuedAt.AddHours(1),
        //signingCredentials: credentials
        //          );

        //          return new JwtSecurityTokenHandler().WriteToken(token);
        //      }


        [AllowAnonymous]
        [HttpPost]

        public IActionResult Login(Users user)
        {

            IActionResult response = Unauthorized();
            try
            {
                string emailOrUsername = user.emailOrUsername;
                string password = user.password;
                string userRole = user.userRole;

                bool isEmail = emailOrUsername.Contains('@');
                string columnName = isEmail ? "Email" : "username";
                string columnValue = isEmail ? $"='{emailOrUsername}'" : $"='{emailOrUsername}'";

                string query = $"SELECT U.*, R.RoleName, R.RoleID FROM Users_mst U JOIN Roles_R R ON U.RoleID = R.RoleID WHERE U.{columnName} {columnValue}";
                DataTable result = _connection.ExecuteQueryWithResult(query);
                DataRow userData = result.Rows.Count > 0 ? result.Rows[0] : null;

                if (userData == null)
                {
                    return Unauthorized(new { message = "User not found" });
                }

                if (Convert.ToInt32(userData["status"]) != 1)
                {
                    return Unauthorized(new { message = "User is not active. Please contact the administrator." });
                }

                string matchPassword = HashedPassword.HashPassword(password);

                if (matchPassword != userData["password"].ToString())
                {
                    return Unauthorized(new { message = "Password not matched" });
                }

                if (userData["rolename"].ToString() != userRole)
                {
                    return Unauthorized(new { message = "role not matched" });
                }

                string token = GenerateToken(new Users
                {
                    User_ID = Convert.ToInt32(userData["User_ID"]),
                    username = userData["username"].ToString(),
                });

                ExtractTokenInformation(token);
                Console.WriteLine($"Here is the token {token}");

                response = Ok(new { token });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
            }

            return response;
        }



        //public IActionResult Login(Users user)
        //{
        //    IActionResult response = Unauthorized();
        //    try
        //    {


        //        string query = $"SELECT User_ID, username FROM Users_mst WHERE username = '{user.username}' AND password = '{user.password}'";
        //        DataTable result = _connection.ExecuteQueryWithResult(query);
        //        DataRow userData = result.Rows.Count > 0 ? result.Rows[0] : null;

        //        if (userData == null)
        //        {
        //            return Unauthorized(new { message = "Invalid username or password" });
        //        }

        //        string token = GenerateToken(new Users
        //        {
        //            User_ID = Convert.ToInt32(userData["User_ID"]),
        //            username = userData["username"].ToString(),
        //        });

        //        ExtractTokenInformation(token);
        //        Console.WriteLine($"Here is the token: {token}");

        //        response = Ok(new { token });
        //    }
        //    catch (Exception ex)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, $"Error: {ex.Message}");
        //    }

        //    return response;
        //}


        //[AllowAnonymous]
        //[HttpPost]
        //public IActionResult Login(Users user)
        //{
        //    IActionResult response = Unauthorized();
        //    var authenticatedUser = AuthenticateUser(user);

        //    if (authenticatedUser != null)
        //    {
        //        var token = GenerateToken(authenticatedUser);
        //        response = Ok(new { token = token });
        //    }

        //    return response;
        //}

        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] Users newUser)
        {
            string hashedPassword = HashedPassword.HashPassword(newUser.password);

            string insertquery = $"insert into Users_mst(username,password,Email,RoleID)Values('{newUser.username}','{hashedPassword}','{newUser.Email}','1')";
            try
            {
                _connection.ExecuteQueryWithResult(insertquery);
                return Ok("USer Register successfully");

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error:{ex.Message}");
            }

        }
        private void ExtractTokenInformation(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(token) as JwtSecurityToken;

            DateTime expires = DateTimeOffset.FromUnixTimeSeconds(jsonToken.Payload.Exp ?? 0).LocalDateTime;
            Console.WriteLine($"Token Expires: {expires}");

            DateTime issuedAt = DateTimeOffset.FromUnixTimeSeconds(jsonToken.Payload.Iat ?? 0).LocalDateTime;
            Console.WriteLine($"Token Issued At: {issuedAt}");

            var userIdClaim = jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (userIdClaim != null)
            {
                string userId = userIdClaim.Value;
                Console.WriteLine($"User ID: {userId}");
            }

            var usernameClaim = jsonToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);
            if (usernameClaim != null)
            {
                string username = usernameClaim.Value;
                Console.WriteLine($"Username: {username}");
            }
        }

       

    }
}
