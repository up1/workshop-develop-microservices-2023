# Microservices workshop
* Frontend => ReactJS
* API gateway => APISIX or Kong
* Services
  * NodeJS
  * .NET C#
* Database => MS SQL Server
* Working with Docker


## Step to run workshop with Docker
1. Start Distributed tracing :: [Jaeger](https://www.jaegertracing.io/)
2. Start API gateway :: [APISIX](https://apisix.apache.org/)


```
$docker compose -f docker-compose-build.yml up -d jaeger
$docker compose -f docker-compose-build.yml up -d gateway
```

URL of Jaeger dashboard :: http://localhost:16686/

Start `Stock service`
```
$docker compose -f docker-compose-build.yml build stock
$docker compose -f docker-compose-build.yml up -d stock
```

Call Stock servie with URL :: http://localhost:9080/stock/

