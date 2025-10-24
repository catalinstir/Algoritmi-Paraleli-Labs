#include <pthread.h>
#include <stdio.h>
#include <unistd.h>

int printLevel;
int N;
int P;

pthread_mutex_t mutex;
pthread_mutexattr_t mutexattr;
pthread_barrier_t bar1, bar2;

void* threadFunction(void* var) {
    int thread_id = *(int*)var;
    if (thread_id == 0) {
        pthread_mutex_lock(&mutex);
        pthread_mutex_lock(&mutex);
        printf("There should be two messages displayed, I am one of them\n");
    } else {
        printf("There should be two messages displayed, I am one of them\n");
    }

    return NULL;
}

int main() {
    P = 2;
    int i;

    pthread_mutexattr_init(&mutexattr);
    pthread_mutexattr_settype(&mutexattr, PTHREAD_MUTEX_RECURSIVE);
    pthread_mutex_init(&mutex, &mutexattr);

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
    pthread_mutexattr_destroy(&mutexattr);

    return 0;
}
