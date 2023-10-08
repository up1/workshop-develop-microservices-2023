FROM mcr.microsoft.com/mssql-tools:latest
WORKDIR /works
COPY . .
RUN chmod a+x initial.sh
CMD [ "/works/initial.sh" ]