using bloggie.web.Data;
using bloggie.web.Reposetory;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<BloggieDbContext>(option =>
option.UseSqlServer(builder.Configuration.GetConnectionString("BloggieDbConnectionString")));

builder.Services.AddDbContext<AuthDbContext>(option =>
option.UseSqlServer(builder.Configuration.GetConnectionString("BloggieAuthDbConnectionString")));

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>();

builder.Services.Configure<IdentityOptions>(option =>
{
    option.Password.RequireDigit = true;
    option.Password.RequireLowercase = true;
    option.Password.RequireNonAlphanumeric = true;
    option.Password.RequireUppercase = true;
    option.Password.RequiredLength = 6;
    option.Password.RequiredUniqueChars = 1;
});

builder.Services.AddScoped<ITagReposetory, TagReposetory > ();
// for Bulding When I Asked For <This , Give Me This>
builder.Services.AddScoped<IBlogPostReposetory, BlogPostReposetory>();
//we inject inteface imges reposetory when i call for IImgReposetory this give me the implemtation of CloudineryImagesReposetory
builder.Services.AddScoped<IImageReposetory, CloudineryImagesReposetory>();
builder.Services.AddScoped<IBlogPostLikeReposetory, BlogPostLikeReposetory>();
builder.Services.AddScoped<IBlogPostCommentReposetory, BlogPostCommentReposetory>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


//before UseAuthorization this we must have UseAuthentication first 
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
