using FireManagerServer;

var builder = WebApplication.CreateBuilder(args)
           .AddMySql()
           .AddServicesBase()
           .AddBackgroundServices();
var app = builder.Build();
app.UsesService();
app.Run();
