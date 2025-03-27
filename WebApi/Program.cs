using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApi.Config;
using WebApi.Entities;
using WebApi.Repository;
using WebApi.Token;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<ContextModel>(options =>
        options.UseSqlServer(
            builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ContextModel>()
    .AddDefaultTokenProviders();     

//Interfaces and repository injection
builder.Services.AddSingleton<IRepositoryProduct, RepositoryProduct>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
        option =>
        {
            option.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = "wmandradedev.Security.Bearer",
                ValidAudience = "wmandradedev.Security.Bearer",
                IssuerSigningKey = JWTSecurityKey.Create("Secret-Key-12345-wmandradedev")
            };

            option.Events = new JwtBearerEvents
            {
                OnAuthenticationFailed = context =>
                {
                    Console.WriteLine("OnAuthenticationFailed: " + context.Exception.Message);
                    return Task.CompletedTask;
                },

                OnTokenValidated = context =>
                {
                    Console.WriteLine("OnTokenValidated" + context.SecurityToken);
                    return Task.CompletedTask;
                }
            };

        });



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
