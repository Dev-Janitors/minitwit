import { AppState, Auth0Provider } from "@auth0/auth0-react";
import React, {FC, ReactNode} from "react";
import { useNavigate } from "react-router-dom";

interface Auth0ProviderProps {
  children: ReactNode;
}

const Auth0ProviderWithNavigate: FC<Auth0ProviderProps> = ({children}) => {
  const navigate = useNavigate();

  const domain = `${process.env.REACT_APP_AUTH0_DOMAIN}`;
  const clientId = `${process.env.REACT_APP_AUTH0_CLIENT_ID}`;
  const redirectUri = `${process.env.REACT_APP_AUTH0_CALLBACK_URL}`;
  const audience = `${process.env.REACT_APP_AUTH0_AUDIENCE}`;

  const onRedirectCallback = (appState?: AppState) => {
    navigate(appState?.returnTo || window.location.pathname);
  };

  if(!(domain && clientId && redirectUri)){
    return null;
  }

  return (
    <Auth0Provider 
    domain={domain} 
    clientId={clientId} 
    authorizationParams={{redirect_uri: redirectUri, audience: audience }} 
    onRedirectCallback={onRedirectCallback}
    >
      {children}
    </Auth0Provider>
  )
}

export default Auth0ProviderWithNavigate;