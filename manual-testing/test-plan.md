# Test plan

## Scope and priority

Exercise the public storefront and published REST examples with synthetic data. Prioritize cart integrity, authentication, order totals and delivery details. Browser checks cover customer-visible behavior. API checks exercise the published contract and account lifecycle. Payment uses the practice interface. Data reconciliation uses the public API and browser.

Start with a clean browser and the smoke suite. A full run adds account setup, checkout and catalogue variation. Use Firefox for cross-browser comparison. For manual sessions record browser version, viewport and local time.

## Design techniques

| Technique | Application |
| --- | --- |
| Boundary value analysis | EC-17 probes -1, 0, 1 and 2 around the control's minimum. Browser validity and acceptance by the cart are separate observations. |
| Equivalence partitioning | EC-01/16 compare matching and unmatched terms. EC-09 uses valid credentials, an incorrect password and an unknown account. EC-17 investigates blank and fractional inputs. |
| Decision table | Use the checkout decision table to choose a test for each cart and authentication condition. |
| State transition | EC-06/10/18 track cart and authentication across login, logout and independent sessions. API-11/13/12 cover account creation, update and deletion. |
| Risk-based testing | EC-02 and EC-12 validate financial consistency using independently computed expectations. |
| Exploratory testing | Use the [charters](exploratory-testing.md) to investigate quantity handling, keyboard access and session edges. |

## Checkout decision table

| Cart state | Authentication state | Expected action or result | Case |
| --- | --- | --- | --- |
| Contains items | Signed out | Checkout offers registration or login | EC-05 |
| Contains items | Signs in from guest state | The recorded cart lines are preserved | EC-06 |
| Contains items | Signed in | Order review shows the cart lines, address and computed total | EC-11, EC-12 |
| Empty after removing final item | Either | Empty cart state appears without the removed line | EC-22 |

## Execution and triage

Record Pass, Fail, Blocked or Needs clarification for executed cases. Use Not Run for planned cases. Compare a failure with a manual reproduction and a documented expectation before assigning a product defect. Use an observation for shared-site capacity, third-party ads, browser setup or an unresolved expectation. The [execution log](test-execution.md) links to the evidence.
