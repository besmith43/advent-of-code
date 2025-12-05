#!/usr/bin/env dotnet-script

using System.IO;
using System.Runtime.CompilerServices;

string path = "";

public static string GetScriptPath([CallerFilePath] string path = null) => path;
public static string GetScriptFolder([CallerFilePath] string path = null) => Path.GetDirectoryName(path);

path = GetScriptFolder();


#region main

List<Tuple<int, int>> update_rules = new List<Tuple<int, int>>();
List<List<int>> update_printing_order = new List<List<int>>();

read($"{path}/example_input.txt");
// read($"{path}/input.txt");

// Console.WriteLine("Update Rules");
// foreach (var rule in update_rules)
// {
// 	Console.WriteLine($"{rule.Item1} {rule.Item2}");
// }

// foreach(var order in update_printing_order)
// {
// 	Console.Write("Printing Order: ");
// 	foreach (var page in order)
// 	{
// 		Console.Write($"{page},");
// 	}
// 	Console.WriteLine();
// }

// should be 61 for the example_input.txt
// Console.WriteLine($"Middle Check: {getMiddleNum(update_printing_order[0])}");

Console.WriteLine($"Sum of Correct Middle Numbers: {process()}");


#endregion


#region helper functions

int process()
{
	List<int> correct_middle_nums = new List<int>();

	foreach (List<int> order in update_printing_order)
	{
		if (check(order))
		{
			correct_middle_nums.Add(getMiddleNum(order));
		}
	}

	return sum(correct_middle_nums);
}


bool check(List<int> order)
{
	List<Tuple<int,int>> active_rules = new List<Tuple<int, int>>();

	foreach (Tuple<int,int> rule in update_rules)
	{
		if (order.Contains(rule.Item1))
		{
			active_rules.Add(rule);
		}
	}

	// Console.WriteLine($"Active Rule Count: {active_rules.Count}");

	foreach (Tuple<int,int> rule in active_rules)
	{
		var index_item1 = order.IndexOf(rule.Item1);
		var index_item2 = order.IndexOf(rule.Item2);

		if (index_item1 > index_item2 && index_item2 != -1)
		{
			return false;
		}
	}

	return true;
}

int getMiddleNum(List<int> order)
{
	if (order.Count%2 == 0)
	{
		foreach (int i in order)
		{
			Console.Error.Write($"{i},");
		}
		throw new Exception("This order has an even count");
	}

	int half = order.Count / 2;

	return order[half];
}

int sum(List<int> middle)
{
	int total = 0;

	foreach (var num in middle)
	{
		total += num;
	}

	return total;
}

void read(string input)
{
	if (!File.Exists(input))
	{
		Console.Error.WriteLine($"{input} can't be found");
		Environment.Exit(1);
	}

	string[] content = File.ReadAllLines(input);

	bool rules = true;
	foreach(string line in content)
	{
		if (String.IsNullOrEmpty(line))
		{
			rules = false;
			continue;
		}


		if (rules)
		{
			var temp = line.Split('|');
			update_rules.Add(new Tuple<int, int>(Int32.Parse(temp[0]), Int32.Parse(temp[1])));
		}
		else
		{
			var tempList = new List<int>();
			foreach (var tempNum in line.Split(','))
			{
				tempList.Add(Int32.Parse(tempNum));
			}

			update_printing_order.Add(tempList);
		}
	}
}

#endregion
