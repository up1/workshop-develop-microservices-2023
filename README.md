# Microservices workshop
* Frontend => ReactJS
* API gateway => APISIX or Kong
* Services
  * NodeJS
  * .NET C#
* Database => MS SQL Server
* Working with Docker

## Clone project
```
$git clone https://github.com/up1/workshop-develop-microservices-2023.git demo
$cd demo
```

## Step to run workshop with Docker
1. Start Distributed tracing :: [Jaeger](https://www.jaegertracing.io/)
2. Start API gateway :: [APISIX](https://apisix.apache.org/)


```
$docker compose -f docker-compose-build.yml up -d jaeger
$docker compose -f docker-compose-build.yml up -d gateway
```

URL of Jaeger dashboard :: http://localhost:16686/

### Start `database service`
```
$docker compose -f docker-compose-build.yml up -d database
$docker compose -f docker-compose-build.yml ps
```

### Start `Stock service`
```
$docker compose -f docker-compose-build.yml build stock
$docker compose -f docker-compose-build.yml up -d stock
```

Call Stock servie with URL :: http://localhost:9080/stock/


### Start `Catalog service`
```
$docker compose -f docker-compose-build.yml build catalog
$docker compose -f docker-compose-build.yml up -d catalog
```

Call Stock servie with URL :: http://localhost:9080/catalog/


## Deploy with single command
```
$sh deploy_with_docker.sh
$docker compose -f docker-compose-build.yml ps
$docker compose -f docker-compose-build.yml logs --follow
```

## Testing Process

1. Stock service
```
$docker compose -f docker-compose-build.yml build stock
$docker compose -f docker-compose-build.yml up -d stock
$docker compose -f docker-compose-build.yml logs --follow
$docker compose -f docker-compose-build.yml ps
NAME                IMAGE               COMMAND                  SERVICE             CREATED             STATUS                   PORTS
demo-stock-1        somkiat/stock:1.0   "docker-entrypoint.s…"   stock               6 seconds ago       Up 5 seconds (healthy)

$docker compose -f docker-compose-testing.yml up stock_testing --force-recreate

$docker compose -f docker-compose-build.yml down
$docker compose -f docker-compose-testing.yml down
$docker volume prune
```

2. Pricing service
```
$docker compose -f docker-compose-build.yml build pricing
$docker compose -f docker-compose-build.yml up -d pricing
$docker compose -f docker-compose-build.yml logs --follow
$docker compose -f docker-compose-build.yml ps
NAME                IMAGE                 COMMAND                  SERVICE             CREATED             STATUS                   PORTS
demo-pricing-1      somkiat/pricing:1.0   "docker-entrypoint.s…"   pricing             8 seconds ago       Up 7 seconds (healthy)

$docker compose -f docker-compose-testing.yml up  pricing_testing --force-recreate

$docker compose -f docker-compose-build.yml down
$docker compose -f docker-compose-testing.yml down
$docker volume prune
```

3. Catalog service
```
$docker compose -f docker-compose-build.yml down
$docker compose -f docker-compose-build.yml build
$docker compose -f docker-compose-build.yml up -d gateway
$docker compose -f docker-compose-build.yml logs --follow
$docker compose -f docker-compose-build.yml ps
NAME                IMAGE                              COMMAND                  SERVICE             CREATED             STATUS                   PORTS
database            mcr.microsoft.com/azure-sql-edge   "/opt/mssql/bin/perm…"   database            4 minutes ago       Up 4 minutes (healthy)   1401/tcp, 0.0.0.0:1433->1433/tcp, :::1433->1433/tcp
demo-catalog-1      somkiat/catalog:1.0                "dotnet catalog.dll"     catalog             10 seconds ago      Up 8 seconds             
demo-jaeger-1       demo-jaeger                        "/go/bin/all-in-one-…"   jaeger              10 seconds ago      Up 9 seconds             5775/udp, 5778/tcp, 14250/tcp, 6831-6832/udp, 14268/tcp, 0.0.0.0:16686->16686/tcp, :::16686->16686/tcp
demo-pricing-1      somkiat/pricing:1.0                "docker-entrypoint.s…"   pricing             4 minutes ago       Up 4 minutes (healthy)   
demo-stock-1        somkiat/stock:1.0                  "docker-entrypoint.s…"   stock               4 minutes ago       Up 4 minutes (healthy)


$sh initial_data.sh

$docker compose -f docker-compose-testing.yml up catalog_testing --force-recreate
```

4. Gateway Testing
```
$docker compose -f docker-compose-testing.yml up gateway_testing --force-recreate
```

5. Delete all resources
```
$docker compose -f docker-compose-build.yml down
$docker compose -f docker-compose-testing.yml down
$docker volume prune
```

## Website references
* [Course Microservices Workshop](https://github.com/up1/course_microservices-3-days)
* [NodeJS](https://github.com/up1/workshop-nodejs-web)
