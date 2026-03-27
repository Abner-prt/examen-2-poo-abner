using Microsoft.EntityFrameworkCore;
using Examen2Api.Data;
using Examen2Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

// SQLite Database Configuration
builder.Services.AddDbContext<PayrollDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Dependency Injection
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<ISheetService, SheetService>();
builder.Services.AddScoped<ISheetDetailService, SheetDetailService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// Apply pending migrations or create DB if not exists
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<PayrollDbContext>();
    context.Database.Migrate();
}

app.Run();
