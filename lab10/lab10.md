## Exerciții

1. Analizați și rulați programul din **ex01**.
    - Notați output-ul din consolă al programului și explicați-l.
    - De ce credeți că sunt afișate TID-uri diferite?
    - Citiți mai multe în articolul despre [SynchronizationContext](https://hamidmosalla.com/2018/06/24/what-is-synchronizationcontext/).
    - Decomentați linia 32 `await task_1`. Descrieți ce se întâmplă față de versiunea cu linia comentată.

2. În proiectul **ex02** implementați, folosind `async` și `await`, un client (`Requester`) ce trimite cereri către **backend**-ul de la laboratorul **09**, exercițiul 5, folosind exponential backoff.
    - În timp ce `Requester` pune într-o listă url-urile primite, imaginile referențiate de către acestea vor fi descărcate folosind un alt obiect, `Downloader`.
    - Spre deosebire de laboratorul **09**, în implementarea lui `Requester` va trebui să utilizați `HttpClient`. În proiect regăsiți exemplu de trimitere cerere și deserializare răspuns.
    - Hint: Pentru introducerea unei perioade de așteptare, puteți sa va folosiți de `Task.Delay()`.

3. Analizați și rulați programul din **ex03**.
    - Ce se întamplă daca decomentați linia 28, `await Task.Delay((i - 1) * 100);`? Cum se modifică afișarea? Explicați comportamentul.

4. Analizați și rulați programul din **ex04** ce exemplifică așteptarea rulării unui set de task-uri.
    - Modificați codul astfel încât raportarea progresului să se diferențieze pentru fiecare task în parte.
    - Modificați codul astfel încât să salvați și printați în `Main()` rezultatul fiecărui task fără a elimina `Task.WhenAll()`.

5. Analizați și rulați programul din **ex05** ce exemplifică rularea unui set de task-uri și procesarea primului rezultat întors folosind `Task.WhenAny()`.
    - Decomentați linia 17 `await Task.Delay(4000);`. Ce observați?
    - Modificați codul în așa fel încât să preveniți risipa de resurse computaționale.

6. Pornind de la codul din **ex06**, implementați un program care tratează rezultatul task-urilor pe măsură ce acestea se finalizează.
    - Atenție, nu aveți voie să modificați ordinea task-urilor din array-ul `Task<string>[] tasks = new[] { task_1, task_2, task_3 };`.

7. Analizați și rulați programul din **ex07** ce exemplifică întreruperea după o perioadă de timp.
    - Ambele task-uri folosesc același `CancellationToken`. Modificați codul astfel încât în momentul în care unul dintre task-uri finalizează primul, rezultatul său va fi salvat și afișat în `Main()`, iar celălalt își va întrerupe activitatea.

8. La **ex08** implementați un program care interoghează cel puțin două API-uri ce oferă informații legate de bursă (de exemplu: interogarea prețului acțiunii unei companii listate la bursă).
    - Pentru fiecare API va fi nevoie să implementați un client (de exemplu: ClientAPI_1, Client_API_2, ..., ș.a.m.d.) ce realizează requestul conform cu documentația oferită pe site-ul fiecărui furnizor.
    - În paralel, sub forma unui `Task`, fiecare client va trimite o interogare către API-ul proprietar. În momentul în care una dintre cereri se finalizează cu un răspuns de succes, răspunsul va fi propagat și afișat în `Main()`, iar celelalte task-uri vor fi întrerupte.
    - Exemple de API-uri ce oferă acces gratuit pentru un număr limitat de request-uri: [alphavantage](https://www.alphavantage.co/), [devapi](https://devapi.ai/), [eohd](https://eodhd.com/), [marketstack](https://marketstack.com/).