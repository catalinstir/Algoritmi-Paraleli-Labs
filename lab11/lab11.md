## Exerciții

1. Analizați și rulați programul din **ex01** care exemplifică producerea și consumarea elementelor folosind `IAsyncEnumerable<T>`.
   - Dupa porțiunea de cod care realizează procesarea elementelor din rezultatul asincron are loc apelul unei funcții sincrone `DoOtherWork()`.
   - Modificați codul astfel încât procesarea valorilor din rezultatul asincron sa se intercaleze cu execuția funcției sincrone.
   - Citiți mai multe despre `yield`: [1](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/statements/yield) și [2](https://www.c-sharpcorner.com/UploadFile/5ef30d/understanding-yield-return-in-C-Sharp/).
   
2. În **ex02**, implementați un client care trimite cereri către un REST API cu paginare și întoarce rezultatele folosind IAsyncEnumerable<T>. În arhiva **backend.zip** există un server HTTP ce expune pe portul `5000` endpoint-ul `/api/products?offset={OFFSET}&limit={LIMIT}` (e.g. `http://localhost:5000/api/products?offset=3&limit=4`) care la primirea unui request de tip GET întoarce o listă de `LIMIT` produse cu elementele ce încep de la indexul cu valoarea `OFFSET`.
   - Implementați apoi procesarea (printarea șirului `Processing product [ID]: Name=[NAME], Category=[CATEGORY], Description=[DESCRIPTION], Price=[PRICE]` per produs) asincronă a produselor primite.

3. În **ex03**, pornind de la clientul implementat la exercițiul anterior, dezvoltați un program ce realizează căutarea a `K` produse cu prețul `P`.
   - În momentul în care sunt găsite primele `K` produse, este necesară întreruperea căutării.
   - Hint: Puteți să vă folosiți de `CancellationTokenSource`.

4. Analizați și rulați programul din **ex04** ce exemplifică utilizarea structurii `Parallel.ForEach()`.
   
5. În **ex05** implementați, folosind `Parallel.ForEach()` sau `Parallel.For()`, un program ce verifică primalitatea elementelor unui vector.
   - Numerele prime vor fi scrie într-un fișier, `primes_out.txt`.
   - Numărul de numere prime va fi scris într-un fișier, `primes_out_count.txt`.
   - Hint: Pentru contorizarea protejată a numărului de numere prime puteți să vă folosiți de `Interlocked.Add()`.

6. În **ex06** implementați un program ce afișează primul număr prim găsit dintr-o secvență de mai multe numere implementând o întrerupere din **exteriorul buclei** `Parallel.ForEach()`. Astfel, la determinarea primului număr ce respectă condiția de primalitate, programul își va întrerupe căutarea.
   - Hint: Puteți să vă folosiți de `ParallelOptions` și `CancellationTokenSource`.

7. În **ex07**, implementați un program ce afișează primul număr prim găsit dintr-o secvență de mai multe numere implementând o întrerupere din **interiorul buclei** `Parallel.ForEach()`. Astfel, la determinarea primului număr ce respectă condiția de primalitate, programul își va întrerupe căutarea.
   - Hint: Puteți să vă folosiți de `ParallelLoopState`.

8. Analizați și rulați programul din **ex08** ce oferă un exemplu de agregare a rezultatelor din cadrul `Parallel.ForEach()`.

9. În **ex05** implementați, folosind agregarea de tip `Parallel.ForEach()`, un program ce verifică primalitatea elementelor unui vector.
   - Numărul de numere prime va fi scris într-un fișier, `primes_out_count.txt`.

10. Analiza și rulați programul din **ex10** ce exemplifică utilizarea structurii `Parallel.Invoke()`.

11. În **ex11** implementați, folosind `Parallel.Invoke()`, un program ce verifică primalitatea elementelor unui vector folosind un număr parametrizabil de secțiuni.
   - Numerele prime vor fi scrie într-un fișier, `primes_out.txt`.
   - Numărul de numere prime va fi scris într-un fișier, `primes_out_count.txt`.