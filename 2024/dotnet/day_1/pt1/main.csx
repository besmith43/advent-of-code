#!/usr/bin/env dotnet-script

using System.Collections;
using System.IO;
using System.Runtime.CompilerServices;

string path = "";

public static string GetScriptPath([CallerFilePath] string path = null) => path;
public static string GetScriptFolder([CallerFilePath] string path = null) => Path.GetDirectoryName(path);

path = GetScriptFolder();

// Console.WriteLine("Path: " + path);


#region Main

List<int> left_list = new List<int>();
List<int> right_list = new List<int>();

// read_list($"{path}/example_input.txt");
read_list($"{path}/input.txt");

left_list.Sort();
right_list.Sort();

List<int> diff_list = new List<int>();

for (int i = 0; i < left_list.Count; i++)
{
	diff_list.Add(Math.Abs(left_list[i] - right_list[i]));
}

int total_distance = sum(diff_list);

Console.WriteLine($"Total Distance: {total_distance}");

#endregion Main

#region helper functions

void print_list(List<int> list)
{
	foreach (var num in list)
	{
		Console.WriteLine($"Order List Item: {num}");
	}
}


int sum(List<int> numbers)
{
	int sum = 0;

	foreach (var num in numbers)
	{
		sum += num;
	}

	return sum;
}


void read_list(string file)
{
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
			var temp = s.Split(); // assumed to be whitespace
			// Console.WriteLine("left list: " + temp[0]);
			left_list.Add(Int32.Parse(temp[0]));
			// Console.WriteLine("right list: " + temp[3]);
			right_list.Add(Int32.Parse(temp[3]));
        }
    }
}


#endregion helper functions
