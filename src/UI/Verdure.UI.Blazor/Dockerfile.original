FROM node:14.18.1 as builder

WORKDIR /code

ADD VClient/package.json /code

RUN npm config set metrics-registry="https://registry.npmjs.org/"
RUN npm config set registry="https://registry.npmjs.org/"
RUN npm config set proxy="http://172.25.0.88:80/"

#ENV http_proxy http://172.25.0.88:80

#ENV https_proxy http://172.25.0.88:80

#RUN npm install -g npm@8.0.0

#RUN npm install -g @vue/cli

#RUN rm -rf node_modules

RUN npm install

ADD VClient/. /code

RUN npm run build

FROM nginx

EXPOSE 80

COPY --from=builder code/dist /usr/share/nginx/html/

COPY --from=builder code/nginx/default.conf /etc/nginx/conf.d/default.conf

CMD [ "nginx", "-g", "daemon off;"]

# FROM nginx
# COPY ./dist /usr/share/nginx/html/
# COPY ./nginx/default.conf /etc/nginx/conf.d/default.conf
# EXPOSE 80
# CMD ["nginx","-g","daemon off;"]
