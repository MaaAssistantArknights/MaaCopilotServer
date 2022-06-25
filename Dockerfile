FROM mcr.microsoft.com/dotnet/aspnet:6.0

ARG APP_VERSION=0.0.0

WORKDIR /app

COPY ./publish /app/

RUN ["mkdir", "/app/data"]

ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV ASPNETCORE_URLS=http://+:80

ENV MAA_COPILOT_APP_VERSION=$APP_VERSION
ENV MAA_DATA_DIRECTORY=/app/data
ENV MAA_DEFAULT_USER_EMAIL="super@prts.plus"
ENV MAA_DEFAULT_USER_NAME="Maa"
ENV MAA_DEFAULT_USER_PASSWORD=""

VOLUME ["/app/data"]

EXPOSE 80/tcp

ENTRYPOINT ["dotnet", "MaaCopilotServer.Api.dll"]
