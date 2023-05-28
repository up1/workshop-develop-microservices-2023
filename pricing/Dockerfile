#docker build -t pricing:1.0 .
FROM node:20-alpine3.16
RUN apk update && \
    apk add --no-cache tzdata
ENV TZ=Asia/Bangkok
WORKDIR /app
COPY package*.json .
RUN npm install
COPY . .
CMD node index.js