#!/bin/bash

if [ -z "$1" ]; then
    echo "Usage: $0 <number_of_programs>"
    exit 1
fi

N=$1

mkdir -p src bin

for i in $(seq 1 $N); do
    file="src/$i.c"
    if [ ! -f "$file" ]; then
        cat > "$file" <<EOF
#include <stdio.h>
#include <pthread.h>

void* threadFunction(void *arg) {
    int id = *(int*)arg;
    printf("Hello from thread %d in program $i\\n", id);
    return NULL;
}

int main() {
    pthread_t t;
    int id = 1;
    pthread_create(&t, NULL, threadFunction, &id);
    pthread_join(t, NULL);
    return 0;
}
EOF
    fi
done

cat > Makefile <<EOF
CFLAGS = -g -O0 -Wall -Wextra -pthread -fsanitize=thread
LDFLAGS = -pthread -fsanitize=thread

SRC_DIR = src
BIN_DIR = bin

TARGETS = $(seq -s ' ' 1 $N)

all: \$(TARGETS)
	@echo "Build complete."

\$(TARGETS): %: \$(SRC_DIR)/%.c
	@mkdir -p \$(BIN_DIR)
	gcc \$(CFLAGS) \$< -o \$(BIN_DIR)/\$@ \$(LDFLAGS)

clean:
	rm -f \$(BIN_DIR)/*
EOF

echo "✅ Generated Makefile and $N C source files in ./src"
