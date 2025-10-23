#include <stdio.h>
#include <stdlib.h>
#include <pthread.h>
#include <semaphore.h>

int printLevel;
int N;
int P;
sem_t sem1, sem2;


void* threadFunction(void *var)
{
	//TODO preserve the correct order by using semaphores you will need to add a library and the correct flag when compiling
	int thread_id = *(int*)var;
	if(thread_id==0){
        sem_wait(&sem2);
		printf("I should be displayed last\n");
    }
	else if(thread_id==1){
		printf("I should be displayed first or in the middle\n");
        sem_post(&sem1);
    }
	else if(thread_id==2){
        sem_wait(&sem1);
		printf("I should be displayed first or in the middle\n");
        sem_post(&sem2);
    }

    return NULL;
}

int main()
{
	P = 3; // ATTENTION, WE OVERWRITE THE NUMBER OF THREADS. WE ONLY NEED 3
	int i;

    sem_init(&sem1, 0, 0);
    sem_init(&sem2, 0, 0);

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

    sem_destroy(&sem1);
    sem_destroy(&sem2);

	return 0;
}
