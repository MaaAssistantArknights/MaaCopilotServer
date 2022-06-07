if [ -z "$1" ]; then
  echo ">>>>> ERROR: Version could not be empty!"
  exit
fi

docker buildx build --load --platform linux/amd64 -t maa-copilot-server:"$1"-amd64 .
docker buildx build --load --platform linux/arm64 -t maa-copilot-server:"$1"-arm64 .
