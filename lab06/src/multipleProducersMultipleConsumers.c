#include <pthread.h>
#include <semaphore.h>
#include <stdio.h>
#include <stdlib.h>

int N;
int P;
int printLevel;
pthread_mutex_t produceMutex = PTHREAD_MUTEX_INITIALIZER;
pthread_mutex_t consumeMutex = PTHREAD_MUTEX_INITIALIZER;
pthread_mutex_t resultsMutex = PTHREAD_MUTEX_INITIALIZER;
int prodIdx = 0;
int consIdx = 0;
sem_t empty, full;
int* buffer;
int BUFFER_SIZE = 5;
int* results;

void getArgs(int argc, char** argv) {
    if (argc < 4) {
        printf("Not enough paramters: ./program N printLevel P\nprintLevel: 0=no, 1=some, "
               "2=verbouse\n");
        exit(1);
    }
    N = atoi(argv[1]);
    printLevel = atoi(argv[2]);
    P = atoi(argv[3]);
    if (P % 2 || P < 4) {
        printf("P needs to be even and bigger or equal to 4\n");
        exit(1);
    }
}

int get() {
    sem_wait(&full);
    pthread_mutex_lock(&consumeMutex);

    int value = buffer[consIdx];
    consIdx = (consIdx + 1) % BUFFER_SIZE;

    pthread_mutex_unlock(&consumeMutex);
    sem_post(&empty);
    return value;
}

void put(int value) {
    sem_wait(&empty);
    pthread_mutex_lock(&produceMutex);

    buffer[prodIdx] = value;
    prodIdx = (prodIdx + 1) % BUFFER_SIZE;

    pthread_mutex_unlock(&produceMutex);
    sem_post(&full);
}

void* consumerThread(void* var) {
    (void)var;
    int aux;
    int i;
    for (i = 0; i < N; i++) {
        aux = get();
        pthread_mutex_lock(&resultsMutex);
        results[aux]++;
        pthread_mutex_unlock(&resultsMutex);
    }
    return NULL;
}

void* producerThread(void* var) {
    (void)var;
    int i;

    for (i = 0; i < N; i++) {
        put(i);
    }

    return NULL;
}

int main(int argc, char** argv) {
    getArgs(argc, argv);

    int i;
    int NPROD = P / 2;
    int NCONS = P / 2;
    pthread_t tid[NPROD + NCONS];

    sem_init(&full, 0, 0);
    sem_init(&empty, 0, BUFFER_SIZE);

    buffer = malloc(BUFFER_SIZE * sizeof(int));
    results = calloc(N, sizeof(int));

    for (i = 0; i < NPROD; i++) {
        pthread_create(&(tid[i]), NULL, producerThread, NULL);
    }
    for (; i < NPROD + NCONS; i++) {
        pthread_create(&(tid[i]), NULL, consumerThread, NULL);
    }

    for (i = 0; i < NPROD + NCONS; i++) {
        pthread_join(tid[i], NULL);
    }

    for (int i = 0; i < N; i++) {
        if (results[i] != NPROD) {
            printf("FAILED, the produced data does not match the consumed data\n");
            printf("Should be %d, is %d\n", NPROD, results[i]);
            exit(1);
        }
    }
    printf("CORRECT\n");

    return 0;
}
