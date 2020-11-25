# Hacker News API Integrarion

## Requirement
[ASP.NET Core Hosting Bundle](https://dotnet.microsoft.com/download/dotnet-core/thank-you/runtime-aspnetcore-2.2.5-windows-hosting-bundle-installer)

## How to run
1. Open Solution on Visual Studio;
2. Run with IIS Express;
3. Get the response from the browser or make a request by any API Testing Tool.

## Assumptions 
1. The endpoint for best stories always return the list ordened by score;
2. The commentCount property on the response is the total amount of comments on all levels (comments of comments) of the story;
3. The time property is based on the local time, since the date format has an offset from UTC.

## Enhancements and/or Changes
1. Add logs on a database of similar;
2. Treat better the possible exceptions types and return the proper http code;

## License
[MIT](https://choosealicense.com/licenses/mit/)
