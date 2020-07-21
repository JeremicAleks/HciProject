# HciProject
Records of natural monuments

## VOĐENJE EVIDENCIJE O MAPI SVETSKIH PRIRODNIH SPOMENIKA
Aplikacija za vođenje evidencije o geografskoj distribuciji svetskih prirodnih spomenika, odnosno mesta izuzetne prirodne lepote i značaja. Potrebno je realizovati distribuciju preko mape sveta na koju se prevlače i spuštaju simboli različitih spomenika. Mapa je fiksna slika koja se ne skroluje i ne zumira. Svi podaci se čuvaju u fajlu i učitavaju prilikom startovanja aplikacije.
Svaki spomenik je opisan preko: svoje jedinstvene ljudski-čitljive oznake koju unosi korisnik, imena, opisa, tipa, klime u kojoj se nalazi, ikonice, da li je ekološki ugrožen, da li je stanište ugroženih vrsta, da li je u naseljenom regionu ili ne, turističkog statusa, godišnjeg prihoda od turizma, i datuma otkrivanja. Ikonica je sličica koja se učitava i koja se koristi da se spomenik označi na mapi i može da se i ne postavi i, ako se ne postavi, onda se podrazumevano uzima ikonica tipa. Klima je jedna od sledećih vrednosti: polarna, kontinentalna, umereno-kontinentalna, pustinjska, suptropska, i tropska. Turistički status je jedna od sledećih vrednosti: eksploatisan, dostupan, nedostupan, a prihod je u dolarima. Spomenici takođe mogu biti i "tagovani" sa nijednom, jednom, ili više etiketa. Etikete specificira korisnik i one su opisane svojom jedinstvenom ljudski-čitljivom oznakom koju unosi korisnik, bojom i opisom.
Tip spomenika je opisan preko svoje jedinstvene ljudski-čitljive oznake koju unosi korisnik, imena, ikonice, i opisa. Ikonica je sličica koja se učitava i koja se koristi da se taj tip spomenika označi na mapi.
## Osnovni zadaci aplikacije. 
##### Aplikacija treba da obezbedi:
1. Ažuriranje osnovnih podataka o spomenicima, tipovima i etiketama.
2. Prikaz mape i direktnu manipulaciju simbola na mapi na pregledan način.
3. Nameštanje tipa spomenicima kao i njihovo "tagovanje" etiketama.
4. Omogućiti tabelarni pregled spomenika uz filtriranje i pretragu.
5. Sistem pomoći.

**Zadatak je realizovan direktnom manipulacijom i upotrebom drag&drop tehnike.**
##### Omogućiti korisniku:
• Vizuelni pregled mape.
• Vizuelni pregled rasporeda spomenika.
• Prevlačenje predstave spomenika sa kontrole koja vizuelizuje skup dostupnih spomenika na predstavu mape. Spomenici se ne smeju preklapati.
• Prikaz svih atributa bitnih za jasnu i jedinstvenu identifikaciju spomenika prilikom izbora.

## DODATNA FUNKCIONALNOST — TUTORIAL
Implementirati u okviru aplikacije podsistem koji omogućava upoznavanje sa nekima od funkcionalnosti aplikacije preko interaktivne demnostracije (eng. tutorial). Priroda ove demonstracije zavisi od ostalih faktora zadatka, poglavito profila korisnika.
 ## PROFIL KORISNIKA
Pol: *Muški.*
Starost: *57 godina*
#### Domensko znanje
Izuzetno visoko. Korisnik je stručnjak za oblast u kojoj aplikacija radi.
##### Znanje rada na računaru:
Izuzetno slabo. Korisnik se u opšte ne snalazi u radu na računaru.
#### Ograničavajuće osobine:
Korisnik je navikao da ne koristi računar u toku poslovanja. Kao rezultat, korisnik strahuje da će nepažljivim rukovanjem "pokvariti računar." Aplikacija i njen interfejs moraju biti tako projektovani da ovaj strah umanje.

## SCENARIO KORIŠĆENJA
U ovom scenariju, program se koristi kao jedan od više alata. Kao rezultat, pažnja korisnika će verovatno biti skrenuta tokom rada sa programom. Takođe, program mora da ispoštuje to što se ne koristi sam, te da ne pokušava da zazume više pažnje nego što mu je neophodno, ili više resursa interfejsa nego što je neophodno.
