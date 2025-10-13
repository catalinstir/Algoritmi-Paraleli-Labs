#include <stdlib.h>
#include <stdio.h>
#include <time.h>

double **a, **b, **c;
int N, M;

void parseArgs(int argc, char* argv[]){
    if(argc != 3){
        printf("Wrong arguments. Usage:\n"
                "----------------------\n"
                "bin/7 N M\n"
                "N - Columns of matrix\n"
                "M - Lines of matrix\n");
        exit(EXIT_FAILURE);
    }
    N = atoi(argv[1]);
    M = atoi(argv[2]);
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

void addMatrices(){
    for(int i = 0; i < M; i++){
        for(int j = 0; j < N; j++){
            c[i][j] = a[i][j] + b[i][j];
        }
    }
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
    addMatrices();
    printf("Matrices sum:\n");
    printMatrix(c);

    return 0;
}
