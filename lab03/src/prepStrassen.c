#include <pthread.h>
#include <stdio.h>
#include <stdlib.h>

int printLevel;
int N;
int P = 7;
int A11, A12, A21, A22, B11, B12, B21, B22;
int M1, M2, M3, M4, M5, M6, M7;
int C11, C12, C21, C22;

void init() {
    A11 = 3213215;
    A12 = 5454;
    A21 = 5456;
    A22 = 9898;
    B11 = 54544;
    B12 = 90821;
    B21 = 9807879;
    B22 = 329132;
}

void print() {
    printf("%i %i %i %i \n", C11, C12, C21, C22);
}

void* calcM1(void* arg) {
    (void)arg;
    M1 = (A11 + A22) * (B11 + B22);

    return NULL;
}

void* calcM2(void* arg) {
    (void)arg;
    M2 = (A21 + A22) * B11;

    return NULL;
}

void* calcM3(void* arg) {
    (void)arg;
    M3 = A11 * (B12 - B22);

    return NULL;
}

void* calcM4(void* arg) {
    (void)arg;
    M4 = A22 * (B21 - B11);

    return NULL;
}

void* calcM5(void* arg) {
    (void)arg;
    M5 = (A11 + A12) * B22;

    return NULL;
}

void* calcM6(void* arg) {
    (void)arg;
    M6 = (A21 - A11) * (B11 + B12);

    return NULL;
}

void* calcM7(void* arg) {
    (void)arg;
    M7 = (A12 - A22) * (B21 + B22);

    return NULL;
}

int main() {
    init();

    long long i = 0;
    pthread_t threads[P];

    pthread_create(&threads[0], NULL, calcM1, &i);
    pthread_create(&threads[1], NULL, calcM2, &i);
    pthread_create(&threads[2], NULL, calcM3, &i);
    pthread_create(&threads[3], NULL, calcM4, &i);
    pthread_create(&threads[4], NULL, calcM5, &i);
    pthread_create(&threads[5], NULL, calcM6, &i);
    pthread_create(&threads[6], NULL, calcM7, &i);

    for (i = 0; i < P; ++i) {
        pthread_join(threads[i], NULL);
    }

    C11 = M1 + M4 - M5 + M7;
    C12 = M3 + M5;
    C21 = M2 + M4;
    C22 = M1 - M2 + M3 + M6;

    print();

    return 0;
}
