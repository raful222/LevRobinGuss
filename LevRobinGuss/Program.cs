using LevRobinGuss.Core.Data;
using LevRobinGuss.Core.Entities.Identity;
using LevRobinGuss.Service;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;
using System.Globalization;
using System.Text.Json;
using Quartz;
using LevRobinGuss.Jobs;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("Initializing host");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection"),
                x => x.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
            /*EnableSensitiveDataLogging()*/);

    builder.Services.AddIdentity<ApplicationUser, ApplicationRole>(options => options.SignIn.RequireConfirmedAccount = false)
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddUserManager<ApplicationUserManager>()
        .AddDefaultTokenProviders();

    builder.Services.Configure<IdentityOptions>(options =>
    {
        // Password settings.
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 6;
        options.Password.RequiredUniqueChars = 1;

        // Lockout settings.
        options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.AllowedForNewUsers = true;

        // User settings.
        options.User.RequireUniqueEmail = true;
    });

    builder.Services.Configure<SecurityStampValidatorOptions>(options =>
        options.ValidationInterval = TimeSpan.FromMinutes(30));

    builder.Services.ConfigureApplicationCookie(options =>
    {
        // Cookie settings
        options.Cookie.HttpOnly = true;
        options.ExpireTimeSpan = TimeSpan.FromDays(30);

        options.LoginPath = "/Identity/Account/Login";
        options.LogoutPath = "/Identity/Account/Logout";
        options.AccessDeniedPath = "/Identity/Account/AccessDenied";
        options.SlidingExpiration = true;
    });

    builder.Services.AddHttpContextAccessor();

    builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
        .AddCookie(options =>
        {
            options.Cookie.Name = "MyAuthCookie";
            options.Cookie.SameSite = SameSiteMode.Strict;
            options.Cookie.HttpOnly = true;
            options.ExpireTimeSpan = TimeSpan.FromDays(30);
            options.SlidingExpiration = true;
        });


    builder.Services.AddControllersWithViews()
        .AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase);

    builder.Services.AddRazorPages()
        .AddMvcOptions(options =>
        {
            options.MaxModelValidationErrors = 50;
            options.ModelBindingMessageProvider.SetValueMustNotBeNullAccessor(
                _ => "The field is required.");
        });

    //builder.Services.AddSingleton<IValidationAttributeAdapterProvider,
    //               CustomValidationAttributeAdapterProvider>();



    builder.Services.AddTransient<FootballDataAPi>();
    builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
    builder.Services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

    builder.Services.Configure<IISServerOptions>(options =>
    {
        options.MaxRequestBodySize = int.MaxValue;
    });
    builder.Services.AddQuartz(q =>
    {
        q.UseMicrosoftDependencyInjectionScopedJobFactory();
        // Just use the name of your job that you created in the Jobs folder.
        var jobKey = new JobKey("SendEmailJob");
        q.AddJob<PayJob>(opts => opts.WithIdentity(jobKey));

        q.AddTrigger(opts => opts
            .ForJob(jobKey)
            .WithIdentity("SendEmailJob-trigger")
            //This Cron interval can be described as "run every minute" (when second is zero)
            .StartNow()
        );
    });
    builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);


#if !DEBUG
            
#endif

    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
    builder.Host.UseNLog();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        //app.UseMigrationsEndPoint();
    }
    else
    {
        app.UseDeveloperExceptionPage();
        //app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    var supportedCultures = new[]
          {
                new CultureInfo("he")
            };

    app.UseRequestLocalization(new RequestLocalizationOptions
    {
        DefaultRequestCulture = new RequestCulture("he"),
        SupportedCultures = supportedCultures,
        SupportedUICultures = supportedCultures
    });
    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    app.MapRazorPages();

    app.Run();
}
catch (Exception exception)
{
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    LogManager.Shutdown();
}

