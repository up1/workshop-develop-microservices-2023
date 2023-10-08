/opt/mssql-tools/bin/sqlcmd -Usa -PzitgmLwmp1@q -Sdatabase -d catalog_api -i setup.sql
if [ $? -eq 0 ]
then
    echo "setup.sql completed"
    break
else
    echo "not ready yet..."
    sleep 1
fi