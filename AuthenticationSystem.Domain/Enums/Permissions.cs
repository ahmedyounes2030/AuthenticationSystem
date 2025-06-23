[Flags]
public enum Permissions
{
    None = 0,
    CanRead = 1,    // 0001
    CanAdd = 2,     // 0010
    CanUpdate = 4,  // 0100
    CanDelete = 8,  // 1000
    All = CanRead | CanAdd | CanUpdate | CanDelete // 1111 (15)
}