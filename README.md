# Automation Exercise software testing

**Gülsüm Nur Maslak**

## Project Overview

This project tests the Automation Exercise storefront using risk-based manual scenarios, C# browser automation and Postman API checks. It follows the customer journey from product discovery through checkout and reconciles observable data across the browser and REST API.

## Application Under Test

[Automation Exercise](https://automationexercise.com/) is a public practice storefront. The [published UI scenarios](https://automationexercise.com/test_cases) and [API reference](https://automationexercise.com/api_list) provide the baseline.

## Testing Objectives

Verify that the cart preserves product identity, quantity and pricing through sign-in and checkout. Check that account validation supports recovery and that authentication and cart state behave predictably across sessions. Detect API contract changes and compare product name, category and price across layers.

## Test Scope

Scope includes signup, duplicate email, login, logout, discovery and filters, cart arithmetic, checkout, synthetic payment, invoices and documented REST behavior. Manual sessions cover keyboard access, narrow screens and ambiguous input boundaries.

## Technology Stack

C# on .NET 10 LTS, Selenium WebDriver with Selenium Manager, NUnit with native XML and TRX reports, Postman Collection v2.1, Newman with JUnit XML, and GitHub Actions.

## Test Automation Architecture

Each NUnit case starts a fresh Chrome or Firefox session. Page objects encapsulate selectors and explicit waits. The reusable header component models authenticated navigation. Account tests create unique synthetic data, clean up through the documented API and save an attached screenshot on failure. The Newman account lifecycle keeps its generated credentials inside collection scope and deletes the fixture before verifying the post-delete state.

## Test Design Techniques

The [test plan](manual-testing/test-plan.md) explains boundary value analysis, equivalence partitioning, decision tables, state transitions, risk prioritization and exploratory testing. The [traceability matrix](manual-testing/traceability-matrix.md) maps the manual catalogue to the automated suite.

## Test Coverage

| Layer | Implemented coverage |
| --- | --- |
| Manual | 23 designed cases, including the original EC-01 through EC-20 |
| Selenium | 32 implemented cases including parameterized variants across catalogue, cart, account and checkout |
| Newman | 17 requests across the full published API case set and three additional lifecycle or validation checks |

## Project Structure

- `tests/` contains the browser factory and fixture lifecycle, pages, components, test data and NUnit fixtures.
- `manual-testing/` contains the test plan, case catalogue, manual execution log, exploratory charters and traceability.
- `api-testing/` contains the Postman collection and environment, Newman runner and npm lockfile.
- `bug-reports/` contains defect criteria and the reporting template.
- `evidence/` indexes execution results. CI keeps generated reports as workflow artifacts.

## Installation

Install .NET 10 SDK, Node.js 22 and Chrome or Firefox. Selenium Manager resolves the compatible WebDriver when the browser launches.

```sh
dotnet restore AutomationExercise.slnx
dotnet build AutomationExercise.slnx --no-restore
cd api-testing
npm ci
cd ..
```

## Running UI Tests

```sh
dotnet test AutomationExercise.slnx --filter "TestCategory=Smoke"
dotnet test AutomationExercise.slnx --filter "TestCategory=Regression"
dotnet test AutomationExercise.slnx --filter "TestCategory=E2E"
```

Set `QA_BROWSER=firefox` to use Firefox. Set `QA_HEADED=1` for a visible window. PowerShell syntax is `$env:QA_BROWSER = "firefox"`. Set `QA_ARTIFACTS` to an absolute path for failure screenshots. To generate reports, pass `--logger "trx;LogFileName=ui.trx" --results-directory artifacts/ui -- NUnit.TestOutputXml=artifacts/ui`.

## Running API Tests

```sh
cd api-testing
npm test
```

Import both JSON files in Postman and run the full collection in order. Newman saves the JUnit XML and counters under `artifacts/api/`. Each request asserts the HTTP transport status and the JSON `responseCode` separately.

## CI/CD

Pushes to main and pull requests run headless Chrome smoke coverage and the Newman collection in separate jobs. Run the **QA checks** workflow manually for full UI regression with Chrome or Firefox. Download NUnit XML, TRX, failure screenshots and Newman reports from the workflow's artifact list.

## Test Execution Results

The [evidence index](evidence/README.md) records executed results. The [manual execution log](manual-testing/test-execution.md) records the manual status independently.

## Defects and Observations

See [defect triage and reporting](bug-reports/README.md) for confirmed defects, observations and product questions.

## Known Limitations

The suite depends on a shared public test environment. Inspect test output and screenshots for CAPTCHA, advertising overlays, rate limits and network failures. If a challenge blocks execution, record the environment and affected cases. The public interfaces provide the available validation surface. Practice payments use synthetic values.
