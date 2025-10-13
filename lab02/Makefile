CFLAGS = -g -O0 -Wall -Wextra -pthread
LDFLAGS = -pthread

SRC_DIR = src
BIN_DIR = bin

TARGETS = 1 3 4 6 7 8 9

all: $(TARGETS)

$(TARGETS): %: $(SRC_DIR)/%.c
	gcc $(CFLAGS) $< -o $(BIN_DIR)/$@ $(LDFLAGS)

clean:
	rm -f $(BIN_DIR)/*
