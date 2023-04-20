# Azure Functions for Managing AD419 Data

This repository contains Azure Functions for managing AD419 report data.

# Required cli tools
- https://docs.microsoft.com/en-us/cli/azure/
- https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local

# Some useful commands

## Log in and set current subscription
```bash
az login
az account set --subscription <SUBSCRIPTION_ID>
```

## Create Function App (assuming resource group and storage account already exist)
```bash
az functionapp create --resource-group ad419 --consumption-plan-location westus2 --runtime dotnet --functions-version 4 --name AD419Functions --storage-account ad419 --os-type Windows
```

## Run the function locally
```bash
func start
```

## Publish
```bash
func azure functionapp publish AD419Functions
```