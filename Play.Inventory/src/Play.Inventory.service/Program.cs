using Play.common.MassTransit;
using Play.common.MongoDB;
using Play.Inventory.service.Client;
using Play.Inventory.service.Entities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMongo()
    .AddMongoReposiory<InventoryItem>("inventoryitems")
    .AddMongoReposiory<CatalogItem>("CatalogItems")
    .AddMassTransitWithRabbitMQ();


builder.Services.AddHttpClient<CatalogClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7168/");
});
//.AddPolicyHandler(Policy.TimeoutAsync<HttpResponseMessage>(1));
builder.Services.AddControllers();
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
