using Microsoft.EntityFrameworkCore;
using nyerievents.services.iservices;
using NyeriEvents.Data;
using NyeriEvents.Extensions;
using NyeriEvents.Services;
using NyeriEvents.Services.IServices;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//addDbContext
builder.Services.AddDbContext<EventDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("defaultConnection"));
});



//add services
builder.Services.AddScoped<IUserService, UserServices>();
builder.Services.AddScoped<IEventService, EventService>();

//Automapper
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

//builder.AddAppAuthentication();

//builder.AddAuthorizationExtension();

//builder.AddSwaggenGenExtension();

var app = builder.Build();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.ApplyMigration();

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();
app.ApplyMigration();

app.Run();


