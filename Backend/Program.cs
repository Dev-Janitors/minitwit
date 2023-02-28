using Backend.Infrastructure;
using Backend.Core.EF;
var builder = WebApplication.CreateBuilder(args);


// Cors
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options => {
    options.AddPolicy(name: MyAllowSpecificOrigins, policy => {
        policy.WithOrigins("http://localhost:3000", "https://localhost:3000", "http://104.248.101.163:3000");
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
    });
});

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

// Cors
app.UseCors(MyAllowSpecificOrigins);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();