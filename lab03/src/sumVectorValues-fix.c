#include <pthread.h>
#include <stdio.h>
#include <stdlib.h>

int printLevel;
int N;
int P;
long long* v;
long long sum;

pthread_mutex_t sum_mutex;

void getArgs(int argc, char** argv) {
    if (argc < 4) {
        printf("Not enough paramters: ./program N printLevel P\nprintLevel: 0=no, 1=some, "
               "2=verbouse\n");
        exit(1);
    }
    N = atoi(argv[1]);
    printLevel = atoi(argv[2]);
    P = atoi(argv[3]);
}

long long* allocVector(int N) {
    long long* v = malloc(sizeof(long long) * N);
    if (v == NULL) {
        printf("malloc failed!");
        exit(1);
    }
    return v;
}

void init() {
    v = allocVector(N);

    long long i;
    for (i = 0; i < N; i++) {
        v[i] = i + 1;
    }
}

void printPartial() {
    printf("Sum: %lli \n", sum);
}

void printAll() {
    printPartial();
}

void print() {
    if (printLevel == 0)
        return;
    else if (printLevel == 1)
        printPartial();
    else
        printAll();
}

void* threadFunciton(void* arg) {
    int tid = *(int*)arg;
    int chunk = N / P;
    int start = tid * chunk;
    int end = (tid == (P - 1)) ? N : start + chunk;
    // printf("Thread %d start: %d and end: %d\n", tid, start, end);

    long long local_sum = 0;
    for (int i = start; i < end; ++i) {
        local_sum += v[i];
    }

    pthread_mutex_lock(&sum_mutex);
    sum += local_sum;
    pthread_mutex_unlock(&sum_mutex);

    return NULL;
}

int main(int argc, char* argv[]) {
    getArgs(argc, argv);
    init();

    pthread_mutex_init(&sum_mutex, NULL);

    pthread_t* threads;
    int* thread_id;
    threads = (pthread_t*)malloc(sizeof(pthread_t) * P);
    thread_id = (int*)malloc(sizeof(int) * P);
    if (!threads || !thread_id) {
        perror("Error allocating memory");
        exit(EXIT_FAILURE);
    }

    for (int i = 0; i < P; ++i) {
        thread_id[i] = i;
        pthread_create(&threads[i], NULL, threadFunciton, &thread_id[i]);
    }

    for (int i = 0; i < P; ++i) {
        pthread_join(threads[i], NULL);
    }

    print();
    free(threads);
    free(thread_id);
    pthread_mutex_destroy(&sum_mutex);

    return 0;
}
