namespace Tournament.Controllers
{
    using Tournament.Data;
    using Tournament.Models.Home;
    using Tournament.Models.Teams;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class TeamsController : Controller
    {
        private readonly TurnirDbContext data;

        public TeamsController(TurnirDbContext data)
        {
            this.data = data;
        }

        public IActionResult All()
        {
            var team = new List<IndexModel>()
            {
                new IndexModel{
                Id=1,
                Name="Team A",
                City="City A",
                Trener="Penev",
                Wins=1,
                Losts=0 },
                new IndexModel{
                Id=2,
                Name="Team B",
                City="City B",
                Trener="Iliev",
                Wins=6,
                Losts=2 },
            }
            .ToList();

            return View(team);
        }
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize]
        public IActionResult Add(TeamFormModel team)
        {
            if (!ModelState.IsValid)
            {
                return View(team);
            }

            //this.teams.Create(
            //    team.Name,
            //    team.City,
            //    team.Description,
            //    team.TeamLogo,
            //    team.Year,
            //    team.GroupId,
            //    trenerId);

            return RedirectToAction(nameof(All));
        }
    }

}
