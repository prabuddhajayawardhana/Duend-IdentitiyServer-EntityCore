import { OidcSecure, useOidcAccessToken } from "@axa-fr/react-oidc";

function Profile() {
    const { accessToken, accessTokenPayload } = useOidcAccessToken();

    if (!accessToken) {
      return <p>you are not authentified</p>;
    }
    return (
      <OidcSecure>
        <div className="card text-white bg-info mb-3">
          <div className="card-body">
            <h5 className="card-title">Access Token</h5>
            <p style={{ color: "red", backgroundColor: "white" }}>
              Please consider to configure the ServiceWorker in order to protect
              your application from XSRF attacks. ""access_token" and
              "refresh_token" will never be accessible from your client side
              javascript.
            </p>
            {<p className="card-text">{JSON.stringify(accessToken)}</p>}
            {accessTokenPayload != null && (
              <p className="card-text">{JSON.stringify(accessTokenPayload)}</p>
            )}
          </div>
        </div>
      </OidcSecure>
    );
}

export default Profile