# Execution summary — 2026-11-19

## Environment

Windows, .NET SDK 10.0.401, Chrome 157.0.8103.79 headless, Firefox 160.0.1 headless, Node.js 22.13.0 and Newman 6.2.3. The target was the public Automation Exercise site. All account and order data were synthetic.

## API

Newman completed 17 requests and 52 assertions with zero failures. All 17 HTTP responses returned 200. The application response code varied by scenario (200, 201, 400, 404 and 405). The run created a unique account, verified credentials, checked the persisted address update, deleted the account and verified that the old credentials returned application code 404.

Reports are generated at `artifacts/api/newman.xml` and `artifacts/api/summary.json`. The install audit reported 19 findings in the Newman dependency tree (6 moderate, 12 high, 1 critical). This run used the pinned upstream dependency set and the public practice endpoint.

## Browser runs

| Run | Passed | Failed | Evidence |
| --- | --- | --- | --- |
| Initial Chrome baseline | 21 of 28 | 7 | `artifacts/ui/ui.trx` |
| Intermediate Chrome regression | 30 of 32 | 2 | `artifacts/ui/release-chrome/ui.trx` and NUnit XML |
| Firefox smoke | 4 of 4 | 0 | `artifacts/ui/release-firefox/ui.trx` and NUnit XML |
| Final Chrome regression | 32 of 32 | 0 | `artifacts/ui/final-chrome/ui.trx` and NUnit XML |

The initial run passed checkout, order confirmation and invoice download. Test failures involved the H&M fixture, spacing in storefront labels and advertising text inserted inside labels. The intermediate run encountered one culture-dependent heading comparison and one queue-full response from the public site. See [OBS-001](../bug-reports/OBS-001-public-site-delivery.md) for the screenshot and environment details.

The manual test log remains Not Run.
