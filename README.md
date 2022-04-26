# HOW TO RUN THIS BAD BOI

```
docker swarm init
docker network create -d overlay blackops-network
docker stack deploy -d docker-compose.yml webserver
docker stack deploy -d docker-compose.logging.yml logging
```
