# Lab 07 - Backtracking Paralel cu Workers

## Exercițiu 1: Testare sample-par și sample-seq

Am testat programele `sample-par` și `sample-seq` pentru a înțelege paradigma replicated workers. Am analizat `Workers.h` și `Workers.c`:

- Coadă circulară thread-safe cu dimensiune `QUEUE_SIZE = 1000000`
- Sincronizare cu semafoare: `sem_full` (câte task-uri sunt în coadă), `sem_empty` (câte locuri libere)
- Mutex pentru protejarea accesului la coadă
- Funcții: `putTask()`, `getTask()`, `startWorkers()`, `joinWorkerThreads()`, `forceShutDownWorkers()`

## Exercițiu 2: Paralelizare getPath

Am paralelizat `getPath-seq.c` creând `getPath-par.c`:

- Structura `GetPathData` conține: `partialPath`, `step`, `destination`
- Fiecare extensie de cale devine un `Task` adăugat cu `putTask()`
- La găsirea a N căi, se apelează `forceShutDownWorkers()`
- Mutex pentru protejarea afișării rezultatelor

## Exercițiu 3: Rezultate getPath - Sanity Check și Timpi

```
Sanity Check: ./getPath-par 1 1 4
 0  5  8  3
 0  5  7  2  3
 0  4  3
Total paths found: 4
✓ Output-ul paralel este subset din output-ul secvențial

Timpi de execuție (N=1):
- Sequential: real 0m0.003s
- Parallel (P=4): real 0m0.005s
```

## Exercițiu 4: Paralelizare colorGraph

Am paralelizat `colorGraph-seq.c` creând `colorGraph-par.c`:

- Structura `ColorGraphData` conține: `colors[]`, `step`
- Pentru fiecare culoare validă (verificată cu `verifyColors()`), se creează un task nou
- La găsirea a N soluții, se apelează `forceShutDownWorkers()`

## Exercițiu 5: Rezultate colorGraph - Sanity Check și Timpi

```
Sanity Check: ./colorGraph-par 1 1 4
 2  0  1  0  1  0  2  2  1  0
Total solutions found: 1
✓ Output-ul paralel este subset din output-ul secvențial

Timpi de execuție (N=1):
- Sequential: real 0m0.002s
- Parallel (P=4): real 0m0.002s
```

## Exercițiu 6: Paralelizare queens (opțional)

Am paralelizat `queens-seq.c` creând `queens-par.c`:

- Structura `partialSolution` conține: `queens[]`, `position`, `step`
- Funcția `backtrackingTask()` verifică cu `canReject()`/`isSolution()`
- Generează task-uri pentru fiecare poziție posibilă pe rândul următor
- La găsirea a N soluții, se apelează `forceShutDownWorkers()`

## Exercițiu 7: Rezultate queens - Sanity Check și Timpi

```
Sanity Check: ./queens-par 8 0 4
Total solutions found: 8
✓ Soluțiile găsite paralel sunt valide

Timpi de execuție (N=8 dame):
- Sequential: real 0m0.003s
- Parallel (P=4): real 0m0.010s
```

## Exercițiu 8: Program subset verification

Am implementat `subset-check.c`:

- Primește două fișiere ca argumente
- Verifică dacă liniile primului fișier sunt un subset din liniile celui de-al doilea
- Returnează 0 (succes) dacă DA, 1 (eșec) dacă NU

## Exercițiu 9: Modificare script cu subset-check

Am creat `testCorrectnessSubset.sh` care folosește `subset-check` în loc de `diff`:

```bash
./subset-check "$outfile" out
if [ $? -ne 0 ]; then
    echo "FAIL: $outfile is not a subset of sequential output"
fi
```

Acest lucru permite verificarea corectă a programelor paralele care găsesc doar un subset din soluții.

## Exercițiu 10: Soluție recursivă Turnuri Hanoi

Am implementat `hanoi-seq.c`:

- Funcția recursivă `hanoi(n, from, to, aux)` mută n discuri
- Pasul de bază: n=0 nu face nimic
- Pasul recursiv: mută n-1 discuri pe aux, mută discul n pe destinație, mută n-1 discuri pe destinație
- Numără mutările totale: 2^N - 1

## Exercițiu 11: Paralelizare Turnuri Hanoi

Am implementat `hanoi-par.c`:

- Structura `HanoiData` conține: `n`, `from`, `to`, `aux`
- Pentru probleme mici (n≤3), rezolvă recursiv pentru a evita overhead-ul task-urilor
- Pentru probleme mari, creează task-uri pentru sub-probleme
- Monitorizează `moveCount` și oprește când atinge 2^N - 1 mutări

```
Rezultate (N=5 discuri):
- Sequential: Total moves: 31, real 0m0.002s
- Parallel (P=4): Total moves: 31, real 0m0.004s
```

Notă: Hanoi are dependențe secvențiale intrinseci, deci paralelizarea nu oferă speedup.
