# BFS_Maze
C# algorithm that finds the shortest path in a maze using BFS algorithm

We have a matrix and we know:
- N - size of the matrix
- k - number of impassable cells
- 2 teleport cells marked with '2'

In the matrix, the player can move right, down, left, right and up (clockwise).
If he gets into a teleporter, he automatically goes to the other teleporter. 
Once passed through a teleporter, the two teleporters are declared impassable and can no longer be traversed. 
The two teleporters are considered as one step.
The start and end state cannot be a teleporter!

With '0' are marked impassable cells and with '1' - those we can go through.

The algorithm finds the shortest path from start to finish using [BFS](https://en.wikipedia.org/wiki/Breadth-first_search). 
And the length of the path and the path itself are saved in file.
