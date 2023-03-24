namespace Backend.Infrastructure;
public static class DatabaseManagementService
{
  // Getting the scope of our database context
  public static void MigrationInitialisation(IApplicationBuilder app)
  {
    using (var serviceScope = app.ApplicationServices.CreateScope())
    {
      var context = serviceScope.ServiceProvider.GetService<MinitwitContext>()!.Database;
      if(context.GetPendingMigrations().Any()){
        // Takes all of our migrations files and apply them against the database in case they are not implemented
        context.Migrate();
      }
    }
  }
}