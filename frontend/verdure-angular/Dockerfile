FROM node as builder

WORKDIR /code

ADD package.json /code

RUN npm install -g @angular/cli

RUN npm install

ADD . /code

RUN ng build

#FROM nginx:alpine

LABEL maintainer="GreenShade Zhang <gil.zhang.dev@outlook.com>"

#COPY /nginx/ /etc/nginx/

#COPY  /dist/ /usr/share/nginx/html/

#EXPOSE 80




FROM nginx

COPY --from=builder code/dist/verdure-angular /usr/share/nginx/html/

COPY --from=builder code/nginx/default.conf /etc/nginx/conf.d/default.conf

CMD [ "nginx", "-g", "daemon off;"]
