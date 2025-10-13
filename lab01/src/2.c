#include <unistd.h>
#include <stdio.h>

int main(){
    long nprocs = sysconf(_SC_NPROCESSORS_ONLN);
    long nprocs_conf = sysconf(_SC_NPROCESSORS_CONF);

    if (nprocs < 1) {
        perror("sysconf");
        return 1;
    }

    printf("Available cores: %ld\n", nprocs);
    printf("Configured cores: %ld\n", nprocs_conf);
    return 0;
}
