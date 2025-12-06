package main

import (
	"bufio"
	"fmt"
	"os"
	"strconv"
	"strings"
)

func main() {
	file, err := os.Open("input.txt")
	if err != nil {
		fmt.Fprintf(os.Stderr, "Error opening file: %v\n", err)
		os.Exit(1)
	}
	defer file.Close()

	index := 50
	count := 0

	scanner := bufio.NewScanner(file)
	for scanner.Scan() {
		line := strings.TrimSpace(scanner.Text())
		if line == "" {
			continue
		}

		direction := line[0]
		steps, err := strconv.Atoi(line[1:])
		if err != nil {
			fmt.Fprintf(os.Stderr, "Error parsing steps: %v\n", err)
			continue
		}

		if direction == 'L' {
			index = (index - steps) % 100
			if index < 0 {
				index += 100
			}
		} else if direction == 'R' {
			index = (index + steps) % 100
		}

		if index == 0 {
			count++
		}
	}

	if err := scanner.Err(); err != nil {
		fmt.Fprintf(os.Stderr, "Error reading file: %v\n", err)
		os.Exit(1)
	}

	fmt.Printf("Index stopped on 0 a total of %d times\n", count)
}
