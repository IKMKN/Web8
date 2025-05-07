using System.ComponentModel.DataAnnotations;

namespace Web8.Models.Requests;

public class CreateUserRequest
{
    [Required] public string Login { get; set; }
    [Required] public string Password { get; set; }
    [Required] public int UserGroupId { get; set; }
}
