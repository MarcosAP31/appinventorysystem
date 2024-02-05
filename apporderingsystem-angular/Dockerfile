FROM node:latest as builder

WORKDIR /app

COPY . .

RUN npm install --force
RUN npm run build --configuration=production

FROM nginx:latest

COPY --from=builder /app/dist/* /usr/share/nginx/html/