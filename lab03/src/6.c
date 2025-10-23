#include <pthread.h>
#include <stdio.h>

int printLevel;
int N;
int P;

pthread_mutex_t mutex;
pthread_barrier_t bar1, bar2;

void* threadFunction(void* var) {
    int thread_id = *(int*)var;

    pthread_barrier_wait(&bar1);

    if (thread_id == 0) {
        pthread_mutex_lock(&mutex);
        printf("There should be two messages displayed, I am one of them\n");
    }

    pthread_barrier_wait(&bar2);

    if (thread_id == 1) {
        // pthread_mutex_lock(&mutex);
        printf("There should be two messages displayed, I am one of them\n");
    }

    return NULL;
}

int main() {
    P = 2;
    int i;

    pthread_mutex_init(&mutex, NULL);

    pthread_barrier_init(&bar1, NULL, 2);
    pthread_barrier_init(&bar2, NULL, 2);

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

    pthread_mutex_destroy(&mutex);
    pthread_barrier_destroy(&bar1);
    pthread_barrier_destroy(&bar2);
    return 0;
}
