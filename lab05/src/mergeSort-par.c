#include <math.h>
#include <pthread.h>
#include <stdint.h>
#include <stdio.h>
#include <stdlib.h>

int printLevel;
int N;
int P;
int* v;
int* vQSort;
int* vNew;

pthread_barrier_t bar;

unsigned int isPowerOf2(const int n) {
    return n && !(n & (n - 1));
}

unsigned int fastLog2(const unsigned int n) {
    if (!isPowerOf2(n)) {
        return log2(n);
    }
    return ((sizeof(n) * 8) - 1) - __builtin_clz(n);
}

void merge(int source[], int start, int mid, int end, int destination[]) {
    // DO NOT MODIFY
    int iA = start;
    int iB = mid;
    int i;

    for (i = start; i < end; i++) {
        if (end == iB || (iA < mid && source[iA] <= source[iB])) {
            destination[i] = source[iA];
            iA++;
        } else {
            destination[i] = source[iB];
            iB++;
        }
    }
}

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
    if (!isPowerOf2(N)) {
        perror("N needs to be a power of 2");
        exit(EXIT_FAILURE);
    }
    printLevel = atoi(argv[2]);
    P = atoi(argv[3]);
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
    int width;

    for (width = 1; width < N; width = 2 * width) {
        int total_merges = (N + 2 * width - 1) / (2 * width);
        int local_merges = (total_merges + P - 1) / P;

        int start = tid * local_merges;
        int end = (start + local_merges < total_merges) ? (start + local_merges) : total_merges;

        for (int idx = start; idx < end; idx++) {
            int left = idx * 2 * width;
            if (left >= N)
                continue;

            int mid = (left + width > N) ? N : left + width;
            int right = (left + 2 * width > N) ? N : left + 2 * width;

            merge(v, left, mid, right, vNew);
        }

        pthread_barrier_wait(&bar);

        if (tid == 0) {
            int* temp = v;
            v = vNew;
            vNew = temp;
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

    pthread_barrier_init(&bar, NULL, P);

    pthread_t tid[P];
    int thread_id[P];
    for (i = 0; i < P; i++)
        thread_id[i] = i;

    for (i = 0; i < P; i++) {
        pthread_create(&(tid[i]), NULL, threadFunction, &(thread_id[i]));
    }

    for (i = 0; i < P; i++) {
        pthread_join(tid[i], NULL);
    }

    print();
    return 0;
}
