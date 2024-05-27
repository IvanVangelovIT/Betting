using Domain;
using Domain.Extensions;
using Hangfire;
using Hangfire.MySql;
using WebApi.Jobs.Hangfire;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ApplySharedConfig(builder.Configuration);

var connectionString = builder.Configuration.GetHangfireConnectionString();

builder.Services.AddHangfire(configuration => configuration
    .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
    .UseSimpleAssemblyNameTypeSerializer()
    .UseRecommendedSerializerSettings()
    .UseStorage(
        new MySqlStorage(
            connectionString,
            new MySqlStorageOptions
            {
                QueuePollInterval = TimeSpan.FromSeconds(10),
                JobExpirationCheckInterval = TimeSpan.FromHours(1),
                CountersAggregateInterval = TimeSpan.FromMinutes(5),
                PrepareSchemaIfNecessary = true,
                DashboardJobListLimit = 25000,
                TransactionTimeout = TimeSpan.FromMinutes(1),
                TablesPrefix = "Hangfire",
            }
        )
    ));

builder.Services.AddHangfireServer(options => options.WorkerCount = 1);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers(); // Add this line to map controllers endpoints.
    
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapHangfireDashboard("/hangfire", new DashboardOptions
    {
        IgnoreAntiforgeryToken = true
    });
});

RecurringJob.AddOrUpdate<ProcessSportsEveryOneMinuteJob>("ProcessSports", x => x.ExecuteAsync(), Cron.MinuteInterval(1));

app.Run();