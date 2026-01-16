# LAB 11 - Asynchronous Streams

**Student:** Stir Catalin
**Group:** C114A

## Overview

This lab explores asynchronous streams in C# using `IAsyncEnumerable<T>`, demonstrating how to produce and consume values asynchronously with proper interleaving of synchronous and asynchronous operations.

## Exercise

### Exercise 1: IAsyncEnumerable<T> Implementation

**Objective:** Demonstrate asynchronous value production and consumption using `IAsyncEnumerable<T>`.

**Implementation:**

- `GetNumbersAsync()` method generates values incrementally using `yield return`
- Introduces asynchronous delays between value production
- `await foreach` consumes values as they become available without blocking execution

**Key Concepts:**

1. **Asynchronous Value Production**
   - `yield return` preserves method state between iterations
   - Values are produced gradually with async delays
   - Enables incremental and efficient processing

2. **Initial vs Modified Implementation**
   - **Initial version:** `DoOtherWork()` runs in a separate task with no clear interleaving
   - **Modified version:** Synchronous work is executed after each produced value, achieving natural interleaving between async processing and sync execution

3. **Benefits of yield return**
   - Maintains producer method state
   - Resumes execution at each iteration
   - Enables efficient incremental processing

## Technologies Used

- C# `IAsyncEnumerable<T>`
- `yield return` statement
- `await foreach` loop
- Asynchronous iteration patterns

## Learning Outcomes

- Understanding asynchronous stream processing
- Implementing producer-consumer patterns with async enumerables
- Managing execution flow between sync and async operations
- Utilizing `yield return` for stateful iteration
