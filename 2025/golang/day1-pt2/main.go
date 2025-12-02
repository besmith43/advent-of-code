package main

import (
	"fmt"
	"os"
	"regexp"
	"strconv"
	"strings"

	"github.com/bitfield/script"
)

var count int = 0

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
	lines := strings.Split(contents, "\n")

	var point int = 50

	// Iterate over the resulting slice of lines
	for i, line := range lines {
		fmt.Printf("Line %d: %s\n", i+1, line)
		point = turnLock(point, line)

		// if point == 0 {
		// count++
		// }

		fmt.Printf("point: %d\n", point)
	}

	fmt.Printf("the password is %d", count)
}

func turnLock(point int, instruction string) int {
	if instruction == "" {
		fmt.Printf("instruction is empty\n")
		return point
	}

	directionByte := instruction[0]
	direction := string(directionByte)

	re := regexp.MustCompile(`[LR]`)
	tmp := re.ReplaceAllString(instruction, "")
	num, err := strconv.Atoi(tmp)
	if err != nil {
		fmt.Printf("failed to get a number out of %s\n", tmp)
		os.Exit(1)
	}

	fmt.Printf("Directions - %s %d\n", direction, num)

	if direction == "L" {
		point = goLeft(point, num)
	} else if direction == "R" {
		point = goRight(point, num)
	} else {
		fmt.Printf("something went wrong and the direction was neither left or right: %s\n", direction)
		os.Exit(1)
	}

	return point
}

func goLeft(point int, num int) int {
	for i := 0; i < num; i++ {
		if point == 0 {
			point = 99
		} else {
			point--
			if point == 0 {
				count++
			}
		}
	}
	return point
}

func goRight(point int, num int) int {
	for i := 0; i < num; i++ {
		if point == 99 {
			point = 0
			count++
		} else {
			point++
		}
	}

	return point
}
