﻿using Domain.Enums;

namespace Domain.Entities;

public class UserGroup
{
    public int UserGroupId { get; set; }
    public UserGroupCode UserGroupCode{ get; set; }
    public string Description { get; set; }

    public List<User> Users { get; set; }
}
