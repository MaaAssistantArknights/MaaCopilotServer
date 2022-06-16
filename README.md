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

## 开发时运行

若要在本机进行开发，可以使用以下命令，请复制一份 `appsettings.json`，命名为 `appsettings.Development.json`，并修改相关配置。

你需要运行有以下服务：

1. 数据库（必须）：Postgres 数据库，v14.x 版本
2. SMTP 服务器（必须）：推荐使用 [rnwood/smtp4dev](https://github.com/rnwood/smtp4dev) 在本地启动一个测试 SMTP 服务器
3. ELK Stack + APM（可选）：v8.x.x 版本，请参考 [LiamSho/docker-compose#elk-apm](https://github.com/LiamSho/docker-compose/tree/main/elk-apm)

数据库与 SMTP 服务器开发时使用参考 docker compose 文件：

```yaml
version: '3'

services:
  pgsql:
    image: postgres:14
    container_name: postgres
    restart: unless-stopped
    environment:
      POSTGRES_DB: maa_copilot
      POSTGRES_PASSWORD: maa@passwd
      POSTGRES_USER: maa_admin
    volumes:
      - maa-copilot-data:/var/lib/postgresql/data
    ports:
     - 5432:5432

  smtp4dev:
    image: rnwood/smtp4dev:v3
    restart: unless-stopped
    ports:
      - '8089:80'
      - '25:25'
    volumes:
        - maa-smtp4dev-data:/smtp4dev
    environment:
      - ServerOptions__HostName=smtp.maa.plus
      - ServerOptions__LockSettings=true
      - ServerOptions__NumberOfMessagesToKeep=1000
      - ServerOptions__NumberOfSessionsToKeep=100

volumes:
  maa-copilot-data:
  maa-smtp4dev-data:
```

可以登录 `localhost:8089` 查看测试用 SMTP 服务器监控面板，SMTP 将在 25 端口监听，数据库将在 5432 端口监听

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
    volumes:
      - ./data:/app/data
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
