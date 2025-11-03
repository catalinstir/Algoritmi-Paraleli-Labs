#include <math.h>
#include <pthread.h>
#include <stdint.h>
#include <stdio.h>
#include <stdlib.h>

int printLevel;
int N;
int P;
int root;
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
}

int getNextSquare(int n) {
    int root = sqrt(n);
    if (root * root < n)
        root++;
    return root * root;
}

void init() {
    int i;

    // resizing N to a square
    N = getNextSquare(N);
    root = sqrt(N);
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

void compare_swap(int* a, int* b, int ascending) {
    if (ascending) {
        if (*a > *b) {
            int temp = *a;
            *a = *b;
            *b = temp;
        }
    } else {
        if (*a < *b) {
            int temp = *a;
            *a = *b;
            *b = temp;
        }
    }
}

void sort_row(int* matrix, int row, int cols, int ascending) {
    int start = row * cols;

    for (int i = 0; i < cols - 1; i++) {
        for (int j = 0; j < cols - 1 - i; j++) {
            compare_swap(&matrix[start + j], &matrix[start + j + 1], ascending);
        }
    }
}

void sort_column(int* matrix, int col, int rows, int cols, int ascending) {
    for (int i = 0; i < rows - 1; i++) {
        for (int j = 0; j < rows - 1 - i; j++) {
            int idx1 = j * cols + col;
            int idx2 = (j + 1) * cols + col;
            compare_swap(&matrix[idx1], &matrix[idx2], ascending);
        }
    }
}

void snake_to_rowmajor(int* matrix, int rows, int cols, int* output) {
    int idx = 0;
    for (int row = 0; row < rows; row++) {
        if (row % 2 == 0) {
            for (int col = 0; col < cols; col++) {
                output[idx++] = matrix[row * cols + col];
            }
        } else {
            for (int col = cols - 1; col >= 0; col--) {
                output[idx++] = matrix[row * cols + col];
            }
        }
    }
}

void* threadFuncion(void* arg) {
    int tid = *(int*)arg;

    int phases = (int)ceil(log2(N)) + 1;
    int chunks = (root + P - 1) / P;
    int start = tid * chunks;
    int end = (start + chunks < root) ? start + chunks : root;


    for (int phase = 0; phase < phases; phase++) {
        for (int row = start; row < end; row++) {
            int ascending = (row % 2 == 0);
            sort_row(v, row, root, ascending);
        }

        pthread_barrier_wait(&bar);

        for (int col = start; col < end; col++) {
            sort_column(v, col, root, root, 1);
        }

        pthread_barrier_wait(&bar);
    }
    for (int row = start; row < end; row++) {
        int ascending = (row % 2 == 0);
        sort_row(v, row, root, ascending);
    }

    pthread_barrier_wait(&bar);

    if (tid == 0) {
        snake_to_rowmajor(v, root, root, vNew);
        for (int i = 0; i < N; i++) {
            v[i] = vNew[i];
        }
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

    pthread_t* threads;
    threads = (pthread_t*)malloc(sizeof(pthread_t) * P);
    int* thread_ids;
    thread_ids = (int*)malloc(sizeof(int) * P);
    if(!threads || !thread_ids){
        perror("Error allocating memory!");
        exit(EXIT_FAILURE);
    }

    pthread_barrier_init(&bar, NULL, P);

    for (int i = 0; i < P; i++) {
        thread_ids[i] = i;
        pthread_create(&threads[i], NULL, threadFuncion, &thread_ids[i]);
    }

    for (int i = 0; i < P; i++) {
        pthread_join(threads[i], NULL);
    }
    pthread_barrier_destroy(&bar);

    print();

    return 0;
}
