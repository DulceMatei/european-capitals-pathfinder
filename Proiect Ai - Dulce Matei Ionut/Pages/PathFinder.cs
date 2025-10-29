using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.IO;

public class PathFinder
{
    private Dictionary<string, CountryInfo> countries;

    public object CountriesData { get; internal set; }

    public class CountryInfo
    {
        public string Capital { get; set; }
        public Dictionary<string, int> Neighbors { get; set; }
        public Coordinates Coordinates { get; set; }
    }

    public class Coordinates
    {
        public double Lat { get; set; }
        public double Lon { get; set; }
    }

    public PathFinder(string jsonPath)
    {
        var jsonString = File.ReadAllText(jsonPath);
        var data = JsonSerializer.Deserialize<RootObject>(jsonString);
        countries = data.Countries;
    }

    public class RootObject
    {
        public Dictionary<string, CountryInfo> Countries { get; set; }
    }
    public List<string> GetCountries()
    {
        return countries.Keys.ToList();
    }

    public int NodesVisited { get; private set; }
    public TimeSpan ExecutionTime { get; private set; }

    // Breadth-First Search (BFS)
    public List<string> BFSPath(string start, string end)
    {
        // Reset counters
        NodesVisited = 0;
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        var queue = new Queue<List<string>>();
        var visited = new HashSet<string>();
        queue.Enqueue(new List<string> { start });
        visited.Add(start);
        NodesVisited++;

        while (queue.Count > 0)
        {
            var path = queue.Dequeue();
            var lastCountry = path[path.Count - 1];

            if (lastCountry == end)
            {
                stopwatch.Stop();
                ExecutionTime = stopwatch.Elapsed;
                return path;
            }

            foreach (var neighbor in countries[lastCountry].Neighbors.Keys)
            {
                if (!visited.Contains(neighbor))
                {
                    var newPath = new List<string>(path) { neighbor };
                    queue.Enqueue(newPath);
                    visited.Add(neighbor);
                    NodesVisited++;
                }
            }
        }

        stopwatch.Stop();
        ExecutionTime = stopwatch.Elapsed;
        return null; // No path found
    }

    // A* Search
    public List<string> AStarPath(string start, string end)
    {
        // Reset counters
        NodesVisited = 0;
        var stopwatch = System.Diagnostics.Stopwatch.StartNew();

        var openSet = new PriorityQueue<List<string>, double>();
        var visited = new HashSet<string>();
        openSet.Enqueue(new List<string> { start }, 0);

        while (openSet.Count > 0)
        {
            var currentPath = openSet.Dequeue();
            var lastCountry = currentPath[currentPath.Count - 1];

            if (lastCountry == end)
            {
                stopwatch.Stop();
                ExecutionTime = stopwatch.Elapsed;
                return currentPath;
            }

            if (!visited.Contains(lastCountry))
            {
                visited.Add(lastCountry);
                NodesVisited++;

                foreach (var neighbor in countries[lastCountry].Neighbors)
                {
                    if (!visited.Contains(neighbor.Key))
                    {
                        var newPath = new List<string>(currentPath) { neighbor.Key };
                        var distance = CalculatePathDistance(newPath);
                        var heuristic = EstimateDistance(neighbor.Key, end);
                        openSet.Enqueue(newPath, distance + heuristic);
                    }
                }
            }
        }

        stopwatch.Stop();
        ExecutionTime = stopwatch.Elapsed;
        return null; // No path found
    }

    public double CalculatePathDistance(List<string> path)
    {
        double totalDistance = 0;
        for (int i = 0; i < path.Count - 1; i++)
        {
            totalDistance += countries[path[i]].Neighbors[path[i + 1]];
        }
        return totalDistance;
    }

    private double EstimateDistance(string country1, string country2)
    {
        var coord1 = countries[country1].Coordinates;
        var coord2 = countries[country2].Coordinates;

        // Haversine formula for geographic distance
        var R = 6371; // Earth's radius in km
        var dLat = ToRadians(coord2.Lat - coord1.Lat);
        var dLon = ToRadians(coord2.Lon - coord1.Lon);

        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRadians(coord1.Lat)) * Math.Cos(ToRadians(coord2.Lat)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return R * c;
    }

    private double ToRadians(double degrees)
    {
        return degrees * (Math.PI / 180);
    }

    public string GetCapital(string country)
    {
        if (countries.ContainsKey(country))
        {
            return countries[country].Capital;
        }
        return "Unknown";
    }


}