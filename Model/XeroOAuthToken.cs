using System;
using System.Collections.Generic;

namespace Xero.Net.OAuth2.Authenticator.Model
{
    /// <summary>
    /// Hold the Auth token Returned from Xero
    /// </summary>
    public class XeroAccessToken
    {

        /// <summary>
        /// The AccessToken used for API calls
        /// </summary>
        public string AccessToken { get; set; }
        /// <summary>
        /// The Refresh token required to refresh the AccessToken
        /// </summary>
        public string RefreshToken { get; set; }
        /// <summary>
        /// When the Access Token will expire
        /// </summary>
        public DateTime ExpiresAtUtc { get; set; }
        /// <summary>
        /// Record the Scope used. If the scope is changed on a refresh then force a re-authentication
        /// </summary>
        public string RequestedScopes { get; set; }
    }
}
