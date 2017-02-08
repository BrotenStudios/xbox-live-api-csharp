// -----------------------------------------------------------------------
//  <copyright file="XboxLiveUser.cs" company="Microsoft">
//      Copyright (c) Microsoft. All rights reserved.
//      Internal use only.
//  </copyright>
// -----------------------------------------------------------------------

namespace Microsoft.Xbox.Services
{
    using global::System.Text;
    using global::System.Threading.Tasks;

    public partial class XboxLiveUser
    {
        private const string authHeader = "XBL3.0 x=12183921627189836106;eyJlbmMiOiJBMTI4Q0JDK0hTMjU2IiwiYWxnIjoiUlNBLU9BRVAiLCJjdHkiOiJKV1QiLCJ6aXAiOiJERUYiLCJ4NXQiOiIxZlVBejExYmtpWklFaE5KSVZnSDFTdTVzX2cifQ.EcvFkT8t6LT741xSXRCCGmnfWXX0Z9gToyYy3x4mLwc3dbqhGo0QO-tFUEG9fyP6QmXRJc7k0-cebbR6dnihrSTFVt0_PtkILv5jQZSaX3PyrRJwB6kRjbZG60lSHRYp4gGNlCN0P2fqoWFk97adMaZ2vr66iK6djS-mnFO297w.Hn6j6FPIQh3uT-_SEf1F_w.9AAyQMIoedQyT_gpfEQAdwTZdLTTZo21ynasngRI3O5-Aoc-4r4N_3Gc9GtMKgviUxTirIZQJWksBYmZRuWqVXYJjqcG29wl0_bntekT5U8pNB4MqdBZ_uTju2mGjwRuOSyjXNXsblR40xyqGDvj4Ix63rBHCyT9_kzTkKAaNSeQhlZGwVNMd4iR3P6hKzOegedgnkMGIfiwQth7fCobwFInM2xyfuhigoLNZ_lgfY5GFwap1LDMv8UurFxY4o-tlI6dcMXiYtYeox-Edwn3m4mXiI3mYoL0ow4tmfQP4dfQ3ktLL0N28KG_a-8vMXxcUV6vA-QZSFLxY1fq_9Bz7g-ueZEBx6TV6nEqXfLkckbsUYT6_uzrktKrFCntTFCjlT_PUdROaSAElbIahrhsrpiATrttABMsUjKvGV6SeG8djeUoEaTpQE2dsY7KMxvZqLxqNCu-UNYZIbz7QnFB5ZUhNMWm_C-4It0SdGw6CkEGN2zvFnI7mEtBuLDwLDUFjddyF6dUzGEAarS63TCBpqYpSOTI4mQHqT3gAs2imrM5-JErDr9alc1jJKrZbX1u7bF8giVmkXBfz_kjHi3LD8o2Hmx71LkiVmM8jw3GQRP-5If7yVewdtyo-4X9mgZB4ig4D2B7hRhgFZvfvUf3pgHQ27DXhNjl5SA5X5rBrGYKME-57uhvVuHG4bzVodOEi_X5AS-mft5hcOukIWxTZ6HV0O5fEuHeYM5lmqxxzuPjE-5iaaSzm4-DRgLZATGFqKxP8g8Uq0FUHuFxg0PptTKvC1CAiNbRA-II71fo9S1PdnyRaPRdPNEkTsvzqqWWqmDzKwkFlCyB_4rvuxMJbt7EKtjfut2Kpo2qj_ov6JSBUzDUwHyIynq9MlZjaEb_TAVvhGdxL1HY4JrOOj7agztt3yrJ-p6T2QljW2eX2_MJevqL_VU-USG3E2muVy-EHiy6JK-ds0LYyNnOde9CUtMgrmH6z3BypZMcT2vsZMyMfRLwDjXxYr_20bIw5zxZubXzITypGYRdvCX1sfUZQM5OE8GFfd21_xrmGGR4ckL12QW-xvvqWYE3WPmfjuEFQLNZlqnCKu8mW-98aZVNdxQ9yVobLNMnzjAhvcsY2lxSF9MFiBcN299XAogiu8IeV4w3gUJlKfBqklvnW63vGSZRkFCVORN7LtIFRc86LNGMCzTKmH5J8z98daf5vA0ncc5QBryOAaZEjqPtug1rP2KrgxEKeh_6orFM3y6bL-CxX1LckOmlhoZq-ZiPdWkGiTnAl38ZBzsr6mvAKZQ7ShBT-Hy8zu3fX4YQEQ_-l6qBFmWWCRl78SBJvBbc4iKG4HonS_JSPNDNV4hbwO0u_BwtyiQUjGhi664DtEKaPkDqut3FcVE1EcP5dzMzsU318_nEfN8NIC72HixUsmBaOMsTd9omimbu6J2TP1WGejPrqjsIvkSvgggL9E_hoK-tg8rd0unyFBApfG7GoKVYC9z6RnTYg94T8HohHjCS4HxLKOdAhrzT5nGCnl2K1xwaAT8WJw8UEpQFmtwZ1CCUK8mfFNUUkvkSDfQsk24Hdc10GAKN5agecq27IRVz1hMR34IlKiMe5KjSsGA7FGdH9pDzA-OY3B5I3Ep7l2TbykRPpBcYJZP6sZHdIC3RduiN61WE6Dig6ly5UjPt8gGJn75N0epC7_hH4_gwRL92XwruLDgTp--o2ek0VSDBBXBqCnyZxcswrlnKrsD4fKxT-xO4Wd9q1n03YCiD_4YkVBT5xw6kJeBaNQtk-DsHd8zNKro20MmkE0gOiTJSUgMIdbc2A80Wiq9Iytn45UtOH0TCLHqgyDiBk9Z5AFZeQ7kQEYycC-wWzt20rFMjImF-g2KMTtvL6ho9vqhsD0d6q5zoIuinfBJnS2H2fyUN4yWGsKpj4xen3Uynj0UCaLKJ-mGhCwPuZmVntKOWkQc2FNOGxu76oFiwBomdtrBzWrEKEzsNJ7mwIoLHp3XkfIEUoe2nKs2A7xSgLyf9AYxMyLS6yLr5izSBSjxrtrCKVcVb.a8bq1U27TY3hHpCQVScS2ZCuv6Z7smU7qpoaWVVhNZY";

        public XboxLiveUser() : this("1", "Gamertag")
        {
        }

        public XboxLiveUser(string xuid, string gamertag)
        {
            this.Gamertag = gamertag;
            this.XboxUserId = xuid;
        }

        public Task<SignInResult> SignInAsync()
        {
            return Task.FromResult(new SignInResult(SignInStatus.Success));
        }

        public Task<SignInResult> SignInSilentlyAsync()
        {
            return Task.FromResult(new SignInResult(SignInStatus.Success));
        }

        public Task<SignInResult> SwitchAccountAsync()
        {
            return Task.FromResult(new SignInResult(SignInStatus.Success));
        }

        public Task<GetTokenAndSignatureResult> GetTokenAndSignatureAsync(string httpMethod, string url, string headers)
        {
            return this.GetTokenAndSignatureAsync(httpMethod, url, headers, (byte[])null);
        }

        public Task<GetTokenAndSignatureResult> GetTokenAndSignatureAsync(string httpMethod, string url, string headers, string body)
        {
            return this.GetTokenAndSignatureAsync(httpMethod, url, headers, Encoding.UTF8.GetBytes(body));
        }

        public Task<GetTokenAndSignatureResult> GetTokenAndSignatureAsync(string httpMethod, string url, string headers, byte[] body)
        {
            string[] authHeaderParts = authHeader.Substring(9).Split(';');
            return Task.FromResult(new GetTokenAndSignatureResult
            {
                Gamertag = this.Gamertag,
                XboxUserId = this.XboxUserId,
                XboxUserHash = authHeaderParts[0],
                Token = authHeaderParts[1],
                Signature = "==",
            });
        }
    }
}