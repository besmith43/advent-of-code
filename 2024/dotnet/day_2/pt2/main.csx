#!/usr/bin/env dotnet script


using System.IO;
using System.Runtime.CompilerServices;

string path = "";

public static string GetScriptPath([CallerFilePath] string path = null) => path;
public static string GetScriptFolder([CallerFilePath] string path = null) => Path.GetDirectoryName(path);

path = GetScriptFolder();


// var list = read_file($"{path}/example_input.txt");
// var list = read_file($"{path}/example_input2.txt");
var list = read_file($"{path}/input.txt");

int count = 0;

foreach (var line in list)
{
	var array = convert(line);

	var result = process(array);
	if (result)
	{
		count++;
	}
	else
	{
		Console.Write("Original Array: ");
		print(array);
		for (int i = 0; i < array.Count; i++)
		{
			List<int> temp_line = new List<int>(array);
			temp_line.RemoveAt(i);
			Console.Write("Testing Array: ");
			print(temp_line);
			if (process(temp_line))
			{
				Console.WriteLine("Passed");
				count++;
				break;
			}

			Console.WriteLine("Failed");
		}
	}
}

Console.WriteLine($"Count: {count}");


#region helper functions


bool process(List<int> line)
{
	bool? increasing = null;

	for (int i = 1; i < line.Count; i++)
	{
		int diff = line[i-1] - line[i];

		if (diff == 0 || diff > 3 || diff < -3)
		{
			return false;
		}
		else if (diff < 0)
		{
			if (increasing is null)
			{
				increasing = true;
			}
			else if (increasing == false)
			{
				return false;
			}
		}
		else if (diff > 0)
		{
			if (increasing is null)
			{
				increasing = false;
			}
			else if (increasing == true)
			{
				return false;
			}
		}
	}

	return true;
}


List<int> convert(string line)
{
	List<int> array = new List<int>();

	var temp = line.Split();

	foreach (var item in temp)
	{
		if (!String.IsNullOrEmpty(item))
		{
			array.Add(Int32.Parse(item));
		}
	}

	return array;
}

List<string> read_file(string file)
{
	if (!File.Exists(file))
	{
		throw new Exception($"{file} not found");
	}

	List<string> temp = new List<string>();

	using (StreamReader sr = File.OpenText(file))
	{
		string s;
		while ((s = sr.ReadLine()) != null)
		{
			// Console.WriteLine(s);
			temp.Add(s);
		}
	}

	return temp;
}


void print(List<int> line)
{
	string strLine = "";

	foreach (var item in line)
	{
		strLine+=$"{item} ";
	}

	Console.WriteLine(strLine);
}

#endregion