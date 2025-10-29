# 🌍 European Capitals Pathfinder

A C# project (Visual Studio 2022) that visualizes and compares **BFS** and **A\*** algorithms for finding routes between European capitals.

## 🧠 Description
This application allows the user to:
- Select two European countries directly on an interactive SVG map
- Calculate the shortest path between their capitals
- Display the route visually
- Measure performance (execution time, nodes explored, total distance)

## ⚙️ Features
- BFS (Breadth-First Search)  
- A* Search with heuristic  
- Interactive SVG map integration  
- Path visualization  
- Performance metrics: time, nodes explored, distance  

## 🛠️ Technologies
- C# (.NET, Visual Studio 2022)  
- Windows Forms / WPF  
- Stopwatch for execution time  
- SVG rendering for map 

### 📊 Algorithm Examples

**BFS Example:**  
Path Details:  
Sweden (Stockholm) → Denmark (Copenhagen) → Germany (Berlin) → Poland (Warsaw) → Ukraine (Kyiv) → Romania (Bucharest) → Bulgaria (Sofia) → Turkey (Ankara)  

Total Distance: 3979 km  
Algorithm Performance:  
Execution Time: 0.24 ms  
Nodes Visited: 41  

![BFS Algorithm](gif/BFS.gif)

**A* Example:**  
Path Details:  
Sweden (Stockholm) → Denmark (Copenhagen) → Germany (Berlin) → Czech Republic (Prague) → Slovakia (Bratislava) → Hungary (Budapest) → Serbia (Belgrade) → Bulgaria (Sofia) → Turkey (Ankara)  

Total Distance: 3108 km  
Algorithm Performance:  
Execution Time: 1.84 ms  
Nodes Visited: 12  

![A* Algorithm](gif/A_Search.gif)

## ✅ Conclusion
This project demonstrates how different pathfinding algorithms, BFS and A*, perform when finding routes between European capitals. It highlights differences in path efficiency, number of nodes explored, and execution time, providing a clear visual and quantitative comparison. The application can be used for educational purposes or as a base for further route optimization projects.
