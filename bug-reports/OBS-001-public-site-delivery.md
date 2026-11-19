# OBS-001 Public site delivery affected Chrome execution

**Classification** External environment and automation observation
**Environment** Windows, Chrome 157.0.8103.79 headless, 2026-11-19
**Related cases** EC-15 and EC-20

During the intermediate regression run, navigating to Products showed a queue-full message in place of the catalogue. The category link could not be exercised in that case. Another diagnostic navigation to a product detail also displayed the queue-full page. The suite identifies the known overload message in its failure output and captures the browser state.

During the initial run, a sponsored link and its call to action appeared inside the product category label. Related sponsored copy also appeared inside search results. The catalogue search variants now assert stable product links and product comparison normalizes whitespace. Keep screenshot evidence when sponsored content changes the visible page or blocks interaction.

**Evidence** [Queue-full screenshot](../evidence/2026-11-19-queue-full.png). The screenshot contains only the public error page.
**Retest** See the [dated execution summary](../evidence/2026-11-19-execution.md) for measured results.
