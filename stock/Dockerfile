#docker build -t stock:1.0 .
FROM node:20
WORKDIR /app
COPY package.json .
RUN npm install
COPY . .
CMD node index.js