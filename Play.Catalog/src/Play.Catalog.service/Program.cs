using Play.Catalog.service.Entities;
using Play.common.MassTransit;
using Play.common.MongoDB;
using Play.common.Settings;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

ServiceSettings? serviceSettings = configuration?.GetSection(nameof(ServiceSettings))
.Get<ServiceSettings>();

builder.Services.AddMongo()
    .AddMongoReposiory<Item>("items")
    .AddMassTransitWithRabbitMQ();




// Add services to the container.
builder.Services.AddControllers(options =>
{
    options.SuppressAsyncSuffixInActionNames = false;
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
