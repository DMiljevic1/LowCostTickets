King.Tickets projekt
Projekt se moze pokrenuti na dva nacina:
- Pokrenuti docker-compose projekt. Migracije ce se same izvrsiti - krairati bazu i tablice.
- Ukoliko ne zelite ici preko docker-compose-a, onda preko docker-compose-a podignite bazu a blazorUI i API podignite preko zasebnih instanci - ukoliko se ide ovim pristupit potrebno je promjenit url u configuration file-u(appsettings.json) u blazorUI projektu jer on trenutno gleda ime docker containera iz docker-compose.yml file-a.
Prije pokretanja u King.Tickets.API projektu u appsettings.json mora se podesiti API_KEY i API_SECRET od amadeusa(ukoliko ga nemate prijavite se na njihovu stranicu gdje ce te ih dobiti).
Frontend blazor url - "https://localhost:8081/"
Backend API url - "https://localhost:5001/swagger/index.html"
