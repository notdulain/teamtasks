using DbUp;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton(builder.Configuration.GetConnectionString("TasksDb")!);

// Register HTTP client for calling User Service
builder.Services.AddHttpClient("UserService", client =>
{
    client.BaseAddress = new Uri(builder.Configuration["ServiceUrls:UserService"]!);
});

var app = builder.Build();

var connectionString = builder.Configuration.GetConnectionString("TasksDb")!;
EnsureDatabase.For.SqlDatabase(connectionString);
var upgrader = DeployChanges.To
    .SqlDatabase(connectionString)
    .WithScriptsFromEmbeddedInAssembly(Assembly.GetExecutingAssembly())
    .LogToConsole()
    .Build();
upgrader.PerformUpgrade();

app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();
app.Run();
