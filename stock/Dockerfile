#docker build -t stock:1.0 .
FROM node:20-alpine3.16
WORKDIR /app
COPY package*.json .
RUN npm install
COPY . .
CMD node index.js