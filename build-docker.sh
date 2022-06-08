if [ -z "$1" ]; then
  echo ">>>>> ERROR: Version could not be empty!"
  exit 1
fi

docker buildx build --load --build-arg APP_VERSION="$1" --platform linux/amd64 -t maa-copilot-server:"$1"-amd64 .
docker buildx build --load --build-arg APP_VERSION="$1" --platform linux/arm64 -t maa-copilot-server:"$1"-arm64 .
