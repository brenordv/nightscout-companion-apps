[![Quality gate status](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nightscout-companion-apps&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=brenordv_nightscout-companion-apps)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nightscout-companion-apps&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=brenordv_nightscout-companion-apps)
[![Duplicated Lines (%)](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nightscout-companion-apps&metric=duplicated_lines_density)](https://sonarcloud.io/summary/new_code?id=brenordv_nightscout-companion-apps)
[![Security issues](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nightscout-companion-apps&metric=software_quality_security_issues)](https://sonarcloud.io/summary/new_code?id=brenordv_nightscout-companion-apps)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nightscout-companion-apps&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=brenordv_nightscout-companion-apps)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nightscout-companion-apps&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=brenordv_nightscout-companion-apps)
[[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nightscout-companion-apps&metric=coverage)](https://sonarcloud.io/summary/new_code?id=brenordv_nightscout-companion-apps)]([![Coverage](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nightscout-companion-apps&metric=coverage)](https://sonarcloud.io/summary/new_code?id=brenordv_nightscout-companion-apps))
[![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=brenordv_nightscout-companion-apps&metric=sqale_index)](https://sonarcloud.io/summary/new_code?id=brenordv_nightscout-companion-apps)
[![Glucose Monitor Function App CI/CD](https://github.com/brenordv/nightscout-companion-apps/actions/workflows/master-publish-glucosemonitor.yml/badge.svg)](https://github.com/brenordv/nightscout-companion-apps/actions/workflows/master-publish-glucosemonitor.yml)

# Nightscout Companion Apps

## Table of Contents

- [Intro](#intro)
- [How to get started with Nightscout](#how-to-get-started-with-nightscout)
- [Context](#context)
- [Note](#note)
- [Big picture](#big-picture)
- [MongoDB support](#mongodb-support)
- [Next steps](#next-steps)
- [Environment variables](#environment-variables)
- [Functions](#functions)
    - [DataTransferFunc](#datatransferfunc)
    - [DataApiFunc](#dataapifunc)
- [Disclaimer](#disclaimer)
    - [Licensing and Contributions](#licensing-and-contributions)
    - [Affiliation](#affiliation)
    - [No Medical Advice or Treatment](#no-medical-advice-or-treatment)
    - [No Guarantees or Warranties](#no-guarantees-or-warranties)
    - [Trust Your Body](#trust-your-body)

## Intro

So I finally was able to use a CGM (continuous glucose monitor) and I was able to own the data from it, thanks to
Nightscout and Azure! <3

This project is one Azure Function app (`Raccoon.Ninja.Fn.GlucoseMonitor`) with two functions:

1. A timer function that gets the data from MongoDB and saves it in Azure CosmosDB.
2. An HTTP function that serves the data from Azure CosmosDB.

Both ship in a single artifact: `AzFnGlucoseMonitor_win-x64_<version>.zip`.

> More about the functions in the context section.

> The function app has a CI/CD pipeline that builds and publishes it to GitHub releases and deploys it to Azure.
> The pipeline is triggered by changes in the `master` branch.

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

My plan is to set the MongoDb to auto delete old documents and keep everything in Azure Cosmos. Since the documents
are smaller, it is going to take a while for me to fill the 25Gb of free storage there.

I know that the Functions (probably the storage account associated with them) will cost me some money, but It's not
going to be that much. (In any case, I've set a budget alert to let me know if I'm spending too much.)

## Note

I created these apps with a quick and dirty approach. There's still lots to improve, but they work. If you see
something that could be improved, please let me know or send a PR. Help and feedback are always welcome!

## Big picture

```uml
package "Azure Function App: GlucoseMonitor" {
  [DataTransferFunc] as DataTransferFunc
  [DataApiFunc] as DataApiFunc
}

package "Databases" {
  [MongoDB] as MongoDb
  [Azure CosmosDB] as CosmosDb
}

package "Clients" {
  [HTTP Client] as HttpClient
}

package "External Services" {
  [Nightscout] as Nightscout
  [Dexcom Share] as DexcomShare
}

[DexcomShare] --> [Nightscout] : Provide Data
[Nightscout] --> [MongoDb] : Save Data

[DataTransferFunc] --> [MongoDb] : Get Latest Documents
[DataTransferFunc] --> [CosmosDb] : Save Documents

[DataApiFunc] --> [CosmosDb] : Read Data
[HttpClient] --> [DataApiFunc] : Request Data
```

This diagram illustrates the system components and their relationships. Nightscout reads data from Dexcom Share and
saves it in MongoDB. The timer-triggered `DataTransferFunc` gets the latest documents from MongoDB and saves them in
Azure CosmosDB. The HTTP-triggered `DataApiFunc` serves the data stored in CosmosDB to clients. Both functions run in
the same function app.

## MongoDB support

This repo used to ship a general-purpose, reusable MongoDB integration library. That's gone now. The desktop apps that
relied on it were retired, so the only MongoDB code left is the small read `DataTransferFunc` does to pull new entries,
and it lives inside the function app as internal code.

If you want the broader MongoDB support back, open an issue or get in touch and I can bring it back.

## Next steps

I have a few plans for these functions, in no particular order.

1. Add more (meaningful) tests.
2. For the Data API, allow fetching more data at once (like the last 24 hours or the last week) and add pagination
   depending on the amount of data requested.
3. General code clean-up.

# Environment variables

The function app needs these environment variables. On Azure they are application settings; for local runs they go in
`local.settings.json` (which is gitignored and must never be committed).

- `CosmosConnectionString`: Connection string to your Azure CosmosDB database.
- `CosmosDatabaseName`: Name of the Azure CosmosDB database.
- `CosmosContainerName`: Name of the container that holds the data. You probably want the "entries" container.
- `MongoDbConnectionString`: Connection string to your MongoDb database.
- `MongoDbDatabaseName`: Name of the MongoDb database.
- `MongoDbCollectionName`: Name of the collection that has the data. You probably want the "entries" collection.
- `SillySecret`: An arbitrary string the Data API expects in the request body. If you don't send it, the function
  returns a 401 error. I know it's not the best extra security ever implemented in an application, but I like the idea.
  If you're going to use it and don't want that, feel free to remove it from the implementation.

# Functions

## DataTransferFunc

This function runs every 5 minutes, gets the latest documents from MongoDb and saves them in Azure CosmosDB.
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

## DataApiFunc

This function serves blood sugar readings from CosmosDB. By default it returns the latest reading available. If the
request includes a `readSince` query parameter (a Unix timestamp in milliseconds), it instead returns every reading
recorded after that moment, sorted from newest to oldest.

The request body must include the shared secret described above:

```json
{
  "key": "your-silly-secret"
}
```

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
