#!/bin/bash

echo "Building images for MiniTwit"
docker build -t faurby/webserver -f Dockerfile-webserver .

echo "Push images to docker hub"
docker push faurby/webserver:latest
