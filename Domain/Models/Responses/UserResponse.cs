namespace Domain.Models.Responses;

public class UserResponse
{
    public long UserId { get; set; }
    public string Login { get; set; }
    public DateTime CreatedDate { get; set; }

    public UserGroupResponce UserGroup { get; set; }
    public UserStateResponse UserState { get; set; }
}
