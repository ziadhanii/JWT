

namespace JWT.Models;

public class ApplicationUser : IdentityUser
{
    [Required, MaxLength(20)]

    public string FirstName { get; set; }
    [Required, MaxLength(20)]

    public string LastName { get; set; }

}
