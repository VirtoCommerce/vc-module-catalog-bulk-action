# vc-module-catalog-bulk-action

## Known issues:
1. When we trying to perform bulk action operation we have an issue with smart caching. It leads to the fact that we getting an effect when the result of the operation can't be shown in UI before the cache will be invalidated. To avoid this problem we recommend disabling the smart caching in system settings or reduce the caching time.
These settings can be found in the admin menu: Setting>Cache>General

2. We have not made the performance tests at large and very large data sets yet. This optimization will be performed in further iterations. 

Please, consider this information in your work.
