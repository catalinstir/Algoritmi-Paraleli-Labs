#include <time.h>
#include <unistd.h>
#include <stdlib.h>
#include <pthread.h>

double glob = 0;

void* threadFunction(void *var)
{
    (void)var; // unused

    glob = rand();
    while(1){
        glob /= 3.0;
        glob *= 1.6;
    }

    return NULL;
}

int main()
{
	long P = sysconf(_SC_NPROCESSORS_ONLN);
	int i;

    srand(time(NULL));
	pthread_t tid[P];
	int thread_id[P];

	for(i = 0;i < P; i++)
		thread_id[i] = i;

	for(i = 0; i < P; i++) {
		pthread_create(&(tid[i]), NULL, threadFunction, &(thread_id[i]));
	}

	for(i = 0; i < P; i++) {
		pthread_join(tid[i], NULL);
	}

	return 0;
}
