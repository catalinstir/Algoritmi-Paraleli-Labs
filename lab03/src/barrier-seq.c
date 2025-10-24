#include <pthread.h>
#include <stdio.h>
#include <stdlib.h>

int printLevel;
int N;
int P;

void* threadFunction(void* var) {
    int thread_id = *(int*)var;
    if (thread_id == 0)
        printf("I should be displayed last\n");
    else if (thread_id == 1)
        printf("I should be displayed in the middle\n");
    else if (thread_id == 2)
        printf("I should be displayed first\n");

    return NULL;
}

int main() {
    P = 3;
    int i;

    int thread_id[P];
    for (i = 0; i < P; i++)
        thread_id[i] = i;

    for (i = P - 1; i >= 0; i--) {
        threadFunction(&(thread_id[i]));
    }

    return 0;
}
