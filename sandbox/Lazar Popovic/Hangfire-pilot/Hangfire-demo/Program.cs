using Hangfire;
using Hangfire.Logging;
using Hangfire.SQLite;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
var configuration = new ConfigurationBuilder().AddJsonFile("C:\\Users\\Lazar\\Desktop\\WattWise-refactor\\wattwise - refactor\\sandbox\\Lazar Popovic\\Hangfire - pilot\\appsettings.json", optional: true, reloadOnChange: true)
.Build();

var connectionString = configuration.GetConnectionString("Hangfire");
GlobalConfiguration.Configuration.UseStorage(new SQLiteStorage(connectionString, new SQLiteStorageOptions
{
    QueuePollInterval = TimeSpan.FromSeconds(15),
    PrepareSchemaIfNecessary = true,
    JobExpirationCheckInterval = TimeSpan.FromHours(1)
}));

var server = new BackgroundJobServer();
Console.WriteLine("Hangfire server started. Press any key to exit...");

BackgroundJob.Enqueue(() => Console.WriteLine("Hello from Hangfire!"));

Console.ReadKey();
server.Dispose();

static void MyJob()
{
    Console.WriteLine("Hello from Hangfire!");
}
