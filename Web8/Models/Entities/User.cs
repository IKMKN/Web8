namespace Web8.Models.Entities;

public class User
{
    public Guid Id { get; set; }
    public string Login { get; set; }
    public string PasswordHash { get; set; }
    public DateTime CreatedDate { get; set; }

    public UserGroup UserGroup { get; set; }
    public int UserGroupId { get; set; }

    public UserState UserState { get; set; }
    public int UserStateId { get; set; }
}
