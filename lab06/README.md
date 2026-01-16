# Laborator 6

## One Producer, One Consumer, One Buffer

```bash
(dev) [2.516s][main][~/atm/Algoritmi-Paraleli/lab06]$ bash testCorrectnessIntensive.sh bin/oneProducerOneConsumer-fakeForScriptSeq bin/oneProducerOneConsumerOneBuffer 1000 100 | tail -n 10
rm: cannot remove 'out*': No such file or directory
Test 92/100
Test 93/100
Test 94/100
Test 95/100
Test 96/100
Test 97/100
Test 98/100
Test 99/100
Test 100/100
Output correct on intensive test
```

## One Producer, One Consumer, Five Buffer

```bash
(dev) [2.146s][main][~/atm/Algoritmi-Paraleli/lab06]$ bash testCorrectnessIntensive.sh bin/oneProducerOneConsumer-fakeForScriptSeq bin/oneProducerOneConsumerFiveBuffer 1000 100 | tail -n 10
rm: cannot remove 'out*': No such file or directory
Test 92/100
Test 93/100
Test 94/100
Test 95/100
Test 96/100
Test 97/100
Test 98/100
Test 99/100
Test 100/100
Output correct on intensive test
```

## Multiple Producers, Multiple Consumers, Five Buffer

```bash
(dev) [3.235s][main][~/atm/Algoritmi-Paraleli/lab06]$ bash testCorrectnessIntensive.sh bin/multipleProducersMultipleConsumers-fakeForScriptSeq bin/multipleProducersMultipleConsumers 1000 1000 4 | tail -n 10
rm: cannot remove 'out*': No such file or directory
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

## Readers Writers

```bash
(dev) [4.938s][main][~/atm/Algoritmi-Paraleli/lab06]$ bash testCorrectnessIntensive.sh bin/readersWriters-fakeForScriptSeq bin/readersWriters 10000 100 | tail -n 10
rm: cannot remove 'out*': No such file or directory
Test 92/100
Test 93/100
Test 94/100
Test 95/100
Test 96/100
Test 97/100
Test 98/100
Test 99/100
Test 100/100
Output correct on intensive test
```

## Dining Philosophers

```bash
(dev) [1.910s][main][~/atm/Algoritmi-Paraleli/lab06]$ bash testCorrectnessIntensive.sh bin/philosophers-fakeForScriptSeq bin/philosophers 1000 1000 8 | tail -
n 10
rm: cannot remove 'out*': No such file or directory
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
