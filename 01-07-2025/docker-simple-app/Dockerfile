# First Docker build
FROM debian:bullseye

WORKDIR /app

RUN apt-get update &&\
    apt-get install -y curl &&\
    curl -fsSL https://deb.nodesource.com/setup_22.x | bash - &&\
    apt-get install -y nodejs

# Copying the required files
COPY . .

EXPOSE 3000

CMD ["node", "server.js"]
