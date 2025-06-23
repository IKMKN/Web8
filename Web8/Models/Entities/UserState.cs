using Web8.Models.Enums;

namespace Web8.Models.Entities;

public class UserState
{
    public int UserStateId { get; set; }
    public UserStateCode UserStateCode { get; set; }
    public string Description { get; set; }

    public List<User> Users { get; set; }
}

