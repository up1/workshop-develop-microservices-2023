#!/bin/bash
echo "Intial database for testing ..."
DB_USER=SA
DB_PASSWORD=zitgmLwmp1@q
DB_NAME=catalog_api

docker container exec database /opt/mssql-tools/bin/sqlcmd -U "$DB_USER" -P "$DB_PASSWORD" -d master -i /app/setup.sql
echo "Success ..."
