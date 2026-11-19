# Portfolio roadmap

The portfolio starts with 20 risk-based scenarios. Each phase adds evidence of how risks were selected, tested and reported.

| Phase | Scope | Completion evidence |
| --- | --- | --- |
| 1. Exploratory baseline | Execute EC-01 to EC-04 in one clean browser session. Record product, unit price, quantity and line total. | Four scenario outcomes with timestamp, browser version and evidence references. File a defect report for each confirmed issue. |
| 2. UI regression | Automate EC-01 to EC-04 with Playwright. Add guest checkout and cart continuity after sign-in once the baseline is stable. | Each automated test references a scenario ID and runs independently. CI retains traces and screenshots on failure. |
| 3. API and cross-layer checks | For EC-20, cover catalogue retrieval, a valid search and a search with the required parameter omitted. Compare the observed HTTP status with the application response code. Check that a chosen product agrees with the UI. | A documented contract with assertions for response structure, expected codes and matching product fields. |
| 4. Checkout and usability | Execute checkout, invoice, session isolation and keyboard investigations. Use a dedicated synthetic account and document any open product questions separately. | Checkout evidence, relevant defect reports and recorded keyboard and narrow-screen findings. |
| 5. CI and portfolio review | Run a small smoke suite on pull requests and provide a manual workflow for broader regression. Map implemented tests and evidence to scenario IDs. | A GitHub Actions run with a downloadable report and a README that explains coverage, execution and open findings. |

## First session

1. Open a fresh browser context and record the environment in [the session record](session-record.md).
2. Execute EC-01 through EC-04 from the [scenario matrix](test-cases.md). Reuse the selected product and observed price across the steps.
3. Calculate the expected line total independently. Record the cart values before and after removing an item and refreshing.
4. Assign a result to each scenario. Use Needs clarification when the expected behavior requires a product decision.
5. Use the observations to choose stable assertions for the first Playwright suite.

Automation should use isolated browser contexts and synthetic test data. Check evidence for credentials and personal details before committing it.
