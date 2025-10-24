# Lab 3 - Algoritmi Paraleli

## 1. Race condition
Can run `watch -n .5 bin/1 10000 1 2` or `watch -n .5 bin/1 1 1 2` to see for N=1.

## 2. Race condition comparison
```bash
$ bash testCorrectnessIntensive.sh bin/2 bin/1 10000 10 
The result of your parallel program is
======================================
40000 answer should be 40000
======================================
Running intensive correctness test with threads
Test 1/10
Test 2/10
Test 3/10
Test 4/10
Test 5/10
Test 6/10
Test 7/10
Test 8/10
Test 9/10
Test 10/10
Files out and out.10.2 differ
Files out and out.10.4 differ
Files out and out.10.8 differ
Files out and out.1.1 differ
Files out and out.1.2 differ
Files out and out.2.1 differ
Files out and out.2.2 differ
Files out and out.2.4 differ
Files out and out.2.8 differ
Files out and out.3.1 differ
Files out and out.3.2 differ
Files out and out.3.8 differ
Files out and out.4.4 differ
Files out and out.5.1 differ
Files out and out.5.2 differ
Files out and out.5.4 differ
Files out and out.5.8 differ
Files out and out.6.1 differ
Files out and out.6.2 differ
Files out and out.6.4 differ
Files out and out.6.8 differ
Files out and out.7.1 differ
Files out and out.7.2 differ
Files out and out.8.1 differ
Files out and out.8.2 differ
Files out and out.8.4 differ
Files out and out.8.8 differ
Files out and out.9.1 differ
Files out and out.9.8 differ
```

## 3. Fixing race condition
```bash
$ bash testCorrectnessIntensive.sh bin/2 bin/3 10000 10 
rm: cannot remove 'out*': No such file or directory
The result of your parallel program is
======================================
40000 answer should be 40000
======================================
Running intensive correctness test with threads
Test 1/10
Test 2/10
Test 3/10
Test 4/10
Test 5/10
Test 6/10
Test 7/10
Test 8/10
Test 9/10
Test 10/10
Output correct on intensive test
```
## 4. Barriers
```bash
$ bash testCorrectnessIntensive.sh bin/barrier-seq bin/4 1 10000 | tail 
Test 9992/10000
Test 9993/10000
Test 9994/10000
Test 9995/10000
Test 9996/10000
Test 9997/10000
Test 9998/10000
Test 9999/10000
Test 10000/10000
Output correct on intensive test

```

## 5. Semaphore Signal
```bash
$ bash testCorrectnessIntensive.sh bin/semaphoreSignal-seq bin/5 1 10000 | tail
Test 9992/10000
Test 9993/10000
Test 9994/10000
Test 9995/10000
Test 9996/10000
Test 9997/10000
Test 9998/10000
Test 9999/10000
Test 10000/10000
Output correct on intensive test
```

## 6. Deadlock 1
Avem deadlock deoarece al doilea thread care ajunge la primul `lock` asteapta intr-un loop `unlock`-ul care nu exista.

## 7. Deadlock 2
Avem deadlock deoarece dupa ce thread-urile dau `sleep`, fiecare va ajunge sa astepte pentru celalalt sa dea `unclock`. Thread-ul `0` va astepta dupa `mutexB` care este la thread-ul 1, iar thread-ul 1 va astepta dupa `mutexB`, care e la thread-ul 0.
Daca scoatem apelurile de `sleep` la prima vedere pare sa fi scapat de deadlock, dar ruland script-ul vedem ca apare la fiecare ~400 rulari.
Putem avea o versiune care pune `lock`-urile in ordinea corecta, independenta de thread_id, pastrand `sleep`-urile.

## 8. Deadlock 3
Exercitiul se rezolva prin setarea atributului `PTHREAD_MUTEX_RECURSUVE` pe mutex, lucru ce ne permite un dublu-lock.

## 9. Sum Vector Values
Timp pentru programul secvential si `N=1.000.000.000`:
```bash
catalin@debian:~/atm/Algoritmi-Paraleli/lab03$ time bin/sumVectorValues-seq 1000000000 $(nproc) 0
Sum: 500000000500000000 

real    0m8.419s
user    0m4.797s
sys     0m2.901s
```
Timp executie program paralel:
```bash
catalin@debian:~/atm/Algoritmi-Paraleli/lab03$ time bin/9 1000000000 $(nproc) 0
Sum: 500000000500000000 

real    0m5.908s
user    0m4.459s
sys     0m1.444s
```
Test cu script-ul:
```bash
(dev) [17ms][main][~/atm/Algoritmi-Paraleli/lab03]$ bash testCorrectnessIntensive.sh bin/sumVectorValues-seq bin/9 100000 10000 0 | tail
rm: cannot remove 'out*': No such file or directory
Test 9992/10000
Test 9993/10000
Test 9994/10000
Test 9995/10000
Test 9996/10000
Test 9997/10000
Test 9998/10000
Test 9999/10000
Test 10000/10000
Output correct on intensive test
```
## 10. Sum Vec Val Paralel
Comparatie timpi:
Algoritmul secvential:
```bash
catalin@debian:~/atm/Algoritmi-Paraleli/lab03$ time bin/sumVectorValues 1000000000 0 $(nproc)

real    0m5.702s
user    0m4.385s
sys     0m1.316s

```
Algoritmul paralelizat:
```bash
catalin@debian:~/atm/Algoritmi-Paraleli/lab03$ time bin/sumVectorValues-fix 1000000000 0 $(nproc)

real    0m4.133s
user    0m6.104s
sys     0m1.215s

```

## 11. PrepStrassen
Am creat cate un thread pentru fiecare M<sub>i</sub> si cate o functie worker.
