using DbUp;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(builder.Configuration.GetConnectionString("NotificationsDb")!);

var app = builder.Build();

var connectionString = builder.Configuration.GetConnectionString("NotificationsDb")!;
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
