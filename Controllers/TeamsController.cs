namespace Tournament.Controllers
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Linq;
    using System.Collections.Generic;
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

        public IActionResult AllTeams([FromQuery] TeamsQueryModel query)
        {
            var teamsQuery = this.data.Teams.AsQueryable();

            var teams = teamsQuery
                .OrderBy(t => t.Wins)
                .ThenBy(t => t.Losts)
                .Select(t => new TeamsQueryModel
                {
                    Id = t.Id,
                    Name = t.Name,
                    City = t.City,
                    Trener = t.Trener,
                    Wins = t.Wins,
                    Losts = t.Losts,
                    IsEditable = this.data.Matches.Any()
        })
                .ToList();

            return View(teams);

        }

        [Authorize(Roles = "Administrator")]
        public IActionResult Add()
        {
            var teamsCount = this.data.Teams.Count();

            if (teamsCount>=4)
            {
                TempData[GlobalMessageKey] = "Турнират е ограничен само за четири отбора!";
                return RedirectToAction(nameof(AllTeams));
            }
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
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

            TempData[GlobalMessageKey] = "Your team was added and is awaiting for approval!";
           
            return RedirectToAction(nameof(AllTeams));
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult EditTeam(int Id)
        {
            var userid = this.User.Id();

            var team = this.data.Teams
                .Where(t => t.Id == Id)
                .Select(t=>new TeamFormModel
                {
                    Id=t.Id,
                    Name = t.Name,
                    City = t.City,
                    Trener = t.Trener,
                    Wins = t.Wins,
                    Losts = t.Losts
                })
                .FirstOrDefault();

            TempData[GlobalMessageKey] = "Поредно редактиране.";

            return View(team);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult EditTeam(TeamFormModel team)
        {
            if (!ModelState.IsValid)
            {
                return View(team);
            }

            var teamData = new Team()
            {
                Id=team.Id,
                Name = team.Name,
                City = team.City,
                Trener = team.Trener,
                Wins = team.Wins,
                Losts = team.Losts
            };

            this.data.Teams.Update(teamData);

            this.data.SaveChanges();

            TempData[GlobalMessageKey] = "Your team was edited and is awaiting for approval!";
           
            return RedirectToAction(nameof(AllTeams));
        }

        [Authorize]
        [Authorize(Roles = "Administrator")]
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

            TempData[GlobalMessageKey] = "Your team was deleted successfully!";

            return View(team);
        }

        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public IActionResult DeleteConfirmed(int id)
        {
            var team = this.data.Teams.FirstOrDefault(t => t.Id == id);
            if (team == null)
            {
                return NotFound();
            }
            this.data.Teams.Remove(team);
            this.data.SaveChanges();

            TempData[GlobalMessageKey] = "Записът за отбор: " + team.Name + "  беше изтрит успешно!";
            return RedirectToAction(nameof(AllTeams));
        }

        [Authorize(Roles = "Administrator")]
        public IActionResult ResetSchedel()
        {
            TempData[GlobalMessageKey] = "Нулирането отменено.";

            return View();
        }

        [Authorize(Roles = "Administrator")]

        public IActionResult Reset()
        {
            List <Match> itemsToDelete = this.data.Matches.ToList();
            this.data.Matches.RemoveRange(itemsToDelete);
            this.data.SaveChanges();

            TempData[GlobalMessageKey] = "Графикът е нулиран. За създаване на нов график - избери 'Generate Schedale' от дъното на екрана";

            return RedirectToAction(nameof(AllTeams));
        }


    }

}
