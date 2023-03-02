FROM node:16.14.2-alpine

WORKDIR /App

COPY ./Frontend ./Frontend

WORKDIR /App/Frontend

RUN --mount=type=secret,id=REACT_APP_AUTH0_DOMAIN \
    --mount=type=secret,id=REACT_APP_AUTH0_CLIENT_ID \
    --mount=type=secret,id=REACT_APP_AUTH0_CALLBACK_URL \
    --mount=type=secret,id=REACT_APP_API_SERVER_URL \
    --mount=type=secret,id=REACT_APP_AUTH0_AUDIENCE \
    export API_ENDPOINT=$(cat /run/secrets/REACT_APP_AUTH0_DOMAIN) && \
    export API_PASSWORD=$(cat /run/secrets/REACT_APP_AUTH0_CLIENT_ID) && \
    export API_PASSWORD=$(cat /run/secrets/REACT_APP_AUTH0_CALLBACK_URL) && \
    export API_PASSWORD=$(cat /run/secrets/REACT_APP_API_SERVER_URL) && \
    export API_PASSWORD=$(cat /run/secrets/REACT_APP_AUTH0_AUDIENCE) && \
    yarn gen

RUN --mount=type=secret,id=_env,dst=/etc/secrets/.env cat /etc/secrets/.env

RUN yarn set version 1.22.19

RUN yarn

EXPOSE 3000

RUN yarn build

RUN yarn global add serve

CMD [ "yarn","serve" ]