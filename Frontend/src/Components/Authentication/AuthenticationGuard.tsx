import { useAuth0, withAuthenticationRequired } from "@auth0/auth0-react";
import React, {FC, ComponentType} from "react";
import { PageLoader } from "../Authentication/PageLoader";

interface AuthenticationGuardProps {
  component: ComponentType;
  returnTo?: string;
}

const AuthenticationGuard: FC<AuthenticationGuardProps> = ({ component, returnTo }) => {
  const Component = withAuthenticationRequired(component, {
    onRedirecting: () => (
      <div className="page-layout">
        <PageLoader />
      </div>
    ),
  });

  return <Component />;
};

export default AuthenticationGuard;