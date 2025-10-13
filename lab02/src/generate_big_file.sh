#!/bin/bash

if [[ $# -ne 1 ]]
then
    echo "Invalid parameters. Usage:"
    echo "src/generate_big_file.sh FILENAME"
    exit 1
fi 

filename="resources/$1"
# touch $filename
base64 /dev/urandom | tr -dc '[:print:]\n' | head -c 2G > "$filename"

echo -n "Created file succesfully: "
file $filename
