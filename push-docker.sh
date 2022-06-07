docker image tag maa-copilot-server:"$1"-amd64 "$2"/maa-copilot-server:"$1"-amd64
docker image tag maa-copilot-server:"$1"-arm64 "$2"/maa-copilot-server:"$1"-arm64

docker image push --all-tags "$2"/maa-copilot-server

docker manifest create "$2"/maa-copilot-server:"$1" \
  "$2"/maa-copilot-server:"$1"-amd64 \
  "$2"/maa-copilot-server:"$1"-arm64

docker manifest push "$2"/maa-copilot-server:"$1"

docker manifest create --amend "$2"/maa-copilot-server:latest \
  "$2"/maa-copilot-server:"$1"-amd64 \
  "$2"/maa-copilot-server:"$1"-arm64

docker manifest push "$2"/maa-copilot-server:latest
