namespace Gpt_Turnir.Controllers
{
    using Gpt_Turnir.Models.Home;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class TeamsController : Controller
    {
        public IActionResult All()
        {
            var team = new List<IndexModel>()
            {
                new IndexModel{
                Id=1,
                Name="Team A",
                City="City A",
                Coach="Penev",
                Wins=1,
                Losts=0 },
                new IndexModel{
                Id=2,
                Name="Team B",
                City="City B",
                Coach="Iliev",
                Wins=6,
                Losts=2 },
            }
            .ToList();

            return View(team);
        }
    }
}
