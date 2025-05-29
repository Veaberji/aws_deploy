#!/bin/bash
set -e

EC2_IP=$1
ENV_FILE=UI/src/environments/environment.prod.ts
LOGGER_LEVEL=ERROR
TITLE=MusiciansAPP

if [ -z "$EC2_IP" ]; then
  echo "Missing EC2 IP"
  exit 1
fi

echo "Generating environment.prod.ts with EC2 IP: $EC2_IP"

cat > $ENV_FILE <<EOL
import { NgxLoggerLevel } from 'ngx-logger';

export const environment = {
  title: '$TITLE',
  baseApiUrl: 'http://$EC2_IP:7093/api/',
  production: true,
  logging: {
    level: NgxLoggerLevel.$LOGGER_LEVEL,
  },
};
EOL

echo "Generated $ENV_FILE"