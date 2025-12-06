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

	// Try all possible pattern lengths from 1 to length/2
	for patternLen := 1; patternLen <= length/2; patternLen++ {
		if length%patternLen == 0 {
			pattern := s[:patternLen]
			matches := true

			// Check if the entire string is made of this pattern repeated
			for i := patternLen; i < length; i += patternLen {
				if s[i:i+patternLen] != pattern {
					matches = false
					break
				}
			}

			if matches {
				return true
			}
		}
	}

	return false
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
