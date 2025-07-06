using Domain.Enums;

namespace Domain.Entities;

public class UserState
{
    public int UserStateId { get; set; }
    public UserStateCode UserStateCode { get; set; }
    public string Description { get; set; }

    public List<User> Users { get; set; }
}

