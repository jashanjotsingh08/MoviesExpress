using System;
using System.Configuration;
using Lab3_Arshdeep_Jashanjot_.Areas.Identity.Data;
using Lab3_Arshdeep_Jashanjot_.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: HostingStartup(typeof(Lab3_Arshdeep_Jashanjot_.Areas.Identity.IdentityHostingStartup))]
namespace Lab3_Arshdeep_Jashanjot_.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                var x = new SqlConnectionStringBuilder(context.Configuration.GetConnectionString("AuthDbContextConnection"));
                x.UserID = context.Configuration["DbUser"];
                x.Password = context.Configuration["DbPassword"];
                var connection = x.ConnectionString;
                services.AddDbContext<AuthDbContext>(options =>
                    options.UseSqlServer(connection));

                services.AddDefaultIdentity<ApplicationUser>(options => 
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequireNonAlphanumeric = false;
                })
                    .AddEntityFrameworkStores<AuthDbContext>();
            });
        }
    }
}