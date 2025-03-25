namespace Tournament.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using Tournament.Data;
    using Tournament.Data.Models;
    using Tournament.Models.Teams;
    using Tournament.Infrastructure.Extensions;

    using static WebConstants;

    public class TeamsController : Controller
    {
        private readonly TurnirDbContext data;

        public TeamsController(TurnirDbContext data)
        {
            this.data = data;
        }

        public IActionResult All([FromQuery] TeamsQueryModel query)
        {
            var teamsQuery = this.data.Teams.AsQueryable();

            var teams = teamsQuery
                .OrderByDescending(t => t.Wins)
                .ThenBy(t => t.Losts)
                .Select(t => new TeamsQueryModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    City = t.City,
                    Trener = t.Trener,
                    Wins = t.Wins,
                    Losts = t.Losts
                })
                .ToList();

            return View(teams);

        }

        [Authorize(Roles = "Administrator,Editor")]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator,Editor")]
        public IActionResult Add(TeamFormModel team)
        {
            if (!ModelState.IsValid)
            {
                return View(team);
            }

            var teamData = new Team()
            {
                Name = team.Name,
                City = team.City,
                Trener = team.Trener,
                Wins = team.Wins,
                Losts = team.Losts
            };

            this.data.Teams.Add(teamData);

            this.data.SaveChanges();

            TempData[GlobalMessageKey] = "You team was added and is awaiting for approval!";
           
            return RedirectToAction(nameof(All));
        }
        [Authorize]
        public IActionResult Edit(int Id)
        {
            var userid = this.User.Id();

            if (!User.IsAdmin())
            {
                return Unauthorized();
            }

            var teamsQuery = this.data.Teams.AsQueryable();

            var team = teamsQuery
                .Where(t => t.Id == Id)
                .Select(t=>new TeamFormModel
                {
                    Name = t.Name,
                    City = t.City,
                    Trener = t.Trener,
                    Wins = t.Wins,
                    Losts = t.Losts
                })
                .FirstOrDefault();

            return View(team);
        }

        [HttpPost]
        [Authorize]
        public IActionResult Edit(TeamFormModel team)
        {
            if (!ModelState.IsValid)
            {
                return View(team);
            }

            var teamData = new Team()
            {
                Name = team.Name,
                City = team.City,
                Trener = team.Trener,
                Wins = team.Wins,
                Losts = team.Losts
            };

            this.data.Teams.Add(teamData);

            this.data.SaveChanges();

            TempData[GlobalMessageKey] = "You team was edited and is awaiting for approval!";
           
            return RedirectToAction(nameof(All));
        }

        [Authorize]
        public IActionResult Delete(int id)
        {


            var teamsQuery = this.data.Teams.AsQueryable();

            var team = teamsQuery
                .Where(t => t.Id == id)
                .Select(t=>new TeamFormModel
                {
                    Id=t.Id,
                    Name=t.Name,
                    City = t.City,
                    Trener = t.Trener,
                    Wins = t.Wins,
                    Losts = t.Losts
                })
                .FirstOrDefault();

            TempData[GlobalMessageKey] = "You team was edited and is awaiting for approval!";

            return View(team);
        }

        [HttpPost]
        [Authorize]
        public IActionResult DeleteConfirmed(int id)
        {
            var team = this.data.Teams.FirstOrDefault(t => t.Id == id);
            if (team == null)
            {
                return NotFound();
            }
            this.data.Teams.Remove(team);
            this.data.SaveChanges();

            TempData[GlobalMessageKey] = "Записът за отбор: " +team.Name +"  беше изтрит успешно!";
            return RedirectToAction(nameof(All));
        }

    }

}
