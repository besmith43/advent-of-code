#!/usr/bin/env dotnet-script

using System.Collections;
using System.IO;
using System.Runtime.CompilerServices;

string path = "";

public static string GetScriptPath([CallerFilePath] string path = null) => path;
public static string GetScriptFolder([CallerFilePath] string path = null) => Path.GetDirectoryName(path);

path = GetScriptFolder();


#region Main

List<int> left_list = new List<int>();
List<int> right_list = new List<int>();

// read_list($"{path}/example_input.txt");
read_list($"{path}/input.txt");

List<int> common_list = new List<int>();

foreach(var item in left_list)
{
	var results = right_list.FindAll(
		delegate(int num)
		{
			return num == item;
		}
	);

	common_list.Add(results.Count * item);
}

int total = sum(common_list);

Console.WriteLine($"Total: {total}");

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
