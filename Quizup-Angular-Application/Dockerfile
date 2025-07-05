FROM node:22

WORKDIR /app

COPY package.json package-lock.json ./

RUN npm install -g @angular/cli

RUN npm install --force

COPY . .

EXPOSE 4200

CMD ["ng", "serve", "--host", "0.0.0.0"]
