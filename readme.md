[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nightscout-companion-apps&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=brenordv_nightscout-companion-apps)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nightscout-companion-apps&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=brenordv_nightscout-companion-apps)
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nightscout-companion-apps&metric=duplicated_lines_density)](https://sonarcloud.io/summary/new_code?id=brenordv_nightscout-companion-apps)
[![Vulnerabilities](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nightscout-companion-apps&metric=vulnerabilities)](https://sonarcloud.io/summary/new_code?id=brenordv_nightscout-companion-apps)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nightscout-companion-apps&metric=bugs)](https://sonarcloud.io/summary/new_code?id=brenordv_nightscout-companion-apps)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nightscout-companion-apps&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=brenordv_nightscout-companion-apps)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nightscout-companion-apps&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=brenordv_nightscout-companion-apps)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nightscout-companion-apps&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=brenordv_nightscout-companion-apps)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nightscout-companion-apps&metric=coverage)](https://sonarcloud.io/summary/new_code?id=brenordv_nightscout-companion-apps)
[![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nightscout-companion-apps&metric=sqale_index)](https://sonarcloud.io/summary/new_code?id=brenordv_nightscout-companion-apps)
[![CGM Data Display App CI/CD Using Reusable Workflow](https://github.com/brenordv/nightscout-companion-apps/actions/workflows/master-publish-glucose-mon-app.yml/badge.svg)](https://github.com/brenordv/nightscout-companion-apps/actions/workflows/master-publish-glucose-mon-app.yml)
[![Data API Function App CI/CD Workflow Using Template](https://github.com/brenordv/nightscout-companion-apps/actions/workflows/master-publish-dataapi.yml/badge.svg)](https://github.com/brenordv/nightscout-companion-apps/actions/workflows/master-publish-dataapi.yml)
[![Data Transfer Function CI/CD Using Reusable Workflow](https://github.com/brenordv/nightscout-companion-apps/actions/workflows/master-publish-datatransfer.yml/badge.svg)](https://github.com/brenordv/nightscout-companion-apps/actions/workflows/master-publish-datatransfer.yml)

# Nightscout Companion Apps

## Table of Contents
- [Nightscout Companion Apps](#nightscout-companion-apps)
    - Table of Contents
    - [Intro](#intro)
    - [How to get started with Nightscout](#how-to-get-started-with-nightscout)
    - [Context](#context)
    - [Note](#note)
    - [Big picture](#big-picture)
    - [Next steps](#next-steps)
        - [General](#general)
        - [CGM Data Display (Windows Forms App)](#cgm-data-display-windows-forms-app)
        - [Data Api Function](#data-api-function)
- [CGM Data Display (Windows Forms app)](#cgm-data-display)
    - [How to use](#how-to-use)
    - [Known Issues](#known-issues)
    - [Settings](#settings)
- [Azure Function - Data Transfer](#azure-function---data-transfer)
    - [DataTransferFunc](#datatransferfunc)
    - [HbA1cCalcFunc](#hba1ccalcfunc)
- [Azure Function - Data Api](#azure-function---data-api)
    - [DataApiFunc](#dataapifunc)
    - [DataSeriesApiFunc](#dataseriesapifunc)
    - [DataLatestHbA1cFunc](#datalatesthba1cfunc)
- [Disclaimer](#disclaimer)
    - [Licensing and Contributions](#licensing-and-contributions)
    - [Affiliation](#affiliation)
    - [No Medical Advice or Treatment](#no-medical-advice-or-treatment)
    - [No Guarantees or Warranties](#no-guarantees-or-warranties)
    - [Trust Your Body](#trust-your-body)


## Intro
So I finally was able to use a CGM (continuous glucose monitor) and I was able to own the data from it, thanks to 
Nightscout and Azure! <3

In this project we currently have 3 applications:
1. CGM Data Display - An Windows forms application that shows the data from Nightscout in your taskbar.
   - File: `CGMDataDisplayApp_win-x64_<version>.zip`
2. An Azure Function that gets the data from MongoDB and saves it in Azure CosmosDB.
    - File: `AzFnDataTransfer_win-x64_<version>.zip`
3. An Azure Function that serves the data from Azure CosmosDB.
    - File: `AzFnDataApi_win-x64_<version>.zip`

> More about the functions in the context section.

> If you want to know how to use the windows app, there's a section below with all the instructions.

> All apps have a CI/CD pipeline that builds and publishes the apps to GitHub releases. The pipeline is triggered by changes in the `master` branch. 


## How to get started with Nightscout
It is important to say that I'm not affiliated with this awesome project, but if you want to get started, I can point
you in the right direction.

- Official site
  - New Users: https://nightscout.github.io/nightscout/new_user/
  - Uploaders (Is your CGM supported?): https://nightscout.github.io/uploader/uploaders/
 
- [Recommended] Tutorial on making it work in Azure for free: https://www.youtube.com/watch?v=EDADrteGBnY

## Context
While I love MongoDb, I love Azure Cosmos with Core API even more and the data stored in MongoDb is a bit big and with 
a bunch of fields that I'm not going to use. So to really own my data and use it without any collisions, conflicts or 
concerns, I decided to add an extra step in the process: Enter the Azure Functions! 

The data transfer function grabs the data from MongoDb, converts it to an internal format (created in this repo) and
saves it in Azure Cosmos. The data API function is used for me to extract that data.

> Just want to use the windows app? No worries. It also works reading data directly from MongoDb!

My plan is to set the MongoDb to auto delete old documents and keep everything in Azure Cosmos. Since the documents
are smaller, it is going to take a while for me to fill the 25Gb of free storage there.

I know that the Functions (probably the storage account associated with them) will cost me some money, but It's not
going to be that much. (In any case, I've set a budget alert to let me know if I'm spending too much.)

## Note
I created all those apps with a quick and dirty approach. They are not my best work there's lots to improve and I think
they could use some love, but they work. If you see something that could be improved, please let me know or send a PR.
Help and feedback are always welcome!

## Big picture
![Big Picture Diagram](./.readme.imgs/big-picture.jpg)

```uml
package "Azure Functions" {
  [DataTransferFunc] as DataTransferFunc
  [DataApiFunc] as DataApiFunc
  [DataSeriesApiFunc] as DataSeriesApiFunc
  [HbA1cCalcFunc] as HbA1cCalcFunc
}

package "Databases" {
  [MongoDB] as MongoDb
  [Azure CosmosDB] as CosmosDb
}

package "Client Applications" {
  [CGM Data Display] as CGMDataDisplay
  [e-Paper Display] as EPaperDisplay
}

package "External Services" {
  [Nightscout] as Nightscout
  [Dexcom Share] as DexcomShare
}

[DataTransferFunc] --> [MongoDb] : Get Latest Documents
[DataTransferFunc] --> [CosmosDb] : Save Documents

[DataApiFunc] --> [CosmosDb] : Serve Latest Data
[DataSeriesApiFunc] --> [CosmosDb] : Serve Data Series
[HbA1cCalcFunc] --> [CosmosDb] : Calculate and Save HbA1c

[CGMDataDisplay] --> [DataApiFunc] : Request Latest Data
[CGMDataDisplay] --> [DataSeriesApiFunc] : Request Data Series

[EPaperDisplay] --> [DataSeriesApiFunc] : Fetch Data Series
[EPaperDisplay] --> [HbA1cCalcFunc] : Fetch HbA1c Data

[Nightscout] --> [MongoDb] : Save Data
[DexcomShare] --> [Nightscout] : Provide Data
```

This diagram illustrates the system components and their relationships. The Azure Functions consist of a Timer Function
and an HTTP Function. The Timer Function accesses the MongoDB database to get the latest documents and then saves them
in the Azure CosmosDB database. The HTTP Function serves the data stored in the CosmosDB to clients. 
The Nightscout service/app reads data from Dexcom Share and saves it in the MongoDB database.

## Next steps
I have a few plans for all the apps here. The items are in no particular order.

### General
1. Add repo to SonarCloud and fix all the issues;
2. Add more (meaningful) tests.

### CGM Data Display (Windows Forms App)
1. Add tabs to the main form and:
   - Chart: Showing the data collected since the app was opened;
   - Settings: To allow the user to change the settings without having to edit the config file;
   - Logs: To show what the app is doing and help users troubleshoot issues;
2. Add a button to force refreshing the data.
3. Add an option to show different data in the notification icon (like a color depending on the current glucose value or an icon that represents the current trend);
4. General code clean-up.

### Data Api Function
1. Allow to fetch more data at once (like last 24 hours, last week, etc);
2. Add an option pagination, depending on the amount of data requested.
3. General code clean-up.


# CGM Data Display
## How to use
After downloading the zip file and extracting it, you'll need to edit the `appsettings.json` with your data, then
just run the `CGMDataDisplayApp.exe` file and wait. After the refresh time is up, the app will show the data and show it
in the Taskbar and notification area. If you keep it open, it will refresh the data automatically.

> If you don't want to use the compiled app, feel free to clone this repo and build the app.

## Known Issues
Due to the way Windows caches icons, if you create a shortcut to the app (`CGMDataDisplay.exe`) the taskbar the icon 
will not update. To work around that, you can create a shortcut to the launcher (`CGMDataDisplay.Launcher.exe`). 
This launcher will simply start the CGM Data Display app and go away. This will make the taskbar icon update properly.

## Settings
This repo as an `appsettings.sample.json` ([link](Raccoon.Ninja.WForm.GlucoseIcon/appsettings.sample.json)) that can be 
used as a base, but here's how it work:

```json
{
  "General": {
    "RefreshIntervalInMinutes": 5, // Data fresh time in minutes. Nightscout updates every 5 minutes, so we're using the same value.
    "FontFamily": "Arial" // Font family to be used in the taskbar and notification area.
  },
  "DataSource": { // Data source configuration. This is where you configure how the app will get the data.
    "SelectedSource": 2, // 1 - MongoDb, 2 - Azure Functions
    "AzureFunction": { // Only needed if you selected Azure Functions as the data source. (SelectedSource: 2)
      "BaseUrl": "", // Full URL for the Azure Function with the route.
      "ApiKey": "", // The API key for Azure Functions.
      "PostBodyText": { // This is something that I created. It's not native to Azure. It's like an extra key. 
        "Key": "" // Can be anything, as long as it is the same in the Azure Function.
      }
    },
    "MongoDb": { // Only required if you selected MongoDb as the data source. (SelectedSource: 1)
      "ConnectionString": "", // Connection string to your MongoDb database.
      "DatabaseName": "", // Name of the MongoDb database.
      "CollectionName": "entries" // Name of the collection that has the data. You probably want to add the "entries" collection.
    }
  }, // The following sections are here to configure the font size for the taskbar and notification area. I created 1 for Windows 10 and another for Windows 11 because I use this app in both OSes.
  "Win10TaskbarIconOverlayFontConfig": {
    "FirstLineFontSize": 12, // First line of the Taskbar icon. (Glucose value)
    "SecondLineFontSize1Char": 12, // When the second line of the Taskbar icon (Trend) has only 1 character.
    "SecondLineFontSize2Char": 10, // When the second line of the Taskbar icon (Trend) has 2 characters.
    "SecondLineFontSize3Char": 8 // When the second line of the Taskbar icon (Trend) has 3 characters.
  },
  "Win11TaskbarIconOverlayFontConfig": { // Same logic as above, but for Windows 11.
    "FirstLineFontSize": 8,
    "SecondLineFontSize1Char": 10,
    "SecondLineFontSize2Char": 8,
    "SecondLineFontSize3Char": 6
  },
  "Win10NotificationIconFontConfig": { 
    "FirstLineFontSize": 7, // First line of the notification icon. (Glucose value)
    "SecondLineFontSize": 6 // Second line of the notification icon. (Trend)
  },
  "Win11NotificationIconFontConfig": { // Same logic as above, but for Windows 11.
    "FirstLineFontSize": 5,
    "SecondLineFontSize": 4
  }
}
```

![Example of Taskbar Icon 1](./.readme.imgs/cgm-data-display-1.jpeg)
![Example of Taskbar Icon 2](./.readme.imgs/cgm-data-display-2.jpeg)
![Example of Taskbar Icon 3](./.readme.imgs/cgm-data-display-3.jpeg)


# Azure Function - Data Transfer
If you want to use this function, you'll need to set the following environment variables:
- `MongoDbConnectionString`: Connection string to your MongoDb database.
- `MongoDbDatabaseName`: Name of the MongoDb database.
- `MongoDbCollectionName`: Name of the collection that has the data. You probably want to add the "entries" collection.
- `CosmosConnectionString`: Connection string to your Azure CosmosDB database.
- `CosmosDatabaseName`: Name of the Azure CosmosDB database.
- `CosmosContainerName`: Name of the collection that has the data. You probably want to add the "entries" collection.
- `CosmosAggregateContainerName`: Name of the collection that will have the aggregated data. (Like HbA1c calculations)

## DataTransferFunc
This function runs every 5 minutes minutes, gets the latest documents from MongoDb and saves them in Azure CosmosDB.
Besides converting the data to a smaller "native" type, it does not do much else.

It gets this document (what Nightscout generates on MongoDb):
```json
{
    "_id": "1587456b012db3df45678987",
    "sgv":99,
    "date": 1694843348000,
    "dateString":"2023-09-16T05:49:08.000Z",
    "trend": 3,
    "direction":"FortyFiveUp",
    "device":"",
    "type":"sgv",
    "utcOffset": 0,
    "sysTime": "2023-09-16T05:49:08.000Z"
}
```

and converts it to this:
```json
{
  "id": "D034E1C4-357E-4578-8D65-C031C7ED83B7",
  "trend": 4,
  "value": 179,
  "readAt": 1694836148000
}
```

There are no particular reasons for this conversion, other than to save storage space and use a more "native" type (to 
help with future changes in Nightscout that could break the app). The property `trend` directly maps to the `trend` in 
the MongoDb document, but I created an enum so I won't have to save in the doc what it means (property `direction`).

The property `date` (MongoDb) is mapped to `readAt` in my document.

## HbA1cCalcFunc
This function runs daily and tries to get the latest 115 days of data from CosmosDB and calculate the HbA1c value (in 
percentage) based on the data. It then saves the result in CosmosDB.

Formula:
`HbA1c = (Average Blood Glucose + 46.7) / 28.7`

It will calculate if there are enough data or not. If we there's not enough data, the calculation will be marked
as partial. 
In the response only, I've added a field to tell if the calculation is stale or not. A stale calculation is one that is 
more than a day old.

- References:
  - https://www.diabetes.org.uk/guide-to-diabetes/managing-your-diabetes/hba1c
  - https://www.diabetes.co.uk/hba1c-to-blood-sugar-level-converter.html
  - https://www.britannica.com/science/red-blood-cell
  - https://www.ncbi.nlm.nih.gov/pmc/articles/PMC3678251/
  - 


# Azure Function - Data Api
If you want to use this function, you'll need to set the following environment variables:
- `CosmosConnectionString`: Connection string to your Azure CosmosDB database.
- `CosmosDatabaseName`: Name of the Azure CosmosDB database.
- `CosmosContainerName`: Name of the collection that has the data. You probably want to add the "entries" collection.
- `CosmosAggregateContainerName`: Name of the collection that will have the aggregated data. (Like HbA1c calculations)
- `SillySecret`: This is an arbitrary string that the function expects to receive. If you don't send it, the function will return a 401 error. I know it's not the best extra security ever implemented in an application, but I like the idea. If you're going to use and don't want that, feel free to remove it from the implementation.
- `DataSeriesMaxRecords`: Max number of records that can be returned at once by the DataSeriesApiFunc. If not provided, will use 4032 (two weeks worth of data).

## DataApiFunc
This function returns the latest blood sugar reading available in CosmosDB. Pretty simple and straightforward.

## DataSeriesApiFunc
This function returns a series of blood sugar readings available in CosmosDB. It will return the N latest data, sorted 
from newest to oldest. 

## DataLatestHbA1cFunc
This function returns the latest successful and partially successful HbA1c calculations available in CosmosDB.


# Disclaimer
## Licensing and Contributions
This project is open-source and can be freely modified, distributed, or used in any manner you see fit. While there is 
no obligation to do so, keeping the project public and aligned with the goal of helping people is highly encouraged. 
Although the project does not enforce any specific license, a simple acknowledgment or "thanks" would be greatly 
appreciated if you find the project useful or if you improve upon it.

## Affiliation
This project is not officially affiliated with, endorsed by, or connected to Nightscout or any of its subsidiaries or
its affiliates. The project is an independent functionality built on top of the Nightscout platform.

## No Medical Advice or Treatment
The project is intended for informational and educational purposes only. It is not a substitute for professional 
medical advice, diagnosis, or treatment. Always seek the advice of your physician or another qualified healthcare 
provider for any questions you may have regarding a medical condition.

## No Guarantees or Warranties
This project comes with absolutely no guarantees or warranties, either expressed or implied. It is provided "as-is," 
and you use it at your own risk. While the project aims to display blood sugar levels collected through Nightscout, 
there is no guarantee regarding the accuracy, timeliness, or completeness of the data.

## Trust Your Body
If you experience symptoms or conditions that do not correspond with the data displayed by this project, you 
should **always trust your body**. Immediately consult with a healthcare provider for accurate diagnosis and 
appropriate treatment.
