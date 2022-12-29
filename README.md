This is a sample code targeted to use the "[TestContainers package](https://www.nuget.org/packages/Testcontainers)" for integration tests. you can find more complex examples by checking the official package's GitHub repository, [here](https://github.com/testcontainers/testcontainers-dotnet).

In this project, I created a sample API to save data in an SQL server by EF core7, third-party API, etc. Then I started to develop some tests for these APIs.

TestContainers package helps you to run your project and its dependencies with docker technology and request them independently or with a shared context. I configure this project to run each test independently.

Please read the wiki page to undestand the code.

Hope it could be useful. ;)
