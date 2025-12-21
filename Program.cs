using api.Data;
using api.Interfaces;
using Microsoft.EntityFrameworkCore;
using api.Repository;
using api.InterfacesService;
using api.Services;
//export DOTNET_ROOT="/opt/homebrew/opt/dotnet@8/libexec"
//export PATH="/opt/homebrew/opt/dotnet@8/bin:$PATH"
//export PATH="$PATH:/Users/matvii/.dotnet/tools"
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddNewtonsoftJson(options =>
{
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
});

builder.Services.AddControllers();
builder.Services.AddScoped<ICryptoAssetRepository, CryptoAssetRepository>();
builder.Services.AddHttpClient<ICoinGeckoService, CoinGeckoService>(client =>
{
    client.BaseAddress = new Uri("https://api.coingecko.com/api/v3/");
    client.DefaultRequestHeaders.Add("x-cg-demo-api-key", builder.Configuration["CoinGecko:DemoApiKey"]);
    client.DefaultRequestHeaders.UserAgent.ParseAdd("cryptoProject/1.0"); // інколи потрібно для Cloudflare
});
builder.Services.AddScoped<ICommentRepository, CommentRepository>();

builder.Services.AddDbContext<ApplicationDBContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.MapControllers();

app.Run();


