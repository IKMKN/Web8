﻿using Web8.Models.Entities;

namespace Web8.Models.Responses;

public class UserGroupResponce
{
    public int UserGroupId { get; set; }
    public UserGroupCode UserGroupCode { get; set; }
    public string Description { get; set; }
};