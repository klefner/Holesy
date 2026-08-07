import assert from 'node:assert/strict';
import { readFileSync } from 'node:fs';

const sourcePath = new URL('../../10_SOURCE/Masters/Master 16/js/main.js', import.meta.url);
const manifestPath = new URL('../../10_SOURCE/Masters/Master 16/assets/environments/harvest-county/pack-manifest.json', import.meta.url);
const source = readFileSync(sourcePath, 'utf8');
const manifest = JSON.parse(readFileSync(manifestPath, 'utf8'));

assert.equal(manifest.coreVisualScale, 0.96, 'Harvest cores must remain visually substantial');
const expectedCounts = new Map([
  ['harvest-small-barn', 8],
  ['harvest-barn', 12],
  ['harvest-big-barn', 18],
  ['harvest-silo', 16],
  ['harvest-water-tower', 16],
  ['harvest-windmill', 20],
]);
for (const model of manifest.models) {
  if (!expectedCounts.has(model.id)) continue;
  const count = model.grid.cols * model.grid.rows * model.grid.floors;
  assert.equal(count, expectedCounts.get(model.id), `${model.id} must keep its compact structural-piece budget`);
}

const waterTower = manifest.models.find(model => model.id === 'harvest-water-tower');
const windmill = manifest.models.find(model => model.id === 'harvest-windmill');
assert.deepEqual(waterTower.excludeMaterialPatterns, ['^DarkGrey$']);
assert.deepEqual(windmill.meshIndexes, [0]);

assert.match(source, /function activateHarvestSupportColumn\(/);
assert.match(source, /function startHarvestLandmarkCollapse\(/);
assert.match(source, /function updateHarvestLandmarkCollapses\(/);
assert.match(source, /function releaseHarvestLandmarkAfterImpact\(/);
assert.match(source, /authoredValuePieceCount: 128, structuralValuePieceCount: 96/);
assert.match(source, /authoredValuePieceCount: 96, structuralValuePieceCount: 72/);
assert.match(source, /definition\.unusualElement === 'windmill_blades'/);
assert.match(source, /definition\.unusualElement === 'water_tank'/);

console.log('Harvest collapse contract checks passed (compact solids, local supports, coherent landmarks, conserved value)');
