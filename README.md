# Xero.Net.OAuth2.Authenticator
Authenticate a connection to Xero using OAuth2. Retrieve your Access and Refresh tokens

Using this you can request an authenitcation with Xero using the new OAuth2 protocol.

Supports both PKCE and Code flows (Currently tested with PKCE Only)

An an external browser window will be opened to request the permissions required and will return the required tokens for further access.

You can also refresh the token using this code.

## Inspired By
My Xero..Net wrapper. I felt it might be usefull to have a stand alone oAuth2 token authentication process.

### Create a Xero App first 
You can follow these steps to create your Xero app to allow access to your Xero tenant(s)

* Create a free Xero user account (if you don't have one)
* Use this URL for beta access to oAuth2 [https://developer.xero.com/myapps](https://developer.xero.com/myapps)
* Click "New app" link
* Enter your App name, company url, privacy policy url, and redirect URI (this is your callback url - localhost, etc) I would suggest http://localhost:8888/callback/
* Choose PKCE
* Agree to terms and condition and click "Create App".
* Copy your client id and client secret and save for use later.
* Click the "Save" button. 

## Getting Started
Clone/Download the repository
To restore the packages used from NuGet and you may need to execute in the Nuget package console

``
Update-Package -reinstall
``

### Setup the configuration and Invoke the initial Authorization: PKCE Example :-

```c#
// Setup New Config
XeroConfig = new XeroConfiguration
{
    ClientID = XeroClientID,
    CallbackUri = XeroCallbackUri,
    State = XeroState, // Optional - Not needed for a desktop app
    codeVerifier = null // Code verifier will be generated if empty
};
XeroConfig.AddScope(XeroScope.accounting_all_read);
XeroConfig.StoreReceivedScope = true;
SaveConfig(null);

try
{
    _auth2 = new Xero.Net.OAuth2.Authenticator.oAuth2(XeroConfig);
    XeroAccessToken newToken = _auth2.InitializeoAuth2();


    string accessToken = newToken.AccessToken;
    string refreshToken = newToken.RefreshToken;
    DateTime tokenExpires = newToken.ExpiresAtUtc;

    SaveConfig(newToken);
    UpdateStatus($"AccessToken:{accessToken}");
    UpdateStatus($"RefreshToken:{refreshToken}");
    UpdateStatus($"ExpiresAtUtc:{tokenExpires}");

}
catch (Exception)
{
    // Error happened
    throw;
}
```
### Perform a Token Refresh


```c#
string tokendata = Utils.ReadTextFile("XeroAccessToken.XML");
UpdateStatus($"Loaded Token");
if (!string.IsNullOrEmpty(tokendata))
{
    XeroAccessToken token = Utils.DeSerializeObject<XeroAccessToken>(tokendata);

    _auth2 = new Xero.Net.OAuth2.Authenticator.oAuth2();
    XeroAccessToken newToken = _auth2.RefreshToken(XeroClientID, token, xeroClientSecret);

    string accessToken = newToken.AccessToken;
    string refreshToken = newToken.RefreshToken;
    DateTime tokenExpires = newToken.ExpiresAtUtc;
                
    UpdateStatus($"AccessToken:{accessToken}");
    UpdateStatus($"RefreshToken:{refreshToken}");
    UpdateStatus($"ExpiresAtUtc:{tokenExpires}");
}
```

or you can use the Config to perform a refresh, this uses the client ID and Secret from the config

```c#
string configData = Utils.ReadTextFile("XeroConfig.XML");
UpdateStatus($"Loaded Config Data");
if (!string.IsNullOrEmpty(configData))
{
    XeroConfig = Utils.DeSerializeObject<XeroConfiguration>(configData);

    _auth2 = new Xero.Net.OAuth2.Authenticator.oAuth2(XeroConfig);
    XeroAccessToken newToken = _auth2.RefreshToken();

    string accessToken = newToken.AccessToken;
    string refreshToken = newToken.RefreshToken;
    DateTime tokenExpires = newToken.ExpiresAtUtc;

    UpdateStatus($"AccessToken:{accessToken}");
    UpdateStatus($"RefreshToken:{refreshToken}");
    UpdateStatus($"ExpiresAtUtc:{tokenExpires}");
}
```

There is more information returned from the Auth process (e.g. Authorised Tenants collection). I would suggest saving the entire ReturnedToken object returned from the process

This can be re-loaded to the Config   XeroConfig..XeroApiToken

## Known Issues and Future Updates
* Currently the oAuth2 handles PKCE only. Not tested for Code flow just yet
* The State value used in the OAuth2 process is sent and received but is not checked for validity


## License

MIT License

Copyright (c) 2021 AGenius

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.