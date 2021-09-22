# NoeliaStoryteller
Hello! 👋 This repository contains an example of CRUD API written in C# and ASP.NET Core 5 with Entity Framework 
using in-memory.

## App Logic Flow
![image](https://user-images.githubusercontent.com/27290461/134086243-786aed1c-0b8e-445e-943d-cac33e3ab453.png)


## NUGET
- Entity Framework Core
- JWT
- ASP net core MVC
- MSTest core
- Entity Framework core inMemory

## DEV Enviroment

1- Clone this repository

2- Build the solution using Visual Studio, or on the command line with dotnet build.

3- Run the project. The API will start up on https://localhost:44331, or https://localhost:44331 with dotnet run.

4- Use an HTTP client like Postman to GET http://localhost:44331.

## Test REST

#### Client
### Post Creat Client

body json

https://localhost:44331/api/Clients

{
  "email": "user@example.com",
  "userName": "string"
}

### Post get Token

https://localhost:44331/api/Clients/token

body json

{
  "email": "user@example.com",
  "userName": "string"
}

#### Messages

### Get 

Note: just to dev proposals return all messages

https://localhost:44331/api/MessageItems

### Post all client messages 

https://localhost:44331/api/MessageItems/AllMessageByEmail

body json

{
  "email": "user@example.com",
  "userName": "string"
}

### Get all messages just for cliet registered 

https://localhost:44331/api/MessageItems/byEmail/{email}

### Post Create new Message

https://localhost:44331/api/MessageItems

body json

{
  "id": 0,
  "message": "string",
  "client": {
    "email": "user@example.com",
    "userName": "string"
  }
}

### Put update message

https://localhost:44331/api/MessageItems/{messageId}

body json
{
  "id": 0,
  "message": "string",
  "client": {
    "email": "user@example.com",
    "userName": "string"
  }
}

### Delete message

param Message Id

https://localhost:44331/api/MessageItems/{messageId}

body json
{
  "email": "user@example.com",
  "userName": "string"
}


## Docker Image
https://hub.docker.com/repository/docker/saintnb/noeliastorytellerapi
