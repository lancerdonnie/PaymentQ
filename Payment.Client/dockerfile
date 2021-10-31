FROM node:14-alpine as build
WORKDIR /app
COPY package*.json ./
RUN npm install
COPY . ./
RUN npm run build

FROM nginx:alpine
WORKDIR /usr/share/nginx/html
COPY --from=build /app/dist/ ./
RUN  rm /etc/nginx/conf.d/default.conf
COPY  nginx.conf /etc/nginx/conf.d/
EXPOSE 80
ENTRYPOINT ["nginx", "-g", "daemon off;"]