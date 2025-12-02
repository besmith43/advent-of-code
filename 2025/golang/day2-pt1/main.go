package main

import (
	"fmt"
	"os"
	"regexp"
	"strconv"
	"strings"

	"github.com/bitfield/script"
)

func main() {
	if len(os.Args) < 2 {
		fmt.Println("no input file was passed in")
		os.Exit(1)
	}

	inputFile := os.Args[1]

	if inputFile == "" {
		fmt.Println("no input file was passed in")
		os.Exit(1)
	}

	fmt.Printf("input file: %s\n", inputFile)

	contents, err := script.File(inputFile).String()
	if err != nil {
		fmt.Println("failed to read the contents of input file")
		os.Exit(1)
	}

	// fmt.Printf("contents variable type: %T\n", contents)
	// fmt.Printf("contents of input file: %s\n", contents)

	// Split the string by the newline character
	ranges := strings.Split(contents, ",")

	var sum = 0

	// Iterate over the resulting slice of lines
	for _, line := range ranges {
		// fmt.Printf("Line %d: %s\n", i+1, line)
		pair := strings.Split(line, "-")
		left, err := strconv.Atoi(pair[0])
		if err != nil {
			fmt.Printf("failed to get a number out of %s\n with error: %s", pair[0], err)
			os.Exit(1)
		}

		re := regexp.MustCompile(`\n`)
		tmp := re.ReplaceAllString(pair[1], "")

		right, err := strconv.Atoi(tmp)
		if err != nil {
			fmt.Printf("failed to get a number out of %s\n with error: %s", pair[1], err)
			os.Exit(1)
		}

		tmp_sum := findInvalidNumbers(left, right)
		sum += tmp_sum
	}

	fmt.Printf("Sum: %d\n", sum)
}

func findInvalidNumbers(start int, end int) int {
	var sum = 0

	for i := start; i <= end; i++ {
		// fmt.Printf("checking num: %d\n", i)
		iString := strconv.Itoa(i)

		if i < 100 {
			tmp := reverse(iString)

			if iString == tmp {
				fmt.Println(i)
				sum += i
			}
		} else if len(iString)%2 == 0 {
			half1, half2 := splitStringInHalf(iString)

			if half1 == half2 {
				fmt.Println(i)
				sum += i
			}
		}
	}

	return sum
}

func reverse(s string) string {
	r := []rune(s)
	for i, j := 0, len(r)-1; i < j; i, j = i+1, j-1 {
		r[i], r[j] = r[j], r[i]
	}
	result := string(r)
	return result
}

func splitStringInHalf(s string) (string, string) {
	length := len(s)
	mid := length / 2

	// For even length strings, 'mid' will be the exact middle.
	// For odd length strings, the first half will be one character shorter.
	firstHalf := s[:mid]
	secondHalf := s[mid:]

	return firstHalf, secondHalf
}
