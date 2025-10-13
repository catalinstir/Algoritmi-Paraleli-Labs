#include <stddef.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

char* haystack;
char* needle;

void parse_args(int argc){
    if(argc != 3){
        printf("Invalid arguments. Usage:\n"
               "-------------------------\n"
               "bin/8 INPUT_FILE SUBSTRING\n");
        exit(EXIT_FAILURE);
    }
}

void init_strings(const char* filename, const char* substring){
    FILE *fin = fopen(filename, "r");
    if(fin == NULL){
        perror("Error opening file");
        exit(EXIT_FAILURE);
    }

    fseek(fin, 0, SEEK_END);
    long file_len = ftell(fin);
    fseek(fin, 0, SEEK_SET);

    haystack = (char*)malloc(sizeof(char) * file_len);
    if(haystack == NULL){
        perror("Error allocating memory to file contents string");
        exit(EXIT_FAILURE);
    }
    int read_len = fread(haystack, 1, file_len, fin);
    if(read_len == -1){
        perror("Error reading file");
        exit(EXIT_FAILURE);
    }
    haystack[read_len] = '\0';

    needle = strdup(substring);

    fclose(fin);
}

void find_substr(){
    char* ret = strstr(haystack, needle);
    long index = -1;
    while(ret){
        index = ret - haystack;
        printf("Substring found at index:\t%li\n", index);
        ret = strstr(haystack + index + 1, needle);
    }
    if(index == -1){
        printf("Substring not found in file\n");
    }

    free(haystack);
    free(needle);
}

int main(int argc, char* argv[]) {
    parse_args(argc);
    init_strings(argv[1], argv[2]);
    find_substr();

    return 0;
}
