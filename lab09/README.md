# LAB 09 - CancellationToken and Multithreading

**Student:** Stir Catalin
**Group:** C114A

## Overview

This lab focuses on cooperative thread cancellation using `CancellationToken`, multi-threaded AES encryption/decryption, and implementing a multi-threaded HTTP client.

---

## Exercise 1: CancellationToken Analysis

**Key Concepts:**

- `CancellationToken` enables cooperative cancellation of async operations
- `ThrowIfCancellationRequested()` throws `OperationCanceledException` when cancellation is requested
- Token must be checked periodically in long-running loops
- Uncommenting line 32 causes immediate exception when cancellation is requested

---

## Exercise 2: Multi-threaded Substring Search

**Objective:** Search for substring in large text using multiple threads. When one finds the result, others stop.

**Implementation Highlights:**

```csharp
CancellationTokenSource cts = new CancellationTokenSource();
int chunkSize = booksString.Length / numThreads;

// Each thread searches its chunk
if (cts.Token.IsCancellationRequested) return;

if (MatchAt(j)) {
    lock (lockObj) {
        if (foundIndex == -1) {
            foundIndex = j;
            cts.Cancel();  // Signal other threads to stop
        }
    }
}
```

**Key Techniques:**

- `CancellationTokenSource` for cancellation signaling
- Periodic checking with `IsCancellationRequested`
- Lock to ensure only first match is recorded

---

## Exercise 3: Multi-threaded AES Encryption

**Objective:** Parallel encryption of a file with different keys per section.

**Implementation:**

- Generate unique password and IV for each thread
- Save keys to files (`key_{i}.bin`, `iv_{i}.bin`)
- Each thread encrypts its segment independently
- Concatenate encrypted segments into `_encrypted.bin`
- Save metadata (ranges, key files) to `_encrypted_keys.txt`

**Metadata Format:**

```
NUM_THREADS
START_RANGE END_RANGE KEY_FILE IV_FILE
...
```

---

## Exercise 4: Multi-threaded AES Decryption

**Objective:** Parallel decryption using saved keys and ranges.

**Implementation:**

- Read metadata from keys file
- Each thread decrypts its segment with corresponding key/IV
- Concatenate decrypted segments in correct order
- Verify against original file

---

## Exercise 5: HTTP Client with 3 Threads

**Architecture:** Requester → Downloader → Processer (pipeline pattern)

**1. Requester Thread - Exponential Backoff:**

```csharp
int backoffSeconds = 1;
if (response.Status == "SUCCESS") {
    urlQueue.Enqueue(response.Url);
    backoffSeconds = 1;  // Reset
} else if (response.Status == "RETRY-LATER") {
    Thread.Sleep(backoffSeconds * 1000);
    backoffSeconds = Math.Min(backoffSeconds * 2, maxBackoff);
}
```

**2. Downloader Thread:**

- Dequeues URLs from `urlQueue`
- Uses `ConcurrentDictionary` to download each URL only once
- Enqueues downloaded files to `processQueue`

**3. Processer Thread:**

- Adds watermark to downloaded images

**Key Concepts:**

- **Exponential Backoff:** Prevents server overload (1s, 2s, 4s, 8s, 16s, 32s, 64s max)
- **Thread-safe Collections:** `ConcurrentQueue`, `ConcurrentDictionary`

---

## Exercise 6: Thread-Safe REST API

**Problem 1: Singleton Race Condition**

**Solution - Double-Checked Locking:**

```csharp
private static readonly object _lock = new object();

public static SingletonRepository<T, T_Obj, T_Builder> GetInstance() {
    if (_instance == null) {
        lock (_lock) {
            if (_instance == null) {
                _instance = new SingletonRepository<T, T_Obj, T_Builder>();
                _instance._repository = _instance._builder.Build();
            }
        }
    }
    return _instance;
}
```

**Problem 2: Write-Write Conflicts**

**Solution - Write Lock:**

```csharp
private static readonly object _writeLock = new object();

public void Add(Order newOrder) {
    newOrder.Id = Guid.NewGuid().ToString();
    lock (_writeLock) {
        AddOrder(newOrder);
    }
}
```

---

## Exercise 7: Parallel Testing Tool (Optional)

**Features:**

- `-P=8`: Number of parallel requests
- `--total=100`: Total requests to send
- `--url=<URL>`: Target URL
- `--method=GET|POST`: HTTP method
- `--stat-countsuccess`: Count successful requests
- `--stat-countfail`: Count failed requests
- `--stat-meantime`: Calculate mean response time

**Example:**

```bash
ex07 --url=http://localhost:5007/product -P=8 --total=100 --stat-countsuccess
```

**Key Techniques:**

- Parallel HTTP request execution
- Performance metrics collection using `ConcurrentBag`
- Statistics aggregation (success rate, mean time, error rate)

---

## Technologies Used

- `CancellationToken` / `CancellationTokenSource`
- Multi-threading with `Thread` class
- Thread-safe collections: `ConcurrentQueue`, `ConcurrentDictionary`, `ConcurrentBag`
- AES encryption/decryption
- HTTP client operations
- Lock synchronization primitives

## Learning Outcomes

- Implementing cooperative cancellation patterns
- Managing parallel task execution and coordination
- Building thread-safe data structures
- Designing multi-threaded pipelines
- Performance testing and metrics collection
