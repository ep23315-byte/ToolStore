using Microsoft.EntityFrameworkCore;
using ToolStore.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();
app.UseSession();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    if (!context.Users.Any())
    {
        context.Users.Add(new ToolStore.Models.User { Name = "Иван (Менеджер)", Login = "manager", Password = "123", Role = "Менеджер" });
        context.Users.Add(new ToolStore.Models.User { Name = "Петр (Продавец)", Login = "seller", Password = "123", Role = "Продавец" });
        context.Users.Add(new ToolStore.Models.User { Name = "Олег (Покупатель)", Login = "buyer", Password = "123", Role = "Покупатель" });
        context.SaveChanges();
    }
}

app.Run();
