# HOW TO RUN THIS BAD BOI

```
docker swarm init
docker network create -d overlay blackops-network
docker stack deploy -c docker-compose.yml webserver
docker stack deploy -c docker-compose.logging.yml logging
```
