namespace AuthenticationSystem.Infrastructure.Data.Configurations;

internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        builder.HasKey(role => role.Id);

        builder.Property(role => role.Permissions)
               .HasConversion<int>();

        builder.HasMany(role => role.Users)
               .WithMany(user => user.Roles)
               .UsingEntity<UserRole>();

        builder.HasData(
           new Role(
               id: 1,
               name: "Admin",
               permissions: Permissions.All
           ),
           new Role(
               id: 2,
               name: "User",
               permissions: Permissions.CanRead | Permissions.CanUpdate
           )
       );

        builder.ToTable("Roles");
    }
}