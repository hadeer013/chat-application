using ChatWithAuth.DAL.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.Tasks;

namespace chatWithAuthentication.Token
{
    public static class IdentityService
    {
        public static void AddIdentityServices(this IServiceCollection services/*, IConfiguration configuration*/)
        {
            //services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            //{
            //    options.User.RequireUniqueEmail = true;
            //})
            //.AddEntityFrameworkStores<ApplicationContext>();
            //services.AddAuthentication(
            //    options =>
            //    {
            //        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            //        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            //    })
            //    .AddJwtBearer(options =>
            //    {
            //        options.Events = new JwtBearerEvents
            //        {
            //            OnMessageReceived = context =>
            //            {
            //                var accessToken = context.Request.Query["access_token"];

            //                // If the request is for our hub...
            //                var path = context.HttpContext.Request.Path;
            //                if (!string.IsNullOrEmpty(accessToken))
            //                {
            //                    // Read the token out of the query string
            //                    context.Token = accessToken;
            //                }
            //                return Task.CompletedTask;
            //            }
            //        };
            //        options.TokenValidationParameters = new TokenValidationParameters()
            //        {
            //            ValidateAudience = true,
            //            ValidAudience = configuration["JWT:Audience"],
            //            ValidateIssuer = true,
            //            ValidIssuer = configuration["JWT:Issuer"],
            //            ValidateLifetime = true,
            //            ValidateIssuerSigningKey = true,
            //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Key"]))
            //        };
            //    });
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(CookieAuthenticationDefaults.AuthenticationScheme, options =>
                {
                    options.LoginPath = new PathString("/Account/Login");
                    options.AccessDeniedPath = new PathString("/Home/Error");
                });

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 5;
                options.SignIn.RequireConfirmedAccount = false;
                options.User.RequireUniqueEmail = true;
            })
                        .AddEntityFrameworkStores<ApplicationContext>()
                        .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>(TokenOptions.DefaultProvider);
        }
    }
}
