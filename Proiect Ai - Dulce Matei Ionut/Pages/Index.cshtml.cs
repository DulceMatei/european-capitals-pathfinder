using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Hosting;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace Proiect_Ai___Dulce_Matei_Ionut.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly IWebHostEnvironment _environment;
        private readonly PathFinder _pathFinder;

        [BindProperty(SupportsGet = true)]
        public List<string> SelectedCountries { get; set; } = new List<string>();

        public List<string> Countries { get; set; }

        public IndexModel(ILogger<IndexModel> logger, IWebHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
            var jsonPath = Path.Combine(_environment.WebRootPath, "countries.json");
            _pathFinder = new PathFinder(jsonPath);
        }

        public void OnGet()
        {
            Countries = _pathFinder.GetCountries(); // Initializeaza lista tarilor

        }


        public JsonResult OnPostSelectCountry(string countryId)
        {
            var selected = TempData["SelectedCountries"] as List<string> ?? new List<string>();

            if (selected.Contains(countryId))
            {
                return new JsonResult(new { success = false, message = "Tara este deja selectata" });
            }

            if (selected.Count >= 2)
            {
                return new JsonResult(new { success = false, message = "Nu poti selecta mai mult de 2 tari!" });
            }

            selected.Add(countryId);
            TempData["SelectedCountries"] = selected;

            return new JsonResult(new { success = true });
        }

        public JsonResult OnPostResetSelection()
        {
            TempData["SelectedCountries"] = new List<string>();
            return new JsonResult(new { success = true });
        }


        public JsonResult OnPostFindPath(string startCountry, string endCountry, string algorithm)
        {
            List<string> path = algorithm switch
            {
                "bfs" => _pathFinder.BFSPath(startCountry, endCountry),
                "astar" => _pathFinder.AStarPath(startCountry, endCountry),
                _ => null
            };

            if (path == null)
                return new JsonResult(new { success = false });

            double distance = _pathFinder.CalculatePathDistance(path);
            var pathWithCapitals = path.Select(country => $"{country} ({_pathFinder.GetCapital(country)})").ToList();

            return new JsonResult(new
            {
                success = true,
                path = path,
                pathWithCapitals = pathWithCapitals,
                distance = Math.Round(distance, 2),
                executionTime = $"{_pathFinder.ExecutionTime.TotalMilliseconds:F2} ms",
                nodesVisited = _pathFinder.NodesVisited
            });
        }
    }
}