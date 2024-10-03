using BotApi.Data;
using BotApi.Notifications;
using BotApi.Services;
using BotApi.TgBot;
using BotApi.TgBot.Commands;
using Microsoft.EntityFrameworkCore;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

builder.Services.AddCors(p => p.AddPolicy("NewPolicy", build =>
{
    build.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
}));

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseMySQL(builder.Configuration.GetConnectionString("MySql")!), ServiceLifetime.Singleton);

builder.Services.AddQuartz(options =>
{
    var key = JobKey.Create(nameof(NotificationSender));
    options
        .AddJob<NotificationSender>(key)
        .AddTrigger(trigger =>
        {
            trigger
                .ForJob(key)
                .WithSimpleSchedule(schedule =>
                {
                    schedule.WithIntervalInMinutes(1).RepeatForever();
                });
        });
});

builder.Services.AddQuartzHostedService();

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IUserService, UserService>();

builder.Services.AddSingleton<TgBotClient>();
builder.Services.AddSingleton<ITgBotService, TgBotService>();
builder.Services.AddSingleton<ICommand, Menu>();
builder.Services.AddSingleton<ICommand, StartCalculator>();
builder.Services.AddSingleton<ICommand, Calculate>();
builder.Services.AddSingleton<ICommand, StartAddNotification>();
builder.Services.AddSingleton<ICommand, AddNotification>();
builder.Services.AddSingleton<ICommand, GetNotifications>();

var app = builder.Build();

app.UseCors("NewPolicy");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

await app.Services.GetRequiredService<TgBotClient>().SetWebhook();

//NotificationScheduler.Start();

app.Run();
