package main

import (
	"fmt"
	"os"
	"strconv"
	"strings"
)

func hasRepeatingPattern(n int) bool {
	s := strconv.Itoa(n)
	length := len(s)

	// Only match if the string length is even and can be split into two equal halves
	if length%2 != 0 {
		return false
	}

	half := length / 2
	firstHalf := s[:half]
	secondHalf := s[half:]

	return firstHalf == secondHalf
}

func main() {
	// Read input file
	data, err := os.ReadFile("input.txt")
	if err != nil {
		fmt.Println("Error reading file:", err)
		return
	}

	input := strings.TrimSpace(string(data))
	ranges := strings.Split(input, ",")

	sum := 0
	var repeatingNumbers []int

	// Process each range
	for _, r := range ranges {
		parts := strings.Split(r, "-")
		if len(parts) != 2 {
			continue
		}

		start, err1 := strconv.Atoi(parts[0])
		end, err2 := strconv.Atoi(parts[1])

		if err1 != nil || err2 != nil {
			continue
		}

		// Check each number in the range
		for num := start; num <= end; num++ {
			if hasRepeatingPattern(num) {
				repeatingNumbers = append(repeatingNumbers, num)
				sum += num
			}
		}
	}

	fmt.Println("Numbers with repeating patterns:", repeatingNumbers)
	fmt.Println("Sum:", sum)
}
