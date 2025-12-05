#!/usr/bin/env dotnet-script

using System.IO;
using System.Runtime.CompilerServices;

string path = "";

public static string GetScriptPath([CallerFilePath] string path = null) => path;
public static string GetScriptFolder([CallerFilePath] string path = null) => Path.GetDirectoryName(path);

path = GetScriptFolder();


#region main

char[][] map = read($"{path}/example_input.txt");
// char[][] map = read($"{path}/input.txt");

Tuple<int, int> StartingPoint;

for (int i = 0; i < map.Length; i++)
{
	for (int j = 0; j < map[i].Length; j++)
	{
		if (map[i][j] == '^')
		{
			StartingPoint = new Tuple<int, int>(i, j);
			Console.WriteLine($"Found Starting Point at {j},{i}");
		}
	}
}

print_grid(map);

Guard guard = new Guard();

guard.y = StartingPoint.Item1;
guard.x = StartingPoint.Item2;

while (guard.onMap)
{
	guard.step(map);
}

// Console.WriteLine($"The Guard was in {count(map)} unique places");
Console.WriteLine($"There are {guard.loopList.Count} possible loops");

#endregion


public class Guard
{
	public int x { get; set; }
	public int y { get; set; }
	public Direction currentDirection = Direction.Up;
	public char character = '^';
	public bool onMap = true;
	public List<Tuple<int, int>> loopList = new List<Tuple<int, int>>();

	public void step(char[][] grid)
	{
		switch (currentDirection)
		{
			// REMEMBER GRID[Y][X]
			case Direction.Up:
				// y--
				if (y - 1 < 0)
				{
					// going out of bounds
					onMap = false;
					grid[y][x] = 'X';
				}
				else if (grid[y - 1][x] == '.' || grid[y - 1][x] == 'X')
				{
					checkForLoop(grid);
					grid[y][x] = 'X';
					y -= 1;
					grid[y][x] = character;
				}
				else if (grid[y - 1][x] == '#')
				{
					updateDirection(Direction.Right);
					checkForLoop(grid);
				}
				else
				{
					throw new Exception("Up hit an uncovered edge case");
				}
				break;
			case Direction.Down:
				// y++
				if (y + 1 >= grid.Length)
				{
					// going out of bounds
					onMap = false;
					grid[y][x] = 'X';
				}
				else if (grid[y + 1][x] == '.' || grid[y + 1][x] == 'X')
				{
					checkForLoop(grid);
					grid[y][x] = 'X';
					y += 1;
					grid[y][x] = character;
					checkForLoop(grid);
				}
				else if (grid[y + 1][x] == '#')
				{
					updateDirection(Direction.Left);
					checkForLoop(grid);
				}
				else
				{
					throw new Exception("Down hit an uncovered edge case");
				}
				break;
			case Direction.Left:
				// x--
				if (x - 1 < 0)
				{
					// going out of bounds
					onMap = false;
					grid[y][x] = 'X';
				}
				else if (grid[y][x - 1] == '.' || grid[y][x - 1] == 'X')
				{
					checkForLoop(grid);
					grid[y][x] = 'X';
					x -= 1;
					grid[y][x] = character;
				}
				else if (grid[y][x - 1] == '#')
				{
					updateDirection(Direction.Up);
					checkForLoop(grid);
				}
				else
				{
					Console.Error.WriteLine($"Current Grid Coordinates: {x}, {y}");
					Console.Error.WriteLine($"Current Grid Coordinates Character: {grid[y][x]}");
					Console.Error.WriteLine($"Attempting to go to Grid Coordinates: {x-1}, {y}");
					Console.Error.WriteLine($"Next Grid Coordinates Character: {grid[y][x-1]}");
					print_grid(grid);
					throw new Exception("Left hit an uncovered edge case");
				}
				break;
			case Direction.Right:
				// x++
				if (x + 1 >= grid.Length)
				{
					// going out of bounds
					onMap = false;
					grid[y][x] = 'X';
				}
				else if (grid[y][x + 1] == '.' || grid[y][x + 1] == 'X')
				{
					checkForLoop(grid);
					grid[y][x] = 'X';
					x += 1;
					grid[y][x] = character;
				}
				else if (grid[y][x + 1] == '#')
				{
					updateDirection(Direction.Down);
					checkForLoop(grid);
				}
				else
				{
					throw new Exception("Right hit an uncovered edge case");
				}
				break;
		}
	}

	void updateDirection(Direction direction)
	{
		switch (direction)
		{
			case Direction.Up:
				currentDirection = Direction.Up;
				character = '^';
				break;
			case Direction.Down:
				currentDirection = Direction.Down;
				character = 'v';
				break;
			case Direction.Left:
				currentDirection = Direction.Left;
				character = '<';
				break;
			case Direction.Right:
				currentDirection = Direction.Right;
				character = '>';
				break;
		}
	}

	void checkForLoop(char[][] grid)
	{
		// drop temp 'O' to the square on the gaurd's left
		// then run a square to see if you end back where you started
		// if yes, catalog success with coordinates in loopList
		// else, return 'O' to original character

		if (currentDirection == Direction.Up)
		{
			checkForLoopUp(grid);
		}
		else if (currentDirection == Direction.Down)
		{
			checkForLoopDown(grid);
		}
		else if (currentDirection == Direction.Left)
		{
			checkForLoopLeft(grid);
		}
		else if (currentDirection == Direction.Right)
		{
			checkForLoopRight(grid);
		}
		else
		{
			// shouldn't be possible
			return;
		}
	}

	void checkForLoopUp(char[][] grid)
	{
		if (x - 1 < 0)
		{
			// out of bounds
			return;
		}

		char orig_char = grid[y][x - 1];

		if (orig_char == '#')
		{
			return;
		}

		Console.Error.WriteLine($"original char for {x} {y} - {orig_char}");
		grid[y][x - 1] = 'O';
		int tmpX = x;
		int tmpY = y;

		// go up until '#'
		for (int i = tmpY; i <= 0; i--)
		{
			if (i - 1 < 0)
			{
				// out of bounds
				return;
			}
			else if (grid[i - 1][tmpX] == '#')
			{
				tmpY = i;
				break;
			}
		}

		// turn 90 degrees
		// go right until '#'
		for (int i = tmpX; i < grid[0].Length; i++)
		{
			if (i + 1 >= grid[0].Length)
			{
				// out of bounds
				return;
			}
			else if (grid[tmpY][i + 1] == '#')
			{
				tmpX = i;
				break;
			}
		}

		// turn 90 degrees
		// go down until '#'
		for (int i = tmpY; i < grid.Length; i++)
		{
			if (i + 1 >= grid.Length)
			{
				// out of bounds
				return;
			}
			else if (grid[i + 1][tmpX] == '#')
			{
				tmpY = i;
				break;
			}
		}

		// turn 90 degrees
		// go left until '#'
		for (int i = tmpX; i > 0; i--)
		{
			if (i - 1 < 0)
			{
				// out of bounds
				return;
			}
			else if (grid[tmpY][i - 1] == '#')
			{
				tmpX = i;
				break;
			}
		}


		if (tmpX == x && tmpY == y)
		{
			loopList.Add(new Tuple<int, int>(y, x - 1));
		}

		grid[y][x - 1] = orig_char;
	}


	void checkForLoopDown(char[][] grid)
	{
		if (x + 1 < 0)
		{
			// out of bounds
			return;
		}

		char orig_char = grid[y][x + 1];


		if (orig_char == '#')
		{
			return;
		}

		Console.Error.WriteLine($"original char for {x} {y} - {orig_char}");
		grid[y][x + 1] = 'O';
		int tmpX = x;
		int tmpY = y;

		// turn 90 degrees
		// go down until '#'
		for (int i = tmpY; i < grid.Length; i++)
		{
			if (i + 1 >= grid.Length)
			{
				// out of bounds
				return;
			}
			else if (grid[i + 1][tmpX] == '#')
			{
				tmpY = i;
				break;
			}
		}

		// turn 90 degrees
		// go left until '#'
		for (int i = tmpX; i > 0; i--)
		{
			if (i - 1 < 0)
			{
				// out of bounds
				return;
			}
			else if (grid[tmpY][i - 1] == '#')
			{
				tmpX = i;
				break;
			}
		}

		// turn 90 degrees
		// go up until '#'
		for (int i = tmpY; i < 0; i--)
		{
			if (i - 1 < 0)
			{
				// out of bounds
				return;
			}
			else if (grid[i - 1][tmpX] == '#')
			{
				tmpY = i;
				break;
			}
		}

		// turn 90 degrees
		// go right until '#'
		for (int i = tmpX; i < grid[0].Length; i++)
		{
			if (i + 1 >= grid[0].Length)
			{
				// out of bounds
				return;
			}
			else if (grid[tmpY][i + 1] == '#')
			{
				tmpX = i;
				break;
			}
		}

		if (tmpX == x && tmpY == y)
		{
			loopList.Add(new Tuple<int, int>(y, x + 1));
		}

		grid[y][x + 1] = orig_char;
	}

	void checkForLoopLeft(char[][] grid)
	{
		if (x - 1 < 0)
		{
			// out of bounds
			return;
		}

		char orig_char = grid[y][x - 1];


		if (orig_char == '#')
		{
			return;
		}

		Console.Error.WriteLine($"original char for {x} {y} - {orig_char}");
		grid[y][x - 1] = 'O';
		int tmpX = x;
		int tmpY = y;

		// go up until '#'
		for (int i = tmpY; i <= 0; i--)
		{
			if (i - 1 < 0)
			{
				// out of bounds
				return;
			}
			else if (grid[i - 1][tmpX] == '#')
			{
				tmpY = i;
				break;
			}
		}

		// turn 90 degrees
		for (int i = tmpX; i < grid[0].Length; i++)
		{
			if (i + 1 >= grid[0].Length)
			{
				// out of bounds
				return;
			}
			else if (grid[tmpY][i + 1] == '#')
			{
				tmpX = i;
				break;
			}
		}

		// turn 90 degrees
		// go right until '#'
		for (int i = tmpX; i < 0; i++)
		{
			if (i - 1 < 0)
			{
				// out of bounds
				return;
			}
			else if (grid[tmpY][i - 1] == '#')
			{
				tmpX = i;
				break;
			}
		}

		// turn 90 degrees
		// go down until '#'
		for (int i = tmpY; i < grid.Length; i++)
		{
			if (i + 1 >= grid.Length)
			{
				// out of bounds
				return;
			}
			else if (grid[i + 1][tmpX] == '#')
			{
				tmpY = i;
				break;
			}
		}

		if (tmpX == x && tmpY == y)
		{
			loopList.Add(new Tuple<int, int>(y, x - 1));
		}

		grid[y][x - 1] = orig_char;
	}

	void checkForLoopRight(char[][] grid)
	{
		if (y - 1 < 0)
		{
			// out of bounds
			return;
		}

		char orig_char = grid[y - 1][x];


		if (orig_char == '#')
		{
			return;
		}

		Console.Error.WriteLine($"original char for {x} {y} - {orig_char}");
		grid[y - 1][x] = 'O';
		int tmpX = x;
		int tmpY = y;

		// go right until '#'
		for (int i = tmpX; i < 0; i++)
		{
			if (i - 1 < 0)
			{
				// out of bounds
				return;
			}
			else if (grid[tmpY][i - 1] == '#')
			{
				tmpX = i;
				break;
			}
		}

		// turn 90 degrees
		// go down until '#'
		for (int i = tmpY; i < grid.Length; i++)
		{
			if (i + 1 >= grid.Length)
			{
				// out of bounds
				return;
			}
			else if (grid[i + 1][tmpX] == '#')
			{
				tmpY = i;
				break;
			}
		}

		// turn 90 degrees
		// go left until '#'
		for (int i = tmpX; i < 0; i--)
		{
			if (i - 1 < 0)
			{
				// out of bounds
				return;
			}
			else if (grid[tmpY][i - 1] == '#')
			{
				tmpX = i;
				break;
			}
		}

		if (tmpX == x && tmpY == y)
		{
			loopList.Add(new Tuple<int, int>(y, x - 1));
		}

		grid[y][x - 1] = orig_char;
	}
}

public enum Direction
{
	Up,
	Down,
	Left,
	Right
}

#region helper functions

int count(char[][] grid)
{
	int total = 0;

	for (int i = 0; i < map.Length; i++)
	{
		for (int j = 0; j < map[i].Length; j++)
		{
			if (map[i][j] == 'X')
			{
				total++;
			}
		}
	}

	return total;
}


char[][] read(string file)
{
	if (!File.Exists(file))
	{
		throw new Exception($"{file} not found");
	}

	string[] input = File.ReadAllLines(file);

	// char[][] result = new char[input.Length][input[0].Length];
	char[][] result = new char[input.Length][];

	for (int i = 0; i < input.Length; i++)
	{
		result[i] = input[i].ToCharArray();
	}

	return result;
}

static void print_grid(char[][] grid)
{
	for (int i = 0; i < grid.Length; i++)
	{
		for (int j = 0; j < grid[0].Length; j++)
		{
			Console.Write(grid[i][j]);
		}
		Console.WriteLine();
	}
}

#endregion
