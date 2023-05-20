using System.ComponentModel.DataAnnotations;

namespace Shorti.Shared.Contracts.Identity
{
    public class LoginDto
    {
        [Required]
        public string UserName { get; set; } = null!;

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; } = null!;
    }
}
