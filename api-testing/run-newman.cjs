const fs = require("node:fs");
const path = require("node:path");
const newman = require("newman");

const results = path.resolve(__dirname, "../artifacts/api");
fs.mkdirSync(results, { recursive: true });

newman.run({
    collection: path.join(__dirname, "automation-exercise.postman_collection.json"),
    environment: path.join(__dirname, "automation-exercise.postman_environment.json"),
    timeoutRequest: 30000,
    delayRequest: 300,
    reporters: ["cli", "junit"],
    reporter: { junit: { export: path.join(results, "newman.xml") } }
}, (error, summary) => {
    if (error || !summary) {
        console.error("Newman could not complete:", error?.message);
        process.exitCode = 1;
        return;
    }
    const { requests, assertions } = summary.run.stats;
    fs.writeFileSync(path.join(results, "summary.json"), JSON.stringify({ requests, assertions }, null, 2));
    process.exitCode = summary.run.failures.length ? 1 : 0;
}).on("request", (error, args) => {
    if (error) {
        console.error(`Request failed for ${args.item.name}: ${error.code || error.name}`);
    } else if (args.response.code !== 200) {
        console.error(`Unexpected HTTP status for ${args.item.name}: ${args.response.code}`);
    } else if (!args.response.text().trimStart().startsWith("{")) {
        console.error(`Non-JSON response for ${args.item.name}. Check the site or proxy response.`);
    }
});
