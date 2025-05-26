#!/bin/bash
set -e

retry() {
  local n=0
  local max=5
  local delay=10
  local cmd="$@"

  until [ $n -ge $max ]; do
    $cmd && break
    n=$((n+1))
    sleep $delay
  done

  if [ $n -eq $max ]; then
    echo "'$cmd' failed after $max attempts"
    return 1
  fi
}

retry sudo yum update -y
retry sudo yum install -y docker
retry sudo service docker start
retry sudo usermod -a -G docker ec2-user

retry sudo curl -L "https://github.com/docker/compose/releases/latest/download/docker-compose-$(uname -s)-$(uname -m)" \
  -o /usr/local/bin/docker-compose
retry sudo chmod +x /usr/local/bin/docker-compose
