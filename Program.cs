using Hangfire;
using Hangfire.BusinessServices;
using Hangfire.Dashboard;
using Hangfire.Dashboard.BasicAuthorization;
using Hangfire.MemoryStorage;
using Hangfire.MySql;
using Microsoft.Extensions.DependencyInjection;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IJobTestService, JobTestService>();

// Memory storage 
builder.Services.AddHangfire(c => c.UseMemoryStorage());


// hangfire storage sql config

//builder.Services.AddHangfire(x =>
//{
//    x.UseSqlServerStorage("Server=.;Database=HangfireApplication;Trusted_Connection=True");
//});

//hangfire storage my sql config
//builder.Services.AddHangfire(x =>
//{
//    var options = new MySqlStorageOptions();
//    options.TablesPrefix = "hangfire";
//    x.UseStorage(new MySqlStorage("persistsecurityinfo=True;port=3308;server=localhost;user id=root;password=password;database=hangfire;allowuservariables=True", options));
//});


// Running server
builder.Services.AddHangfireServer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// hangfire dashboard config
//Default will run on /hangfire 
//app.UseHangfireDashboard();

// hangfire dashboard config
//app.UseHangfireDashboard("/dashboard");

// Basic Basic authentication 
app.UseHangfireDashboard("/dashboard", new DashboardOptions
{
    Authorization = new[]
       {
            new BasicAuthAuthorizationFilter(new BasicAuthAuthorizationFilterOptions
            {
                RequireSsl = false,
                SslRedirect = false,
                LoginCaseSensitive = false,
                Users = new[]
                {
                    new BasicAuthAuthorizationUser
                    {
                        Login = "admin",
                        PasswordClear = "admin",
                    },
                },
            }),
        },
    IsReadOnlyFunc = (DashboardContext context) => true,
});

app.UseAuthorization();




app.MapControllers();

app.Run();
