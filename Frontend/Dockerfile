FROM node:16.14.2-alpine

WORKDIR /App

COPY ./Frontend ./Frontend

WORKDIR ./Frontend

RUN yarn set version 1.22.19

RUN yarn

COPY . .

EXPOSE 3000

RUN yarn build

RUN yarn global add serve

CMD [ "yarn","serve" ]