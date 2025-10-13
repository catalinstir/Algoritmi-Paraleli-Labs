#include <stdlib.h>
#include <stdio.h>
#include <pthread.h>
#include <time.h>

double **a, **b, **c;
int N, M, P;

void parseArgs(int argc, char* argv[]){
    if(argc != 4){
        printf("Wrong arguments. Usage:\n"
                "----------------------\n"
                "bin/7 N M P\n"
                "N - Columns of matrix\n"
                "M - Lines of matrix\n"
                "P - Number of threads\n");
        exit(EXIT_FAILURE);
    }
    N = atoi(argv[1]);
    M = atoi(argv[2]);
    P = atoi(argv[3]);
}

double randfrom(double min, double max) 
{
    double range = (max - min); 
    double div = RAND_MAX / range;
    return min + (rand() / div);
}

void initMatrices(){
    a = (double**)malloc(sizeof(double*) * M);
    b = (double**)malloc(sizeof(double*) * M);
    c = (double**)malloc(sizeof(double*) * M);
    if(a == NULL || b == NULL || c == NULL){
        perror("Error allocating memory");
        exit(EXIT_FAILURE);
    }

    srand(time(NULL));
    for(int i = 0; i < M; i++){
        a[i] = (double*)malloc(sizeof(double) * N);
        b[i] = (double*)malloc(sizeof(double) * N);
        c[i] = (double*)calloc(N, sizeof(double));
        if(a[i] == NULL || b[i] == NULL || c[i] == NULL){
            perror("Error allocating memory");
            exit(EXIT_FAILURE);
        }
        for(int j = 0; j < N; j++){
            a[i][j] = randfrom(-10.0L, 10.0L);
            b[i][j] = randfrom(-10.0L, 10.0L);
        }
    }
}

void* addMatrices(void* arg){
    int tid = *(int*)arg;
    int chunk = M / P;
    int remainder = M % P;
    int start, end;
    
    if (tid < remainder) {
        start = tid * (chunk + 1);
        end = start + (chunk + 1);
    } else {
        start = tid * chunk + remainder;
        end = start + chunk;
    }
    
    for(int i = start; i < end; i++){
        for(int j = 0; j < N; j++){
            c[i][j] = a[i][j] + b[i][j];
        }
    }
    return NULL;
}

void printMatrix(double **mat){
    for(int i = 0; i < M; i++){
        for(int j = 0; j < N; j++){
            if(j == 0)
                printf("[%+06.2lf ", mat[i][j]);
            else if(j == N - 1)
                printf("%+06.2lf]\n", mat[i][j]);
            else
                printf("%+06.2lf ", mat[i][j]);
        }
    }
    // puts("");
}

int main(int argc, char *argv[]) {
    parseArgs(argc, argv);

    initMatrices();
    printf("Matrix A:\n");
    printMatrix(a);
    printf("Matrix B:\n");
    printMatrix(b);

    pthread_t *pid;
    int *thread_id;
    pid = (pthread_t*)malloc(sizeof(pthread_t) * P);
    thread_id = (int*)malloc(sizeof(int) * P);
    if(pid == NULL || thread_id == NULL){
        perror("Error allocating memory");
        exit(EXIT_FAILURE);
    }

    for(int i = 0; i < P; i++){
        thread_id[i] = i;
        pthread_create(&pid[i], NULL, addMatrices, &thread_id[i]);
    }

    for(int i = 0; i < P; i++){
        pthread_join(pid[i], NULL);
    }

    printf("Matrices sum:\n");
    printMatrix(c);

    return 0;
}
