namespace AuthenticationSystem.Infrastructure.Data.Configurations;

internal sealed class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Name).HasMaxLength(50);
        builder.Property(x => x.Value).IsRequired();

        // Get base permissions (single-bit values)
        var permissions = Enum.GetValues<Permissions>()
            .Where(p => p != Permissions.None)
            .Select(p => new
            {
                EnumValue = p,
                IntValue = (int)(object)p
            })
            .Where(x => IsSingleBit(x.IntValue)) // Check for single-bit values
            .Select(x => new Permission(
                id: x.IntValue,       // Use bit value as ID
                name: x.EnumValue.ToString(),
                value: x.IntValue
            ))
            .ToList();

        builder.HasData(permissions);
        builder.ToTable("Permissions");
    }

    // Helper method: checks if a value has exactly one bit set (power of two)
    private static bool IsSingleBit(int value)
    {
        return value != 0 && (value & (value - 1)) == 0;
    }
}