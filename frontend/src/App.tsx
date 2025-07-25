// App.tsx
import React from 'react';
import { ReactKeycloakProvider } from '@react-keycloak/web';
import Keycloak, { KeycloakConfig, KeycloakInitOptions } from 'keycloak-js';
import ReportPage from './components/ReportPage';

const keycloakConfig: KeycloakConfig = {
  url:   process.env.REACT_APP_KEYCLOAK_URL!,
  realm: process.env.REACT_APP_KEYCLOAK_REALM!,
  clientId: process.env.REACT_APP_KEYCLOAK_CLIENT_ID!,
};

const keycloakInitOptions: KeycloakInitOptions = {
  pkceMethod: 'S256',
  onLoad:     'login-required',
  redirectUri: window.location.origin + '/',
  checkLoginIframe: false,
};

const keycloak = new Keycloak(keycloakConfig);

const App: React.FC = () => (
  <ReactKeycloakProvider
    authClient={keycloak}
    initOptions={keycloakInitOptions}
  >
    <ReportPage />
  </ReactKeycloakProvider>
);

export default App;
