#include <math.h>
#include <pthread.h>
#include <stdio.h>
#include <stdlib.h>

int printLevel;
int N;
int P;
int* v;
int* vQSort;
int* vNew;
pthread_barrier_t bar;

void compareVectors(int* a, int* b) {
    // DO NOT MODIFY
    int i;
    for (i = 0; i < N; i++) {
        if (a[i] != b[i]) {
            printf("Sorted incorrectly\n");
            return;
        }
    }
    printf("Sorted correctly\n");
}

void displayVector(int* v) {
    // DO NOT MODIFY
    int i;
    int max = 1;
    for (i = 0; i < N; i++)
        if (max < log10(v[i]))
            max = log10(v[i]);
    int displayWidth = 2 + max;
    for (i = 0; i < N; i++) {
        printf("%*i", displayWidth, v[i]);
        if (!((i + 1) % 20))
            printf("\n");
    }
    printf("\n");
}

int cmp(const void* a, const void* b) {
    // DO NOT MODIFY
    int A = *(int*)a;
    int B = *(int*)b;
    return A - B;
}

void getArgs(int argc, char** argv) {
    if (argc < 4) {
        printf("Not enough paramters: ./program N printLevel P\nprintLevel: 0=no, 1=some, "
               "2=verbouse\n");
        exit(1);
    }
    N = atoi(argv[1]);
    printLevel = atoi(argv[2]);
    P = atoi(argv[3]);
    if (P > (N / 2))
        P = N;
}

void init() {
    int i;
    v = malloc(sizeof(int) * N);
    vQSort = malloc(sizeof(int) * N);
    vNew = malloc(sizeof(int) * N);
    if (v == NULL) {
        printf("malloc failed!");
        exit(1);
    }

    // generate the vector v with random values
    // DO NOT MODIFY
    srand(42);
    for (i = 0; i < N; i++)
        v[i] = rand() % N;
}

void printPartial() {
    compareVectors(v, vQSort);
}

void printAll() {
    displayVector(v);
    displayVector(vQSort);
    compareVectors(v, vQSort);
}

void print() {
    if (printLevel == 0)
        return;
    else if (printLevel == 1)
        printPartial();
    else
        printAll();
}

void* threadFunction(void* arg) {
    int tid = *(int*)arg;
    int pairs = N / 2;
    int chunk = pairs / P;
    int start = tid * chunk;
    int end = (tid == (P - 1)) ? (pairs) : (start + chunk);

    for (int i = 0; i < N; i++) {
        int phase = (i % 2);
        for (int j = start; j < end; j++) {
            int idx = 2 * j + phase;
            if (idx + 1 < N && v[idx] > v[idx + 1]) {
                v[idx] = v[idx] ^ v[idx + 1];
                v[idx + 1] = v[idx] ^ v[idx + 1];
                v[idx] = v[idx] ^ v[idx + 1];
            }
        }
        pthread_barrier_wait(&bar);
    }

    return NULL;
}

int main(int argc, char* argv[]) {
    int i;
    getArgs(argc, argv);
    init();

    // make copy to check it against qsort
    // DO NOT MODIFY
    for (i = 0; i < N; i++)
        vQSort[i] = v[i];
    qsort(vQSort, N, sizeof(int), cmp);

    pthread_t tid[P];
    int thread_id[P];
    for (i = 0; i < P; i++)
        thread_id[i] = i;

    pthread_barrier_init(&bar, NULL, P);

    for (i = 0; i < P; i++) {
        pthread_create(&(tid[i]), NULL, threadFunction, &(thread_id[i]));
    }

    for (i = 0; i < P; i++) {
        pthread_join(tid[i], NULL);
    }

    pthread_barrier_destroy(&bar);

    print();

    return 0;
}
