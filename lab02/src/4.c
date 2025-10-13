#include <stdio.h>
#include <stdlib.h>
#include <pthread.h>

#define NREPS 10000

int NReps;
int printLevel;
int N;
int P;
int* a;
int* b;
int* c;

void getArgs(int argc, char **argv)
{
	if(argc < 4) {
		printf("Not enough paramters: ./program N printLevel P\nprintLevel: 0=no, 1=some, 2=verbouse\n");
		exit(1);
	}
	N = atoi(argv[1]);
	printLevel = atoi(argv[2]);
	P = atoi(argv[3]);
}

void init()
{
	a = malloc(sizeof(int) * N);
	b = malloc(sizeof(int) * N);
	c = malloc(sizeof(int) * N);
	if(a == NULL || b == NULL || c == NULL) {
		printf("malloc failed!");
		exit(1);
	}

	int i;
	for(i = 0; i < N; i++) {
		a[i] = i%500;
		b[i] = i%500;
	}

    NReps = NREPS;
}

void printPartial()
{
	int i;
	for(i = 0; i < 10; i++)
		printf("%i ", c[i]);
	printf("\n");
	for(i = 20; i < N; i+=N/10)
		printf("%i ", c[i]);
	printf("\n");
	for(i = N - 10; i < N; i++)
		printf("%i ", c[i]);
	printf("\n");
}

void printAll()
{
	int i;
	for(i = 0; i < N; i++)
		printf("%i ", c[i]);
	printf("\n");
}

void print()
{
	if(printLevel == 0)
		return;
	else if(printLevel == 1)
		printPartial();
	else
		printAll();
}

void* add_vectors(void *var) {
    int tid = *(int*)var;

    int chunk = N / P;
    int remainder = N % P;
    int start, end;

    if (tid < remainder) {
        start = tid * (chunk + 1);
        end = start + (chunk + 1);
    } else {
        start = tid * chunk + remainder;
        end = start + chunk;
    }

    for (int i = 0; i < NReps; i++) {
        for (int j = start; j < end; j++) {
            c[j] = a[j] + b[j];
        }
    }

    return NULL;
}

int main(int argc, char *argv[])
{
	getArgs(argc, argv);
	init();

    pthread_t *tid = (pthread_t*)malloc(sizeof(pthread_t) * P);
    int *thread_id = (int*)malloc(sizeof(int) * P);
    if(tid == NULL || thread_id == NULL){
        perror("err allocating mem");
        exit(EXIT_FAILURE);
    }

    for(int i = 0; i < P; i++){
        thread_id[i] = i;
        pthread_create(&(tid[i]), NULL, add_vectors, &(thread_id[i]));
    }

    for(int i = 0; i < P; i++){
        pthread_join(tid[i], NULL);
    }

	print();

	return 0;
}
