FROM mcr.microsoft.com/dotnet/aspnet:6.0

ARG APP_VERSION=0.0.0

WORKDIR /app

COPY ./publish /app/

RUN ["mkdir", "/app/data"]

ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV ASPNETCORE_URLS=http://+:80

ENV MAACOPILOT_APP_VERSION=$APP_VERSION

VOLUME ["/app/data"]

EXPOSE 80/tcp

ENTRYPOINT ["dotnet", "MaaCopilotServer.Api.dll"]
