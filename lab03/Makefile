CFLAGS = -g -O0 -Wall -Wextra -pthread
LDFLAGS = -pthread

SRC_DIR = src
BIN_DIR = bin

TARGETS = 1 2 3 4 5 6 7 8 9 10 11 barrier-seq prepStrassen-seq semaphoreSignal-seq sumVectorValues-seq 

all: $(TARGETS)
	@echo "Build complete."

$(TARGETS): %: $(SRC_DIR)/%.c
	@mkdir -p $(BIN_DIR)
	gcc $(CFLAGS) $< -o $(BIN_DIR)/$@ $(LDFLAGS)

clean:
	rm -f $(BIN_DIR)/*
