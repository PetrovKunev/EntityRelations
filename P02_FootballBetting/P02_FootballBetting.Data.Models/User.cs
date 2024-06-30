
using P02_FootballBetting.Data.Common;
using System.ComponentModel.DataAnnotations;

namespace P02_FootballBetting.Data.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [MaxLength(ValidationConstants.UsernameMaxLength)]
        public string Username { get; set; } = null!;

        [MaxLength(ValidationConstants.PasswordMaxLength)]
        public string Password { get; set; } = null!;

        [MaxLength(ValidationConstants.EmailMaxLength)]
        public string Email { get; set; }

        [MaxLength(ValidationConstants.UsersnameMaxLength)]
        public string Name { get; set; }

        public decimal Balance { get; set; }
    }
}
