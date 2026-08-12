import assert from 'node:assert/strict';
import { readFileSync } from 'node:fs';

const sourcePath = new URL('../../10_SOURCE/Masters/Master 16/js/main.js', import.meta.url);
const source = readFileSync(sourcePath, 'utf8');

function functionBody(name) {
  const marker = `function ${name}(`;
  const start = source.indexOf(marker);
  assert.notEqual(start, -1, `${name} must exist`);
  const open = source.indexOf('{', start);
  let depth = 0;
  for (let index = open; index < source.length; index += 1) {
    if (source[index] === '{') depth += 1;
    if (source[index] === '}') depth -= 1;
    if (depth === 0) return source.slice(open + 1, index);
  }
  assert.fail(`${name} must have a complete function body`);
}

const nonMusicOwners = [
  'isGameplayAudioAllowed',
  'playWaveCompletionSound',
  'playPauseHumSound',
  'updateHoleWind',
  'playUnitClearStinger',
  'playMandateRowTick',
  'playRunGoalChime',
  'playBossInboundWarning',
  'playBossVictoryFanfare',
  'createAlienAidChime',
  'awardObjectConsume',
  'holeEatsHole',
  'activatePhysicsStack',
  'activateVoxelBuildingColumn',
  'stopCarAsWreck',
  'spawnWave',
  'playOffensiveUnitWeaponAudio',
  'updateSoldiers',
  'playConsumedSoldierAudio',
];

for (const owner of nonMusicOwners) {
  assert.doesNotMatch(
    functionBody(owner),
    /music\.muted/,
    `${owner} must not couple game sound to the Music preference`,
  );
}

const gameplayPermission = functionBody('isGameplayAudioAllowed');
assert.match(gameplayPermission, /!music\.focusSuspended/);
assert.match(gameplayPermission, /running && isGameState\(GAME_STATES\.PLAYING\)/);

const primeAudio = functionBody('primeAudioFromStartGesture');
assert.match(primeAudio, /initMusicContext\(\)/, 'Begin must initialize Web Audio even with Music off');
assert.match(primeAudio, /if \(!music\.muted\) startMusic\(\)/, 'Begin starts the score only when Music is enabled');
assert.match(primeAudio, /scheduleAudioBankWarmup\(0\)/, 'Begin must warm gameplay samples regardless of Music state');

const startMusic = functionBody('startMusic');
assert.match(startMusic, /music\.muted/, 'the score scheduler must not start silently while Music is disabled');

const aidDrops = functionBody('updateAidDrops');
assert.doesNotMatch(
  aidDrops,
  /if \(!isGameplayAudioAllowed\(\)\)[\s\S]{0,160}?return/,
  'aid movement and pickup simulation must never depend on audio permission',
);

const toggleMusic = functionBody('toggleMusic');
assert.match(toggleMusic, /stopMusic\(200\)/);
assert.match(toggleMusic, /stopArchiveMusic\(200\)/);
assert.doesNotMatch(
  toggleMusic,
  /setNonMusicGainMuted|stopActiveNonMusicSources|stopHoleWindNow|sfxGain|ambienceGain|pauseGain|celebrationGain/,
  'the Music control must not mutate or tear down non-music buses',
);

console.log('audio mute separation checks passed (music-only toggle, independent SFX, simulation-safe aid drops)');
