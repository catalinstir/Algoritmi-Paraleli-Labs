#include <pthread.h>
#include <semaphore.h>
#include <stdio.h>
#include <stdlib.h>
#include <unistd.h>

int N;
int P;
int printLevel;
sem_t eating;

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

pthread_mutex_t* forkmutex;

void* phylosopherThread(void* var) {
    int thread_id = *(int*)var;
    int i;

    for (i = 0; i < N; i++) {
        sem_wait(&eating);

        int right_fork = (thread_id + 1) % P;
        int left_fork = thread_id;
        int first_fork = right_fork > left_fork ? right_fork : left_fork;
        int second_fork = right_fork < left_fork ? right_fork : left_fork;

        // sleep(1);

        pthread_mutex_lock(&forkmutex[first_fork]);
        pthread_mutex_lock(&forkmutex[second_fork]);

        // Works with the sleeps too
        // printf("thinking %i\n", thread_id);
        // sleep(2);

        pthread_mutex_unlock(&forkmutex[second_fork]);
        pthread_mutex_unlock(&forkmutex[first_fork]);

        sem_post(&eating);
    }

    return NULL;
}

int main(int argc, char** argv) {
    getArgs(argc, argv);

    int i;
    pthread_t tid[P];
    int thread_id[P];
    for (i = 0; i < P; i++)
        thread_id[i] = i;
    sem_init(&eating, 0, P - 1);

    forkmutex = malloc(sizeof(pthread_mutex_t) * P);
    for (i = 0; i < P; i++)
        pthread_mutex_init(&forkmutex[i], NULL);

    for (i = 0; i < P; i++) {
        pthread_create(&(tid[i]), NULL, phylosopherThread, &(thread_id[i]));
    }

    for (i = 0; i < P; i++) {
        pthread_join(tid[i], NULL);
    }

    for (i = 0; i < P; i++)
        pthread_mutex_destroy(&forkmutex[i]);

    printf("All phylosophers have eaten\n");

    sem_destroy(&eating);

    return 0;
}
