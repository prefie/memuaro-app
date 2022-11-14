FROM node:18.12.1-alpine as builder
WORKDIR /app
COPY package.json package-lock.json ./
RUN npm install
COPY . .
RUN npm run build

FROM nginx:mainline-alpine

COPY --from=builder /app/dist/memuaro-app /var/www
COPY nginx.conf /etc/nginx/nginx.conf

EXPOSE 80 443
ENTRYPOINT ["nginx", "-g", "daemon off;"]
