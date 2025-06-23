using Web8.Models.Enums;

namespace Web8.Models.Entities;

public class UserGroup
{
    public int UserGroupId { get; set; }
    public UserGroupCode UserGroupCode{ get; set; }
    public string Description { get; set; }

    public List<User> Users { get; set; }
}
