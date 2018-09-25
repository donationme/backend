# SADJZ Backend


## Setup


Things to Install


1. [Postman](https://www.getpostman.com) Used sending HTTP calls to test server
2. [MongoDB](https://docs.mongodb.com/manual/administration/install-community/) Used sending HTTP calls to test server
3. [Dotnet CLI](https://docs.microsoft.com/en-us/dotnet/core/tools/?tabs=netcore2x) Used to compile and run server
4. [NoSqlClient](https://github.com/nosqlclient/nosqlclient/releases) Used to visualize MongoDB database (optional)

```
// To Install and run server from source

//Restore packages 
dotnet restore

//Run
dotnet run

//Intiallly configure database directory with
sudo mkdir -p /data/db

//Launch up MongoDB database with
sudo mongod

Lastly in your Andriod project go to AndroidApp/app/src/main/java/com/github/sadjz/consts/AppConst.java
change the server address to your local ip address

Load the SADJZ.postman_collection.json to get all the API endpoints.
In the endpoints change ```localhost``` to whatever your local ip address is.

```



## Features
1. Add User
2. Add Privlidged User (Admin,Location Employee, Manager) with admin level access
3. Get User
4. Get JWT token to authenticate with

