#include <pthread.h>
#include <stdio.h>
#include <stdlib.h>

pthread_barrier_t barriers[3];

void* threadFunction(void* var) {
    int thread_id = *(int*)var;

    pthread_barrier_wait(&barriers[thread_id]);

    if (thread_id == 0)
        printf("I should be displayed last\n");
    else if (thread_id == 1)
        printf("I should be displayed in the middle\n");
    else if (thread_id == 2)
        printf("I should be displayed first\n");

    if (thread_id != 0) {
        pthread_barrier_wait(&barriers[thread_id - 1]);
    }

    return NULL;
}

int main(int argc, char* argv[]) {
    if (argc < 2) {
        printf("Wrong usage. Usage: program N\n");
        exit(EXIT_FAILURE);
    }
    (void)argv; // unused

    int P = 3;
    int i;

    pthread_t tid[P];
    int thread_id[P];
    for (i = 0; i < P; i++) {
        pthread_barrier_init(&barriers[i], NULL, 2);
    }

    for (i = 0; i < P; i++)
        thread_id[i] = i;

    for (i = 0; i < P; i++) {
        pthread_create(&(tid[i]), NULL, threadFunction, &(thread_id[i]));
    }

    pthread_barrier_wait(&barriers[2]);

    for (i = 0; i < P; i++) {
        pthread_join(tid[i], NULL);
        pthread_barrier_destroy(&barriers[i]);
    }

    return 0;
}
