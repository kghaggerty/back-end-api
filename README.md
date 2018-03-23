## New Day API 
## Author: Kevin Haggerty
---

This repository is the api for my website New Day.  It was made in visual studio code using C#/.NET and SQlite.  Other packages include swagger, CORS, AspNet WebApi.  


If you are intersted in downloading this repo and having it work on your machine, follow these steps!

1. Download .net core https://www.microsoft.com/net/download/windows

1. Clone the API repository
```
git clone https://github.com/kghaggerty/back-end-api.git
```

3. Download SQLite and make sure you change the environment variable in your startup.cs. https://www.sqlite.org/download.html


4. In your terminal, make your way into the top of the project files and run --
```
dotnet ef migrations add Initial Create
```
and

```
dotnet ef database update
```



5.  Ready to run the application. ```dotnet run```



Let's go set up the front end web page if you haven't already.  
https://github.com/kghaggerty/Back-End-Capstone




