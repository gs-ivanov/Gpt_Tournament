namespace Gpt_Turnir.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Threading.Tasks;

    public class Team
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string City { get; set; }

        [Required]
        [StringLength(50)]
        public string Coach { get; set; }

        public int Wins { get; set; } = 0; 

        public int Losts { get; set; } = 0;

    }
}
