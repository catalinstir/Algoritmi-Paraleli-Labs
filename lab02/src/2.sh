#!/bin/bash

# Compiled with:
# CFLAGS = -g -O0 -Wall -Wextra -pthread -fsanitize=thread
# LDFLAGS = -pthread -fsanitize=thread

valgrind --tool=helgrind bin/$1

# in addition to GDB/GEF + 
# watch var
# break func if thread_id == 2
