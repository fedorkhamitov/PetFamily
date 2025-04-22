using PetFamily.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<AppDbContext>();
var app = builder.Build();
app.UseHttpsRedirection();
app.Run();