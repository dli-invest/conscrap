[![codecov](https://codecov.io/gh/dli-invest/conscrap/branch/main/graph/badge.svg?token=1Tlyaj0OO4)](https://codecov.io/gh/dli-invest/conscrap)
# conscrap
conversation scrapper for ceo, yahoo finance and eventually stockhouse

Since I have been very good at keeping up at ceo.ca conversations, I will make the initial package only scan yahoo finance pages.

```
dotnet new console -o ConScrap.Cmd
dotnet new classlib -o ConScrap.Scrap
dotnet new classlib -o ConScrap.Render
dotnet new classlib -o ConScrap.Tests
```
