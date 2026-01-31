
using Is.Domain.Email;
using Is.Domain.Identity;
using Is.Repository;
using Is.Repository.Implementation;
using Is.Repository.Interface;
using Is.Services;
using Is.Services.Implementation;
using Is.Services.Interface;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("init main");
try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddRazorPages();

    // Add services to the container.
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
    builder.Services.AddDbContext<Is.Repository.ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString,
        sql => sql.EnableRetryOnFailure(
            maxRetryCount: 5,
            maxRetryDelay: TimeSpan.FromSeconds(10),
            errorNumbersToAdd: null
        )
        ));
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();

    //builder.Services.AddIdentity<IsApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = false)
    builder.Services.AddDefaultIdentity<IsApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders(); 



    // var emailService = new EmailSettings();
    // builder.Configuration.GetSection("EmailSettings").Bind(emailService);

    //  builder.Services.AddScoped<EmailSettings>(es => emailService);
    // builder.Services.AddScoped<IEmailService, EmailService>(email => new EmailService(emailService));

    builder.Services.Configure<EmailSettings>(
        builder.Configuration.GetSection("EmailSettings")
        );
    builder.Services.AddScoped<IEmailService,EmailService>();
    

    builder.Services.AddScoped<IBackgroundEmailSender, BackgroundEmailSender>();
    builder.Services.AddHostedService<ConsumeScopedHostedService>();

    builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
    builder.Services.AddScoped(typeof(IUserRepository), typeof(UserRepository));
    builder.Services.AddScoped(typeof(IOrderRepository), typeof(OrderRepository));

    builder.Services.AddTransient<ICourseService, CourseService>();
    builder.Services.AddTransient<IMyCoursesCardService, MyCoursesCardService>();
    builder.Services.AddTransient<IEmailService, EmailService>();
    builder.Services.AddTransient<IOrderService, OrderService>();

    builder.Services.AddControllersWithViews()
        .AddNewtonsoftJson(
        o=>
        o.SerializerSettings.ReferenceLoopHandling= Newtonsoft.Json.ReferenceLoopHandling.Ignore
)
        ;

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    var app = builder.Build();

    using (var scope = app.Services.CreateScope())
    {
        var dbContext=scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        if (dbContext.Database.IsRelational())
        {
            dbContext.Database.Migrate();
        }
    }

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseMigrationsEndPoint();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }

    app.UseHttpsRedirection();
    app.UseRouting();


    app.UseAuthentication();

    app.UseAuthorization();

    app.MapStaticAssets();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}")
        .WithStaticAssets();

    app.MapRazorPages()
       .WithStaticAssets();

    app.Run();
}
catch(Exception e)
{
    logger.Error(e, "Stopped program beacuse of exception");
    throw;
}
finally
{
    NLog.LogManager.Shutdown();
}
