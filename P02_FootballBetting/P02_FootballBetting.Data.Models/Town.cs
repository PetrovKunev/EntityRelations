using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using P02_FootballBetting.Data.Common;

namespace P02_FootballBetting.Data.Models
{
    public class Town
    {

        [Key]
        public int TownId { get; set; }

        [MaxLength(ValidationConstants.TownNameMaxLength)]
        [Required]
        public string Name { get; set; }

        public int CountryId { get; set; }
        [ForeignKey(nameof(CountryId))]
        public Country Country { get; set; }
    }
}
