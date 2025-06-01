using Web8.Models.Entities;

namespace Web8.Models.Responses;

public class UserStateResponse
{
    public int UserStateId { get; set; }
    public UserStateCode UserStateCode { get; set; }
    public string Description { get; set; }
};
