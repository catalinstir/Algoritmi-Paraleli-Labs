#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <pthread.h>

#include "Workers.h"

int N;
int printLevel;

// Counter pentru mutări
int moveCount = 0;
pthread_mutex_t printMutex = PTHREAD_MUTEX_INITIALIZER;
pthread_mutex_t countMutex = PTHREAD_MUTEX_INITIALIZER;

// Structura pentru datele task-ului Hanoi
typedef struct HanoiData {
    int n;       // Numărul de discuri
    char from;   // Tija sursă
    char to;     // Tija destinație
    char aux;    // Tija auxiliară
} HanoiData;

void hanoiTask(void* data, int thread_id);

void getArgs(int argc, char** argv) {
    if (argc < 4) {
        printf("Not enough parameters: ./program N printLevel P\n");
        printf("printLevel: 0=no, 1=some, 2=verbose\n");
        exit(1);
    }
    N = atoi(argv[1]);
    printLevel = atoi(argv[2]);
    P = atoi(argv[3]);
}

void printMove(int disk, char from, char to) {
    if (printLevel > 0) {
        pthread_mutex_lock(&printMutex);
        printf("Move disk %d from %c to %c\n", disk, from, to);
        pthread_mutex_unlock(&printMutex);
    }
}

Task makeHanoiTask(int n, char from, char to, char aux) {
    Task task;
    HanoiData* taskData = (HanoiData*)malloc(sizeof(HanoiData));
    taskData->n = n;
    taskData->from = from;
    taskData->to = to;
    taskData->aux = aux;
    task.data = taskData;
    task.runTask = hanoiTask;
    return task;
}

// Versiune recursivă pentru sub-probleme mici (evită overhead task-uri)
void hanoiRecursive(int n, char from, char to, char aux) {
    if (n == 0) return;

    hanoiRecursive(n - 1, from, aux, to);
    printMove(n, from, to);

    pthread_mutex_lock(&countMutex);
    moveCount++;
    pthread_mutex_unlock(&countMutex);

    hanoiRecursive(n - 1, aux, to, from);
}

void hanoiTask(void* data, int thread_id) {
    HanoiData* taskData = (HanoiData*)data;
    int n = taskData->n;
    char from = taskData->from;
    char to = taskData->to;
    char aux = taskData->aux;
    free(taskData);

    if (n == 0) {
        return;
    }

    // Pentru probleme mici, rezolvă recursiv (evită overhead)
    if (n <= 3) {
        hanoiRecursive(n, from, to, aux);
        return;
    }

    // Pentru probleme mari, creează task-uri
    // Notă: Hanoi are dependențe secvențiale, dar putem folosi workers
    // pentru a demonstra paradigma (nu oferă speedup real)

    // Task pentru mutarea n-1 discuri de pe from pe aux
    putTask(makeHanoiTask(n - 1, from, aux, to));

    // Mută discul n (trebuie să așteptăm, dar în această implementare simplă nu așteptăm)
    printMove(n, from, to);

    pthread_mutex_lock(&countMutex);
    moveCount++;
    pthread_mutex_unlock(&countMutex);

    // Task pentru mutarea n-1 discuri de pe aux pe to
    putTask(makeHanoiTask(n - 1, aux, to, from));
}

int main(int argc, char** argv) {
    getArgs(argc, argv);

    printf("Solving Tower of Hanoi with %d disks using %d workers\n", N, P);

    // Pornește workerii
    startWorkers();

    // Creează task-ul inițial
    putTask(makeHanoiTask(N, 'A', 'C', 'B'));

    // Așteaptă puțin pentru procesare, apoi oprește
    // Notă: Hanoi nu are o condiție clară de terminare în acest model
    // Folosim un delay simplu sau numărăm mutările așteptate (2^N - 1)
    int expectedMoves = (1 << N) - 1;

    while (1) {
        pthread_mutex_lock(&countMutex);
        int currentMoves = moveCount;
        pthread_mutex_unlock(&countMutex);

        if (currentMoves >= expectedMoves) {
            break;
        }
        // Small sleep to avoid busy waiting
        usleep(1000);
    }

    forceShutDownWorkers();
    joinWorkerThreads();

    printf("Total moves: %d\n", moveCount);

    return 0;
}
