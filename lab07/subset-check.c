#include <stdio.h>
#include <stdlib.h>
#include <string.h>

#define MAX_LINE_LENGTH 4096
#define MAX_LINES 100000

// Verifică dacă liniile din file1 sunt un subset din liniile din file2
// Returnează 0 dacă DA, 1 dacă NU

int main(int argc, char** argv) {
    if (argc < 3) {
        printf("Usage: %s file1 file2\n", argv[0]);
        printf("Checks if lines of file1 are a subset of lines in file2\n");
        return 2;
    }

    FILE* f1 = fopen(argv[1], "r");
    FILE* f2 = fopen(argv[2], "r");

    if (!f1) {
        printf("Cannot open file: %s\n", argv[1]);
        return 2;
    }
    if (!f2) {
        printf("Cannot open file: %s\n", argv[2]);
        fclose(f1);
        return 2;
    }

    // Citește toate liniile din file2 în memorie
    char** lines2 = malloc(sizeof(char*) * MAX_LINES);
    int numLines2 = 0;
    char buffer[MAX_LINE_LENGTH];

    while (fgets(buffer, MAX_LINE_LENGTH, f2) != NULL && numLines2 < MAX_LINES) {
        // Elimină newline
        int len = strlen(buffer);
        if (len > 0 && buffer[len - 1] == '\n') {
            buffer[len - 1] = '\0';
            len--;
        }
        lines2[numLines2] = malloc(len + 1);
        strcpy(lines2[numLines2], buffer);
        numLines2++;
    }
    fclose(f2);

    // Pentru fiecare linie din file1, verifică dacă există în file2
    int isSubset = 1;
    int lineNum = 0;

    while (fgets(buffer, MAX_LINE_LENGTH, f1) != NULL) {
        lineNum++;
        // Elimină newline
        int len = strlen(buffer);
        if (len > 0 && buffer[len - 1] == '\n') {
            buffer[len - 1] = '\0';
            len--;
        }

        // Caută linia în file2
        int found = 0;
        for (int i = 0; i < numLines2; i++) {
            if (strcmp(buffer, lines2[i]) == 0) {
                found = 1;
                break;
            }
        }

        if (!found) {
            isSubset = 0;
            break;
        }
    }
    fclose(f1);

    // Eliberează memoria
    for (int i = 0; i < numLines2; i++) {
        free(lines2[i]);
    }
    free(lines2);

    if (isSubset) {
        return 0;  // Succes - este subset
    } else {
        return 1;  // Eșec - nu este subset
    }
}
