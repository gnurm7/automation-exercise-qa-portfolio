const fs = require("node:fs");
const path = require("node:path");
const newman = require("newman");
const collection = require("./automation-exercise.postman_collection.json");

const catalogueRequest = collection.item[0].item[0].request;

function classify(response) {
    const status = response.code;
    const body = response.text();
    const title = body.match(/<title[^>]*>([^<]*)<\/title>/i)?.[1].trim().slice(0, 120);
    const contentType = response.headers.get("Content-Type");
    const html = /^\s*(?:<!doctype html|<html)/i.test(body);
    const accessPage = html && /just a moment|checking your browser|verify you are human|cf[-_]chl|under heavy load \(queue full\)|access denied|security verification|captcha/i.test(body);
    if (status === 429 || status === 503 || accessPage) {
        return { outcome: "blocked", status, contentType, title };
    }
    try {
        const data = JSON.parse(body);
        if (status === 200 && data.responseCode === 200 && Array.isArray(data.products)) {
            return { outcome: "available", status, contentType };
        }
    } catch {
        // An unrecognized response remains a failing contract check.
    }
    return { outcome: "unexpected", status, contentType, title };
}

function report(result) {
    const directory = path.resolve(__dirname, "../artifacts/api");
    fs.mkdirSync(directory, { recursive: true });
    const details = JSON.stringify(result, null, 2);
    fs.writeFileSync(path.join(directory, "preflight.json"), details + "\n");
    console.log(details);
    fs.appendFileSync(process.env.GITHUB_OUTPUT, `outcome=${result.outcome}\n`);
    const message = {
        available: "The catalogue endpoint responded with the expected JSON. Newman will run.",
        blocked: "Live API checks were blocked by the external site. Newman was skipped.",
        unexpected: "The API preflight received an unexpected response. See the API artifact."
    }[result.outcome];
    fs.appendFileSync(process.env.GITHUB_STEP_SUMMARY, `### Live API preflight\n\n${message}\n`);
    if (result.outcome === "blocked") console.log(`::warning::${message}`);
    if (result.outcome === "unexpected") process.exitCode = 1;
}

let result;
newman.run({
    collection: { info: collection.info, item: [{ name: "Catalogue preflight", request: catalogueRequest }] },
    environment: path.join(__dirname, "automation-exercise.postman_environment.json"),
    timeoutRequest: 15000,
    reporters: []
}, (error) => {
    report(result ?? { outcome: "unexpected", error: error?.code ?? "No API response" });
}).on("request", (error, args) => {
    result = error
        ? { outcome: "unexpected", error: error.code ?? error.name }
        : classify(args.response);
});
