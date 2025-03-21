namespace Gpt_Turnir.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    using static DataConstants.Team;

    public class Team
    {
        public int Id { get; set; }

        [Required]
        [StringLength(NameMaxLength,MinimumLength =NameMinLength)]
        public string Name { get; set; }

        [Required]
        [StringLength(CityMaxLength, MinimumLength = CityMinLength)]
        public string City { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Trener")]
        public string Trener { get; set; }

        public int Wins { get; set; } = 0; 

        public int Losts { get; set; } = 0;

    }
}
