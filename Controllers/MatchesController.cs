namespace Tournament.Controllers
{
    using Microsoft.AspNetCore.Authorization;
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
            DateTime startDate = DateTime.Now.AddDays(7); // Започваме след една седмица
            //DateTime startDate = DateTime.Parse("2025,01,24");

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
                    Id = m.Id,
                    HomeTeam = m.HomeTeam.Name,
                    AwayTeam = m.AwayTeam.Name,
                    MatchDate = m.MatchDate,
                    HomeTeamGoals = m.HomeTeamGoals,
                    AwayTeamGoals = m.AwayTeamGoals
                })
                .ToList();

            return View(matches);
        }

        [Authorize(Roles = "Editor")]
        public IActionResult Edit(int id)
        {
            var match = data.Matches
                .Where(m => m.Id == id)
                .Select(m => new MatchFormModel
                {
                    Id = m.Id,
                    HomeTeam = m.HomeTeam.Name,
                    AwayTeam = m.AwayTeam.Name,
                    MatchDate = m.MatchDate,
                    HomeTeamGoals = m.HomeTeamGoals,
                    AwayTeamGoals = m.AwayTeamGoals
                })
                .FirstOrDefault();

            if (match == null)
            {
                return NotFound();
            }

            return View(match);
        }

        [HttpPost]
        [Authorize(Roles = "Editor")]
        public IActionResult Edit(int id, MatchFormModel match)
        {
            if (!ModelState.IsValid)
            {
                return View(match);
            }

            var matchData = this.data.Matches
                .FirstOrDefault(m => m.Id == id);

            if (matchData == null)
            {
                return NotFound();
            }

            // Взимаме отборите по Id
            var homeTeam = this.data.Teams.FirstOrDefault(t => t.Id == matchData.HomeTeamId);
            var awayTeam = this.data.Teams.FirstOrDefault(t => t.Id == matchData.AwayTeamId);

            if (homeTeam == null || awayTeam == null)
            {
                return NotFound();
            }

            // Премахваме старите резултати от статистиките
            UpdateTeamStats(homeTeam, matchData.HomeTeamGoals, matchData.AwayTeamGoals, removeOld: true);
            UpdateTeamStats(awayTeam, matchData.AwayTeamGoals, matchData.HomeTeamGoals, removeOld: true);


            matchData.HomeTeamGoals = match.HomeTeamGoals;
            matchData.AwayTeamGoals = match.AwayTeamGoals;
            matchData.MatchDate = match.MatchDate;
            //new results
            matchData.HomeTeamGoals = match.HomeTeamGoals;
            matchData.AwayTeamGoals = match.AwayTeamGoals;
            data.SaveChanges();

            return RedirectToAction(nameof(Index));

           
        } 
        private void UpdateTeamStats(Team team, int scored, int conceded, bool removeOld = false)
            {
                if (removeOld)
                {
                    // Връщаме старите резултати (ако мачът се редактира)
                    if (scored > conceded)
                    {
                        team.Wins--;
                    }
                    else if (scored < conceded)
                    {
                        team.Losts--;
                    }
                }
                else
                {
                    // Добавяме новите резултати
                    if (scored > conceded)
                    {
                        team.Wins++;
                    }
                    else if (scored < conceded)
                    {
                        team.Losts++;
                    }
                }

                // Обновяваме головете
                team.GoalsScored += removeOld ? -scored : scored;
                team.GoalsConceded += removeOld ? -conceded : conceded;
            }

    }

}
