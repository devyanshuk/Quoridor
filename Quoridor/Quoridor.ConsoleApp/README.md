
## Quoridor Console Application

### Prerequisites:
 - [dotnet >= 8.0.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)


### Building the project
```bash
$cd Quoridor.ConsoleApp
$dotnet build
```

### Command-line args
  

__play|p__

- **/agent /mctsagent**  : Agent to perform MCTS simulation (String) (Default = Greedy) (Strategy is one of Random, Greedy, Minimax, MinimaxAB, ParallelMinimaxAB, AStar, MonteCarlo, Human)

- __/c /exploration__  : Exploration parameter for UCT (Double) (Default = 1,41)

- __/d /depth__  : Maximum depth of the search tree (Int32) (Default = 2) (More or equal to 0)

- __/dim /dimension__  : Game Board Dimension (Int32) (Default = 5) (More or equal to 3)

- __/mctssim /mctsims__  : Simulation limit for MCTS (Int32) (Default = 1000)

- __/numsim /numsimulate__ : Number of games to simulate (Int32) (Default = 100) (More or equal to 0)

- __/s /seed__ : Seed for the random number generator (Int32) (Default = 20)

- __/s1 /strategy1__ : First strategy (String) (Default = Minimax) (Strategy is one of Random, Greedy, Minimax, MinimaxAB, ParallelMinimaxAB, AStar, MonteCarlo, Human)

- __/s2 /strategy2__ : Second strategy (String) (Default = Human) (Strategy is one of Random, Greedy, Minimax, MinimaxAB, ParallelMinimaxAB, AStar, MonteCarlo, Human)

- __/sim /simulate__ : Simulate games (Default = False)

- __/v /verbose__  : Display all results of the simulation (Default = True)

- __/wait /waitforinput__  : Enable viewer to press any key before the other agent makes move (Default = False)

- __/walls /numwalls__ : Number of walls each player has at the beginning (Int32) (Default = 8) (More or equal to 0)

---

### Running the program

All arguments are subject to default values. So, you can run the project by typing:
```bash
$dotnet run play 
```

Running with custom args:

```bash
$dotnet run play -dimension=5 -numwalls=20
```

Or you can use the short-form of these arguments aforementioned above
```bash
$dotnet run play -d=5 -n=20
```

---

#### Playing the game
Once the above steps have been correctly followed, the game loads and  
displays the board along with a prompt asking the current player to enter a  
command (if __Human__ strategy was chosen). The commands are divided into two categories:
    
1. **Wall Command**
	The user can place a wall in the board by writing any sentence as long as the  
	key-words  **wall**, a coordinate to place the wall to (*two numbers separated  
	by a comma*), and a direction that corresponds to which side the wall is to  
	be place with respect to that coordinate **(North, South, East, West)** is provided.
	It is case insensitive.

	If an invalid wall command is provided by a user, the game displays an  
	error message and prompts the user for a wall command again until the  
	user enters the correct one.  
	
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
	The user can move their pawn in the board at their  
	turn by writing any sentence as long as the key-words  move  and one of  
	North, South, East or West is provided in the sentence. If a user enters an  
	incorrect move command, the game displays an error message and prompts  
	the user for a move command until the user enters the correct one 
	
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

### Tests

To run tests, navigate to the __Quoridor.Tests/__ directory and type in the __dotnet test__ command.
```bash
$cd Quoridor.Tests/
$dotnet test
```