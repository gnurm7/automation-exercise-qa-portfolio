# E-commerce test scenarios

The manual status starts at **Not Run** for every case. Record outcomes and evidence in the [execution log](test-execution.md). A product question remains open until there is a usable test oracle.

## EC-01 Search and open product

- **Priority** High
- **Execution mode** Manual and automated coverage as mapped
- **Prerequisites** Fresh browser on Products.
- **Test data** Search term: top.
- **Steps** Open Products, search `top`, open a result.
- **Expected result** Results match the term and the selected product detail identifies the same product; published UI case 9.
- **Actual result** Pending manual execution.
- **Status** Not Run.
- **Evidence** Pending.

## EC-02 Quantity and line total

- **Priority** High
- **Execution mode** Manual and automated coverage as mapped
- **Prerequisites** Blue Top detail open; cart initially empty.
- **Test data** Blue Top, quantity 3, displayed unit price.
- **Steps** Record unit price, choose quantity 3 on the detail page, add to cart.
- **Expected result** Cart has the same product and unit price, quantity 3 and line total equal to price × 3; UI case 13.
- **Actual result** Pending manual execution.
- **Status** Not Run.
- **Evidence** Pending.

## EC-03 Two distinct cart lines

- **Priority** High
- **Execution mode** Manual and automated coverage as mapped
- **Prerequisites** One recorded cart line.
- **Test data** Two different products, such as Blue Top and Men Tshirt.
- **Steps** Continue shopping, add a different product, return to cart.
- **Expected result** Both product lines, prices and quantities agree with the selections; UI case 12.
- **Actual result** Pending manual execution.
- **Status** Not Run.
- **Evidence** Pending.

## EC-04 Remove item and refresh

- **Priority** High
- **Execution mode** Manual and automated coverage as mapped
- **Prerequisites** Two recorded cart lines.
- **Test data** Remove first item; keep second item's values.
- **Steps** Remove one product and refresh the cart.
- **Expected result** Removed product stays absent and the other line stays correct; UI case 17.
- **Actual result** Pending manual execution.
- **Status** Not Run.
- **Evidence** Pending.

## EC-05 Guest checkout

- **Priority** High
- **Execution mode** Manual and automated coverage as mapped
- **Prerequisites** Signed-out browser with an item in cart.
- **Test data** Any product.
- **Steps** With a guest cart, choose checkout.
- **Expected result** Registration/login prompt appears; UI case 15.
- **Actual result** Pending manual execution.
- **Status** Not Run.
- **Evidence** Pending.

## EC-06 Cart after login

- **Priority** High
- **Execution mode** Manual and automated coverage as mapped
- **Prerequisites** Guest browser with an item and an existing synthetic account.
- **Test data** Synthetic login and recorded cart lines.
- **Steps** Log in with the test account after adding guest products.
- **Expected result** Previously added products remain available with correct values; UI case 20.
- **Actual result** Pending manual execution.
- **Status** Not Run.
- **Evidence** Pending.

## EC-07 Account registration

- **Priority** High
- **Execution mode** Manual and automated coverage as mapped
- **Prerequisites** Fresh browser, unused synthetic email.
- **Test data** Unique example.com email and synthetic registration form values.
- **Steps** Register a unique synthetic account, continue to the application.
- **Expected result** Account creation succeeds and the correct display name appears; UI case 1.
- **Actual result** Pending manual execution.
- **Status** Not Run.
- **Evidence** Pending.

## EC-08 Duplicate email

- **Priority** Medium
- **Execution mode** Manual and automated coverage as mapped
- **Prerequisites** Synthetic account already exists.
- **Test data** Reuse its email with the signup name.
- **Steps** Try registration with that account's email again.
- **Expected result** Duplicate email error appears; UI case 5.
- **Actual result** Pending manual execution.
- **Status** Not Run.
- **Evidence** Pending.

## EC-09 Invalid and valid login

- **Priority** High
- **Execution mode** Manual and automated coverage as mapped
- **Prerequisites** Existing synthetic account.
- **Test data** Incorrect and correct password for the same account.
- **Steps** Enter an incorrect password, then retry with correct credentials.
- **Expected result** Invalid login gives an error; valid retry signs in as the test user; UI cases 2–3.
- **Actual result** Pending manual execution.
- **Status** Not Run.
- **Evidence** Pending.

## EC-10 Logout session

- **Priority** High
- **Execution mode** Manual and automated coverage as mapped
- **Prerequisites** Signed-in browser.
- **Test data** Existing synthetic account; back, refresh and new tab.
- **Steps** Log out, refresh, navigate back and open a new tab.
- **Expected result** Record authenticated navigation and page behaviour. Product question concerns access to protected pages after logout; UI case 4 covers return to login.
- **Actual result** Pending manual execution.
- **Status** Not Run.
- **Evidence** Pending.

## EC-11 Checkout address consistency

- **Priority** High
- **Execution mode** Manual and automated coverage as mapped
- **Prerequisites** Signed-in synthetic user and item in cart.
- **Test data** Recorded registration address and phone.
- **Steps** Compare delivery and billing address at checkout with registration data.
- **Expected result** Address fields agree with the account inputs; UI case 23.
- **Actual result** Pending manual execution.
- **Status** Not Run.
- **Evidence** Pending.

## EC-12 Order review totals and comment

- **Priority** High
- **Execution mode** Manual and automated coverage as mapped
- **Prerequisites** Signed-in synthetic user and item in cart.
- **Test data** Recorded cart lines and a synthetic comment.
- **Steps** Review order lines, totals and a synthetic order comment.
- **Expected result** Product names, quantities, prices and totals agree with the cart; UI case 16.
- **Actual result** Pending manual execution.
- **Status** Not Run.
- **Evidence** Pending.

## EC-13 Practice payment

- **Priority** High
- **Execution mode** Manual and automated coverage as mapped
- **Prerequisites** Order review complete.
- **Test data** Practice-only card form values and synthetic cardholder.
- **Steps** Complete the practice payment form with synthetic values.
- **Expected result** Confirmation appears; compare any displayed order details with review; UI case 16.
- **Actual result** Pending manual execution.
- **Status** Not Run.
- **Evidence** Pending.

## EC-14 Invoice

- **Priority** Medium
- **Execution mode** Manual and automated coverage as mapped
- **Prerequisites** Completed synthetic order.
- **Test data** Order total and synthetic customer name.
- **Steps** Download invoice after an order and inspect its content.
- **Expected result** Invoice downloads; record and compare available identity and amount fields; UI case 24.
- **Actual result** Pending manual execution.
- **Status** Not Run.
- **Evidence** Pending.

## EC-15 Category and brand filters

- **Priority** Medium
- **Execution mode** Manual and automated coverage as mapped
- **Prerequisites** Fresh browser on Products.
- **Test data** Women > Dress, Men > Tshirts; Polo and H&M.
- **Steps** Filter a category, then a brand; open a product.
- **Expected result** Selected category/brand and displayed products agree; UI cases 18–19.
- **Actual result** Pending manual execution.
- **Status** Not Run.
- **Evidence** Pending.

## EC-16 Empty search recovery

- **Priority** Medium
- **Execution mode** Manual and automated coverage as mapped
- **Prerequisites** Fresh browser on Products.
- **Test data** Unique unmatched term, then Blue Top.
- **Steps** Search a unique nonsense term, then search a valid term.
- **Expected result** Record empty-result feedback and successful recovery. Empty-state presentation is a product question.
- **Actual result** Pending manual execution.
- **Status** Not Run.
- **Evidence** Pending.

## EC-17 Quantity boundaries

- **Priority** Medium
- **Execution mode** Manual intervention required
- **Prerequisites** Product detail open and fresh cart for each input.
- **Test data** Quantity: blank, -1, 0, 1, 2 and 1.5.
- **Steps** On product detail, try quantity 0, -1, 1.5 and empty.
- **Expected result** Record validation and whether a request/cart entry is created. The browser input advertises a minimum of 1 and the default integer step. Enforcement during cart submission is a product question.
- **Actual result** Pending manual execution.
- **Status** Not Run.
- **Evidence** Pending.

## EC-18 Session isolation

- **Priority** High
- **Execution mode** Manual and automated coverage as mapped
- **Prerequisites** Two independent browser profiles.
- **Test data** One synthetic account, one product.
- **Steps** Open a separate private browser session while the first holds a cart.
- **Expected result** Record whether cart and login state stay isolated between independent sessions; product question.
- **Actual result** Pending manual execution.
- **Status** Not Run.
- **Evidence** Pending.

## EC-19 Responsive and keyboard navigation

- **Priority** Medium
- **Execution mode** Manual intervention required
- **Prerequisites** Fresh browser.
- **Test data** 375px wide viewport and Tab, Shift+Tab, Enter.
- **Steps** Follow search → detail → cart at 375px and using keyboard controls.
- **Expected result** Record focus visibility, reachable controls, clipping and any blocked action; usability investigation.
- **Actual result** Pending manual execution.
- **Status** Not Run.
- **Evidence** Pending.

## EC-20 UI and API reconciliation

- **Priority** Medium
- **Execution mode** Manual and automated coverage as mapped
- **Prerequisites** Products endpoint and product detail available.
- **Test data** Blue Top, Men Tshirt; omit search_product.
- **Steps** Compare product/search API results with UI; omit search parameter.
- **Expected result** Catalogue values agree across layers. Missing search parameter returns the documented application code 400 and error message; API cases 1, 5–6.
- **Actual result** Pending manual execution.
- **Status** Not Run.
- **Evidence** Pending.

## EC-21 Default quantity

- **Priority** Medium
- **Execution mode** Manual and automated coverage as mapped
- **Prerequisites** Fresh product detail page.
- **Test data** Blue Top with untouched quantity.
- **Steps** Open the product detail and inspect the initial quantity.
- **Expected result** Initial quantity is 1, consistent with adding a single item.
- **Actual result** Pending manual execution.
- **Status** Not Run.
- **Evidence** Pending.

## EC-22 Remove final item

- **Priority** Medium
- **Execution mode** Manual and automated coverage as mapped
- **Prerequisites** A cart with exactly one line.
- **Test data** Blue Top.
- **Steps** Remove the only item; inspect cart and refresh.
- **Expected result** An empty cart is shown and the item remains absent.
- **Actual result** Pending manual execution.
- **Status** Not Run.
- **Evidence** Pending.

## EC-23 Required signup field

- **Priority** Medium
- **Execution mode** Manual intervention required
- **Prerequisites** Signup page open with unique email.
- **Test data** Leave required postal code empty, then complete it.
- **Steps** Complete signup except for the postal code; submit, supply the code and retry.
- **Expected result** Required-field validation prevents submission; a completed synthetic form creates the account. Confirm the field requirement via the form before execution.
- **Actual result** Pending manual execution.
- **Status** Not Run.
- **Evidence** Pending.
