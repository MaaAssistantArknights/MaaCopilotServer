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
