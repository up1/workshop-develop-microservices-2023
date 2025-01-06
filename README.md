# Microservices workshop 2023
* API gateway
  * APISIX
    * [Tracers](https://apisix.apache.org/docs/apisix/plugins/opentelemetry/)
    * [Metrics](https://apisix.apache.org/docs/apisix/plugins/prometheus/)
    * [Loggers](https://apisix.apache.org/docs/apisix/plugins/http-logger/)
* Services
  * Stock and Pricing
    * NodeJS
    * MySQL
  * Catalog
    * .NET 7
    * C#
    * MSSQL Server
* Observability
  * Distributed Tracing
    * OpenTelemetry
    * Jaeger
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

$docker compose -f docker-compose-build.yml ps
$docker compose -f docker-compose-build.yml logs --follow
```

* URL of Jaeger dashboard :: http://localhost:16686
* URL of API gateway :: http://localhost:9080

### Start `Stock service`
```
$docker compose -f docker-compose-build.yml build stock
$docker compose -f docker-compose-build.yml up -d stock
```

Call Stock servie with URL :: http://localhost:9080/stock/

### Start `Pricing service`
```
$docker compose -f docker-compose-build.yml build pricing
$docker compose -f docker-compose-build.yml up -d pricing
```

Call Stock servie with URL
* http://localhost:9080/pricing/
* http://localhost:9080/pricing/product/1

### Start `database service` with MSSQL Server
```
$docker compose -f docker-compose-build.yml up -d database
$docker compose -f docker-compose-build.yml ps
```

### Start `Catalog service`
```
$docker compose -f docker-compose-build.yml build catalog
$docker compose -f docker-compose-build.yml up -d catalog
```

* Call Stock service from API Gateway with URL :: http://localhost:9080/catalog/
* Initial data for testing :: http://localhost:9080/catalog/init
* API of catalog services
  * Get all product from database :: http://localhost:9080/catalog/products-db
  * Get all product + pricing + stock service :: http://localhost:9080/catalog/products


### Delete all services
```
$docker compose -f docker-compose-build.yml down
```


## Deploy with single command
```
$sh deploy_with_docker.sh
$docker compose -f docker-compose-build.yml ps
$docker compose -f docker-compose-build.yml logs --follow
```

## Working with Application metric
* APISIX 
* [Prometheus](https://prometheus.io/)
* [Grafana](https://grafana.com/)


### Step 1 :: Start APISIX as a API Gateway
```
$docker compose -f docker-compose-build.yml up -d gateway
$docker compose -f docker-compose-build.yml ps
$docker compose -f docker-compose-build.yml logs --follow
```

URL of prometheus metric of APISIX
* http://localhost:9091/apisix/prometheus/metrics

Try to call service from api gateway
* http://localhost:9080/catalog/
* http://localhost:9080/stock/
* http://localhost:9080/pricing/

### Step 2 :: Start Prometheus server to collect metric data from APISIX
```
$docker compose -f docker-compose-build.yml up -d prometheus
$docker compose -f docker-compose-build.yml ps
$docker compose -f docker-compose-build.yml logs --follow
```

URL of prometheus server
* http://localhost:9090
* Go to menu Status -> Targets

List of metrics name
* apisix_http_requests_total
* apisix_http_status
* apisix_http_latency_bucket

### Step 3 :: Start Grafana server
```
$docker compose -f docker-compose-build.yml up -d grafana
$docker compose -f docker-compose-build.yml ps
$docker compose -f docker-compose-build.yml logs --follow
```

URL of grafana server
* http://localhost:3000
  * user=admin
  * password=admin

Try to config
* Datasource
* Dashboard

### Step 4 :: Application metric in Stock service


Build and run
```
$docker compose -f docker-compose-build.yml build stock
$docker compose -f docker-compose-build.yml up -d stock
```
URL of metric
  * http://localhost:9080/stock/metrics
    
Steps
* Add target in Prometheus
* Create dashboard in Grafana


### Step 5 :: Delete all resources
```
$docker compose -f docker-compose-build.yml down
$docker volume prune
```

## Testing Process :: API testing with Postman and newman

### 1. Stock service

Build and run
```
$docker compose -f docker-compose-build.yml build stock
$docker compose -f docker-compose-build.yml up -d stock
$docker compose -f docker-compose-build.yml logs --follow
$docker compose -f docker-compose-build.yml ps
NAME                IMAGE               COMMAND                  SERVICE             CREATED             STATUS                   PORTS
demo-stock-1        somkiat/stock:1.0   "docker-entrypoint.s…"   stock               6 seconds ago       Up 5 seconds (healthy)
```

Testing
```
$docker compose -f docker-compose-testing.yml up stock_testing
```

Delete
```
$docker compose -f docker-compose-build.yml down
$docker compose -f docker-compose-testing.yml down
$docker volume prune
```

### 2. Pricing service

Build and run
```
$docker compose -f docker-compose-build.yml build pricing
$docker compose -f docker-compose-build.yml up -d pricing
$docker compose -f docker-compose-build.yml logs --follow
$docker compose -f docker-compose-build.yml ps
NAME                IMAGE                 COMMAND                  SERVICE             CREATED             STATUS                   PORTS
demo-pricing-1      somkiat/pricing:1.0   "docker-entrypoint.s…"   pricing             8 seconds ago       Up 7 seconds (healthy)
```
Testing
```
$docker compose -f docker-compose-testing.yml up  pricing_testing
```

Delete
```
$docker compose -f docker-compose-build.yml down
$docker compose -f docker-compose-testing.yml down
$docker volume prune
```

### 3. Catalog service

Start Jaeger
```
$docker compose -f docker-compose-build.yml up -d jaeger
$docker compose -f docker-compose-build.yml ps
```

Start MSSQL Server
```
$docker compose -f docker-compose-build.yml up -d database
$docker compose -f docker-compose-build.yml ps
```

Build and run service
```
$docker compose -f docker-compose-build.yml build
$docker compose -f docker-compose-build.yml up -d catalog
$docker compose -f docker-compose-build.yml logs --follow
$docker compose -f docker-compose-build.yml ps
NAME                IMAGE                              COMMAND                  SERVICE             CREATED             STATUS                   PORTS
database            mcr.microsoft.com/azure-sql-edge   "/opt/mssql/bin/perm…"   database            4 minutes ago       Up 4 minutes (healthy)   1401/tcp, 0.0.0.0:1433->1433/tcp, :::1433->1433/tcp
demo-catalog-1      somkiat/catalog:1.0                "dotnet catalog.dll"     catalog             10 seconds ago      Up 8 seconds             
demo-jaeger-1       demo-jaeger                        "/go/bin/all-in-one-…"   jaeger              10 seconds ago      Up 9 seconds             5775/udp, 5778/tcp, 14250/tcp, 6831-6832/udp, 14268/tcp, 0.0.0.0:16686->16686/tcp, :::16686->16686/tcp
demo-pricing-1      somkiat/pricing:1.0                "docker-entrypoint.s…"   pricing             4 minutes ago       Up 4 minutes (healthy)   
demo-stock-1        somkiat/stock:1.0                  "docker-entrypoint.s…"   stock               4 minutes ago       Up 4 minutes (healthy)
```

Initial data for testing
```
$curl http://localhost:9999/init
```

Testing
```
$docker compose -f docker-compose-testing.yml up catalog_testing
```

### 4. Gateway Testing

Build and Testing
```
$docker compose -f docker-compose-build.yml up -d gateway

$docker compose -f docker-compose-testing.yml up gateway_testing
```

Delete all resources
```
$docker compose -f docker-compose-build.yml down
$docker compose -f docker-compose-testing.yml down
$docker volume prune
```

## Website references
* [Course Microservices Workshop](https://github.com/up1/course_microservices-3-days)
* [NodeJS](https://github.com/up1/workshop-nodejs-web)
