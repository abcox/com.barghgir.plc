# Powerful Life Coaching Platform Solution

The Powerful Life Coaching Platform Solution is sponsored by [Solmaz Barghgir of Powerful Life Coaching](http://barghgir.com/)

## Projects

There are 2 projects that deliver the solution. They are:

1. Clients are .NET MAUI work in project [com.barghgir.plc.web](https://github.com/abcox/com.barghgir.plc/tree/master/com.barghgir.plc.web)
2. API is .NET 6 work in project [com.barghgir.plc.api](https://github.com/abcox/com.barghgir.plc/tree/master/com.barghgir.plc.api)

## Developer Notes

1. [Connect to local web services from Android emulators and iOS simulators](https://learn.microsoft.com/en-us/dotnet/maui/data-cloud/local-web-services?view=net-maui-7.0)
2. [Keyvault Setup](https://learn.microsoft.com/en-us/azure/key-vault/general/tutorial-net-create-vault-azure-web-app)
    1. Assign identity to web app (api)
        ```
        az webapp identity assign --name "<your-webapp-name>" --resource-group "myResourceGroup"
        ```
3. Scaffold model from database
    ```
    dotnet-ef dbcontext scaffold '<connection-string>' Microsoft.EntityFrameworkCore.SqlServer -o Models/Test3
    ```
    Note: use single-quotes around connection string to fix "Login failed for user '< some-sql-username >'"

4. Entity Framework Core
    - Managing Database Schemas / [Reverse Engineering](https://learn.microsoft.com/en-us/ef/core/managing-schemas/scaffolding/?tabs=dotnet-core-cli)
    - Miscellaneous / [Connection Strings](https://learn.microsoft.com/en-us/ef/core/miscellaneous/connection-strings)

5. Azure
    - Login:  `Login-AzAccount`

# Road Map

1. Configure keyvault
    1. Setup API appsettings and configuration pipeline
    2. Script keyvault secret powershell - and encrypt this before pushing to code repo (place encryption key in vault) with instructions about the name


# Deployment

1. Create and configure App Service resource
    - to provide authority to keyvault, copy [secret value (Secret ID: 740ede77-1af8-421b-9aab-76877ff4ab14)](https://portal.azure.com/?feature.msaljs=false#view/Microsoft_AAD_RegisteredApps/ApplicationMenuBlade/~/Credentials/appId/56c4326a-6c85-45e7-932a-050350068559/isMSAApp~/false) to Settings / Configuration value named `AZURE_CLIENT_SECRET`, and
    - set up keyvault Access policy for the application (i.e. [com.barghgir.plc](https://portal.azure.com/?feature.msaljs=false#view/Microsoft_AAD_RegisteredApps/ApplicationMenuBlade/~/Overview/appId/56c4326a-6c85-45e7-932a-050350068559/isMSAApp~/false))
2. Publish API from project


# Troubleshooting

## Azure

- [Enterprise Application (cca-managed-ident-01)](https://portal.azure.com/?feature.msaljs=false#view/Microsoft_AAD_IAM/ManagedAppMenuBlade/~/Overview/objectId/98330525-9c64-4fd9-a47d-48b2de7618c8/appId/031f1b49-6dca-4132-8539-90b8fb54e149)
- [App barghgir-cca-236217f7-0ad4-4dd6-8553-dc4b574fd2c5](https://portal.azure.com/?feature.msaljs=false#view/Microsoft_AAD_RegisteredApps/ApplicationMenuBlade/~/Credentials/appId/dd1f0fb0-ef23-4188-8fdb-758a763028e4/isMSAApp~/false)

1. Initial attempt to configure KeyVault (Name: cca-cc-rg-01-kv) raised exception:
   
   NOTES: followed [Azure Key Vault configuration provider in ASP.NET Core](https://learn.microsoft.com/en-us/aspnet/core/security/key-vault-configuration?view=aspnetcore-7.0)
    ```
    Azure.Identity.CredentialUnavailableException: 'DefaultAzureCredential failed to retrieve a token from the included credentials. See the troubleshooting guide for more information. https://aka.ms/azsdk/net/identity/defaultazurecredential/troubleshoot
    - EnvironmentCredential authentication unavailable. Environment variables are not fully configured. See the troubleshooting guide for more information. https://aka.ms/azsdk/net/identity/environmentcredential/troubleshoot
    - ManagedIdentityCredential authentication unavailable. Multiple attempts failed to obtain a token from the managed identity endpoint.
    - Process "C:\Program Files\Microsoft Visual Studio\2022\Professional\Common7\IDE\CommonExtensions\Microsoft\Asal\TokenService\Microsoft.Asal.TokenService.exe" has failed with unexpected error: TS003: Error, TS001: This account 'adam.cox@ceridian.com' needs re-authentication. Please go to Tools->Options->Azure Services Authentication, and re-authenticate the account you want to use..
    - Stored credentials not found. Need to authenticate user in VSCode Azure Account. See the troubleshooting guide for more information. https://aka.ms/azsdk/net/identity/vscodecredential/troubleshoot
    - Please run 'az login' to set up account
    - PowerShell is not installed.'
    ```
    Incorrect account (adam.cox@ceridian.com) was selected. After selected correct account (adam.cox@vorba.com), the following exception message was presented:

    ```
    Azure.Identity.CredentialUnavailableException: 'DefaultAzureCredential failed to retrieve a token from the included credentials. See the troubleshooting guide for more information. https://aka.ms/azsdk/net/identity/defaultazurecredential/troubleshoot
    - EnvironmentCredential authentication unavailable. Environment variables are not fully configured. See the troubleshooting guide for more information. https://aka.ms/azsdk/net/identity/environmentcredential/troubleshoot
    - ManagedIdentityCredential authentication unavailable. Multiple attempts failed to obtain a token from the managed identity endpoint.
    - Process "C:\Program Files\Microsoft Visual Studio\2022\Professional\Common7\IDE\CommonExtensions\Microsoft\Asal\TokenService\Microsoft.Asal.TokenService.exe" has failed with unexpected error: TS003: Error, TS004: Unable to get access token.  'AADSTS50020: User account '{EmailHidden}' from identity provider 'live.com' does not exist in tenant 'Microsoft Services' and cannot access the application '872cd9fa-d31f-45e0-9eab-6e460a02d1f1'(Visual Studio) in that tenant. The account needs to be added as an external user in the tenant first. Sign out and sign in again with a different Azure Active Directory user account.
    Trace ID: b39c71cb-93f1-4e5b-ab1a-fc1020af6c00
    Correlation ID: be90d10c-5361-45bc-b38a-7aeb26b76568
    Timestamp: 2023-01-17 19:15:55Z'.
    - Stored credentials not found. Need to authenticate user in VSCode Azure Account. See the troubleshooting guide for more information. https://aka.ms/azsdk/net/identity/vscodecredential/troubleshoot
    - Please run 'az login' to set up account
    - PowerShell is not installed.'
    ```
    The log after successfully using the Connected Services dialog to connect Azure Key Vault:
    ```
    Verifying that your application running locally will have access to the key vault...
    Connecting to Azure Key Vault dependency secrets1 in the project...
    Configuring settings files...
    Adding settings to C:\Users\adam\source\repos\com.barghgir.plc\com.barghgir.plc.api\Properties\launchSettings.json...
    Adding profiles/com.barghgir.plc.api/environmentVariables/VaultUri to C:\Users\adam\source\repos\com.barghgir.plc\com.barghgir.plc.api\Properties\launchSettings.json...
    Adding profiles/com.barghgir.plc.api/environmentVariables/AZURE_USERNAME to C:\Users\adam\source\repos\com.barghgir.plc\com.barghgir.plc.api\Properties\launchSettings.json...
    Adding profiles/IIS Express/environmentVariables/VaultUri to C:\Users\adam\source\repos\com.barghgir.plc\com.barghgir.plc.api\Properties\launchSettings.json...
    Adding profiles/IIS Express/environmentVariables/AZURE_USERNAME to C:\Users\adam\source\repos\com.barghgir.plc\com.barghgir.plc.api\Properties\launchSettings.json...
    Skipping secrets modification, store is not specified...
    Installing NuGet packages to project...
    Installing package 'Azure.Identity' with version '1.6.0'.
    Installing package 'Azure.Extensions.AspNetCore.Configuration.Secrets' with version '1.0.0'.
    Skipping package 'Azure.Extensions.AspNetCore.Configuration.Secrets', same version or a newer version is already installed.
    Uninstalling package 'Microsoft.AspNetCore.AzureKeyVault.HostingStartup'.
    Uninstalling package 'Azure.Extensions.Configuration.Secrets'.
    Inserting code...
    Inserting code...
    Serializing new Azure Key Vault dependency metadata to disk...
    Generating ARM template...
    SuccessComplete. Azure Key Vault secrets1 is configured.
    ```

2. `HTTP Error 500.30 - ASP.NET Core app failed to start`
    - review logs and appsettings


# References

1. [Material Icons](https://github.com/AathifMahir/MauiIcons)
2. [Converyor extension for VS 2022](https://marketplace.visualstudio.com/items?itemName=vs-publisher-1448185.ConveyorbyKeyoti2022&ssr=false#overview)
3. [Configure Applications with App Configuration and Key Vault](https://learn.microsoft.com/en-us/samples/azure/azure-sdk-for-net/app-secrets-configuration/?tabs=visualstudio)
    1. [What is Azure App Configuration?](https://learn.microsoft.com/en-us/azure/azure-app-configuration/overview)
4. [.NET MAUI Reference](https://learn.microsoft.com/en-us/dotnet/maui/?view=net-maui-7.0)
    1. Fundamentals
        1. [App Lifecycle](https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/app-lifecycle?view=net-maui-7.0)
    2. User Interface
        1. Controls
            1. [Button](https://learn.microsoft.com/en-us/dotnet/maui/user-interface/controls/button?view=net-maui-7.0)
5. Azure
    1. Directories (name / domain / tenant id / primary SPN):
        1. Vorba / Vorba.onmicrosoft.com / a7f7a08d-4e79-4d3d-812f-10bd18abbcfb / admin@vorba.onmicrosoft.com (adam.cox@vorba.com)
            1. Tenant Id: 23fec218-d39a-4797-8127-88d714aeeea9
            2. Subscriptions: 236217f7-0ad4-4dd6-8553-dc4b574fd2c5 (PAYG)
        2. Default / adamcoxvorba@onmicrosoft.com / a580b8ea-c147-4717-8195-427f582329b2 /  adam@adamcox.net (adam_adamcox.net#EXT#@adamadamcox.onmicrosoft.com)
            1. Tenant Id: df86b8aa-8ee7-48d7-b0cc-5c674984b2a3
            2. Subscriptions: 6a1e4df2-a791-4357-981e-69d3332eb767
    2. Subscriptions:
        1. 236217f7-0ad4-4dd6-8553-dc4b574fd2c5
    3. Managed Identity: [barghgir-cca-236217f7-0ad4-4dd6-8553-dc4b574fd2c5](https://portal.azure.com/#view/Microsoft_AAD_IAM/ManagedAppMenuBlade/~/Overview/objectId/994c8efd-3e69-4da1-85d6-20aeda3d3cbb/appId/dd1f0fb0-ef23-4188-8fdb-758a763028e4)
