import React from 'react'
import ReactDOM from 'react-dom/client'
import './index.css'
import { RouterProvider, createBrowserRouter } from 'react-router-dom'
import About from './components/layout/About.tsx'
import Signin from './components/layout/Signin.tsx'
import App from './App.tsx'
import { OidcProvider } from '@axa-fr/react-oidc'
import Profile from './components/layout/Profile.tsx'

const router = createBrowserRouter([
  {
    path: "/",
    element: <App />,
    children: [
      {
        path: "",
        element: <About />
      },
      {
        path: "/profile",
        element: <Profile />
      },
      {
        path: "/sign-in",
        element: <Signin />
      }
    ]
  },
]);

const configuration = {
    client_id: "interactive",
  redirect_uri: window.location.origin + "/authentication/callback",
  silent_redirect_uri:
    window.location.origin + "/authentication/silent-callback",
  scope: "openid", // offline_access scope allow your client to retrieve the refresh_token
  authority: "https://localhost:5001",
  service_worker_relative_url: "/OidcServiceWorker.js", // just comment that line to disable service worker mode
  service_worker_only: false,
  demonstrating_proof_of_possession: false, 
};

ReactDOM.createRoot(document.getElementById('root')!).render(
  <React.StrictMode>
    <OidcProvider configuration={configuration}>
      <RouterProvider router={router} />
    </OidcProvider>
  </React.StrictMode>,
)
