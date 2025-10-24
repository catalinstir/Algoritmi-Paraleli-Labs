#include <pthread.h>
#include <stdio.h>
#include <unistd.h>

int printLevel;
int N;
int P;

pthread_mutex_t mutexA;
pthread_mutex_t mutexB;

void* threadFunction(void* var) {
    // TODO preserve the correct order by using barriers
    int thread_id = *(int*)var;
    (void)thread_id; // unused

    pthread_mutex_lock(&mutexA);
    sleep(1);
    pthread_mutex_lock(&mutexB);

    printf("There should be two messages displayed, I am one of them\n");

    pthread_mutex_unlock(&mutexB);
    pthread_mutex_unlock(&mutexA);

    return NULL;
}

int main() {
    P = 2;
    int i;

    pthread_mutex_init(&mutexA, NULL);
    pthread_mutex_init(&mutexB, NULL);
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

    pthread_mutex_destroy(&mutexA);
    pthread_mutex_destroy(&mutexB);
    return 0;
}
