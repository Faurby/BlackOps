# How to run it?
## Mongo
Make sure you have [MongoDB][1] installed on your device.

Open your terminal and make sure you're in the `BlackOps` directory then run

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

## Vagrant
Open your terminal and make sure you're in the `BlackOps` directory then run

- `vagrant up` - Create a new Droplet.
- `vagrant destroy` - Destroys the Droplet instance.
- `vagrant ssh` - Logs into the Droplet instance using the configured user account.
- `vagrant halt` - Powers off the Droplet instance.
- `vagrant provision` - Runs the configured provisioners and rsyncs any specified `config.vm.synced_folder`.
- `vagrant reload` - Reboots the Droplet instance.
- `vagrant rebuild` - Destroys the Droplet instance and recreates it with the same IP address which was previously assigned.
- `vagrant status` - Outputs the status (active, off, not created) for the Droplet instance.