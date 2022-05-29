using MaaCopilot.Interfaces.Copilot;
using MaaCopilot.DataAccess.Repositories.Copilot;
using MaaCopilot.DataTransferObjects.Copilot;
using MaaCopilot.Interfaces.ORM;
using MaaCopilot.ORM;
using MaaCopilot.Interfaces.DataAccess;
using MaaCopilot.DataAccess;
using MaaCopilot.ORM.Copilot;
using MaaCopilot.Service.Services.Copilot;
using MaaCopilot.Service.Interfaces.Copilot;
using log4net;
using System.Data;
using MySql.Data.MySqlClient;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IDbConnection>(c=> new MySqlConnection("server=localhost;database=maa;uid=root;pwd=root;"));
builder.Services.AddScoped<IDapperWrapper, DapperWrapper>();
builder.Services.AddScoped<IDBProvider, DBProvider>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IDBHelper, DBHelper>();

builder.Services.AddScoped<ICopilotRepository<Copilot>, CopilotRepository>();
builder.Services.AddScoped<ICopilotService, CopilotService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();    
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
