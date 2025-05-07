namespace Web8.Models.Entities;

public class UserState
{
    public int UserStateId { get; set; }
    public UserStateCode UserStateCode { get; set; }
    public string Description { get; set; }
    public List<User> Users { get; set; }
}

public enum UserStateCode
{
    Active = 1,
    Blocked = 2
}