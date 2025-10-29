# Lab 4 - Algoritmi Paraleli

====Exercitiul 2====
Sanity:
```bash
(dev) [22ms][main][~/atm/Algoritmi-Paraleli/lab04]$ bin/multiplyMatrices-out 5 2 $(nproc)
1       2       3       4       5
0       1       2       3       4
0       0       1       2       3
0       0       0       1       2
0       0       0       0       1
```

Stress test:
Rulat cu N=100 P=1,2,4 si 8 NReps = 1000
Ex 1 thread:
```bash
(dev) [13ms][main][~/atm/Algoritmi-Paraleli/lab04]$ time bash testCorrectnessIntensive.sh bin/multiplyMatrices-seq bin/multiplyMatrices-out 100 1000 1 | tail -n 2
rm: cannot remove 'out*': No such file or directory
Test 1000/1000
Output correct on intensive test
bash testCorrectnessIntensive.sh bin/multiplyMatrices-seq  100 1000 1  4.94s user 0.50s system 99% cpu 5.465 total
tail -n 2  0.01s user 0.00s system 0% cpu 5.465 total
```
| P | Time  |
|---|--------|
| 1 | 5.464s |
| 2 | 3.701s |
| 4 | 3.017s |
| 8 | 2.860s |

====Exercitiul 4====
Sanity:
```bash
(dev) [5.865s][main][~/atm/Algoritmi-Paraleli/lab04]$ bin/multiplyMatrices-mid 5 2 $(nproc)
1       2       3       4       5
0       1       2       3       4
0       0       1       2       3
0       0       0       1       2
0       0       0       0       1
```

Stress test:
Ruland cu N=100 P=1,2,4 si 8, NReps=1000:
| P | Time  |
|---|--------|
| 1 | 5.650s |
| 2 | 3.681s |
| 4 | 2.789s |
| 8 | 2.805s |

====Exercitiul 6====
Sanity:
```bash
(dev) [70ms][2][main][~/atm/Algoritmi-Paraleli/lab04]$ bin/multiplyMatrices-in 5 2 $(nproc)
1       2       3       4       5
0       1       2       3       4
0       0       1       2       3
0       0       0       1       2
0       0       0       0       1
```

Stress test:
Rulam cu N=100 P=1, 2, 4 si 8 NReps=1000:
| P | Time  |
|---|--------|
| 1 | 4.277s |
| 2 | 4.955s |
| 4 | 7.659s |
| 8 | 23.92s |

====Exercitiul 8====
Sanity:
```bash

```
