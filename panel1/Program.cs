//using Microsoft.AspNetCore.Builder;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.IdentityModel.Tokens;
//using System.Text;
//using Panel1.Classes;
//using FluentAssertions.Common;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.
//builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//    .AddJwtBearer(options =>
//    {
//        options.TokenValidationParameters = new TokenValidationParameters
//        {
//            ValidateIssuer = true,
//            ValidateAudience = true,
//            ValidateLifetime = true,
//            ValidIssuer = builder.Configuration["Jwt:Issuer"],
//            ValidAudience = builder.Configuration["Jwt:Audience"],
//            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
//        };
//        //options.Events = new JwtBearerEvents
//        //{
//        //    OnChallenge = context =>
//        //    {
//        //        context.HandleResponse();
//        //        context.Response.StatusCode = 401;
//        //        context.Response.ContentType = "application/json";
//        //        return context.Response.WriteAsync("Custom unauthorized message");
//        //    }
//        //};
//        //options.Events = new JwtBearerEvents
//        //{
//        //    OnChallenge = context =>
//        //    {
//        //        if (context.AuthenticateFailure != null)
//        //        {
//        //            if (context.AuthenticateFailure.GetType() == typeof(SecurityTokenExpiredException))
//        //            {
//        //                context.Response.StatusCode = 401;
//        //                context.Response.ContentType = "application/json";
//        //                return context.Response.WriteAsync("Token expired");
//        //            }
//        //            else
//        //            {
//        //                context.Response.StatusCode = 401;
//        //                context.Response.ContentType = "application/json";
//        //                return context.Response.WriteAsync("Invalid token");
//        //            }
//        //        }

//        //        if (!context.Request.Headers.ContainsKey("Authorization"))
//        //        {
//        //            context.Response.StatusCode = 401;
//        //            context.Response.ContentType = "application/json";
//        //            return context.Response.WriteAsync("Authorization header missing");
//        //        }

//        //        var authHeader = context.Request.Headers["Authorization"].ToString();
//        //        if (string.IsNullOrWhiteSpace(authHeader) || !authHeader.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
//        //        {
//        //            context.Response.StatusCode = 401;
//        //            context.Response.ContentType = "application/json";
//        //            return context.Response.WriteAsync("Invalid token format");
//        //        }

//        //        // If none of the above conditions match, provide a general unauthorized message
//        //        context.Response.StatusCode = 401;
//        //        context.Response.ContentType = "application/json";
//        //        return context.Response.WriteAsync("Unauthorized");
//        //    }
//        //};



//    });

//builder.Services.AddScoped<Connection>();
//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

////builder.Services.AddCors(options =>
////{
////    options.AddPolicy("ReactConnection", builder =>
////    {
////        builder.WithOrigins("http://localhost:5174")
////               .AllowAnyHeader()
////               .AllowAnyMethod();
////    });
////});

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("ReactConnection", policy =>
//    {
//        //policy.WithOrigins("http://localhost", "http://192.168.1.60:5173", "http://192.168.1.60:5116", "http://192.168.1.54:5173")
//        policy.WithOrigins("*")

//.AllowAnyHeader()
//               .AllowAnyMethod()
//               .SetIsOriginAllowedToAllowWildcardSubdomains();
//    });
//});

////builder.WebHost.UseUrls("http://192.168.1.60:5116", "http://127.0.0.1:5116", "http://1ocalhost:5173");

//builder.WebHost.UseUrls("http://192.168.1.57:5116");

//var app = builder.Build();
//app.UseCors("ReactConnection");

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}
//app.UseAuthentication();

//app.UseAuthorization();


//app.MapControllers();

//app.Run();



using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Panel1.Classes;
using panel1.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddScoped<Connection>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<TokenValidator>(); 

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactConnection", policy =>
    {
        policy.WithOrigins("*")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowedToAllowWildcardSubdomains();
    });
});

builder.WebHost.UseUrls("http://192.168.1.57:5116");

var app = builder.Build();
app.UseCors("ReactConnection");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
//app.UseTestMiddleware();
app.UseMiddleware<TestMiddleware>();

//app.UseRouting();
//app.Map("/api/Department/GetAllDepartment", config =>
//{
//    config.UseMiddleware<TestMiddleware>();
//    config.UseEndpoints(endpoints =>
//    {
//        endpoints.MapControllers();
//    });
//});
//app.Map("/WeatherForecast", config =>
//{
//    config.UseMiddleware<TestMiddleware>();
//    config.UseEndpoints(endpoints =>
//    {
//        endpoints.MapControllers();
//    });
//});



app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
