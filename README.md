# PragueParking2
Simple OOP program for school

Bakgrund
Parkeringshuset i Prag behöver uppdateras och förbättras. Den är lite knölig att underhålla och 
stränghanteringen av data i arrayen har visat sig vara inflexibel när kunden säger att man i 
framtiden kommer att ta emot olika sorters fordon och kanske även olika stora parkeringsrutor.
Grundförutsättningarna är fortfarande desamma. Samma funktionalitet, endast parkering av MC 
och bilar i nuläget. Kunden har dock pratat om bussar och cyklar. Systemet skall registrera när 
fordonen kommer in och tala om hur länge de stått parkerade när de lämnas ut.

Kundens nya krav på systemet

• Data skall sparas så att det inte försvinner om programmet stängs av.

• Det skall gå att hantera P-platser av olika storlek så att även stora fordon (bussar) och små 
fordon (cyklar) kan hanteras.

• En karta över P-huset skall visas, så att man enkelt kan se beläggningen. Det skall gå att stå 
en bit ifrån skärmen och se översiktligt hur många platser som är lediga

• Det skall finnas en prislista i form av en textfil. Filen läses in vid programstart och kan vid behov läsas in på nytt via ett menyval. Filen skall gå att ändra även för en med låg ITkunskap, så formatet behöver vara lätt att förstå. (TIPS: om allt efter ”#” på en rad är 
kommentarer, kan man ha dokumentation inne i själva filen)

• Det skall finnas en textfil med konfigurationsdata för systemet. Filformatet är fritt, men XML 
eller JSON kan vara lämpliga att använda.

• I konfigurationsfilen skall man kunna konfigurera

o Antal P-platser i garaget

o Fordonstyper (Bil och MC, men det kan komma fler)

o Antal fordon per P-plats för respektive fordonstyp

• Prisstrukturen är till en början

o Bil: 20 CZK per påbörjad timme

o MC: 10 CZK per påbörjad timme

o De första tio minuterna är gratis

• Filerna för prislista och konfiguration kan gärna kombineras till en fil

• Trots att det är en konsolapplikation skall den göras så snygg som möjligt. Användaren skall 
uppleva att bilden är stabil, tydlig och lätt att förstå

Tekniska krav, nya i version 2.0

Alla fordon skall nu hanteras som objekt. Det skall finnas minst fyra klasser:

1. Fordon

2. Bil, som ärver från Fordon
3. MC, som ärver från Fordon
4. PPlats

Om ni föredrar engelska namn på saker och ting:

1. Vehicle

2. Car, som ärver från Vehicle

3. MC, som ärver från Vehicle

4. ParkingSpot

Det kan mycket väl visa sig att det behövs fler klasser. Det kan också behövas en enumerator eller två.
P-huset kan med fördel hanteras som en lista av P-platser. Fortfarande gäller att det finns 100 Prutor som standardvärde, men det kan ändras med konfigurationsfilen.
