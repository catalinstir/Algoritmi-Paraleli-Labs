#include <stdio.h>
#include <pthread.h>
#include <time.h>

void* threadFunction(void *var)
{
	int thread_id = *(int*)var;

	printf("Hello world from thread %i\n", thread_id);

    return NULL;
}

void *threadFunction2(void *var)
{
    int thread_id = *(int*)var;

    printf("Salutare Planeta!\n");

    return NULL;
}

int main(int argc, char **argv)
{
	int P = 2;
	int i;

	pthread_t tid[P];
    int thread_ids[] = {0, 1};

    pthread_create(&(tid[0]), NULL, threadFunction, &thread_ids[0]);
    pthread_create(&(tid[1]), NULL, threadFunction2, &thread_ids[1]);

	for(i = 0; i < 2; i++) {
		pthread_join(tid[i], NULL);
	}

	return 0;
}

