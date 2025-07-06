using System.ComponentModel.DataAnnotations;

namespace API.Models.Requests;

public class CreateUserRequest
{
    [Required] public string Login { get; set; }
    [Required] public string Password { get; set; }
    [Required] public int UserGroupId { get; set; }
}
