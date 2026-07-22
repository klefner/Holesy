import fs from 'node:fs';
import path from 'node:path';
import { spawnSync } from 'node:child_process';

const repo = path.resolve(import.meta.dirname, '..');
const configPath = process.argv[2] ? path.resolve(process.argv[2]) : null;
if (!configPath || !fs.existsSync(configPath)) {
  throw new Error('Usage: node scripts/prepare-imported-city-pack.mjs <pack-manifest.json>');
}
const config = JSON.parse(fs.readFileSync(configPath, 'utf8'));
if (!config.runtimeSourceDir || !config.normalizedSourceDir) throw new Error('Pack manifest must declare runtimeSourceDir and normalizedSourceDir.');
const runtimeSourceDir = path.resolve(repo, config.runtimeSourceDir);
const normalizedSourceDir = path.resolve(repo, config.normalizedSourceDir);
fs.mkdirSync(normalizedSourceDir, { recursive: true });

for (const model of config.models) {
  const input = path.join(runtimeSourceDir, model.sourceFile || `${model.sourceBase}.glb`);
  const output = path.join(normalizedSourceDir, model.sourceGltf || `${model.sourceBase}.gltf`);
  if (!fs.existsSync(input)) throw new Error(`Missing imported source: ${input}`);
  const command = process.platform === 'win32' ? 'powershell.exe' : 'npx';
  const commandArgs = process.platform === 'win32'
    ? ['-NoProfile', '-ExecutionPolicy', 'Bypass', '-File', path.join(process.env.ProgramFiles, 'nodejs', 'npx.ps1'), '--yes', '@gltf-transform/cli@4.4.1', 'copy', input, output]
    : ['--yes', '@gltf-transform/cli@4.4.1', 'copy', input, output];
  const result = spawnSync(command, commandArgs, {
    cwd: repo,
    encoding: 'utf8',
    shell: false,
  });
  if (result.status !== 0) throw new Error(`glTF normalization failed for ${model.id}: ${result.error || result.stderr || result.stdout}`);
  process.stdout.write(result.stdout || '');
}

console.log(JSON.stringify({ packId: config.packId, normalizedSourceDir, modelCount: config.models.length }, null, 2));
