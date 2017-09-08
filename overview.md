#Presentazione di SCARTCH

+ Overview
+ Obiettivi
+ Sintassi (Come sono realizzati gli script)
  + Elementi di Scripting
    + Blocchi
    + Cappelli
    + Blocchi speciali
    + Riferimenti
  + Tipi di dato
  + Opzioni 
+ Semantica
  + Concetto di attore, ambiente locale e ambiente globale
    L'organizzazione del programma in **attori** a cui si associano **script** consente di far familiarizzare l'utente con la programmazione a oggetti.
  + Concetto di Broadcast
+ Grammatica visuale
  + Blocco seguente
  + Motore di valutazione
    + Fondamentalmente, siamo come nel secondo esempio visto durante il corso: il valutatore ad oggetti in cui ogni oggetto racchiude la logica per la valutazione. Non mi è parso opportuno dispiegare un visitor perché **C# consente di raggruppare più classi in un solo file**. In questo modo, ottengo un unico file in cui raggruppo tutti gli script di uno specifico tipo che può essere processato con reflection (lo posso anche compilare in un dll a parte e sostituirlo senza ricompilaretutto il programma).
+ Architettura dell'ambiente di sviluppo
+ Interfaccia grafica
+ Demo
  + Ricorsione
+ Conclusioni
  + Possibilità di sviluppo ulteriore