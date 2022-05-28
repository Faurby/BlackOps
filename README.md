# HOW TO RUN THIS BAD BOI

```
docker swarm init
docker network create -d overlay blackops-network
docker stack deploy -c docker-compose.yml webserver
docker stack deploy -c docker-compose.logging.yml logging
```

When running using the commands above we assume there is a droplet with a database. The system uses a hardcoded connection string to connect ot the database. As of the 10th of May we have stopped that droplet as to not be charged.
