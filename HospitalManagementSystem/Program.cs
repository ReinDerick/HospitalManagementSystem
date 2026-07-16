using HospitalManagementSystem.Api.Users.Infrastructure;
using HospitalManagementSystem.Data;
using HospitalManagementSystem.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDbContext<HospitalDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddAuthorization();
builder.Services.AddAuthentication();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

var app = builder.Build();

// 1. Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// 2. Main structural configurations
app.UseHttpsRedirection();
app.UseCors("AllowFrontend");

// 3. THE CRITICAL JWT GUARD SEQUENCE
app.UseAuthentication(); // First: Reads the token and identifies who the user is
app.UseAuthorization();  // Second: Checks if that identified user has access privileges

// 4. THE EXIT POINT (MILAN'S GLOBAL LOCK APPLIED HERE)
app.MapControllers().RequireAuthorization(); // <-- CHANGED: Chained .RequireAuthorization() here!

app.Run();