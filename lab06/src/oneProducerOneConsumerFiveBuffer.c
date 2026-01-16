#include <pthread.h>
#include <stdio.h>
#include <stdlib.h>
// #include <math.h>
#include <semaphore.h>

int N;
int P;
int printLevel;
int* buffer;
int BUFFER_SIZE = 5;
int prodIdx = 0;
int consIdx = 0;

pthread_mutex_t mut;
sem_t empty, full;

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

int get() {
    sem_wait(&full);
    int value = buffer[consIdx];
    consIdx = (consIdx + 1) % BUFFER_SIZE;
    sem_post(&empty);

    return value;
}

void put(int value) {
    sem_wait(&empty);
    buffer[prodIdx] = value;
    prodIdx = (prodIdx + 1) % BUFFER_SIZE;
    sem_post(&full);
}

void* consumerThread(void* var) {
    int i;
    (void)var;

    for (i = 0; i < N; i++) {
        int value = get();
        if (value != i) {
            printf("Wrong value. I was supposed to get %i but I received %i\n", i, value);
            exit(1);
        }
    }
    printf("I finished Correctly\n");

    return NULL;
}

void* producerThread(void* var) {
    int i;
    (void)var;

    for (i = 0; i < N; i++) {
        put(i);
    }

    return NULL;
}

int main(int argc, char** argv) {
    getArgs(argc, argv);

    int i;
    int NPROD = 1;
    int NCONS = 1;
    pthread_t tid[NPROD + NCONS];
    buffer = malloc(BUFFER_SIZE * sizeof(int));
    sem_init(&empty, 0, BUFFER_SIZE);
    sem_init(&full, 0, 0);

    for (i = 0; i < NPROD; i++) {
        pthread_create(&(tid[i]), NULL, producerThread, NULL);
    }
    for (; i < NPROD + NCONS; i++) {
        pthread_create(&(tid[i]), NULL, consumerThread, NULL);
    }

    for (i = 0; i < NPROD + NCONS; i++) {
        pthread_join(tid[i], NULL);
    }

    return 0;
}
