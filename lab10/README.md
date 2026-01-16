# LAB 10 - Asynchronous Programming

**Student:** Stir Catalin
**Group:** C114A

## Overview

This lab explores asynchronous programming concepts in C# including thread management, task execution, synchronization contexts, progress reporting, and task cancellation.

## Exercises

### Exercise 1: Thread IDs and Task Execution

- Demonstrates how thread IDs (TID) change during asynchronous execution
- Shows the difference between main thread and ThreadPool threads
- Explains the role of `SynchronizationContext` in Console vs GUI applications
- Compares behavior with and without `await task_1`

**Key Concepts:**

- ThreadPool management
- Async/await execution flow
- Console applications lack SynchronizationContext

### Exercise 3: Progress Reporting

- Implements progress reporting with `IProgress<T>`
- Demonstrates timing and synchronization of progress events
- Shows how `Task.Delay` affects event ordering

**Key Finding:** Adding a delay after `progress.Report()` ensures the ProgressChanged event handler executes before subsequent code, making the output order more predictable.

### Exercise 5: Task Cancellation with WhenAny

- Runs three parallel asynchronous tasks
- Uses `Task.WhenAny()` to detect the first completed task
- Implements cancellation tokens to stop remaining tasks
- Displays final task states (RanToCompletion, Canceled)

**Key Mechanism:** A 4-second delay allows canceled tasks to properly update their state before the application terminates.

### Exercises 7 & 8

_Content not provided in source document_

## Technologies Used

- C# async/await
- Task Parallel Library (TPL)
- CancellationToken
- IProgress<T> interface
- ThreadPool

## Learning Outcomes

- Understanding thread execution in asynchronous contexts
- Managing task lifecycle and cancellation
- Implementing progress reporting patterns
- Debugging asynchronous code behavior
