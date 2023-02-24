FROM node:16.14.2-alpine

WORKDIR /Frontend

COPY ./Frontend ./Frontend

RUN yarn set version 1.22.19

RUN yarn

COPY . .

EXPOSE 3333

CMD ["yarn", "build"]
