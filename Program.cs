using Microsoft.EntityFrameworkCore;
using InvigilatorSchedulerStandard.Data;
using InvigilatorSchedulerStandard.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default")
        ?? throw new Exception("Missing ConnectionStrings:Default")));

builder.Services.AddScoped<RuleService>();
builder.Services.AddScoped<InvigilatorSolveService>();
builder.Services.AddScoped<ExcelExportService>();

var app = builder.Build();

app.UseStaticFiles();
app.UseRouting();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
