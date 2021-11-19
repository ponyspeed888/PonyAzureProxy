using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Net;
using Yarp.ReverseProxy.Configuration;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");




#if SQLSERVER
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(
                    Configuration.GetConnectionString("DefaultConnection")));

            //builder.Services.AddDbContext<DeveloperHelperContext>(o => o.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

#else

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("DataSource=asp.net.identity.db"));
//builder.builder.Services.AddDbContext<DeveloperHelperContext>(o => o.UseSqlite("DataSource=asp.net.identity.db" ));
#endif


builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 1;
    options.Password.RequiredUniqueChars = 1;

    options.SignIn.RequireConfirmedAccount = false;
    options.SignIn.RequireConfirmedEmail = false;


    //// Lockout settings.
    //options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    //options.Lockout.MaxFailedAccessAttempts = 5;
    //options.Lockout.AllowedForNewUsers = true;

    //// User settings.
    //options.User.AllowedUserNameCharacters =
    //"abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
    //options.User.RequireUniqueEmail = false;


});



builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>().AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddServerSideBlazor();

//builder.Services.AddSyncfusionBlazor();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = $"{Globals.SWAGGER_TITLE}", Version = "v1" });
});


#if YARP
string Yarpto = Util.GetEnv("YARPFORWARD_PREFIX");
bool bHasYarpto = Yarpto != null ;

var routes = new[]
  {
        new RouteConfig()
        {
            RouteId = "route1",
            ClusterId = "cluster1",
            Match = new RouteMatch
            {
                Path = "{**catch-all}"
            }
        }
    };
var clusters = new[]
{
        new ClusterConfig()
        {
            ClusterId = "cluster1",
            HttpClient = new HttpClientConfig () { DangerousAcceptAnyServerCertificate = true  }, 
            Destinations = new Dictionary<string, DestinationConfig>(StringComparer.OrdinalIgnoreCase)
            {
                { "destination1", new DestinationConfig() { Address = Yarpto } }
            }
        }
    };

if (bHasYarpto)
    builder.Services.AddReverseProxy().LoadFromMemory(routes, clusters);



#endif

#if YARPFULL

//builder.Services.AddReverseProxy().LoadFromMemory(GetRoutes(), GetClusters());

#endif

#if YARPFORWARD
builder.Services.AddHttpForwarder();
#endif




builder.Services.AddControllersWithViews();

#if LOGGING

builder.Services.AddW3CLogging(logging =>
{
    // Log all W3C fields
    logging.LoggingFields = Microsoft.AspNetCore.HttpLogging.W3CLoggingFields.All;

    logging.FileSizeLimit = 5 * 1024 * 1024;
    logging.RetainedFileCountLimit = 2;
    logging.FileName = Globals.W3CLOGFILENAME ;
    logging.LogDirectory = Globals.W3CLOGDIR ;
    logging.FlushInterval = TimeSpan.FromSeconds(2);
});

#endif

var app = builder.Build();

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

IWebHostEnvironment env = app.Services.GetRequiredService<IWebHostEnvironment>();
DiagnosticListener diagnosticSource = app.Services.GetRequiredService<DiagnosticListener>();


app.UseSwagger();

//app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{Globals.SWAGGER_TITLE}"));

app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{Globals.SWAGGER_TITLE}");

    var sidebar = Path.Combine(env.ContentRootPath, "wwwroot/custom-sidebar.html");
    c.HeadContent = File.ReadAllText(sidebar);
    c.InjectStylesheet("/swagger-custom.css");
});



app.UseHttpsRedirection();
app.UseStaticFiles();


#if LOGGING
app.UseW3CLogging();
app.UseHttpLogging();


using var myDiagListener = new MyDiagnosticListener(diagnosticSource, new []  { "Microsoft.AspNetCore.Hosting.HttpRequestIn.Start", "Microsoft.AspNetCore.Server.Kestrel.BadRequest" },
    (pair) =>
    {
        MyDiagnosticListener.ProcessBadRequest(pair, (x) => { app.Logger.LogError(x.Error, "Bad request received"); } );


        if ( pair.Key == "Microsoft.AspNetCore.Hosting.HttpRequestIn.Start")
        {
            app.Logger.LogError("<Microsoft.AspNetCore.Http.DefaultHttpContext", "defaultContextFeature received");


        } ;



    });




#endif



app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();





#if YARPFORWARD

var httpClient = new HttpMessageInvoker(new SocketsHttpHandler()
{
    UseProxy = false,
    AllowAutoRedirect = false,
    AutomaticDecompression = DecompressionMethods.None,
    UseCookies = false
});
var transformer = new CustomTransformer(); // or HttpTransformer.Default;
var requestConfig = new Yarp.ReverseProxy.Forwarder.ForwarderRequestConfig { ActivityTimeout = TimeSpan.FromSeconds(100) };



Yarp.ReverseProxy.Forwarder.IHttpForwarder forwarder = app.Services.GetRequiredService<Yarp.ReverseProxy.Forwarder.IHttpForwarder>();


string? YARPFORWARD_SERVER = Environment.GetEnvironmentVariable("YARPFORWARD_SERVER");

if (YARPFORWARD_SERVER == null)
    YARPFORWARD_SERVER = "http://ponyspeed888.ddns.net/";

string? YARPFORWARD_PREFIX = Environment.GetEnvironmentVariable("YARPFORWARD_PREFIX");
//YARPFORWARD_PREFIX = @"192.168.0.253.nip.io:44310";
if ( YARPFORWARD_PREFIX == null)
{
    YARPFORWARD_PREFIX = "";
}
else if (YARPFORWARD_PREFIX.Trim () == "" || YARPFORWARD_PREFIX.Trim() == "/" )
{
    YARPFORWARD_PREFIX = "";

}
else
  YARPFORWARD_PREFIX += "/";

app.UseEndpoints(endpoints =>
{
    endpoints.Map($"{YARPFORWARD_PREFIX}{{**catch-all}}", async httpContext =>
    //endpoints.Map($"{Globals.YARPFORWARD_PREFIX}/{{**catch-all}}", async httpContext =>
    {
        var error = await forwarder.SendAsync(httpContext, "http://192.168.0.253.nip.io:5010/",
        //var error = await forwarder.SendAsync(httpContext, "https://192.168.0.253.nip.io:44310/",
        //var error = await forwarder.SendAsync(httpContext, "https://localhost:44310/",
            httpClient, requestConfig, transformer);
        // Check if the operation was successful
        if (error != Yarp.ReverseProxy.Forwarder.ForwarderError.None)
        {
            var errorFeature = httpContext.GetForwarderErrorFeature();
            var exception = errorFeature.Exception;
        }
    });
});

#endif





#if YARP
if (bHasYarpto)
    app.MapReverseProxy();
#endif




app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapBlazorHub();
app.MapRazorPages();



app.Run();
