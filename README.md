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

# Road Map

1. Configure keyvault
    1. Setup API appsettings and configuration pipeline
    2. Script keyvault secret powershell - and encrypt this before pushing to code repo (place encryption key in vault) with instructions about the name



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
