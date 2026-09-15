# Changelog

All notable changes to this project are documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/), and this project aims to follow
semantic versioning.

## [6.0.0] - 2026-09-07

### Breaking changes

- Consolidated both function apps into a single function app, `Raccoon.Ninja.Fn.GlucoseMonitor`, hosting both
  `DataTransferFunc` and `DataApiFunc`. The new app has its own hostname and its own access keys, so every Data API
  client must repoint to the new host and key. Function names, the HTTP route, and the 5-minute timer schedule are
  unchanged.

### Changed

- Moved `DataApiFunc`, `DataTransferFunc`, and their helpers into `Raccoon.Ninja.Fn.GlucoseMonitor`. The MongoDB read
  the transfer function needs now lives inside that project as internal code.
- `Program.cs` now uses `FunctionsApplication.CreateBuilder(args)` with `ConfigureFunctionsWebApplication()`.
- Raised the default log level to `Warning` in both `host.json` and the isolated worker, keeping `Raccoon.Ninja`
  categories at `Information`. Host-side, `Function`, `Host.Results`, and `Host.Aggregator` stay at `Information` so the
  requests table and portal metrics keep working, and adaptive sampling now also exempts exceptions, not only requests.
- Reviewed every log statement: dropped the two start-up greeting lines and the trace narration; the empty-window log
  is now `No new readings since {timestamp}` at `Information` (was `No documents to transfer` at `Warning`); the
  transfer heartbeat is now `Transferred {Count} readings to Cosmos DB`; the Data API error log now carries the
  `readSince` value.
- Replaced the two publish workflows with one, `master-publish-glucosemonitor.yml`. The publish template now runs
  `dotnet test` from the repo root (which covers every test project under the Microsoft Testing Platform), builds the
  deployment artifact with `dotnet publish`, gates the deploy job to the `master` ref, and makes the deploy job depend
  on the Azure-secret validation.
- Added a repo-root `.editorconfig` and `Roslynator.Analyzers` to the function app and its test project.

### Removed

- Removed the reusable `Raccoon.Ninja.Extensions.MongoDb` class library and its test project. The desktop apps that
  used it are already retired, so only the minimal internal read the transfer function needs remains. The dead
  `GetLatestDocument` extension was dropped.
- Merged the three function and library test projects into one, `Raccoon.Ninja.Fn.GlucoseMonitor.Tests`.

### Security

- Added `local.settings.json` to `.gitignore` and switched the publish step from `dotnet build --output` to
  `dotnet publish`, so the local connection strings and secrets in that file can no longer reach a release artifact.

## [5.0.0] - 2026-09-07

### Breaking changes

- Removed the `DataSeriesApiFunc` and `DataLatestDailyReportFunc` HTTP endpoints from the Data API function app. Any
  client that called those endpoints (for example an e-Paper display reading data series or HbA1c data) stops working
  after this release. The Data API now exposes only `DataApiFunc`, which returns the latest reading or, when a
  `readSince` timestamp is supplied, the readings recorded after it.
- Removed the `StatisticsCalculationFunc` timer function from the Scheduled Tasks function app.
- Retired the CGM Data Display Windows Forms app, its CLI launcher, and the desktop logging library. The app and its
  releases are no longer built or published.

### Changed

- Migrated every project from .NET 8 to .NET 10 (LTS). `global.json` now pins the .NET 10 SDK.
- Upgraded the Azure Functions isolated worker stack: `Microsoft.Azure.Functions.Worker` 1.22.0 to 2.52.0 and
  `Microsoft.Azure.Functions.Worker.Sdk` 1.17.2 to 2.1.0.
- Upgraded `MongoDB.Driver` from 2.26.0 to 3.11.1, picking up the current security fixes on the supported driver line.
- Upgraded `Microsoft.Azure.Functions.Worker.Extensions.CosmosDB` to 4.16.1 and `Newtonsoft.Json` to 13.0.4 (Data API).
- Converted the solution from the classic `.sln` format to the XML `.slnx` format.
- Migrated the test suites from xUnit v2 to xUnit v3 (4.0.0) running on the Microsoft Testing Platform, and replaced
  FluentAssertions with xUnit's built-in assertions. Code coverage now comes from
  `Microsoft.Testing.Extensions.CodeCoverage`.
- Data API logging no longer records the caller IP address, and a broken structured-logging placeholder in the error
  path was removed.

### Fixed

- Resolved the recurring `System.TimeoutException: Timed out waiting for the function start call` failures on
  `DataApiFunc`. The root cause was a coordinator timing issue in the old ASP.NET Core integration package
  (`Worker.Extensions.Http.AspNetCore` 1.3.2); upgrading to 2.1.1 (together with the worker upgrade above) picks up the
  worker-side fix.

### Security

- Pinned every GitHub Action to a full-length commit SHA and moved the pinned actions to their current major versions.
- Hardened the SonarCloud workflow template: removed the `Invoke-Expression` command-string build in favour of a
  direct, argument-array invocation, routed all workflow inputs and the Sonar token through environment variables, and
  pinned the `dotnet-coverage` and `dotnet-sonarscanner` tool installs to explicit versions.
- Hardened the publish workflow template: stopped echoing the function-app name secret, moved the GitHub token off the
  `curl` command lines into environment variables, and applied least-privilege `permissions` (read by default, write
  only on the deploy job).
- Replaced the API-key equality check in the Data API with a constant-time comparison
  (`CryptographicOperations.FixedTimeEquals`).

### Removed

- Dead code left behind by the retirements, including the statistics and HbA1c calculation pipeline, the associated
  entities, enums, constants, converters, models, and their tests.
- Orphaned package references (`Microsoft.Bcl.AsyncInterfaces` from both function apps, `Newtonsoft.Json` from
  `Domain.Core` and the Scheduled Tasks app, the desktop-only `NLog` and `Refit` references).
