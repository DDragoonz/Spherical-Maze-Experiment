# Spherical Maze Experiment
An experiment to generate spherical maze using recursive backtracking algorithm
![preview.gif](preview.gif)

##Feature
* Generate maze on spherical surface
* Generate Shortcut
* Instant and delayed maze generation
* auto scaling wall (default cube)

##To do
* Improve scaling algorithm
* Make posible using non cube wall prefab
* Using multiple prefab
* Grouping wall prefabs from smallest to hugest

##How to use
* Create empty game object
* Reset transform to default
* add component -> Generate Maze or Generate Maze Delayed
* Set up parameters
* Run game

###GenerateMaze parameters
* Radius 	: sphere surface radius
* Longitude	: how many wall generated verticaly? recomended value = radius * 6
* Latitude	: how many wall generated horizontaly? recomended value = radius * 3
* Latitude Cut	: how many horizontal wall on pole will be deleted? useful to make pole spacious
* Shortcut	: "how many shortcut is generated? will randomly delete wall
* Wall Prefab	: wall object, must have MazeWall component
* Scale Width	: to rescale wall so it apears larger on equator and smaller on pole


