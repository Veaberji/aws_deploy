## MusiciansAPP

---

### The project is a mixture of music.yandex.ru and last.fm. The Yandex functionality is taken as a basis, but the Last.fm API is used to receive data.

---

- Install Visual Studio 2019/2022 Community Edition, MSSQL Server 2019, and Git client (like SourceTree).
- Design an architectural application that will consist of Back-End (API), Front-End, business logic layer, database work layer, and several more levels if necessary. Take the Onion Architecture as an example.
- Create a new project (.NET Core/.NET 6 + Angular 13, i.e. base template in VS). For beautiful UI, it is recommended to use Bootstrap (possible in conjunction with Material Design) or Angular Material (preferred, but not required)
- Implement the main page, where the Top artists from last.fm will be displayed with tiles.
  A tile is a clickable card with a photo and artist name. If you can not get a photo, then put any image by default.
- Implement pagination on the main page with a selector of the numbers of entries per page (12, 24, 48).
- Change the URL on the page, so that direct link access is available.
- Implement the artist's page: photo, name, biography, and three tabs (top songs, top albums, similar artists). The UI can be viewed, for example, on [Yandex Music](https://music.yandex.ru/artist/792433/tracks).
- Info in the tab should be selected by selective queries.
- Refine the "Similar" tab so that the artists in it open in your application in a new panel.
- Implement the "Album" page, where it will have: cover, title, artist name, and link to his page, and a list of tracks from this album.
- To work with the database, use the Entity Framework.
- Generate a database from the models created earlier.
- Implement the Repository template for working with the database.
- For each last.fm call, add a save to the database.
- Update the service methods, so that they first look for the necessary data in the database, and only if they are not there, they call last.fm.
- Implement unit tests for the business logic layer.

### Run the project locally

You need to install Angular 13, .NET Core 6 and SQL Server 2019.
You have to get your LastFm api key and add to the local secret. Use `dotnet user-secrets init` and `dotnet user-secrets set "Movies:ServiceApiKey" "<your api key>"` commands. See more here: [Safe storage of app secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets). Then start the client by running the command `npm start start` inside the Client folder and start the api by running the command `dotnet run` inside the MusiciansAPP folder. The app will be available at `https://localhost:4200/`.

### Run the project in Docker

You need Docker to run this app and the LastFm api key. Create `.env` file at the root project folder. with your lost.fm api key to the `API_KEY` and a DB password `DB_PASSWORD`. Then just run `docker compose up` inside the root folder of the project. The app will be available at `http://localhost:4200/`. You can remove the app containers by running the command `docker compose down`, your DB data still will be available in the Docker volumes.
