# Stage 1: Build the Angular app
FROM node:22 AS build
WORKDIR /app
COPY package.json package-lock.json ./
RUN npm install --force
COPY . .
RUN npm run build -- --output-path=dist

# Stage 2: Serve the Angular app with NGINX
FROM nginx:alpine
COPY --from=build /app/dist/browser/ /usr/share/nginx/html
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]