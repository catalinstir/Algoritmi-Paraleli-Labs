#include <stdlib.h>
#include <stdio.h>
#include <pthread.h>
#include <time.h>

void* threadFunction(void *var)
{
	int thread_id = *(int*)var;

	// printf("Hello world from thread %i\n", thread_id);

    free(var);

    return NULL;
}

int main(int argc, char **argv)
{
	int i;
    int P = 100000;
    pthread_t *tid = (pthread_t*)malloc(P * sizeof(pthread_t));
    if(!tid){
        perror("Error allocating memory\n");
        exit(EXIT_FAILURE);
    }

    int thread_count = 0;
    while(thread_count < P){
        int *arg = malloc(sizeof(int));
        if(!arg){
            perror("Error allocating memory\n");
            exit(EXIT_FAILURE);
        }
        *arg = thread_count;
        int r = pthread_create(&tid[thread_count], NULL, threadFunction , arg);
        if(r != 0){
            printf("pthread_failed at thread %i\n", thread_count);
            exit(EXIT_FAILURE);
        }
        thread_count++;
    }

	for(i = 0; i < P; i++) {
		pthread_join(tid[i], NULL);
	}

	return 0;
}


