using Invoicing.Api.Database;
using Invoicing.Api.Repositories;
using Invoicing.Api.Services;
using Invoicing.Api.Auth;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddScoped<EmailService>();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Register database + repositories + services
builder.Services.AddSingleton<DbHelper>();
builder.Services.AddScoped<AuthRepository>();
builder.Services.AddScoped<InvoiceRepository>();
builder.Services.AddScoped<InvoiceService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Custom API-key authentication middleware (NO JWT)
app.UseMiddleware<ApiKeyMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
