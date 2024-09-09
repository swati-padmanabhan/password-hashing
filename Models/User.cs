using System.ComponentModel.DataAnnotations;

namespace PasswordHashingDemo.Models
{
    public class User
    {
        [Required]
        public virtual string Username { get; set; }

        [Required]
        public virtual string Password { get; set; }

    }
}