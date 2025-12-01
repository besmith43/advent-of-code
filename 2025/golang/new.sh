#!/usr/bin/env bash


if [ ! -f "$(which gum)" ]; then
    echo "you are missing gum" >&2
    exit 1
fi


day="$(gum input --prompt "AOC What day is it? ")"

part="$(gum input --prompt "AOC What part is it? ")"

root_dir="day$day-pt$part"

if [ -d $root_dir ]; then
    echo "directory $root_dir already exists" >&2
    exit 1
fi

mkdir $root_dir

cp blank_program $root_dir/main.go

cp go.mod $root_dir/go.mod

touch $root_dir/example.txt $root_dir/input.txt

