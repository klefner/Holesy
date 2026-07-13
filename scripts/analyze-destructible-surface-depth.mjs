import fs from 'node:fs';
import path from 'node:path';

const input = path.resolve(process.argv[2]);
const gltf = JSON.parse(fs.readFileSync(input, 'utf8'));
const binary = fs.readFileSync(path.join(path.dirname(input), gltf.buffers[0].uri));
const widths = { SCALAR: 1, VEC2: 2, VEC3: 3, VEC4: 4 };
const readers = { 5121: ['readUInt8', 1], 5123: ['readUInt16LE', 2], 5125: ['readUInt32LE', 4], 5126: ['readFloatLE', 4] };
function readAccessor(index) {
  const accessor = gltf.accessors[index]; const view = gltf.bufferViews[accessor.bufferView];
  const [reader, bytes] = readers[accessor.componentType]; const width = widths[accessor.type];
  const stride = view.byteStride || bytes * width; const offset = (view.byteOffset || 0) + (accessor.byteOffset || 0);
  return Array.from({ length: accessor.count }, (_, item) => Array.from({ length: width }, (_, axis) => binary[reader](offset + item * stride + axis * bytes)));
}
const samples = new Map();
for (const node of gltf.nodes) {
  if (!node.extras?.holesyBlock) continue;
  const half = [node.extras.blockWidth / 2, node.extras.blockHeight / 2, node.extras.blockDepth / 2];
  const mesh = gltf.meshes[node.mesh];
  for (const primitive of mesh.primitives.slice(3)) {
    const positions = readAccessor(primitive.attributes.POSITION); const normals = readAccessor(primitive.attributes.NORMAL);
    const material = gltf.materials[primitive.material].name;
    const values = samples.get(material) || [];
    for (let index = 0; index < positions.length; index++) {
      if (Math.max(Math.abs(normals[index][0]), Math.abs(normals[index][2])) < 0.35) continue;
      const axis = Math.abs(normals[index][0]) >= Math.abs(normals[index][2]) ? 0 : 2;
      values.push(Math.abs(positions[index][axis]) / half[axis]);
    }
    samples.set(material, values);
  }
}
const report = {};
for (const [material, values] of samples) {
  values.sort((a, b) => a - b); const quantile = q => values[Math.floor((values.length - 1) * q)];
  report[material] = { count: values.length, min: quantile(0), p01: quantile(.01), p05: quantile(.05), p10: quantile(.1), median: quantile(.5), p90: quantile(.9), max: quantile(1) };
}
console.log(JSON.stringify(report, null, 2));
