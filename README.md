# How to run it?

Make sure you have [MongoDB][1] installed on your device.

open your terminal and make sure you're in the `BlackOps` directory then run

`mongod --dbpath ./MongoDB`

This should run the server. If an error appears, delete all the contents in the `MongoDB` directory and try again.

To see what is in the server via terminal

```bash
mongo

# To show DBs
dbs

# To access a DB
use MiniTwit

# To see collections
show collections

# To create a collection
db.createCollection('Users')

# To view everything in an collection
db.Users.find().pretty()

# To remove data in collection
db.Users.remove({})
```

To run it do `dotnet run --project ./Server/`

Open **https://localhost:7199/** to see the project.

Open **https://localhost:7199/swagger** to see the swagger definitions, and play around with the API.


[1]:https://www.mongodb.com/try/download/community
