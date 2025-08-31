using BOZMANOHERMANO.Repo;
using BOZMANOHERMANO.Services.UserFollowServices;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StartUp.HiddenServices;
using StartUp.Models;
using StartUp.Models.Data;
using StartUp.Repo;
using StartUp.Services;
using StartUp.Services.UserServices;

var builder = WebApplication.CreateBuilder(args);

// DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddHttpContextAccessor();
builder.Services.AddSingleton<IEmailSender<ApplicationUser>, DummyEmailSender>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IUserContext, UserContext>();
builder.Services.AddScoped<IUserFollow, UserFollowRepo>();
builder.Services.AddScoped<IUserFollowService, UserFollowService>();


//Adding Repository
builder.Services.AddScoped<IUserRepo, UserRepo>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Cookie Identity
builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
    .AddEntityFrameworkStores<ApplicationDbContext>();


// Cookies Auth
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.SlidingExpiration = true;
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

app.MapIdentityApi<ApplicationUser>();

app.Run();
