//
// https://habr.com/ru/company/otus/blog/542494/
// 
var fullPath = Path.GetFullPath("log4net.config.xml");
var fileInfo = new FileInfo(fullPath);
log4net.Config.XmlConfigurator.Configure(fileInfo);

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
/*
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Chess}/{action=Join}");
    */
app.Run();