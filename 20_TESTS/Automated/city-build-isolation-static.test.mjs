import assert from 'node:assert/strict';
import { readFileSync } from 'node:fs';

const sourcePath = new URL('../../10_SOURCE/Masters/Master 16/js/main.js', import.meta.url);
const source = readFileSync(sourcePath, 'utf8');

assert.match(source, /const context = Object\.freeze\(\{[\s\S]*?roundRunId,[\s\S]*?environment,[\s\S]*?packs: Object\.freeze/);
assert.match(source, /function tearDownWorld\(\) \{\s*cancelActiveCityBuild\('world-teardown'\)/);
assert.match(source, /async function populateCity\(\)[\s\S]*?beginCityBuildContext\(selectedEnvironment\)/);
assert.match(source, /await populateSelectedCityPacks\(cityBuildContext\);\s*if \(!isCityBuildContextCurrent\(cityBuildContext\)\) return;/);

const populateCity = source.slice(source.indexOf('async function populateCity()'), source.indexOf('function pruneObjectsToArena()'));
const awaitedYields = [...populateCity.matchAll(/await yieldCityBuildFrame\(\);/g)].length;
const postYieldGuards = [...populateCity.matchAll(/await yieldCityBuildFrame\(\);\s*if \(!isCityBuildContextCurrent\(cityBuildContext\)\) return;/g)].length;
assert.equal(postYieldGuards, awaitedYields, 'every core city-build frame yield must immediately validate its immutable context');

assert.match(source, /environment: selectedEnvironment,\s*cityRecipePacks: \[\.\.\.cityRecipe\.packs\]/);
assert.match(source, /if \(!ELIGIBLE_ENVIRONMENTS\.includes\(save\.environment\)\)[\s\S]*?legacy Endless save lacks a district identity/);
assert.match(source, /selectedEnvironment = save\.environment;[\s\S]*?selectedEnvironmentOverride = save\.environment;/);
assert.match(source, /data-holesy-city-build-integrity/);

console.log(`city-build isolation checks passed (${awaitedYields} guarded core yields)`);
