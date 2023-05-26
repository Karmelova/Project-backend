using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dto
{
    public class LoginUserDto
    {
        [Required]
        public string LoginName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}