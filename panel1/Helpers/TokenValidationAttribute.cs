//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc.Diagnostics;
//using Microsoft.AspNetCore.Mvc.Filters;
//using Microsoft.IdentityModel.Tokens;
//using System.ComponentModel.DataAnnotations;
//using System.IdentityModel.Tokens.Jwt;
//using System.Text;
//using System.Net;
//using Microsoft.AspNetCore.Http;

//using System.Collections.Generic;
//using static System.Net.WebRequestMethods;
//using Microsoft.AspNetCore.SignalR;


//namespace panel1.Helpers
//{
//    public class TokenValidationAttribute : ActionFilterAttribute
//    {
//        public override void OnActionExecuting(ActionExecutingContext context)
//        {


//            string token = context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
//            string issuer = "http://localhost:5116/";
//            string audience = "http://localhost:5116/";
//            string secretKey = "2Fsk5LBU5j1DrPldtFmLWeO8uZ8skUzwhe3ktVimUE8=";

//            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
//            var jwt = new JwtSecurityToken();
//    //   public  bool ValidateToken(
//    //    string token,
//    //    string issuer,
//    //    string audience,
//    //    ICollection<SecurityKey> signingKeys,
//    //    out JwtSecurityToken jwt
//    //)
//    //        {
//                if (string.IsNullOrEmpty(token))
//                {
//                    // Token is null or empty, cannot proceed with validation
//                    jwt = null;
//                    //return false;
//                }
//                //string issuer = "http://localhost:5116/";
//                //string audience = "http://localhost:5116/";
//           //  public  static string secretKey = "2Fsk5LBU5j1DrPldtFmLWeO8uZ8skUzwhe3ktVimUE8=";

//             //   var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
//               // IConfiguration config;

//                var validationParameters = new TokenValidationParameters
//                {
//                    ValidateIssuer = true,
//                    ValidIssuer = "http://localhost:5116/",
//                    ValidateAudience = true,
//                    ValidAudience = "http://localhost:5116/",
//                    ValidateIssuerSigningKey = true,
//              //      IssuerSigningKeys = securityKey,
//                    ValidateLifetime = true
//                };

//                try
//                {
//                    var tokenHandler = new JwtSecurityTokenHandler();
//                    tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
//                    jwt = (JwtSecurityToken)validatedToken;

//                    //return true;
//                }
//                catch (SecurityTokenValidationException)
//                {
//                    jwt = null;
//                    //return false;
//                }

//            }
//        }
//    }


using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using panel1.Model;

namespace panel1.Helpers
{
    public class TokenValidationAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var obj = @"{
                    'Error':'123',
                    'statusCode':'400'
                }";
            var obj1 = @"{
                    'Success':'OK',
                    'statusCode':'200'
                }";
            if (ValidateToken(context, out JwtSecurityToken jwt))
            {
              
            }
            else
            {
               
                context.Result = new Microsoft.AspNetCore.Mvc.ObjectResult(obj)
                {
                    StatusCode = 401
                };
            }
        }

        private bool ValidateToken(ActionExecutingContext context, out JwtSecurityToken jwt)
        {

            string token = context.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            string issuer = "http://localhost:5116/";
            var appname = new ConfigurationBuilder().AddJsonFile("AppSettings.json").Build().GetSection("Jwt").Get<jwt>();
            //string issuer = System.Configuration.ConfigurationManager.AppSettings["Jwt"].ToString();

            string audience = "http://localhost:5116/";
            string secretKey = "2Fsk5LBU5j1DrPldtFmLWeO8uZ8skUzwhe3ktVimUE8=";

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = securityKey,
                ValidateLifetime = true
            };

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
                jwt = (JwtSecurityToken)validatedToken;
                return true;
            }
            catch (SecurityTokenValidationException)
            {
                jwt = null;
                return false;
            }
            catch (Exception)
            {
                jwt = null;
                return false;
            }
        }
    }
}
