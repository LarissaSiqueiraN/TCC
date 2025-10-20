using API.Configuration;
using DAL;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddControllersWithViews();

builder.Configuration.AddEnvironmentVariables().Build();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerConfiguration();

var stringConexao = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<BancoAPIContext>(options => options.UseLazyLoadingProxies().UseSqlServer(stringConexao));

builder.Services.WebApiConfig();

builder.Services.AddIdentityConfiguration(builder.Configuration);

builder.Services.AddAutoMapper(Assembly.GetEntryAssembly());
builder.Services.ResolveDependencies();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Development", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader()
               .WithExposedHeaders("Content-Disposition");
    });

    options.AddPolicy("Production", builder =>
    {
        builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders("Content-Disposition");
    });
}); 

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();

app.UseRouting();

if (app.Environment.IsDevelopment())
{
    app.UseCors("Development");
}
else
{
    app.UseCors("Production");
}


app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();