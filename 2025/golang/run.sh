#!/usr/bin/env bash


if [ ! -f "$(which fzf)" ]; then
    echo "you are missing fzf" >&2
    exit 1
fi

if [ ! -f "$(which go)" ]; then
    echo "you are missing go" >&2
    exit 1
fi


chosenDay="$(find ./* -type d | fzf)"

if [ -z $chosenDay ]; then
    echo "nothing was selected.  closing program" >&2
    exit 0
fi

cd $chosenDay

chosenInput="$(find . -iname "*.txt" | fzf)"

if [ -z $chosenInput ]; then
    echo "nothing was selected.  closing program" >&2
    exit 0
fi

go mod tidy

go run main.go $chosenInput

