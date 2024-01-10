# Programmer's Documentation

In this documentation, we will describe the most important components of each project.

## Requirements
 - [dotnet >= 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

## 1. Quoridor.Core
 This project is the backbone of the Quoridor framework. It contains all the necessary model definition required everywhere else in the solution.
 
## 1.1 NuGet packages
 - [ConcurrentHashset Version 1.3.0](https://www.nuget.org/packages/ConcurrentHashSet/)
    Necessary for thread-safe Hashset used in GameEnvironment
 - [Castle Windsor Version 5.1.1](https://www.nuget.org/packages/Castle.Windsor)
    Necessary for managing dependency injection containers for ease of workflow

## 1.2 Method and Property Descriptions

In this section, we will describe the most important properties and methods of most notable classes.

### 1.2.1 GameEnvironment
__Properties__
- __ConcurrentHashset<IWall> walls__
  Keeps a track of all placed walls. Thread safe for safe parallel minimax usage.
- __List<IPlayer> players__
  Keeps a track of all players in the current gameplay. There can be a maximum of 4 players.
- __int turn__
  Keeps a track of the current player, that can be accessed using the list of available players.

__Methods__
- __void Initialize()__ :
Reset the player location, remove all placed walls
- __void MovePlayer(IPlayer player, Direction dir)__ :
 Moves player to specified dir, if the move is legal. Direction is one of North, South, East, West
- __void AddWall(IPlayer player, Vector2 from, Direction placement)__:
    Performs several validation tests to check if wall is valid. Some of which include
    - Check if player has any wall left. Throw __NoWallRemainingException__ otherwise.
    - Check if Wall(from, placement) has already been placed or if it intersects with already placed wall. Throw one of __WallIntersectsException__ or __WallAlreadyPresentException__ exceptions otherwise.
    - Check if the wall blocks any players, Throw __NewWallBlocksPlayerException__ exception othewise.
Adds  __Wall(from, placement)__ to the list of already-placed walls if all these tests succeded.
- __public void Move(Movement move)__: Implementation of the __IMove<AgentMove>__ interface for AI integration.
    Checks if AgentMove is one of __Vector2__ move, in which case move the player to specified location or __Wall__ move, in which case adds a wall to the game environment.
- __IEnumerable<Movement> GetWalkableNeighbors(IPlayer player)__ :
    Used as a helper methof for the IValidMoves.GetValidMoves() method to get a list of all possible places a player can go to. Checks if the new possible position is legal (within bounds, not blocked by a wall), and returns them.
- __IEnumerable<Movement> GetAllUnplacedWalls()__:
    Returns a list of all possible wall segments that have not been placed yet. Performs an exhaustive search over all possible wall segments by checking various constraints, and returning them if they passed all constraints.


### 1.2.2 Wall
__Properties__
 - __Vector2 From__
    An (x,y) coordinate representing the initial point of the wall
 - __Direction placement__
    One of North, South, East, West representing the orientation of the wall

__Methods__
 - __bool Intersects(Vector2 from, Direction dir);__
    Checks if wall(from, dir) intersects the current wall, which can happen in 1 of 3 following situations:
    - If the mid-points of both walls intersect
    - If both walls are vertical or horizontal and mid point of one wall intersects with one of the end-points of another wall
    - If both walls are the same

### 1.2.2 Cell
__Properties__
 - __bool[4] Blocked__
    The enum Direction is defined as Direction.North=0, South=1, East=2, West=3. So, by defining an array of size 4, we can cast the direction value to int to effectively store/get results to/from the array. Blocked[i] == true means that the cell has no access to the cell in the ith direction.

__Methods__
- __void Block(Direction dir)__:
    Blocked[(int)dir] = true.
 - __void Unblock(Direction dir)__:
   Blocked[(int)dir] = false

### 1.2.3 Player
__Properties__
- __Vector2 StartPos__
    A (x, y) coordinate representing the player's initial pos. Needed for when the game environment does a reset.

__Methods__
- __Vector2 CurrentPos__
    A (x, y) coordinate representing the player's current pos.
- __public IsGoal<Vector2> IsGoalMove__
    Checks if a certain position is one of player's desired goal row.

---

# 2. Quoridor.Common

## 2.1 NuGet packages
- [log4net version 2.0.15](https://www.nuget.org/packages/log4net)
    used for logging purposes

## 2.2 Logger
```
public static Logger InstanceFor<T>() where T : class
{
    return InstanceFor(typeof(T));
}

public static Logger InstanceFor(Type type)
{
    return new Logger(type.Name);
}
```
Helps create a logger instance from static call to the InstanceFor method. For example, let's say we want to add logging to our class called MyClass

```
public class MyClass {
  private readonly ILogger _log = Logger.InstanceFor<MyClass>();
  ...
  _log.Info("This is an info")
  _log.Warn("Warning message")
  _log.Error("Critical message")
}
```
## 2.3 XmlHelper

__T Deserialize<T>(string path)__:
Deserializes an xml string from a file at a given path to a class with a similar hierarchy as in the xml string.
Examples include deserializing [ConsoleApp xml](https://github.com/devyanshuk/Quoridor/blob/main/Quoridor/Quoridor.ConsoleApp/ConfigTemplates/BoardCharacters.xml) to [BoardCharacters class](https://github.com/devyanshuk/Quoridor/blob/main/Quoridor/Quoridor.ConsoleApp/Configuration/BoardCharacters.cs)

---

## 3. Quoridor.DesktopApp

## 3.1 NuGet packages
- [Castle.Windsor Version 5.1.1](https://www.nuget.org/packages/Castle.Windsor)
 For effectively managing resources through containers for Dependency Injection

### 3.2 Methods and Property description

The core part of the desktop application is the following class that has Xml attributes

```
[XmlInclude(typeof(MctsStrategy))]
[XmlInclude(typeof(HumanStrategy))]
[XmlInclude(typeof(AStarStrategy))]
[XmlInclude(typeof(RandomStrategy))]
[XmlInclude(typeof(SemiRandomStrategy))]
[XmlInclude(typeof(MinimaxStrategy))]
[XmlInclude(typeof(MinimaxABStrategy))]
[XmlInclude(typeof(ParallelMinimaxABStrategy))]
public abstract class Strategy
{
    [XmlAttribute(nameof(Description))]
    public string Description { get; set; }

    public virtual IAIStrategy<Movement, IGameEnvironment, IPlayer> GetStrategy() => null;

    public override string ToString() => Description;

    public virtual string GetParams() => string.Empty;
}
```

We define each strategy to be one of MctsStrategy, ..., ParallelMinimaxABStrategy, with each strategy being an object itself. This way, in the config template, we can change the strategy setting to our liking. For example, we have the following strategy setting in our xml:

```xml
<SelectedStrategies>
	<HumanStrategy Description="Human"/>
	<MinimaxABStrategy Description="Minimax AB" Depth="2"/>
</SelectedStrategies>
```

This represents the default strategies for the 2-player variant in the desktop application. But, before running the project, we can change the xml to the following if we'd like to change the default to Random vs A* for example:

```xml
<SelectedStrategies>
    <RandomStrategy Description="Random" Seed="42"/>
	<AStarStrategy Description="A*"/>
</SelectedStrategies>
```

the property holding information about the default strategy is as follows:

```
[XmlArray(nameof(SelectedStrategies))]
[XmlArrayItem(nameof(MctsStrategy), typeof(MctsStrategy))]
[XmlArrayItem(nameof(HumanStrategy), typeof(HumanStrategy))]
[XmlArrayItem(nameof(AStarStrategy), typeof(AStarStrategy))]
[XmlArrayItem(nameof(RandomStrategy), typeof(RandomStrategy))]
[XmlArrayItem(nameof(SemiRandomStrategy), typeof(SemiRandomStrategy))]
[XmlArrayItem(nameof(MinimaxStrategy), typeof(MinimaxStrategy))]
[XmlArrayItem(nameof(MinimaxABStrategy), typeof(MinimaxABStrategy))]
[XmlArrayItem(nameof(ParallelMinimaxABStrategy), typeof(ParallelMinimaxABStrategy))]
public List<Strategy> SelectedStrategies { get; set; }
```

It contains explicit definitions for all defined types. Adding a self-defined agent would require appending a similar attributes here and other places that are defined explicitly this way.





