FROM node:16.20-alpine

WORKDIR /App


# COPY ${frontendCopySrc} ./Frontend
COPY . ./Frontend

WORKDIR /App/Frontend

RUN --mount=type=secret,id=REACT_APP_AUTH0_DOMAIN \
    --mount=type=secret,id=REACT_APP_AUTH0_CLIENT_ID \
    --mount=type=secret,id=REACT_APP_AUTH0_CALLBACK_URL \
    --mount=type=secret,id=REACT_APP_API_SERVER_URL \
    --mount=type=secret,id=REACT_APP_AUTH0_AUDIENCE \
    export REACT_APP_AUTH0_DOMAIN=$(cat /run/secrets/REACT_APP_AUTH0_DOMAIN) && \
    export REACT_APP_AUTH0_CLIENT_ID=$(cat /run/secrets/REACT_APP_AUTH0_CLIENT_ID) && \
    export REACT_APP_AUTH0_CALLBACK_URL=$(cat /run/secrets/REACT_APP_AUTH0_CALLBACK_URL) && \
    export REACT_APP_API_SERVER_URL=$(cat /run/secrets/REACT_APP_API_SERVER_URL) && \
    export REACT_APP_AUTH0_AUDIENCE=$(cat /run/secrets/REACT_APP_AUTH0_AUDIENCE) && \
    echo "REACT_APP_AUTH0_DOMAIN=$REACT_APP_AUTH0_DOMAIN" >> .env && \
    echo "REACT_APP_AUTH0_CLIENT_ID=$REACT_APP_AUTH0_CLIENT_ID" >> .env && \
    echo "REACT_APP_AUTH0_CALLBACK_URL=$REACT_APP_AUTH0_CALLBACK_URL" >> .env && \
    echo "REACT_APP_API_SERVER_URL=$REACT_APP_API_SERVER_URL" >> .env && \
    echo "REACT_APP_AUTH0_AUDIENCE=$REACT_APP_AUTH0_AUDIENCE" >> .env

RUN yarn set version 1.22.19

RUN yarn

EXPOSE 3000

RUN yarn build

RUN yarn global add serve

CMD [ "yarn","serve" ]
