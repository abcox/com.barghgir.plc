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

# Road Map

1. Configure keyvault
    1. Setup API appsettings and configuration pipeline
    2. Script keyvault secret powershell - and encrypt this before pushing to code repo (place encryption key in vault) with instructions about the name



# References

1. [Material Icons](https://github.com/AathifMahir/MauiIcons)
2. [Converyor extension for VS 2022](https://marketplace.visualstudio.com/items?itemName=vs-publisher-1448185.ConveyorbyKeyoti2022&ssr=false#overview)
3. [.NET MAUI Reference](https://learn.microsoft.com/en-us/dotnet/maui/?view=net-maui-7.0)
    1. Fundamentals
        1. [App Lifecycle](https://learn.microsoft.com/en-us/dotnet/maui/fundamentals/app-lifecycle?view=net-maui-7.0)
    2. User Interface
        1. Controls
            1. [Button](https://learn.microsoft.com/en-us/dotnet/maui/user-interface/controls/button?view=net-maui-7.0)