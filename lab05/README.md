# Lab 5 - Algoritmi paraleli

## 2. Odd-even Transposition Sort Sanity check and Stress test
Sanity check:
```bash
(dev) [19ms][main][~/atm/Algoritmi-Paraleli/lab05]$ bin/oets-par 50 2 $(nproc)
  0  0  1  1  2  4  4  5  7  8  9  9 10 12 12 14 15 16 16 17
 17 19 21 21 21 24 24 26 27 29 30 31 33 33 34 35 40 40 41 41
 42 43 43 43 46 47 47 47 48 48
  0  0  1  1  2  4  4  5  7  8  9  9 10 12 12 14 15 16 16 17
 17 19 21 21 21 24 24 26 27 29 30 31 33 33 34 35 40 40 41 41
 42 43 43 43 46 47 47 47 48 48
Sorted correctly
```

Scalability test:
```bash
(dev) [17.132s][130][main][~/atm/Algoritmi-Paraleli/lab05]$ time bin/oets-par 100000 0 1 
bin/oets-par 100000 0 1  28.55s user 0.02s system 99% cpu 28.574 total
(dev) [28.582s][main][~/atm/Algoritmi-Paraleli/lab05]$ time bin/oets-par 100000 0 2
bin/oets-par 100000 0 2  33.80s user 0.19s system 196% cpu 17.262 total
(dev) [17.271s][main][~/atm/Algoritmi-Paraleli/lab05]$ time bin/oets-par 100000 0 4
bin/oets-par 100000 0 4  45.38s user 0.52s system 339% cpu 13.512 total
(dev) [13.523s][main][~/atm/Algoritmi-Paraleli/lab05]$ time bin/oets-par 100000 0 8
bin/oets-par 100000 0 8  63.69s user 1.59s system 604% cpu 10.802 total
```
| P | Time  |
|---|--------|
| 1 | 28.574s |
| 2 | 17.262s |
| 4 | 13.512s |
| 8 | 10.802s |

Stress test:
```bash
(dev) [26.747s][main][~/atm/Algoritmi-Paraleli/lab05]$ bash testCorrectnessIntensive.sh bin/bubbleSort-seq bin/oets-par 1000 1000 | tail -n 10
Test 992/1000
Test 993/1000
Test 994/1000
Test 995/1000
Test 996/1000
Test 997/1000
Test 998/1000
Test 999/1000
Test 1000/1000
Output correct on intensive test
```
## 4. Merge Sort parallel 
Sanity test:
```bash
(dev) [19ms][main][~/atm/Algoritmi-Paraleli/lab05]$ bin/mergeSort-par 128 2 $(nproc)
   0   0   1   6   7   7   7  11  11  11  12  13  14  15  15  16  16  17  19  20
  20  20  20  21  21  23  24  24  25  27  27  28  29  30  31  31  31  31  32  32
  32  34  34  34  34  36  37  39  40  40  41  41  41  41  46  48  49  50  50  51
  52  55  58  58  60  62  62  65  65  65  66  66  67  68  69  69  70  72  73  73
  77  78  79  79  79  80  82  83  84  84  84  87  87  89  90  90  91  91  93  94
  96  99  99 100 100 103 103 104 104 104 105 105 107 107 109 110 113 113 113 118
 118 120 120 123 123 123 124 127
   0   0   1   6   7   7   7  11  11  11  12  13  14  15  15  16  16  17  19  20
  20  20  20  21  21  23  24  24  25  27  27  28  29  30  31  31  31  31  32  32
  32  34  34  34  34  36  37  39  40  40  41  41  41  41  46  48  49  50  50  51
  52  55  58  58  60  62  62  65  65  65  66  66  67  68  69  69  70  72  73  73
  77  78  79  79  79  80  82  83  84  84  84  87  87  89  90  90  91  91  93  94
  96  99  99 100 100 103 103 104 104 104 105 105 107 107 109 110 113 113 113 118
 118 120 120 123 123 123 124 127
Sorted correctly
```

Scalability test:
```bash
(dev) [13.940s][main][~/atm/Algoritmi-Paraleli/lab05]$ time bin/mergeSort-par $((2 ** 24)) 0 1 
bin/mergeSort-par $((2 ** 24)) 0 1  6.94s user 0.04s system 99% cpu 6.977 total
(dev) [6.094s][main][~/atm/Algoritmi-Paraleli/lab05]$ time bin/mergeSort-par $((2 ** 24)) 0 2
bin/mergeSort-par $((2 ** 24)) 0 2  8.98s user 0.02s system 132% cpu 6.794 total
(dev) [5.175s][main][~/atm/Algoritmi-Paraleli/lab05]$ time bin/mergeSort-par $((2 ** 24)) 0 4
bin/mergeSort-par $((2 ** 24)) 0 4  10.14s user 0.05s system 165% cpu 6.156 total
(dev) [5.438s][main][~/atm/Algoritmi-Paraleli/lab05]$ time bin/mergeSort-par $((2 ** 24)) 0 8
bin/mergeSort-par $((2 ** 24)) 0 8  11.30s user 0.05s system 211% cpu 5.354 total
```
| P | Time  |
|---|--------|
| 1 | 6.977s |
| 2 | 6.794s |
| 4 | 6.156s |
| 8 | 5.354s |

Stress test:
```bash
(dev) [277ms][main][~/atm/Algoritmi-Paraleli/lab05]$ time bash testCorrectnessIntensive.sh bin/mergeSort-seq bin/mergeSort-par $((2 ** 12)) 1000 | tail -n 10
Test 992/1000
Test 993/1000
Test 994/1000
Test 995/1000
Test 996/1000
Test 997/1000
Test 998/1000
Test 999/1000
Test 1000/1000
Output correct on intensive test
bash testCorrectnessIntensive.sh bin/mergeSort-seq bin/mergeSort-par  1000  15.84s user 6.54s system 114% cpu 19.468 total
tail -n 10  0.02s user 0.00s system 0% cpu 19.468 total
```

## 6. Sheer sort
Sanity check:
```bash
(dev) [15ms][main][~/atm/Algoritmi-Paraleli/lab05]$ bin/shearSort-par 100 2 $(nproc)
  0  0  0  1  1  4  4  4  5  6  7  8  9 12 14 16 16 17 19 20
 20 20 21 21 21 21 22 22 23 23 24 25 27 27 30 33 33 34 34 35
 35 35 36 39 40 40 41 41 43 43 43 47 47 47 48 50 51 51 51 52
 54 55 56 58 59 60 60 62 63 65 66 67 68 69 69 69 69 70 72 73
 74 76 78 79 79 80 81 82 83 84 85 88 91 92 95 95 96 97 98 99

  0  0  0  1  1  4  4  4  5  6  7  8  9 12 14 16 16 17 19 20
 20 20 21 21 21 21 22 22 23 23 24 25 27 27 30 33 33 34 34 35
 35 35 36 39 40 40 41 41 43 43 43 47 47 47 48 50 51 51 51 52
 54 55 56 58 59 60 60 62 63 65 66 67 68 69 69 69 69 70 72 73
 74 76 78 79 79 80 81 82 83 84 85 88 91 92 95 95 96 97 98 99

Sorted correctly
```

Scalability test:
```bash
(dev) [10.692s][main][~/atm/Algoritmi-Paraleli/lab05]$ time bin/shearSort-par 250000 0 1
bin/shearSort-par 250000 0 1  10.79s user 0.00s system 99% cpu 10.795 total
(dev) [10.802s][main][~/atm/Algoritmi-Paraleli/lab05]$ time bin/shearSort-par 250000 0 2
bin/shearSort-par 250000 0 2  11.50s user 0.00s system 198% cpu 5.788 total
(dev) [5.796s][main][~/atm/Algoritmi-Paraleli/lab05]$ time bin/shearSort-par 250000 0 4
bin/shearSort-par 250000 0 4  14.69s user 0.01s system 383% cpu 3.832 total
(dev) [3.985s][main][~/atm/Algoritmi-Paraleli/lab05]$ time bin/shearSort-par 250000 0 8
bin/shearSort-par 250000 0 8  27.46s user 0.05s system 750% cpu 3.665 total
```
| P | Time  |
|---|--------|
| 1 | 10.79s |
| 2 | 5.788s |
| 4 | 3.832s |
| 8 | 3.665s |

Stress test:
```bash
(dev) [3.676s][main][~/atm/Algoritmi-Paraleli/lab05]$ time bash testCorrectnessIntensive.sh bin/shearSort-seq bin/shearSort-par 1000 1000 | tail -n 10
Test 992/1000
Test 993/1000
Test 994/1000
Test 995/1000
Test 996/1000
Test 997/1000
Test 998/1000
Test 999/1000
Test 1000/1000
Output correct on intensive test
bash testCorrectnessIntensive.sh bin/shearSort-seq bin/shearSort-par 1000 100  14.03s user 3.10s system 172% cpu 9.917 total
tail -n 10  0.01s user 0.00s system 0% cpu 9.916 total
```
