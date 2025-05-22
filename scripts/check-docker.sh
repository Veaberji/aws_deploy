#!/bin/bash
set -e

retry() {
  local max_wait=600   # up to 10 minutes
  local interval=15
  local waited=0
  local cmd="$*"

  until $cmd; do
    if [ $waited -ge $max_wait ]; then
      echo "Error: Command '$cmd' failed after waiting $waited seconds."
      return 1
    fi

    echo "Command '$cmd' failed. Retrying in $interval seconds..."
    sleep $interval
    waited=$((waited + interval))
  done
  return 0
}

echo "Checking docker availability..."
retry docker version || exit 1

echo "Checking docker-compose availability..."
retry docker-compose version || exit 1

echo "Docker and docker-compose are ready."
