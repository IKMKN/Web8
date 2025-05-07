namespace Web8.Models.Responses;

public class UserResponse
{
    public Guid UserId { get; set; }
    public string Login { get; set; }
    public DateTime CreatedDate { get; set; }

    public UserGroupResponce UserGroup { get; set; }
    public UserStateResponse UserState { get; set; }
}
