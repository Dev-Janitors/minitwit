FROM mcr.microsoft.com/mssql/server:2017-latest
ENV ACCEPT_EULA=Y
ENV MSSQL_USER=SA
ENV SA_PASSWORD=MyStrongPassword#
ENV MSSQL_PID=Developer
ENV MSSQL_TCP_PORT=1433
ENV NAME = MinitwitDB
WORKDIR /src
RUN (/opt/mssql/bin/sqlservr --accept-eula & ) | grep -q "Service Broker manager has started"