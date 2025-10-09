using BOZMANOHERMANO.Repo;
using BOZMANOHERMANO.Services.DmServices;
using BOZMANOHERMANO.Services.PostServices;
using BOZMANOHERMANO.Services.UserFollowServices;
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
builder.Services.AddSingleton<IEmailSender, EmailSender>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IFileService, FileService>();
builder.Services.AddScoped<IUserContext, UserContext>();

builder.Services.AddScoped<IUserDmService, UserDmService>();

builder.Services.AddScoped<IPostService, PostService>();

//Adding Posts Repository
builder.Services.AddScoped<IUserFollow, UserFollowRepo>();
builder.Services.AddScoped<IUserFollowService, UserFollowService>();
builder.Services.AddScoped<IPostsRepo, PostsRepo>();


//Adding Repository
builder.Services.AddScoped<IUserRepo, UserRepo>();

builder.Services.AddScoped<IUserDmRepo, UserDmRepo>();

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

builder.Services.AddCors(options =>
{
    options.AddPolicy("DefaultCorsPolicy", policy =>
    {
        policy.AllowAnyOrigin();
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
    });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("DefaultCorsPolicy");

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.MapIdentityApi<ApplicationUser>();


app.Run();
