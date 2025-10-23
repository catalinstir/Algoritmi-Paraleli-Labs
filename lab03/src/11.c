#include <stdio.h>
#include <pthread.h>

void* threadFunction(void *arg) {
    int id = *(int*)arg;
    printf("Hello from thread %d in program 11\n", id);
    return NULL;
}

int main() {
    pthread_t t;
    int id = 1;
    pthread_create(&t, NULL, threadFunction, &id);
    pthread_join(t, NULL);
    return 0;
}
