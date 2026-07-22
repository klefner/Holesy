import fs from 'node:fs';
import path from 'node:path';

const input = path.resolve(process.argv[2] || '');
if (!process.argv[2] || !fs.existsSync(input) || !fs.statSync(input).isFile()) {
  console.error('Usage: node scripts/validate-destructible-gltf.mjs <converted.gltf>');
  process.exit(2);
}

const gltf = JSON.parse(fs.readFileSync(input, 'utf8'));
const binary = fs.readFileSync(path.resolve(path.dirname(input), gltf.buffers[0].uri));
const component = {
  5120: ['readInt8', 1], 5121: ['readUInt8', 1],
  5122: ['readInt16LE', 2], 5123: ['readUInt16LE', 2],
  5125: ['readUInt32LE', 4], 5126: ['readFloatLE', 4],
};
const widths = { SCALAR: 1, VEC2: 2, VEC3: 3, VEC4: 4 };

function readAccessor(index) {
  const accessor = gltf.accessors[index];
  const view = gltf.bufferViews[accessor.bufferView];
  const [reader, bytes] = component[accessor.componentType];
  const width = widths[accessor.type];
  const stride = view.byteStride || bytes * width;
  const offset = (view.byteOffset || 0) + (accessor.byteOffset || 0);
  return Array.from({ length: accessor.count }, (_, item) =>
    Array.from({ length: width }, (_, axis) => binary[reader](offset + item * stride + axis * bytes)));
}

const tolerance = 0.02;
const failures = [];
let blockCount = 0;
let overhangingPrimitiveCount = 0;
let overhangingVertexCount = 0;
let authenticPrimitiveCount = 0;
let authenticTriangleCount = 0;
let blocksMissingAuthenticSurface = 0;
let blocksMissingClosedCore = 0;

for (const node of gltf.nodes || []) {
  if (!node.extras?.holesyBlock || node.mesh == null) continue;
  blockCount++;
  const half = [node.extras.blockWidth, node.extras.blockHeight, node.extras.blockDepth].map(value => value / 2 + tolerance);
  const mesh = gltf.meshes[node.mesh];
  const authenticPrimitives = mesh.primitives.filter(primitive => primitive.extras?.holesyAuthenticSurface);
  const corePrimitives = mesh.primitives.filter(primitive => primitive.extras?.holesyCoreSurface);
  authenticPrimitiveCount += authenticPrimitives.length;
  authenticTriangleCount += authenticPrimitives.reduce((sum, primitive) => sum + gltf.accessors[primitive.indices].count / 3, 0);
  if (!authenticPrimitives.length) blocksMissingAuthenticSurface++;
  if (!corePrimitives.length || node.extras.closedFaces !== 6) blocksMissingClosedCore++;
  for (let primitiveIndex = 0; primitiveIndex < mesh.primitives.length; primitiveIndex++) {
    const primitive = mesh.primitives[primitiveIndex];
    const positions = readAccessor(primitive.attributes.POSITION);
    const outside = positions.filter(position => position.some((value, axis) => Math.abs(value) > half[axis]));
    if (!outside.length) continue;
    overhangingPrimitiveCount++;
    overhangingVertexCount += outside.length;
    failures.push({ node: node.name, primitive: primitiveIndex, material: primitive.material, outsideVertices: outside.length });
  }
}

const report = {
  valid: failures.length === 0 && authenticPrimitiveCount > 0 && blocksMissingClosedCore === 0,
  input,
  blockCount,
  tolerance,
  overhangingPrimitiveCount,
  overhangingVertexCount,
  authenticPrimitiveCount,
  authenticTriangleCount,
  blocksMissingAuthenticSurface,
  blocksMissingClosedCore,
  failures: failures.slice(0, 25),
};
console.log(JSON.stringify(report, null, 2));
process.exitCode = report.valid ? 0 : 1;
