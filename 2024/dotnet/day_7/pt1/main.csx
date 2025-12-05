#!/usr/bin/env dotnet-script


using System.IO;
using System.Runtime.CompilerServices;

string path = "";

public static string GetScriptPath([CallerFilePath] string path = null) => path;
public static string GetScriptFolder([CallerFilePath] string path = null) => Path.GetDirectoryName(path);

path = GetScriptFolder();


// var content = read($"{path}/example_input.txt");
var content = read($"{path}/example_input2.txt");
// var content = read($"{path}/input.txt");

Console.WriteLine($"Total: {process(content)}");

int process(List<string> list)
{
	int total = 0;

	foreach (var line in list)
	{
		int index = line.IndexOf(':');
		// Console.Error.WriteLine("{0}", line);
		// Console.Error.WriteLine("{0}", index);
		string newline = line.Remove(index, 1);
		// Console.Error.WriteLine("{0}", newline);
		string[] temp = newline.Split();
		List<int> nums = new List<int>();

		foreach (var num in temp)
		{
			nums.Add(Int32.Parse(num));
		}

		int target = nums[0];
		nums.RemoveAt(0);

		if (check(target, nums))
		{
			Console.WriteLine($"SUCCESS: Target={target} number array {String.Join(",", nums.Select(x => x.ToString()).ToArray())}");
			total += target;
		}
	}

	return total;
}

bool check(int target, List<int> nums)
{
	List<int> localNums = new List<int>();
    localNums.AddRange(nums);
	// foreach (int num in nums)
	// {
		// localNums.Add(num);
	// }

	Console.WriteLine($"Starting Check: Target={target} number array {String.Join(",", localNums.Select(x => x.ToString()).ToArray())}");

	if (nums.Count == 0)
	{
        Console.Error.WriteLine("nums count is 0");
		return target == 0;
	}

	if (target <= 0)
	{
        Console.Error.WriteLine("Target value is equal to or less than 0");
        Console.Error.WriteLine($"Target = {target}");
		return false;
	}

	int last = localNums[localNums.Count - 1];
	nums.RemoveAt(localNums.Count - 1);

	return check(target-last, localNums) || (target%last==0 && check((int)Math.Floor((decimal)target/last), localNums));
}

List<string> read(string path)
{
	string[] content = File.ReadAllLines(path);
	return new List<string>(content);
}
