# MaaCopilotServer

MAA 作业服务器

## API 定义

[API FOX 接口文档](https://www.apifox.cn/apidoc/shared-e9acdf71-e5e6-4198-aaa7-5417e1304335)

## 编译与打包

前提条件：`.NET SDK 6.0.x`

编译：
```shell
dotnet build -c Release ./src/MaaCopilotServer.Api/MaaCopilotServer.Api.csproj
```

发布：
```shell
dotnet publish -c Release -o ./publish ./src/MaaCopilotServer.Api/MaaCopilotServer.Api.csproj
```

制作 Docker 镜像：
```shell
docker build -t maa-copilot-server:latest .
```

使用 BuildX 制作 Docker 镜像：
```shell
docker buildx build --platform linux/amd64 -t maa-copilot-server:latest .
docker buildx build --platform linux/arm64 -t maa-copilot-server:latest .
```

## 使用 Docker 运行

### docker-compose

```yaml
version: "3"

services:

  maa-copilot:
    image: alisaqaq/maa-copilot-server:latest
    container_name: maa-copilot
    depends_on:
      - pgsql
    environment:
      MAA_Database__ConnectionString: "Server=pgsql;Port=5432;Database=maa_copilot;User Id=maa_admin;Password=maa@passwd;"
      MAA_Jwt__Secret: "A VERY LONG (128+ BITS) JWT SECRET STRING"
      OTHER_KEY: OTHER_VALUE
    ports:
      - "8080:80"
  
  pgsql:
    image: postgres:latest
    container_name: pgsql
    environment:
      POSTGRES_USER: maa_admin
      POSTGRES_PASSWORD: maa@passwd
      POSTGRES_DB: maa_copilot
    volumes:
      - pgsql-data:/var/lib/postgresql/data
    
volumes:
    pgsql-data:
```

### Docker 环境变量

| 变量名                            | 默认值                       | 说明                           |
|--------------------------------|---------------------------|------------------------------|
| MAA_Jwt__Secret                | ""                        | Jwt 密钥                       |
| MAA_Jwt__Issuer                | "MaaCopilot"              | Jwt 签发者                      |
| MAA_Jwt__Audience              | "Doctor"                  | Jwt 接收者                      |
| MAA_Jwt__ExpireTime            | 720                       | Jwt 过期时间（小时）                 |
| MAA_Database__ConnectionString | ""                        | 数据库连接字符串                     |
| MAA_ElasticLogSink__Enabled    | false                     | 日志是否推送到 ElasticSearch        |
| MAA_ElasticLogSink__Uris       | "http://localhost:9200"   | ElasticSearch Uri，多个用分号分隔    |
| MAA_ElasticLogSink__Period     | "10"                      | ElasticSearch 日志推送周期（秒）      |
| MAA_ElasticLogSink__ApiId      | ""                        | ElasticSearch ApiId          |
| MAA_ElasticLogSink__ApiKey     | ""                        | ElasticSearch ApiKey         |
| MAA_ElasticApm__Enabled        | false                     | 是否启用 ElasticApm              |
| MAA_ElasticApm__SecretToken    | ""                        | ElasticApm SecretToken       |
| MAA_ElasticApm__ServerUrls     | "http://localhost:8200"   | ElasticApm 服务器地址             |
| MAA_ElasticApm__ServiceName    | "MaaCopilotServer"        | ElasticApm 服务名称              |
| Maa_ElasticApm__Environment    | "production"              | ElasticApm 环境名称              |
