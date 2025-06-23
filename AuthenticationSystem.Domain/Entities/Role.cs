using AuthenticationSystem.Domain.Enums;

namespace AuthenticationSystem.Domain.Entities;

public class Role : Entity<int> 
{
    public string Name { get; private set; }
    
    public Permissions Permissions { get; private set; } 
    public IReadOnlyList<User> Users { get; private set; } = new List<User>();
    public Role(string name)
    {
        this.Name = name;
    }
    public Role(string name, Permissions permissions)
        : this(default, name, permissions)
    {
    }

    public Role(int id, string name, Permissions permissions)
    {
        Id = id;
        Name = name;
        Permissions = permissions;
    }
    private Role()  // Called by ef core
    {
    }
    public void SetName(string name)
    {
        this.Name = name;
    }
    public void GrantPermission(Permissions permissions)
    {
        this.Permissions = (this.Permissions | permissions);
    }
    public void RevokePermission(Permissions permissions)
    {
        this.Permissions = (this.Permissions & ~permissions);
    }
}
