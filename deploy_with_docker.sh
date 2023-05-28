#!/bin/bash
FILE=docker-compose-build.yml
docker compose -f $FILE down
docker compose -f $FILE build

echo "Start jaeger service"
docker compose -f $FILE up -d jaeger

echo "Start gateway service"
docker compose -f $FILE up -d gateway

echo "Start database service"
docker compose -f $FILE up -d database

status="starting"
while [  "$status" != "healthy" ]
do
    status=$(docker container inspect --format {{.State.Health.Status}} database)
    echo "Service status = $status"
    sleep 5
done

echo "Database service started ..."

echo "Start stock service"
docker compose -f $FILE up -d stock

echo "Start catalog service"
docker compose -f $FILE up -d catalog

echo "All services started ..."