#include <pthread.h>
#include <stddef.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>

char* haystack;
char* needle;
long haystack_len;
long needle_len;
short found;
int P;

void parse_args(int argc, char* argv[]){
    if(argc != 4){
        printf("Invalid arguments. Usage:\n"
               "-------------------------\n"
               "bin/8 INPUT_FILE SUBSTRING THREAD_COUNT\n"
               "(run the src/generate_big_file.sh script beforehand for a big search space\n");
        exit(EXIT_FAILURE);
    }
    P = atoi(argv[3]);
}

void init_strings(const char* filename, const char* substring){
    FILE *fin = fopen(filename, "r");
    if(fin == NULL){
        perror("Error opening file");
        exit(EXIT_FAILURE);
    }

    fseek(fin, 0, SEEK_END);
    size_t file_len = ftell(fin);
    fseek(fin, 0, SEEK_SET);

    haystack = (char*)malloc(sizeof(char) * (file_len + 1));
    if(haystack == NULL){
        perror("Error allocating memory to file contents string");
        exit(EXIT_FAILURE);
    }
    size_t read_len = fread(haystack, 1, file_len, fin);
    if(read_len != file_len){
        perror("Error reading file");
        exit(EXIT_FAILURE);
    }
    haystack[read_len] = '\0';
    haystack_len = strlen(haystack);

    needle = strdup(substring);
    needle_len = strlen(needle);

    fclose(fin);
}

void free_strings(){
    if(!found){
        printf("No substring found in file");
    }
    free(haystack);
    free(needle);
}

int fast_strncmp(const char* s1, const char* s2, size_t n) {
    for(size_t i = 0; i < n; i++) {
        if(s1[i] != s2[i]) {
            return 1;
        }
    }
    return 0;
}

void* find_substr(void *arg){
    int tid = *(int*)arg;

    long chunk = haystack_len / P;
    long start = tid * chunk;
    long len = (tid == P - 1) 
                ? haystack_len - start - (needle_len - 1)
                : chunk;
    char first_char = needle[0];
    for(long i = start; i < start + len; i++){
        if(haystack[i] == first_char && fast_strncmp(haystack + i, needle, needle_len) == 0){
            printf("Substring found by thread [%i] at index:\t%li\n", tid, i);
            found = 1;
        }
    }

    return NULL;
}

int main(int argc, char* argv[]) {
    parse_args(argc, argv);
    init_strings(argv[1], argv[2]);

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
        pthread_create(&pid[i], NULL, find_substr, &thread_id[i]);
    }

    for(int i = 0; i < P; i++){
        pthread_join(pid[i], NULL);
    }

    free_strings();

    return 0;
}
