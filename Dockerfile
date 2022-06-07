FROM mcr.microsoft.com/dotnet/aspnet:6.0

WORKDIR /app

COPY ./publish /app/

RUN ["mkdir", "/app/data"]

ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV ASPNETCORE_URLS=http://+:80

ENV MAA_Jwt__Secret=""
ENV MAA_Jwt__Issuer="MaaCopilot"
ENV MAA_Jwt__Audience="Doctor"
ENV MAA_Jwt__ExpireTime=720
ENV MAA_Database__ConnectionString=""
ENV MAA_ElasticLogSink__Enabled=false
ENV MAA_ElasticLogSink__Uris="http://localhost:9200"
ENV MAA_ElasticLogSink__Period=10
ENV MAA_ElasticLogSink__ApiId=""
ENV MAA_ElasticLogSink__ApiKey=""
ENV MAA_ElasticApm__Enabled=false
ENV MAA_ElasticApm__SecretToken=""
ENV MAA_ElasticApm__ServerUrls="http://localhost:8200"
ENV MAA_ElasticApm__ServiceName="MaaCopilotServer"
ENV MAA_ElasticApm__Environment="production"

EXPOSE 80/tcp

ENTRYPOINT ["dotnet", "MaaCopilotServer.Api.dll"]
