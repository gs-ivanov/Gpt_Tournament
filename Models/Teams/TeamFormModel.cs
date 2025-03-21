namespace Tournament.Models.Teams
{
    using System.ComponentModel.DataAnnotations;

    using static Data.DataConstants.Team;

    public class TeamFormModel
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; init; }

        [Required]
        [StringLength(CityMaxLength, MinimumLength = CityMinLength)]
        public string City { get; init; }

        public int Trener { get; init; }

        [Display(Name = "Winning points:")]
        public int Wins { get; init; }

        [Display(Name = "Lost points:")]
        public int Losts { get; init; }

        //[Required]
        //[StringLength(
        //    int.MaxValue,
        //    MinimumLength = DescriptionMinLength,
        //    ErrorMessage = "The field Description must be a string with a minimum length of {2}.")]
        //public string Description { get; init; }

        //[Display(Name = "Team Logo")]
        //[Required]
        //public string TeamLogo { get; init; }

        //[Range(YearMinValue, YearMaxValue)]
        //[Display(Name = "Team established at Year:")]
        //public int Year { get; init; }
    }
}