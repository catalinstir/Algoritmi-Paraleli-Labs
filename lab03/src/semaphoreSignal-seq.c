#include <pthread.h>
#include <stdio.h>
#include <stdlib.h>

int printLevel;
int N;
int P;

void* threadFunction(void* var) {
    // TODO preserve the correct order by using semaphores you will need to add a library and the
    // correct flag when compiling
    int thread_id = *(int*)var;
    if (thread_id == 0)
        printf("I should be displayed last\n");
    else if (thread_id == 1)
        printf("I should be displayed first or in the middle\n");
    else if (thread_id == 2)
        printf("I should be displayed first or in the middle\n");

    return NULL;
}

void getArgs(int argc, char** argv) {
    (void)argc; // unused
    (void)argv; // unused
}

void init() {
}

void printAll() {
}

void printPartial() {
}

void print() {
    if (printLevel == 0)
        return;
    else if (printLevel == 1)
        printPartial();
    else
        printAll();
}

int main(int argc, char* argv[]) {
    getArgs(argc, argv);
    init();

    P = 3; // ATTENTION, WE OVERWRITE THE NUMBER OF THREADS. WE ONLY NEED 3
    int i;

    int thread_id[P];
    for (i = 0; i < P; i++)
        thread_id[i] = i;

    for (i = P - 1; i >= 0; i--) {
        threadFunction(&(thread_id[i]));
    }

    print();

    return 0;
}
