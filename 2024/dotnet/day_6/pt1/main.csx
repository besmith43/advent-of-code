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

Tuple<int,int> StartingPoint;

for (int i = 0; i < map.Length; i++)
{
	for (int j = 0; j < map[i].Length; j++)
	{
		if (map[i][j] == '^')
		{
			StartingPoint = new Tuple<int, int>(i,j);
			Console.WriteLine($"Found Starting Point at {j},{i}");
		}
	}
}


Guard guard= new Guard();

guard.y = StartingPoint.Item1;
guard.x = StartingPoint.Item2;

while (guard.onMap)
{
	guard.step(map);
}

Console.WriteLine($"The Guard was in {count(map)} unique places");


#endregion


public class Guard
{
	public int x { get; set; }
	public int y { get; set; }
	public Direction currentDirection = Direction.Up;
	public char character = '^';
	public bool onMap = true;

	public void step(char[][] grid)
	{
		switch (currentDirection)
		{
			// REMEMBER GRID[Y][X]
			case Direction.Up:
				// y--
				if (y-1 < 0)
				{
					// going out of bounds
					onMap = false;
					grid[y][x] = 'X';
				}
				else if (grid[y-1][x] == '.' || grid[y-1][x] == 'X')
				{
					grid[y][x] = 'X';
					y-=1;
					grid[y][x] = character;
				}
				else if (grid[y-1][x] == '#')
				{
					updateDirection(Direction.Right);
				}
				else
				{
					throw new Exception("Up hit an uncovered edge case");
				}
				break;
			case Direction.Down:
				// y++
				if (y+1 >= grid.Length)
				{
					// going out of bounds
					onMap = false;
					grid[y][x] = 'X';
				}
				else if (grid[y+1][x] == '.' || grid[y+1][x] == 'X')
				{
					grid[y][x] = 'X';
					y+=1;
					grid[y][x] = character;
				}
				else if (grid[y+1][x] == '#')
				{
					updateDirection(Direction.Left);
				}
				else
				{
					throw new Exception("Down hit an uncovered edge case");
				}
				break;
			case Direction.Left:
				// x--
				if (x-1 < 0)
				{
					// going out of bounds
					onMap = false;
					grid[y][x] = 'X';
				}
				else if (grid[y][x-1] == '.' || grid[y][x-1] == 'X')
				{
					grid[y][x] = 'X';
					x-=1;
					grid[y][x] = character;
				}
				else if (grid[y][x-1] == '#')
				{
					updateDirection(Direction.Up);
				}
				else
				{
					throw new Exception("Left hit an uncovered edge case");
				}
				break;
			case Direction.Right:
				// x++
				if (x+1 >= grid.Length)
				{
					// going out of bounds
					onMap = false;
					grid[y][x] = 'X';
				}
				else if (grid[y][x+1] == '.' || grid[y][x+1] == 'X')
				{
					grid[y][x] = 'X';
					x+=1;
					grid[y][x] = character;
				}
				else if (grid[y][x+1] == '#')
				{
					updateDirection(Direction.Down);
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
		throw new Exception ($"{file} not found");
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

#endregion
