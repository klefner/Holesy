import assert from 'node:assert/strict';
import { readFileSync } from 'node:fs';

const sourcePath = new URL('../../10_SOURCE/Masters/Master 16/js/main.js', import.meta.url);
const manifestPath = new URL('../../10_SOURCE/Masters/Master 16/assets/environments/harvest-county/pack-manifest.json', import.meta.url);
const source = readFileSync(sourcePath, 'utf8');
const manifest = JSON.parse(readFileSync(manifestPath, 'utf8'));

assert.equal(manifest.coreVisualScale, 0.96, 'Harvest cores must remain visually substantial');
const expectedCounts = new Map([
  ['harvest-small-barn', 96],
  ['harvest-barn', 96],
  ['harvest-big-barn', 96],
  ['harvest-silo', 128],
  ['harvest-water-tower', 128],
  ['harvest-windmill', 96],
]);
for (const model of manifest.models) {
  if (!expectedCounts.has(model.id)) continue;
  const count = model.grid.cols * model.grid.rows * model.grid.floors;
  assert.equal(count, expectedCounts.get(model.id), `${model.id} must use the established small-fragment grid`);
}

const waterTower = manifest.models.find(model => model.id === 'harvest-water-tower');
const windmill = manifest.models.find(model => model.id === 'harvest-windmill');
assert.deepEqual(waterTower.excludeMaterialPatterns, ['^DarkGrey$']);
assert.deepEqual(windmill.meshIndexes, [0]);

assert.match(source, /function activateHarvestSupportColumn\(/);
assert.match(source, /function startHarvestLandmarkCollapse\(/);
assert.match(source, /function updateHarvestLandmarkCollapses\(/);
assert.match(source, /function releaseHarvestLandmarkAfterImpact\(/);
const frontierDefinitions = source.slice(
  source.indexOf('const HARVEST_FRONTIER_BUILDINGS'),
  source.indexOf('const HARVEST_CROPS')
);
assert.doesNotMatch(frontierDefinitions, /breakupCount:/, 'Harvest frontier buildings must use every converted cube');
assert.match(source, /definition\.unusualElement === 'windmill_blades'/);
assert.match(source, /definition\.unusualElement === 'water_tank'/);

console.log('Harvest collapse contract checks passed (small solids, local supports, coherent landmarks, conserved value)');
