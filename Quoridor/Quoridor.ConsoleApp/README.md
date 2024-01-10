
## Quoridor Console Application

### Prerequisites:
 - [dotnet >= 8.0.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)


### Building the project
```bash
$cd Quoridor.ConsoleApp
Quoridor.ConsoleApp$ dotnet build
```

### Command-line args
  

__play|p__
- __/agent /mctsagent__    : Agent to perform MCTS simulation (String) (Default = parallelminimaxab) (Strategy should be one of RANDOM, SEMIRANDOM, MINIMAX, MINIMAXAB, PARALLELMINIMAXAB, ASTAR, MCTS, HUMAN. Case Insensitive.) 
- __/b /branchingfactor__  : Compute the average branching factor for a specified dimension (Default = False) 
- __/c /exploration__      : Exploration parameter for UCT (String) (Default = 1.7) 
- __/d /depth__            : Maximum depth of the search tree (Int32) (Default = 2) (More or equal to 0) 
- __/dim /dimension__      : Game Board Dimension (Int32) (Default = 5) (More or equal to 3) 
- __/mctssim /mctsims__    : Simulation limit for MCTS (Int32) (Default = 90) 
- __/numsim /numsimulate__ : Number of games to simulate (Int32) (Default = 100) (More or equal to 0) 
- __/s /seed__             : Seed for the random number generator (Int32) (Default = 20) 
- __/s1 /strategy1__       : First strategy (String) (Default = parallelminimaxab) (Strategy should be one of RANDOM, SEMIRANDOM, MINIMAX, MINIMAXAB, PARALLELMINIMAXAB, ASTAR, MCTS, HUMAN. Case Insensitive.) 
- __/s2 /strategy2__       : Second strategy (String) (Default = mcts) (Strategy should be one of RANDOM, SEMIRANDOM, MINIMAX, MINIMAXAB, PARALLELMINIMAXAB, ASTAR, MCTS, HUMAN. Case Insensitive.) 
- __/sim /simulate__       : Simulate games (Default = False) 
- __/v /verbose__          : Display all results of the simulation (Default = False) 
- __/wait /waitforinput__  : Enable viewer to press any key before the other agent makes move (Default = False) 
- __/walls /numwalls__     : Number of walls each player has at the beginning (Int32) (Default = 8) (More or equal to 0) 

---

### Running the program

All arguments are subject to default values. So, you can run the project by typing:
```bash
Quoridor.ConsoleApp$ dotnet run play 
```

This will start a 5x5 instance of the game, with Minimax of depth 2 being the first agent and MCTS with 90 iterations and an exploration parameter of 1.7 being the second agent.

Running with custom args:

```bash
Quoridor.ConsoleApp$ dotnet run play -dimension=3 -numwalls=20 -strategy1=Human -strategy2=Human
```

This will start a 3x3 game session, with each player having 20 walls each. Both strategies are Human, so we need to enter commands to move/place walls.

Or you can use the short-form of these arguments aforementioned above
```bash
$dotnet run play -d=3 -n=20 -s1=human -s2=human 
```

If you want to run, e.g a 200-game simulation between MCTS and Minimax Alpha-beta pruning version, with greedy (minimax with depth 1) being the MCTS simulation strategy
```bash
Quoridor.ConsoleApp$ dotnet run play -s1=mcts -s2=minimaxab -simulate -verbose -numsim=100 -exploration=1.7 -mctsims=90 -mctsagent=parallelminimaxab
```

---

#### Playing the game
Once the above steps have been correctly followed, the game loads and displays the board along with a prompt asking the current player to enter a  command (if __Human__ strategy was chosen). The commands are divided into two categories:
    
1. **Wall Command**
	The user can place a wall in the board by writing any sentence as long as the key-words  **wall**, a coordinate to place the wall to (*two numbers separated  by a comma*), and a direction that corresponds to which side the wall is to be place with respect to that coordinate **(North, South, East, West)** is provided. It is case insensitive.

	If an invalid wall command is provided by a user, the game displays an error message and prompts the user for a wall command again until the user enters the correct one.  
	
	- **Some valid wall commands**
    	- Place wall in the Northern side from the coordinate (5,5)  
    	- wall (5,5) South  
    	- wall 4, 2 easT
	
	- **Some invalid wall commands**
		|   Command | Reason |  Corrected Version |
		| -------- | :-------------- |  :--------- |
		|   Place wall south | No coordinate to place a southern wall at provided |  Place wall **(3,2)** south |
		|.  Place a wall at (5,5) | No direction to place a wall at (5,5) provided | Place a wall at (5,5) **North** |
		|  w a l l a t (5,5) south | Spaces between all letters of the word 'wall' | **wall** at (5,5) south |
		|   wall at (5,5( s o u t h | Spaces between all letters of the word 'south' | wall at (5,5( **south** |
	
2) **Move Command**
	The user can move their pawn in the board at their turn by writing any sentence as long as the key-words  move  and one of North, South, East or West is provided in the sentence. If a user enters an incorrect move command, the game displays an error message and prompts the user for a move command until the user enters the correct one 
	
	- **Some valid move commands**
		-  move north  
		-  could you move my pawn south please?  
		-  west move  
	
	- **Some invalid move commands**  (The italicized words are optional)
		|   Command | Reason |  Corrected Version |
		|:---| :---- | :--- |
		|   move wall     | No direction provided |  move **North** *wall* |
		|   move n o r t h | Spaces between all letters of the word 'north' | move **north** |
		|   north| No key-word 'move' present | **move** north |
		
	Once the user enters a correct command, the game displays an updated version  
	of the board and prompts the opponent to enter the command. This goes on until  
	the game finishes, in which case the game displays the winner and terminates.

[More examples of Valid and Invalid move/wall commands](https://github.com/devyanshuk/Quoridor/blob/main/Quoridor/Quoridor.Tests/GameManager/Command/CommandParserTests.cs)

---

### Example gameplay

We run a 3x3 game instance, with us being the first player and MCTS being the second
```bash
Quoridor.ConsoleApp$ dotnet run play -s1=human -s2=mcts -exploration=0.6 -mctsims=500 -mctsagent=parallelminimaxab -dimension=3
```

```
     |  0  |  1  |  2  |
=====+=====+=====+=====+
  0  |     |  A  |     |
     |     |     |     |
=====+=====+=====+=====+
  1  |     |     |     |
     |     |     |     |
=====+=====+=====+=====+
  2  |     |  B  |     |
     |     |     |     |
=====+=====+=====+=====+

Player 'A''s Turn. 0 moves made. 8 wall(s) left. Using HumanAgentConsole strategy
move east
     |  0  |  1  |  2  |
=====+=====+=====+=====+
  0  |     |     |  A  |
     |     |     |     |
=====+=====+=====+=====+
  1  |     |     |     |
     |     |     |     |
=====+=====+=====+=====+
  2  |     |  B  |     |
     |     |     |     |
=====+=====+=====+=====+

Player 'B''s Turn. 0 moves made. 8 wall(s) left. Using MCTS strategy
     |  0  |  1  |  2  |
=====+=====+=====+=====+
  0  |     |     |  A  |
     |     |     |     |
=====+=====+■■■■■■■■■■■+
  1  |     |     |     |
     |     |     |     |
=====+=====+=====+=====+
  2  |     |  B  |     |
     |     |     |     |
=====+=====+=====+=====+

Player 'A''s Turn. 1 moves made. 8 wall(s) left. Using HumanAgentConsole strategy
place wall (0,1) south
     |  0  |  1  |  2  |
=====+=====+=====+=====+
  0  |     |     |  A  |
     |     |     |     |
=====+=====+■■■■■■■■■■■+
  1  |     |     |     |
     |     |     |     |
=====+■■■■■■■■■■■+=====+
  2  |     |  B  |     |
     |     |     |     |
=====+=====+=====+=====+

Player 'B''s Turn. 1 moves made. 7 wall(s) left. Using MCTS strategy
     |  0  |  1  |  2  |
=====+=====+=====+=====+
  0  |     |     |  A  |
     |     |     |     |
=====+=====+■■■■■■■■■■■+
  1  |     |     |     |
     |     |     |     |
=====+■■■■■■■■■■■+=====+
  2  |     |     |  B  |
     |     |     |     |
=====+=====+=====+=====+

Player 'A''s Turn. 2 moves made. 7 wall(s) left. Using HumanAgentConsole strategy
move west
     |  0  |  1  |  2  |
=====+=====+=====+=====+
  0  |     |  A  |     |
     |     |     |     |
=====+=====+■■■■■■■■■■■+
  1  |     |     |     |
     |     |     |     |
=====+■■■■■■■■■■■+=====+
  2  |     |     |  B  |
     |     |     |     |
=====+=====+=====+=====+

Player 'B''s Turn. 2 moves made. 7 wall(s) left. Using MCTS strategy
     |  0  |  1  |  2  |
=====+=====+=====+=====+
  0  |     |  A  |     |
     |     |     |     |
=====+=====+■■■■■■■■■■■+
  1  |     |     |  B  |
     |     |     |     |
=====+■■■■■■■■■■■+=====+
  2  |     |     |     |
     |     |     |     |
=====+=====+=====+=====+

Player 'A''s Turn. 3 moves made. 7 wall(s) left. Using HumanAgentConsole strategy
move west
     |  0  |  1  |  2  |
=====+=====+=====+=====+
  0  |  A  |     |     |
     |     |     |     |
=====+=====+■■■■■■■■■■■+
  1  |     |     |  B  |
     |     |     |     |
=====+■■■■■■■■■■■+=====+
  2  |     |     |     |
     |     |     |     |
=====+=====+=====+=====+

Player 'B''s Turn. 3 moves made. 7 wall(s) left. Using MCTS strategy
     |  0  |  1  |  2  |
=====+=====+=====+=====+
  0  |  A  |     |     |
     |     |     |     |
=====+=====+■■■■■■■■■■■+
  1  |     |  B  |     |
     |     |     |     |
=====+■■■■■■■■■■■+=====+
  2  |     |     |     |
     |     |     |     |
=====+=====+=====+=====+

Player 'A''s Turn. 4 moves made. 7 wall(s) left. Using HumanAgentConsole strategy
move south
     |  0  |  1  |  2  |
=====+=====+=====+=====+
  0  |     |     |     |
     |     |     |     |
=====+=====+■■■■■■■■■■■+
  1  |  A  |  B  |     |
     |     |     |     |
=====+■■■■■■■■■■■+=====+
  2  |     |     |     |
     |     |     |     |
=====+=====+=====+=====+

Player 'B''s Turn. 4 moves made. 7 wall(s) left. Using MCTS strategy
     |  0  |  1  |  2  |
=====+=====+=====+=====+
  0  |  B  |     |     |
     |     |     |     |
=====+=====+■■■■■■■■■■■+
  1  |  A  |     |     |
     |     |     |     |
=====+■■■■■■■■■■■+=====+
  2  |     |     |     |
     |     |     |     |
=====+=====+=====+=====+

Game  over. Player B : MCTS won in 5 moves. Player A : HumanAgentConsole lost. 5 moves made
```

As we can see above in the gameplay, we used wall-placement command and used a series of moves, but the MCTS agent won the game.

---
### Changing the displayed board characters

You can change the board characters to anything of your liking. To do so, navigate to the [ConfigTemplates/BoardCharacters.xml](https://github.com/devyanshuk/Quoridor/blob/main/Quoridor/Quoridor.ConsoleApp/ConfigTemplates/BoardCharacters.xml) file, and update it as per your liking.

```xml
<BoardChars>
    <CellProperties>
        <CellWidth>5</CellWidth>
    </CellProperties>
    <BorderSeparators>
        <HorizontalBorderSeparator>=</HorizontalBorderSeparator>
        <VerticalBorderSeparator>|</VerticalBorderSeparator>
        <IntersectionBorderSeparator>+</IntersectionBorderSeparator>
    </BorderSeparators>
    <WallSeparators>
        <HorizontalWallSeparator>■</HorizontalWallSeparator>
        <VerticalWallSeparator>█</VerticalWallSeparator>
    </WallSeparators>
</BoardChars>
```

---
### Tests

To run tests, navigate to the __Quoridor.Tests/__ directory and type in the __dotnet test__ command.
```bash
$cd Quoridor.Tests/
$dotnet test
```