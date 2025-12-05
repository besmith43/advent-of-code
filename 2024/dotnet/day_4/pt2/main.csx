#!/usr/bin/env dotnet-script


using System.IO;
using System.Runtime.CompilerServices;

string path = "";

public static string GetScriptPath([CallerFilePath] string path = null) => path;
public static string GetScriptFolder([CallerFilePath] string path = null) => Path.GetDirectoryName(path);

path = GetScriptFolder();


#region main

// var input = read_list($"{path}/example_input.txt");
var input = read_list($"{path}/input.txt");

int result = process(input);

Console.WriteLine($"Number of XMAS's Found: {result}");

#endregion

#region helper functions

int process(char[][] input)
{
	int count = 0;
	List<Tuple<int, int>> Starting = new List<Tuple<int, int>>();

	// Console.WriteLine($"Outer loop length: {input.Length}");
	// Console.WriteLine($"Inner loop length: {input[0].Length}");

	for (int i = 0; i < input.Length; i++)
	{
		// Console.WriteLine("Starting new outer loop: " + i);
		for (int j = 0; j < input[0].Length; j++)
		{
			// Console.WriteLine("Starting new inner loop: " + j);
			// Console.WriteLine($"Checking {i}{j} with a value of {input[i][j]}");
			if (input[i][j] == 'A')
			{
				Starting.Add(new Tuple<int, int>(j,i));
			}
		}
	}

	foreach (Tuple<int, int> start_point in Starting)
	{
		int found = check_for_XMAS(input, start_point.Item1, start_point.Item2);
		count += found;
		// count++;
	}

	return count;
}

int check_for_XMAS(char[][] grid, int x, int y)
{
	int count = 0;

	if (check_left(grid, x, y))
	{
		// Console.WriteLine($"Success: check left {x} {y}");
		count++;
	}

	if (check_right(grid, x, y))
	{
		// Console.WriteLine($"Success: check right {x} {y}");
		count++;
	}

	if (check_up(grid, x, y))
	{
		// Console.WriteLine($"Success: check up {x} {y}");
		count++;
	}

	if (check_down(grid, x, y))
	{
		// Console.WriteLine($"Success: check down {x} {y}");
		count++;
	}

	// if (check_up_left(grid, x, y))
	// {
	// 	// Console.WriteLine($"Success: check up left {x} {y}");
	// 	count++;
	// }

	// if (check_up_right(grid, x, y))
	// {
	// 	// Console.WriteLine($"Success: check up right {x} {y}");
	// 	count++;
	// }

	// if (check_down_left(grid, x, y))
	// {
	// 	// Console.WriteLine($"Success: check down left {x} {y}");
	// 	count++;
	// }

	// if (check_down_right(grid, x, y))
	// {
	// 	// Console.WriteLine($"Success: check down right {x} {y}");
	// 	count++;
	// }

	return count;
}



bool check_up(char[][] grid, int x, int y)
{
	try
	{
		if (grid[up(y)][right(x)] == 'M' &&
			grid[up(y)][left(x)] == 'M' &&
			grid[down(y)][right(x)] == 'S' &&
			grid[down(y)][left(x)] == 'S'
		)
		{
			return true;
		}
	} catch
	{
		return false;
	}

	return false;
}


bool check_down(char[][] grid, int x, int y)
{
	try
	{
		if (grid[down(y)][right(x)] == 'M' &&
			grid[down(y)][left(x)] == 'M' &&
			grid[up(y)][right(x)] == 'S' &&
			grid[up(y)][left(x)] == 'S'
		)
		{
			return true;
		}
	} catch
	{
		return false;
	}

	return false;
}

bool check_left(char[][] grid, int x, int y)
{
	try
	{
		// Console.WriteLine($"Checking Left {x} {y} - {grid[y][x]}");
		// Console.WriteLine($"Up and to the Left: {up(y)} {left(x)} {grid[up(y)][left(x)]}");
		// Console.WriteLine($"Down and to the Left: {down(y)} {left(x)} {grid[down(y)][left(x)]}");
		// Console.WriteLine($"Up and to the Right: {up(y)} {right(x)} {grid[up(y)][right(x)]}");
		// Console.WriteLine($"Down and to the Right: {down(y)} {right(x)} {grid[down(y)][right(x)]}");

		if (grid[up(y)][left(x)] == 'M' &&
			grid[down(y)][left(x)] == 'M' &&
			grid[up(y)][right(x)] == 'S' &&
			grid[down(y)][right(x)] == 'S'
		)
		{
			return true;
		}
	} catch
	{
		return false;
	}

	return false;
}


bool check_right(char[][] grid, int x, int y)
{
	try
	{
		if (grid[up(y)][right(x)] == 'M' &&
			grid[down(y)][right(x)] == 'M' &&
			grid[up(y)][left(x)] == 'S' &&
			grid[down(y)][left(x)] == 'S'
		)
		{
			return true;
		}
	} catch
	{
		return false;
	}

	return false;
}


bool check_up_right(char[][] grid, int x, int y)
{
	if (y-3 < 0 || x+3 >=grid[0].Length)
	{
		return false;
	}

	if (grid[y-1][x+1] != 'M')
	{
		return false;
	}

	if (grid[y-2][x+2] != 'A')
	{
		return false;
	}

	if (grid[y-3][x+3] != 'S')
	{
		return false;
	}

	return true;
}


bool check_down_right(char[][] grid, int x, int y)
{
	if (y+3 >= grid.Length || x+3 >= grid[0].Length)
	{
		return false;
	}

	if (grid[y+1][x+1] != 'M')
	{
		return false;
	}

	if (grid[y+2][x+2] != 'A')
	{
		return false;
	}

	if (grid[y+3][x+3] != 'S')
	{
		return false;
	}

	return true;
}

bool check_up_left(char[][] grid, int x, int y)
{
	if (x-3 < 0 || y-3 < 0)
	{
		return false;
	}

	if (grid[y-1][x-1] != 'M')
	{
		return false;
	}

	if (grid[y-2][x-2] != 'A')
	{
		return false;
	}

	if (grid[y-3][x-3] != 'S')
	{
		return false;
	}

	return true;
}


bool check_down_left(char[][] grid, int x, int y)
{
	if (x-3 < 0 || y+3 >= grid.Length)
	{
		return false;
	}

	if (grid[y+1][x-1] != 'M')
	{
		return false;
	}

	if (grid[y+2][x-2] != 'A')
	{
		return false;
	}

	if (grid[y+3][x-3] != 'S')
	{
		return false;
	}

	return true;
}


int up(int y)
{
	return --y;
}

int down(int y)
{
	return ++y;
}

int left(int x)
{
	return --x;
}

int right(int x)
{
	return ++x;
}



char[][] read_list(string file)
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

