#!/usr/bin/env dotnet-script


using System.Text.RegularExpressions;
using System.IO;
using System.Runtime.CompilerServices;

string path = "";

public static string GetScriptPath([CallerFilePath] string path = null) => path;
public static string GetScriptFolder([CallerFilePath] string path = null) => Path.GetDirectoryName(path);

path = GetScriptFolder();

#region Main Function

// var commands = read_list($"{path}/example_input.txt");
var commands = read_list($"{path}/input.txt");

string pattern = @"mul\([0-9]{1,3}\,[0-9]{1,3}\)|don\'t\(\)|do\(\)";

int sum = 0;
bool enabled = true;

foreach (var command in commands)
{
	foreach (Match match in Regex.Matches(command, pattern))
	{
		// Console.WriteLine(match.Value);
		if (match.Value.Contains("don't"))
		{
			enabled = false;
		}
		else if (match.Value.Contains("do"))
		{
			enabled = true;
		}
		else
		{
			if (enabled)
			{
				sum += extract_and_multiply(match.Value);
			}
		}
	}
}

Console.WriteLine($"Sum: {sum}");

#endregion

#region Helper Functions

int extract_and_multiply(string value)
{
	value = value.Remove(0, 4);
	value = value.Remove(value.IndexOf(')'), 1);
	// Console.WriteLine(value);
	var temp = value.Split(',');
	// Console.WriteLine($"{temp[0]} {temp[1]}");
	int x = Int32.Parse(temp[0]);
	int y = Int32.Parse(temp[1]);

	return x*y;
}

List<string> read_list(string file)
{
	List<string> temp = new List<string>();
	if (!File.Exists(file))
	{
		throw new Exception ($"{file} not found");
	}

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

#endregion
