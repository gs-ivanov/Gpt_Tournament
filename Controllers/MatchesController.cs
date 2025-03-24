namespace Tournament.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Tournament.Data;
    using Tournament.Data.Models;
    using Tournament.Models.Matches;

    using static WebConstants;

    public class MatchesController : Controller
    {
        private readonly TurnirDbContext data;

        public MatchesController(TurnirDbContext data)
        {
            this.data = data;
        }

        public IActionResult GenerateSchedule()
        {
            var teams = data.Teams.ToList();
            if (teams.Count != 4)
            {
                TempData[GlobalMessageKey] = "Трябва да има точно 4 отбора, за да се генерира график.";
                return RedirectToAction("All", "Teams");
            }

            List<Match> matches = new List<Match>();
            //DateTime startDate = DateTime.Now.AddDays(7); // Започваме след една седмица
            DateTime startDate = DateTime.Parse("2025,01,24");

            for (int i = 0; i < teams.Count; i++)
            {
                for (int j = i + 1; j < teams.Count; j++)
                {
                    matches.Add(new Match { HomeTeamId = teams[i].Id, AwayTeamId = teams[j].Id, MatchDate = startDate });
                    matches.Add(new Match { HomeTeamId = teams[j].Id, AwayTeamId = teams[i].Id, MatchDate = startDate });
                    startDate = startDate.AddDays(7);
                }
            }

            data.Matches.AddRange(matches);
            data.SaveChanges();

            TempData["Success"] = "Графикът е успешно генериран!";
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Index()
        {
            var matches = data.Matches
                .OrderBy(m => m.MatchDate)
                .Select(m => new MatchViewModel
                {
                    HomeTeam = m.HomeTeam.Name,
                    AwayTeam = m.AwayTeam.Name,
                    MatchDate = m.MatchDate
                })
                .ToList();

            return View(matches);
        }
    }

}
