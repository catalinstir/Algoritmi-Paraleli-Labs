#include <math.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#include "Workers.h"

int N;
int printLevel;

// Counter for solutions found
int solutionsFound = 0;
pthread_mutex_t printMutex = PTHREAD_MUTEX_INITIALIZER;

typedef struct partialSolution {
	int* queens;
	int position;
	int step;
} partialSolution;

void backtrackingTask(void* data, int thread_id);

void getArgs(int argc, char** argv)
{
	if (argc < 4) {
		printf("Not enough paramters: ./program N printLevel P\nprintLevel: 0=no, 1=some, 2=verbouse\n");
		exit(1);
	}
	N = atoi(argv[1]);
	printLevel = atoi(argv[2]);
	P = atoi(argv[3]);
}

int canReject(partialSolution PS)
{
	for (int i = 0; i < PS.step; i++) {
		if (PS.queens[i] == PS.queens[PS.step])
			return 1;
		if (PS.queens[i] - PS.queens[PS.step] == i - PS.step)
			return 1;
		if (PS.queens[PS.step] - i == PS.queens[i] - PS.step)
			return 1;
	}
	return 0;
}

int isSolution(partialSolution PS)
{
	if (PS.step < N - 1)
		return 0;
	return !canReject(PS);
}

void printAll(partialSolution PS)
{
	for (int i = 0; i < N; i++)
		printf("%3i ", PS.queens[i]);
	printf("\n");
}

void print(partialSolution PS)
{
	if (printLevel == 0)
		return;
	pthread_mutex_lock(&printMutex);
	printAll(PS);
	pthread_mutex_unlock(&printMutex);
}

partialSolution increaseStep(partialSolution PS)
{
	partialSolution newPS;
	newPS.queens = malloc(sizeof(int) * N);
	memcpy(newPS.queens, PS.queens, sizeof(int) * N);
	newPS.step = PS.step + 1;
	newPS.position = 0;
	return newPS;
}

partialSolution getNextPartialSolution(partialSolution PS)
{
	partialSolution newPS;
	newPS.queens = malloc(sizeof(int) * N);
	memcpy(newPS.queens, PS.queens, sizeof(int) * N);
	newPS.queens[PS.step]++;
	newPS.position = PS.position + 1;
	newPS.step = PS.step;
	return newPS;
}

int hasSolutionAtStep(partialSolution PS)
{
	return (PS.step < N) && (PS.position < N);
}

Task makeBacktrackingTask(partialSolution PS)
{
	Task task;
	partialSolution* taskData = (partialSolution*)malloc(sizeof(partialSolution));
	*taskData = PS;
	task.data = taskData;
	task.runTask = backtrackingTask;
	return task;
}

void backtrackingTask(void* data, int thread_id)
{
	partialSolution* PSptr = (partialSolution*)data;
	partialSolution PS = *PSptr;
	free(PSptr);

	if (canReject(PS)) {
		free(PS.queens);
		return;
	}
	if (isSolution(PS)) {
		print(PS);
		solutionsFound++;
		free(PS.queens);
		// Force shutdown after finding N solutions
		if (solutionsFound >= N) {
			forceShutDownWorkers();
		}
		return;
	}

	PS = increaseStep(PS);
	while (hasSolutionAtStep(PS)) {
		PS = getNextPartialSolution(PS);
		// Submit task for next position
		partialSolution newPS;
		newPS.queens = malloc(sizeof(int) * N);
		memcpy(newPS.queens, PS.queens, sizeof(int) * N);
		newPS.step = PS.step;
		newPS.position = PS.position;
		putTask(makeBacktrackingTask(newPS));
	}

	free(PS.queens);
}

int main(int argc, char** argv)
{
	getArgs(argc, argv);

	// Start worker threads
	startWorkers();

	// Create initial task
	partialSolution initialValues;
	initialValues.queens = malloc(sizeof(int) * N);
	for (int i = 0; i < N; i++)
		initialValues.queens[i] = -1;
	initialValues.position = -1;
	initialValues.step = -1;
	putTask(makeBacktrackingTask(initialValues));

	// Wait for workers to finish
	joinWorkerThreads();

	printf("Total solutions found: %d\n", solutionsFound);
	return 0;
}