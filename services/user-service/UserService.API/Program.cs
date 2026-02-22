using DbUp;
using Microsoft.Data.SqlClient;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register connection string so controllers can use it via DI
builder.Services.AddSingleton(builder.Configuration.GetConnectionString("UsersDb")!);

var app = builder.Build();

// Run DB migrations on startup
var connectionString = builder.Configuration.GetConnectionString("UsersDb")!;
EnsureDatabase.For.SqlDatabase(connectionString);
var upgrader = DeployChanges.To
    .SqlDatabase(connectionString)
    .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
    .LogToConsole()
    .Build();
upgrader.PerformUpgrade();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();
app.Run();


//using direct az cli