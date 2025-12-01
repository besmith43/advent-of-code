#!/usr/bin/env bash


if [ ! -f "$(which fzf)" ]; then
    echo "you are missing fzf" >&2
    exit 1
fi

if [ ! -f "$(which go)" ]; then
    echo "you are missing go" >&2
    exit 1
fi


go mod tidy

chosen="$(find . -iname "*.go" | fzf)"


go run $chosen

