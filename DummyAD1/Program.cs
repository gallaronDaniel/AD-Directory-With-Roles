using Microsoft.AspNetCore.Authentication.Negotiate;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Add Windows AD authentication (disable this during development)
// Uncomment when you want to enable AD auth
builder.Services.AddAuthentication(NegotiateDefaults.AuthenticationScheme)
     .AddNegotiate();

// Add authorization (disable this during development)
 builder.Services.AddAuthorization(options =>
 {options.FallbackPolicy = options.DefaultPolicy; });

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

// Uncomment these when enabling AD auth
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
