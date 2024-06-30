using System.ComponentModel.DataAnnotations;
using P02_FootballBetting.Data.Common;

namespace P02_FootballBetting.Data.Models
{
    public class Country
    {

        [Key]
        public int CountryId { get; set; }

        [MaxLength(ValidationConstants.CountryNameMaxLength)]
        [Required]
        public string Name { get; set; }

        public ICollection<Town> Towns { get; set; } = new HashSet<Town>();

       
    }
}
