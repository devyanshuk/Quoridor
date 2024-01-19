
## Quoridor Desktop Application

### Prerequisites:
 - [dotnet >= 8.0.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
 - __OS requirement:__ Windows (it is a winforms application)

### Building the project
```bash
$cd Quoridor.DesktopApp
Quoridor.DesktopApp$ dotnet build
```
### Running the project
1) __Visual Studio__
    Run the __Quoridor.DesktopApp__ project on visual studio.
2) __Terminal__
    ```bash
    Quoridor.DesktopApp$ dotnet run
    ```

Upon successful build and run, you will see the main menu, which asks for the number of players playing the game - 2, 3 or 4.
If you select 3 or 4, you will be directed towards the game screen.

If you select 2, you'll be directed towards the strategy selection window. You can select any strategy for player-1 and/or player-2, and configure their parameters.

Also, to change any game settings, navigate to the [ConfigTemplates/DesktopApp.xml](https://github.com/devyanshuk/Quoridor/blob/main/Quoridor/Quoridor.DesktopApp/ConfigTemplates/DesktopAppSettings.xml) file. The xml is as follows:

```xml
<?xml version="1.0" encoding="utf-8" ?>
<DesktopAppSettings>
	<FormSettings Description ="values (in pixels) used in the game form">
		<Title>Quoridor</Title>
		<ScreenWidth>600</ScreenWidth>
		<ScreenHeight>700</ScreenHeight>
		<OffsetY>70</OffsetY>
		<WallWidth>5</WallWidth>
	</FormSettings>
	<FontSettings>
		<PlayerFont>Arial</PlayerFont>
	</FontSettings>
	<ColorSettings>
		<BackgroundColor>LightGray</BackgroundColor>
		<OddTileColor>LightSkyBlue</OddTileColor>
		<EvenTileColor>WhiteSmoke</EvenTileColor>
		<GoalTileColor>LightPink</GoalTileColor>
		<PlayerStatsColor>Black</PlayerStatsColor>
		<CurrentPlayerCellColor>Green</CurrentPlayerCellColor>
		<PossibleCellMoveColor>Green</PossibleCellMoveColor>
		<PossibleWallColor>Red</PossibleWallColor>
		<PlayerColor>Black</PlayerColor>
		<WallColor>Black</WallColor>
		<Opacity>200</Opacity>
		<OpacityAnimationVelocity>25</OpacityAnimationVelocity>
	</ColorSettings>
	<GameSettings>
		<Dimension>5</Dimension>
		<Players>2</Players>
		<Walls>8</Walls>
		<SelectedStrategies>
			<HumanStrategy Description="Human"/>
			<MinimaxABStrategy Description="Minimax AB" Depth="2"/>
		</SelectedStrategies>
		<Strategies>
			<HumanStrategy Description="Human"/>
			<AStarStrategy Description="A*"/>
			<RandomStrategy Description="Random" Seed="42"/>
			<SemiRandomStrategy Description="Semi-Random" Seed="42" />
			<MinimaxABStrategy Description="Minimax AB" Depth="2"/>
			<MinimaxStrategy Description="Minimax" Depth="2" />
			<ParallelMinimaxABStrategy Description="Parallel Minimax" Depth="2" />
			<MctsStrategy Description="MCTS" C="1.41" Simulations="1000">
				<SemiRandomStrategy Description="Semi-Random" Seed="42" />
			</MctsStrategy>
		</Strategies>
		<!--<Moves>
			<WallMove X="5" Y="5" Placement="North"/>
			<PlayerMove X="4" Y="7"/>
			<WallMove X="5" Y="5" Placement="South"/>
		</Moves>-->
	</GameSettings>
</DesktopAppSettings>
```

This represents the parameters on startup. For example, on startup, the first strategy is Human by default and the second strategy is Minimax AB with a depth of 2 by default.
To change strategy to something else on startup, you need to modify the xml element. You can use one of (you can modify the parameters to your need)
- \<HumanStrategy Description="Human"/>
- \<AStarStrategy Description="A*"/>
- \<RandomStrategy Description="Random" Seed="42"/>
- \<SemiRandomStrategy Description="Semi-Random" Seed="42" />
- \<MinimaxABStrategy Description="Minimax AB" Depth="2"/>
- \<MinimaxStrategy Description="Minimax" Depth="2" />
- \<ParallelMinimaxABStrategy Description="Parallel Minimax" Depth="2" />
- \<MctsStrategy Description="MCTS" C="1.41" Simulations="1000">


### Playing the game

#### Moving the player
    Press the mouse button in the cell the current player is at. You will see one(or more) green-highlighted squares. Those refer to the places you can go to. To move, click on one of those highlighted squares.
    
#### Placing a wall
    Click on any cell that does not contain a player. You will see a red line appear and disappear with a certain opacity velocity. Click on that cell more, and you'll see a change in the orientation of the (possible) wall.
    Finally, press the right mouse button to place the wall.
    
#### Moving an AI Agent
    When it's the AI agent's turn, press the 'm' key to move the agent.
