# **[OAuthServer.NET](https://github.com/liveordevtrying/oauthserver.net)**<!-- omit in toc -->
OAuthServer.NET is a simple yet scalable OAuth 2.0 provider for the [4 standard types of OAuth 2.0](https://tools.ietf.org/html/rfc6749) (Implicit, Authorization Code, Resource Owner Password, and Client Credentials), [proof key for code exchange (PKSE)](https://tools.ietf.org/html/rfc7636) for native Authorization Code clients, and custom grants. OAuthServer.NET supports local login and 3rd party logins including Google, Facebook, Twitter, Microsoft, and Twitch. OAuthServer.NET is integrated with the Asp.NET Identity system and includes full managemant of accounts including password management and multiple 3rd party logins per account. OAuthServer.NET includes a UI (created in Angular CLI) to easily set up and configure clients. OAuthServer.NET is easily deployed through Docker, NuGet, or can be modified directly from the source https://github.com/liveordevtrying/oauthserver.net. 

[![Image of the OAuthServer.NET Logo](https://pixelhorrorstudios.s3-us-west-2.amazonaws.com/Packages/OAuthServer.NET.png)](https://github.com/liveordevtrying/oauthserver.net)

# **Creating OAuthServer.NET**

There are 3 ways to implement OAuthServer.NET:
* Docker
* NuGet
* Sourcecode
  
OAuthServer.NET includes 2 projects - 1 for the OAuthServer itself and another (OAuthServer.NET - UI) for configuring the OAuthServer.
## **Docker**
***
The quickest way to get started with OAuthServer.NET and OAuthServer.NET - UI is to use the official OAuthServer.NET Docker images.
### **OAuthServer.NET**
You can spin up an unconfigured OAuthServer.NET using Docker. You must set the required environmental variables detailed below. An example of a docker run command for OAuthServer.NET is:

``` docker
docker run -p 80:80 -d -e ConnectionStrings__Database=Server=localhost;Database=OAuthServer.NET;Trusted_Connection=False;User Id=sa;Password=;MultipleActiveResultSets=true -e Admin__Password=Password1! liveordevtrying/oauthserver.net:latest
```

This command can be use with docker-compose as follows:
``` docker-compose
oauthserver.net:
    image: liveordevtrying/oauthserver.net:latest
    hostname: oauthserver.net
    ports:
        - "80:80"
    restart: always
    environment:
        - ConnectionStrings__Database=OAuthServer.NET;Trusted_Connection=False;User Id=sa;Password=;MultipleActiveResultSets=true
        - Admin__Password=Password1!
```

When OAuthServer.NET starts for the first time, the database will be automatically created for the database specified in the connection string.
The following  tags are available for OAuthServer.NET on DockerHub:

> * liveordevtrying/oauthserver.net:latest
> * liveordevtrying/oauthserver.net:arm32v7
> * liveordevtrying/oauthserver.net:ui
> * liveordevtrying/oauthserver.net:ui-arm32v7

If additional tags / platforms are required, please post them as an issue at [the public git repository] for this project or send an email to liveordevtrying@gmail.com and new tags will be added promptly.

### **Environmental Variables**:

#### **Required**
* ConnectionStrings__Database - *string* - **Required**
  * The connection string to your SQL server database. Other databases may work (such as MySql, Postgres, etc) if an appropriate connection string is provided. However, OAuthServer.NET is written primarily for a MS SQL Server database.
  * `Example: Server=localhost;Database=OAuthServer.NET;Trusted_Connection=False;User Id=sa;Password=Password1!;MultipleActiveResultSets=true`
* Admin__Password - *string* - **Required**
  * The password for the admin account used to log into OAuthServer.NET - UI. The password must be at least 7 characters long with at least 1 lowercase character, at least 1 uppercase character, at least 1 number, and at least 1 special character (such as *, %, !, $, etc.).
  * `Example: Password1!`
#### **Optional**
* Admin__Username - *string* - **Optional**
  * The username for the admin account used to log into OAuthServer.NET UI. Defaults to `admin`.
* RequireConfirmedEmail - *boolean* - **Optional**
  * A flag that defaults to false, but when set true, will not allow a user to log in without first clicking on the email confirmation email sent to their inbox. If you set this to true, you *must* implement the interface `IEmailSender` and code in logic to send emails. The `IEmailSender` and implemented class *must* be registered in Startup.cs ConfigureServices for OAuthServer's dependency injection to find the implemented email sender - otherwise after login, user's will be sent to an error page after login specifying that no implementation for `IEmailSender` could be located.
* Show3rdPartyLoginGraphics - *boolean* - **Optional**
  * A flag that defaults to false, but when set true, will display graphics instead of bootstrap buttons for 3rd party providers (assuming 3rd party providers are enabled for the client). 
* OAuthServerCookieExpirationDays - *int* - **Optional**
  * An integer that defaults to 7, this value represents the number of days the cookie generated for the OAuthServer.NET's login page is valid. After expiration, if the user was persistently logged in, they will be forced to re-login.
#### **3rd Party Authorization**
* Facebook__ClientId - *string* - **Optional**
  * To enable Facebook authentication, first login to the Facebook developer portal and create an OAuth 2.0 application for your client. This value should be the client Id assigned to your application.
* Facebook__ClientSecret - *string* - **Optional**
  * This value should be set to the client_secret assigned to your client registered in the Facebook developer portal.
* Twitter__ClientId - *string* - **Optional**
  * To enable Twitter authentication, first login to the Twitter developer portal and create an OAuth 2.0 application for your client. This value should be the client_id assigned to your application.
* Twitter__ClientSecret - *string* - **Optional**
  * This value should be set to the client_secret assigned to your client registered in the Twitter developer portal.
* Google__ClientId - *string* - **Optional**
  * To enable Google authentication, first login to the Google developer portal and create an application. Next registered an OAuth 2.0 set of credentials. This value should be the client_id assigned to your application.
* Google__ClientSecret - *string* - **Optional**
  * This value should be set to the client_secret assigned to your client registered in the Google developer portal.
* Microsoft__ClientId - *string* - **Optional**
  * To enable Microsoft authentication, first login to the Azure Client Application developer page and create a new client. This value should be the client_id assigned to your client.
* Microsoft__ClientSecret - *string* - **Optional**
  * This value should be set to the client_secret assigned to your client registered in the Azure Client Application developer page.
* Twitch__ClientId - *string* - **Optional**
  * To enable Twitch authentication, first login to the Twitch developer portal and create a new client. This value should be the client_id assigned to your client.
* Twitch__ClientSecret - *string* - **Optional**
  * This value should be set to the client_secret assigned to your client registered in the Twitch developer portal.

### **OAuthServer.NET - UI**
Before installing OAuthServer.NET - UI, make sure that OAuthServer.NET has successfully started at least one time and the database was created. OAuthServer.NET UI is designed to interact with OAutherServer's existing database - starting OAuthServer.NET UI will not create a database.

To use the OAuthServer.NET - UI Docker image, you must set the required environmental variables detailed below and set the connection string to the same connection string used for OAuthServer.NET. An example of a docker run command for OAuthServer.NET - UI is:

``` docker
docker run -p 80:80 -d -e ConnectionStrings__Database=Server=localhost;Database=OAuthServer.NET;Trusted_Connection=False;User Id=sa;Password=Password1!;MultipleActiveResultSets=true liveordevtrying/oauthserver.net:ui
```

This command can be use with docker-compose as follows:
``` docker
oauthserver.net:
    image: liveordevtrying/oauthserver.net:ui
    hostname: oauthserver.net
    ports:
        - "80:80"
    restart: always
    environment:
        - ConnectionStrings__Database=Server=localhost;OAuthServer.NET;Trusted_Connection=False;User Id=sa;Password=Password1!;MultipleActiveResultSets=true
```

### **Environmental Variables**:
#### **Required**
* ConnectionStrings__Database - *string* - **Required**
  * The connection string to your SQL server database. This should be the exact same connection string used in OAuthServer.NET.
  * `Example: Server=localhost;Database=OAuthServer.NET;Trusted_Connection=False;User Id=sa;Password=Password1!;MultipleActiveResultSets=true`
## **NuGet**
***
OAuthServer.NET is available on NuGet and easily implemented with extension methods for methods in Startup.cs. OAuthServer.NET - UI is not available on NuGet but can be implemented with Docker (above) or with Source Code (below).
### **OAuthServer.NET**
To install OAuthServer.NET through NuGet, first create a .NET 5 web application without authentication (Figure 1).

0pen the NuGet Package Manager in Visual Studio and issue the following command:

``` nuget
install-package oauthserver.net
```

This will install OAuthServer.NET and dependencies in the new project. Once completed, we need to use the provided extension methods to enable required services and configuration settings.


``` c#
        // This method gets called by the runtime. Use this method to add services to the   container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.StartOAuthServer(new OAuthServerParams
            {
                AdminPassword = Configuration["Admin:Password"],
                AdminUsername = Configuration["Admin:Username"],
                OAuthCookieExpirationDays = int.Parse(Configuration["OAuthServerCookieExpirationDays"]),
                ConnectionString = Configuration["ConnectionStrings:Database"],
                RequireConfirmedEmail = bool.Parse(Configuration["RequireConfirmedEmail"]),
                Show3rdPartyLoginGraphics = bool.Parse(Configuration["Show3rdPartyLoginGraphics"]),
                ExternalProviders = new OAuthServerExternalProviders
                {
                    GoogleClientId = Configuration["Google:ClientId"],
                    GoogleClientSecret = Configuration["Google:ClientSecret"],

                    TwitchClientId = Configuration["Twitch:ClientId"],
                    TwitchClientSecret = Configuration["Twitch:ClientSecret"],

                    FacebookClientId = Configuration["Facebook:ClientId"],
                    FacebookClientSecret = Configuration["Facebook:ClientSecret"],

                    TwitterClientId = Configuration["Twitter:ClientId"],
                    TwitterClientSecret = Configuration["Twitter:ClientSecret"],

                    MicrosoftClientId = Configuration["Microsoft:ClientId"],
                    MicrosoftClientSecret = Configuration["Microsoft:ClientSecret"],
                }
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, OAuthServerServices services)
        {
            app.StartOAuthServer(services);
        }
```
### **OAuthServerParams Parameters**:
#### **Required**
* ConnectionStringe - *string* - **Required**
  * The connection string to your SQL server database. Other databases may work (such as MySql, Postgres, etc) if an appropriate connection string is provided. However, OAuthServer.NET is written primarily for a MS SQL Server database.
  * `Example: Server=localhost;Database=OAuthServer.NET;Trusted_Connection=False;User Id=sa;Password=Password1!;MultipleActiveResultSets=true`
* AdminPassword - *string* - **Required**
  * The password for the admin account used to log into OAuthServer.NET - UI. The password must be at least 7 characters long with at least 1 lowercase character, at least 1 uppercase character, at least 1 number, and at least 1 special character (such as *, %, !, $, etc.).
  * `Example: Password1!`
#### **Optional**
* AdminUsername - *string* - **Optional**
  * The username for the admin account used to log into OAuthServer.NET UI. Defaults to `admin`.
* RequireConfirmedEmail - *boolean* - **Optional**
  * A flag that defaults to false, but when set true, will not allow a user to log in without first clicking on the email confirmation email sent to their inbox. If you set this to true, you *must* implement the interface `IEmailSender` and code in logic to send emails. The `IEmailSender` and implemented class *must* be registered in Startup.cs ConfigureServices for OAuthServer's dependency injection to find the implemented email sender - otherwise after login, user's will be sent to an error page after login specifying that no implementation for `IEmailSender` could be located.
* Show3rdPartyLoginGraphics - *boolean* - **Optional**
  * A flag that defaults to false, but when set true, will display graphics instead of bootstrap buttons for 3rd party providers (assuming 3rd party providers are enabled for the client). 
* OAuthServerCookieExpirationDays - *int* - **Optional**
  * An integer that defaults to 7, this value represents the number of days the cookie generated for the OAuthServer.NET's login page is valid. After expiration, if the user was persistently logged in, they will be forced to re-login.
#### **3rd Party Authorization**
* FacebookClientId - *string* - **Optional**
  * To enable Facebook authentication, first login to the Facebook developer portal and create an OAuth 2.0 application for your client. This value should be the client Id assigned to your application.
* FacebookClientSecret - *string* - **Optional**
  * This value should be set to the client_secret assigned to your client registered in the Facebook developer portal.
* TwitterClientId - *string* - **Optional**
  * To enable Twitter authentication, first login to the Twitter developer portal and create an OAuth 2.0 application for your client. This value should be the client_id assigned to your application.
* TwitterClientSecret - *string* - **Optional**
  * This value should be set to the client_secret assigned to your client registered in the Twitter developer portal.
* GoogleClientId - *string* - **Optional**
  * To enable Google authentication, first login to the Google developer portal and create an application. Next registered an OAuth 2.0 set of credentials. This value should be the client_id assigned to your application.
* GoogleClientSecret - *string* - **Optional**
  * This value should be set to the client_secret assigned to your client registered in the Google developer portal.
* MicrosoftClientId - *string* - **Optional**
  * To enable Microsoft authentication, first login to the Azure Client Application developer page and create a new client. This value should be the client_id assigned to your client.
* MicrosoftClientSecret - *string* - **Optional**
  * This value should be set to the client_secret assigned to your client registered in the Azure Client Application developer page.
* TwitchClientId - *string* - **Optional**
  * To enable Twitch authentication, first login to the Twitch developer portal and create a new client. This value should be the client_id assigned to your client.
* TwitchClientSecret - *string* - **Optional**
  * This value should be set to the client_secret assigned to your client registered in the Twitch developer portal.

### **OAuthServer.NET - UI**
Unfortunately, it is not feasible for OAuthServer.NET-UI to be available on NuGet: to spin up the UI, please refer to the above Docker implementation or the SourceCode implementation below.
## **Source Code**

***

OAuthServer.NET is completely open-source and highly scalable using the patterns found within the sourcecode. Using sourcode, you can easily create newor update existing grant flows and authorization flows. 

First, go to GitHub.com and download / clone the project and extract / pull into a directory of your choosing.

Once you have the sourcecode, you need to create a new .NET 5 (or other specified supported version) web application and add the pulled class library as a reference to the new web application. Following the steps above for NuGet, you need to modify the Startup.cs to use the provided extension methods.

### **Project Structure**
At a high level, the project is structured with 3 controllers - Authorize, Login, and Token. Each controller has its own business logic layer (BLL) containing logic specific for the controller's actions. Each BLL has a reference to a data-access-layer (DAL) which by default is Entity Framework with SQL server. 

#### **Authorize Request and IAuthorizeTypeProvider**
When an authorize request is received, the applicaiton will look for the approriate IAuthorizeTypeProvider that matches the provided grant_type. The grant_type will need to be registered in the OAuthServer.NET - UI prior to being implemented in code. To add a new grant type, create a new class and implement the interface IAuthorizeTypeProvider. The required function, ValidateAsync(HttpRequest request), will contain the logic to follow for the specific grant. 2 authorize type providers are provided by default - AuthorizeTypeProviderAuthorizationCode and AuthorizeTypeProviderImplicit. ValidateAsync() returns a compound result of (bool, string), where the bool is the flag to indicate is validation is successful, and the string represents the error message if the authorization was not successful. 

To create a new Authorization Grant, first log into the OAuthServer.NET - UI, navigate to Grants, and create the new Grant with the requested AuthorizeResponseType. The 2 default registered response types are code and token - make sure to choose an AuthorizeResponseType not already in use.

Next, create a new class in your web application and implement IAuthorizeTypeProvider. You will be required to specify a AuthorizeResponseType (which should exactly match the AuthorizeResponeType specified in the OAuthServer.NET - UI Grant created above), and ValidateAsync(HttpRequest request).  ValidateAsync(HttpRequest request), will contain the logic to follow for the specific grant. 2 authorize type providers are provided by default - AuthorizeTypeProviderAuthorizationCode and AuthorizeTypeProviderImplicit.

**Now, in Startup.cs ConfigureServerices(), add a new trasient type that specifies the interface IAuthorizeResponseType and the implementation of the new class created above.** This is extremely important as the Authorize controller injects an array of all IAuthorizeResponseType[] classes registered in dependency injection, and if you do not register your new class in Startup.cs ConfigureServices(), it will not be found in the Authorize Controller when executing the Authorize request.

Each IAuthorizeTypeProvider will inject an IAuthorizeBLL (AuthorizeBLL) that should contain the logic required for all AuthorizeResponseType implementations. AuthorizeBLL in turn is dependent on a application DAL, which is by default, dependent on Entity Framework, Asp.NET Identity, and SQL Server. 

 ValidateAsync() returns a compound result of (bool, string), where the bool is the flag to indicate if validation is successful, and the string represents the error message if the authorization was not successful. A successful validation request will redirect to the Login page provided by OAuthServer.NET.

#### **Login Request**

#### **Token Request**

***


# **Configuring OAuthServer.NET**

***
# **Using OAuthServer.NET**
OAuthServer.NET is written following the OAuth 2.0 protocol, the PKCE extesions, and allows for custom grants. Once your instance of OAuthServer.NET and OAuthServer.NET-UI are running, it is time to configure your server.

1. In UI, remove non-supported grants or create grants if required.
2. In UI, Create clients.
3. 
# **Table of Contents**<!-- omit in toc -->
- [**Creating OAuthServer.NET**](#creating-oauthservernet)
  - [**Docker**](#docker)
    - [**OAuthServer.NET**](#oauthservernet)
    - [**Environmental Variables**:](#environmental-variables)
      - [**Required**](#required)
      - [**Optional**](#optional)
      - [**3rd Party Authorization**](#3rd-party-authorization)
    - [**OAuthServer.NET - UI**](#oauthservernet---ui)
    - [**Environmental Variables**:](#environmental-variables-1)
      - [**Required**](#required-1)
  - [**NuGet**](#nuget)
    - [**OAuthServer.NET**](#oauthservernet-1)
    - [**OAuthServerParams Parameters**:](#oauthserverparams-parameters)
      - [**Required**](#required-2)
      - [**Optional**](#optional-1)
      - [**3rd Party Authorization**](#3rd-party-authorization-1)
    - [**OAuthServer.NET - UI**](#oauthservernet---ui-1)
  - [**Source Code**](#source-code)
    - [**Project Structure**](#project-structure)
      - [**Authorize Request and IAuthorizeTypeProvider**](#authorize-request-and-iauthorizetypeprovider)
      - [**Login Request**](#login-request)
      - [**Token Request**](#token-request)
- [**Configuring OAuthServer.NET**](#configuring-oauthservernet)
- [**Using OAuthServer.NET**](#using-oauthservernet)
***