global using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure;

public class MinitwitContext : DbContext 
{
    public DbSet<User> users => Set<User>();
    public DbSet<Message> messages => Set<Message>();
    public DbSet<Follower> followers => Set<Follower>();
    
    public MinitwitContext(DbContextOptions<MinitwitContext> options) : base(options) {}
}
