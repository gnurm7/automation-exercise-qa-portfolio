# Traceability matrix

| Case | Automation scenario | Manual follow-up |
| --- | --- | --- |
| EC-01 | SearchAndOpenMatchingProduct, SearchFindsKnownProduct | Explore relevancy for broad terms |
| EC-02 | CartPreservesQuantityAndComputesTotal | Verify price and line total independently |
| EC-03 | TwoProductsRemainDistinct | Check continued navigation between pages |
| EC-04 | RemovalSurvivesRefresh | Capture before and after |
| EC-05 | GuestCheckoutPromptsForAuthentication | Check available prompt controls |
| EC-06 | GuestCartSurvivesLogin | Repeat with mixed quantities |
| EC-07 | RegistersSyntheticCustomer | Explore optional fields |
| EC-08 | DuplicateEmailIsRejected | Record shown feedback |
| EC-09 | WrongPasswordCanBeCorrected, UnknownEmailCannotSignIn | Verify keyboard recovery |
| EC-10 | LogoutClearsAuthenticatedNavigationAfterRefresh | Back navigation and new tab |
| EC-11 | CheckoutUsesRegistrationAddresses | Visual alignment |
| EC-12 | CheckoutPreservesCartLineAndOrderTotal | Multi-line totals and order comment |
| EC-13, EC-14 | OrderConfirmationOffersDownloadableInvoice | Verify available details and download behavior |
| EC-15 | CategoryShowsMatchingHeading, BrandShowsKnownProduct | Combined navigation and labels |
| EC-16 | EmptySearchResultsAllowRecovery | Empty-state feedback |
| EC-17 | QuantityFieldReportsNumberConstraints | Required manual cart-submission charter |
| EC-18 | IndependentBrowserDoesNotShareCart | Auth isolation between profiles |
| EC-19 | Manual | Responsive and keyboard charter |
| EC-20 | ProductDetailsAgreeWithPublicApi, API-01/05/06 | Additional catalogue samples |
| EC-21 | ProductDefaultsToOneItem | Compare default and number control |
| EC-22 | RemovingFinalItemShowsEmptyCart | Refresh empty cart |
| EC-23 | Manual | Required-field validation and recovery |

API-01 through API-14 map to the matching [published API cases](https://automationexercise.com/api_list). API-15 covers missing login password, API-16 verifies update persistence, and API-17 checks credentials after deletion.
