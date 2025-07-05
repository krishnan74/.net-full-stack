# Initialize the Docker Swarm
docker swarm init

# Build the Docker images
docker build -t api:latest ./api
docker build -t web:latest ./web

# Deploy the stack using Docker Compose
docker stack deploy -c docker-compose.yml fullstack

# List the services in the stack
docker stack services fullstack

# Scale the web service to 5 replicas
docker service scale fullstack_web=5

# Remove the stack
docker stack rm fullstack

# Leave the Docker Swarm
docker swarm leave --force
