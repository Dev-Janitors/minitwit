using Backend.Infrastructure;
using Backend.Core.EF;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Cors
var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options => {
    options.AddPolicy(name: MyAllowSpecificOrigins, policy => {
        policy.WithOrigins("http://localhost:3000", "https://localhost:3000", "http://104.248.101.163:3000", "https://146.190.206.71/", "http://146.190.206.71/", "https://104.248.101.163");
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
    });
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "MinitwitApi", Version = "v1.0.0" });

        //👇 new code
        var securitySchema = new OpenApiSecurityScheme
        {
          Description = "Using the Authorization header with the Bearer scheme.",
          Name = "Authorization",
          In = ParameterLocation.Header,
          Type = SecuritySchemeType.Http,
          Scheme = "bearer",
          Reference = new OpenApiReference
          {
            Type = ReferenceType.SecurityScheme,
            Id = "Bearer"
          }
        };

        c.AddSecurityDefinition("Bearer", securitySchema);

        c.AddSecurityRequirement(new OpenApiSecurityRequirement
          {
              { securitySchema, new[] { "Bearer" } }
          });
});

builder.Services.AddDbContext<MinitwitContext>(
    options => options.UseSqlServer(builder.Configuration.GetConnectionString("Minitwit"))
);

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IFollowerRepository, FollowerRepository>();
builder.Services.AddScoped<IMessageRepository, MessageRepository>();

// Authentication - Auth0
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.Authority = "https://dev-m7wdzvfk.eu.auth0.com/";
    options.Audience = builder.Configuration.GetConnectionString("AUTH0_AUDIENCE");
});


var app = builder.Build();

DatabaseManagementService.MigrationInitialisation(app);

// Cors
app.UseCors(MyAllowSpecificOrigins);

// Authentication
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();