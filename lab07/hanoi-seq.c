#include <stdio.h>
#include <stdlib.h>

int N;
int printLevel;
int moveCount = 0;

void getArgs(int argc, char** argv) {
    if (argc < 3) {
        printf("Not enough parameters: ./program N printLevel\n");
        printf("printLevel: 0=no, 1=some, 2=verbose\n");
        exit(1);
    }
    N = atoi(argv[1]);
    printLevel = atoi(argv[2]);
}

void printMove(int disk, char from, char to) {
    if (printLevel > 0) {
        printf("Move disk %d from %c to %c\n", disk, from, to);
    }
}

// Funcția recursivă pentru Turnurile din Hanoi
// n = numărul de discuri
// from = tija sursă
// to = tija destinație
// aux = tija auxiliară
void hanoi(int n, char from, char to, char aux) {
    if (n == 0) {
        return;
    }

    // Mută n-1 discuri de pe from pe aux, folosind to ca auxiliar
    hanoi(n - 1, from, aux, to);

    // Mută discul n de pe from pe to
    printMove(n, from, to);
    moveCount++;

    // Mută n-1 discuri de pe aux pe to, folosind from ca auxiliar
    hanoi(n - 1, aux, to, from);
}

int main(int argc, char** argv) {
    getArgs(argc, argv);

    printf("Solving Tower of Hanoi with %d disks\n", N);

    // Mută N discuri de pe tija A pe tija C, folosind B ca auxiliar
    hanoi(N, 'A', 'C', 'B');

    printf("Total moves: %d\n", moveCount);

    return 0;
}
