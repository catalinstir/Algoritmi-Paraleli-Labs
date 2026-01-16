#include <pthread.h>
#include <stdio.h>
#include <stdlib.h>

int N;
int P;
int printLevel;

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

int resource = 0;

int read_write_check = 0;
int write_write_check = 0;
int read_read_check = 0;
pthread_mutex_t rwmutex = PTHREAD_MUTEX_INITIALIZER;
pthread_mutex_t wmutex = PTHREAD_MUTEX_INITIALIZER;
pthread_mutex_t rmutex = PTHREAD_MUTEX_INITIALIZER;
int readers = 0;

int get() {
    read_read_check += 2;
    pthread_mutex_lock(&rwmutex);
    read_write_check += 2;
    int value = resource;
    pthread_mutex_unlock(&rwmutex);
    return value;
}

void put(int value) {
    write_write_check += 2;
    read_write_check += 2;
    resource = value;
}

int get_safe() {
    int aux;
    pthread_mutex_lock(&rmutex);
    readers++;
    if (readers == 1) {
        pthread_mutex_lock(&wmutex);
    }
    pthread_mutex_unlock(&rmutex);

    aux = get();

    pthread_mutex_lock(&rmutex);
    readers--;
    if (readers == 0) {
        pthread_mutex_unlock(&wmutex);
    }
    pthread_mutex_unlock(&rmutex);

    return aux;
}

void put_safe(int value) {
    pthread_mutex_lock(&wmutex);
    put(value);
    pthread_mutex_unlock(&wmutex);
}

int value;
void* readerThread(void* var) {
    (void)var;
    int i;

    for (i = 0; i < N; i++) {
        value = get_safe();
    }

    return NULL;
}

void* writerThread(void* var) {
    (void)var;
    int i;

    for (i = 0; i < N; i++) {
        put_safe(i);
    }

    return NULL;
}

int main(int argc, char** argv) {
    getArgs(argc, argv);

    int i;
    int NREAD = P;
    int NWRITE = P;
    pthread_t tid[NREAD + NWRITE];

    for (i = 0; i < NREAD; i++) {
        pthread_create(&(tid[i]), NULL, readerThread, NULL);
    }

    for (; i < NREAD + NWRITE; i++) {
        pthread_create(&(tid[i]), NULL, writerThread, NULL);
    }

    for (i = 0; i < NREAD + NWRITE; i++) {
        pthread_join(tid[i], NULL);
    }

    if (N * 2 * P == read_read_check && P > 1) {
        printf("Failed two simultaneous readers\n");
        return 1;
    }
    if (N * 2 * P * 2 != read_write_check) {
        printf("Failed read when write %i %i \n", N * 2 * P * 2, read_write_check);
        return 1;
    }
    if (N * 2 * P != write_write_check) {
        printf("Failed write when write\n");
        return 1;
    }
    printf("Passed all\n");

    return 0;
}
