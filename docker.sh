#!/bin/bash

echo "Building images for MiniTwit"
docker build -t faurby/webserver -f Dockerfile-webserver .
docker build -t faurby/db -f Dockerfile-db .

echo "Push images to docker hub"
docker push faurby/webserver:latest
docker push faurby/db:latest