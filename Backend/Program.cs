using Backend.Infrastructure;
using Backend.Core.EF;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MinitwitContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("Minitwit"))
);

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IFollowerRepository, FollowerRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();


var app = builder.Build();

DatabaseManagementService.MigrationInitialisation(app);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();