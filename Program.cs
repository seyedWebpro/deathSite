using System.Text;
using api.Context;
using api.Middleware;
using api.Services;
using deathSite.Services.Payment;
using deathSite.View.PaymentMelat;
using deathSite.View.PaymentParsian;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;


var builder = WebApplication.CreateBuilder(args);

// 1. Configuration را از appsettings.json بارگذاری کنید:
builder.Configuration.AddJsonFile("appsettings.json");

#region policy‍
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "myCors", policy =>{

        //policy.WithOrigins("http://example.com","http://www.contoso.com");
        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });
});
#endregion policy

// HttpClient
builder.Services.AddHttpClient();

// Add services to the container.

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter()); // ثبت کانورتر
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

#region swagger 
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "SimoAPI", Version = "v1" });

    #region JWT Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme."
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement 
    {
        {
            new OpenApiSecurityScheme {
            Reference = new OpenApiReference {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
    #endregion

});
#endregion


#region JWT
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});
#endregion

#region  httpcontext
builder.Services.AddHttpContextAccessor();
#endregion

    // use in OTP 
builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

#region dbContext
builder.Services.AddDbContext<apiContext>(options =>
{
    options.UseSqlServer(builder.Configuration["defultConnection"]); //       localConnection                                                          
});


#endregion


// Register IMemoryCache
builder.Services.AddMemoryCache();

// ثبت سرویس SmsService
builder.Services.AddScoped<ISmsService, SmsService>();

builder.Services.AddScoped<ICalculatorService, CalculatorService>();

builder.Services.AddScoped<ICacheService, CacheService>();

builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddScoped<IFileUploadService, FileUploadService>();

builder.Services.AddScoped<IMelatPaymentService, MelatPaymentService>();

builder.Services.AddScoped<IParsianPaymentService, ParsianPaymentService>();

var app = builder.Build();

app.UseDeveloperExceptionPage();

// Configure the HTTP request pipeline.
// if (app.Environment.IsDevelopment())
// {
    app.UseSwagger();
    app.UseSwaggerUI(c =>{
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
        c.DocExpansion(DocExpansion.None);
    });
// }

app.UseHttpsRedirection();

app.UseRouting();

// ثبت Middleware مدیریت خطا
app.UseMiddleware<ErrorHandlingMiddleware>();

//jwt
app.UseAuthentication();

//CORS policy
app.UseCors("myCors");

app.UseAuthorization();

app.UseStaticFiles(); // برای wwwroot

app.MapControllers();

app.MapFallbackToFile("index.html");

app.Run();
