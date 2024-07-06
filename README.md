## About the code:

We converted the console app which was getting the access token via client credential non interactively into an interactive mode and acquire the token using PKCE flow.

Originally:
This .NET (C#) console application uses a client secret as its credentials to retrieve an access token that's scoped for the Microsoft Graph API, and then uses that token to access its own application registration information.

The Microsoft Graph response data is then written to the console. This .NET (C#) console app uses [Microsoft Authentication Library (MSAL)](https://github.com/AzureAD/microsoft-authentication-library-for-dotnet).
