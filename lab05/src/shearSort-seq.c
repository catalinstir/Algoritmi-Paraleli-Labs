#include <stdint.h>
#include <math.h>
#include <pthread.h>
#include <stdio.h>
#include <stdlib.h>

int printLevel;
int N;
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
    if (argc < 3) {
        printf("Not enough paramters: ./program N printLevel\nprintLevel: 0=no, 1=some, "
               "2=verbouse\n");
        exit(1);
    }
    N = atoi(argv[1]);
    printLevel = atoi(argv[2]);
}

int getNextSquare(int n){
    int root = sqrt(n);
    if(root * root < n)
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

void compare_swap(int *a, int *b, int ascending) {
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

void sort_row(int *matrix, int row, int cols, int ascending) {
    int start = row * cols;
    
    for (int i = 0; i < cols - 1; i++) {
        for (int j = 0; j < cols - 1 - i; j++) {
            compare_swap(&matrix[start + j], &matrix[start + j + 1], ascending);
        }
    }
}

void sort_column(int *matrix, int col, int rows, int cols, int ascending) {
    for (int i = 0; i < rows - 1; i++) {
        for (int j = 0; j < rows - 1 - i; j++) {
            int idx1 = j * cols + col;
            int idx2 = (j + 1) * cols + col;
            compare_swap(&matrix[idx1], &matrix[idx2], ascending);
        }
    }
}
void shear_sort(int *matrix, int rows, int cols) {
    int phases = (int)ceil(log2(rows * cols)) + 1;
    
    for (int phase = 0; phase < phases; phase++) {
        for (int row = 0; row < rows; row++) {
            int ascending = (row % 2 == 0);
            sort_row(matrix, row, cols, ascending);
        }
        for (int col = 0; col < cols; col++) {
            sort_column(matrix, col, rows, cols, 1);
        }
    }
    
    for (int row = 0; row < rows; row++) {
        int ascending = (row % 2 == 0);
        sort_row(matrix, row, cols, ascending);
    }
}
void snake_to_rowmajor(int *matrix, int rows, int cols, int *output) {
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

int main(int argc, char* argv[]) {
    int i;
    getArgs(argc, argv);
    init();

    // make copy to check it against qsort
    // DO NOT MODIFY
    for (i = 0; i < N; i++)
        vQSort[i] = v[i];
    qsort(vQSort, N, sizeof(int), cmp);

    shear_sort(v, root, root);
    snake_to_rowmajor(v, root, root, vNew);
    for (i = 0; i < N; i++) {
        v[i] = vNew[i];
    }

    print();

    return 0;
}

