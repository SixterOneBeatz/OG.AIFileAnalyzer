using OG.AIFileAnalyzer.Business;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddBusinessServices(configuration);
builder.Services.AddCors(options =>
{
    options.AddPolicy("CORS_POLICY", policy => 
    policy.WithOrigins(configuration["frontendUrl"])
          .AllowAnyMethod()
          .AllowAnyHeader());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CORS_POLICY");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
