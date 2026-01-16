# LAB 12 - Task Parallel Library (TPL) and Dataflow

**Student:** Stir Catalin
**Group:** C114A

## Overview

This lab explores advanced TPL concepts including task creation options, child task attachment, and TPL Dataflow blocks for building concurrent processing pipelines.

## Exercises

### Exercise 1: Task Creation and Parent-Child Relationships

**TaskCreationOptions.DenyChildAttach:**

- When applied to a parent task, prevents child tasks from attaching even with `AttachedToParent` flag
- Parent task completes without waiting for child execution
- Child task becomes independent and may complete after program termination

**Task.Run() vs Task.Factory.StartNew():**

- **Task.Run():** High-level, recommended for most scenarios with safe default settings on ThreadPool
- **Task.Factory.StartNew():** Fine-grained control over scheduler, attachment options, and cancellation policies
- StartNew() can lead to unexpected behavior if used incorrectly
- Task.Run() is preferred for simple async code; StartNew() for advanced scenarios only

### Exercise 4: Dataflow Blocks - Basic Types

**BufferBlock<T>:**

- Functions as FIFO queue
- Receives items one at a time in order
- Additional `Receive()` calls beyond posted items will block waiting for data
- Solution: Use `TryReceive()` or limit loop iterations to available elements

**BroadcastBlock<T>:**

- Retains only the last posted value
- Transmits same value to all consumers
- Subsequent `Post()` calls overwrite previous value
- All `Receive()` calls return the most recent value

**WriteOnceBlock<T>:**

- Accepts only a single value
- First `SendAsync()` succeeds, subsequent calls are rejected
- All `ReceiveAsync()` calls return the initially accepted value

### Exercise 5: Action vs Transform Blocks

**ActionBlock:**

- Consumes data and executes an action
- No output produced
- Used for side effects (display, save data)

**TransformBlock:**

- Receives input and produces output
- Transforms data for pipeline processing
- Enables data flow between blocks

**TransformManyBlock with Negative Values:**

- Original delegate not designed for negative values
- Solution: Filter negative values before sending to block
- Ensures only valid data is processed, preventing errors

### Exercise 6: Grouping and Joining Blocks

**BatchBlock:**

- Groups elements into fixed-size batches (e.g., 3 elements)
- Creates consecutive groups from input stream
- Last batch may contain fewer elements if total not divisible by batch size

**JoinBlock:**

- Combines elements from two sources based on arrival order
- Creates pairs of (car, price) in one-to-one correspondence
- Association follows data sending order

**BatchedJoinBlock:**

- Combines BatchBlock and JoinBlock functionality
- Groups elements from two streams into batches (max size 3)
- Each batch contains lists from both sources
- Example use: Finding car with minimum price in each batch

### Exercise 7: Greedy Option in JoinBlock

**Greedy = true (default):**

- Immediately consumes available elements
- May reserve elements without complete pairs

**Greedy = false:**

- Conservative approach
- Reserves elements only when complete combinations can be formed

**Resource Distribution:**

- Setting Greedy = false on one join allows more shared resources for the other
- Setting Greedy = false on both joins distributes shared resources more evenly
- Prevents one join from monopolizing common resources (e.g., wood)

## Technologies Used

- Task Parallel Library (TPL)
- TPL Dataflow
- TaskCreationOptions
- BufferBlock, BroadcastBlock, WriteOnceBlock
- ActionBlock, TransformBlock, TransformManyBlock
- BatchBlock, JoinBlock, BatchedJoinBlock

## Learning Outcomes

- Understanding task parent-child relationships
- Choosing appropriate task creation methods
- Building concurrent processing pipelines
- Managing data flow with various block types
- Optimizing resource distribution in parallel workflows
