namespace AuthenticationSystem.Infrastructure.Data.DbContexts;

internal class DbContextDesignTimeFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        optionsBuilder.UseSqlServer("Server = DESKTOP-B35KRGE ;Database =AdvanceAuthenticationSystem  ;Integrated Security = SSPI; TrustServerCertificate = True");
        return new ApplicationDbContext(optionsBuilder.Options);
    }
}