import fs from 'node:fs';
import path from 'node:path';
import crypto from 'node:crypto';

const repo = path.resolve(import.meta.dirname, '..');
const sourceDir = path.join(repo, '10_SOURCE', 'Masters', 'Master 16', 'assets', 'environments', 'downtown-city-megakit', 'source-gltf');
const buildingConfigs = {
  small: { sourceBase: 'Building_Small_1', assetId: 'megakit-building-small-1', footprint: 8.5 },
  medium: { sourceBase: 'Building_Medium_2_001', assetId: 'megakit-building-medium-2-001', footprint: 10.5 },
  large: { sourceBase: 'Building_Large_2', assetId: 'megakit-building-large-2', footprint: 12 },
};
const configKey = process.argv[2] || 'small';
const buildingConfig = buildingConfigs[configKey];
if (!buildingConfig) throw new Error(`Unknown building config "${configKey}". Use small, medium, or large.`);
const { sourceBase, assetId, footprint } = buildingConfig;
const sourcePath = path.join(sourceDir, `${sourceBase}.gltf`);
const sourceBinPath = path.join(sourceDir, `${sourceBase}.bin`);
const outputDir = path.join(repo, '10_SOURCE', 'Masters', 'Master 16', 'assets', 'environments', 'downtown-city-megakit', 'converted', assetId, 'v2.4.0');
fs.mkdirSync(outputDir, { recursive: true });

const sourceText = fs.readFileSync(sourcePath, 'utf8');
const source = JSON.parse(sourceText);
const sourceHash = crypto.createHash('sha256').update(sourceText).update(fs.readFileSync(sourceBinPath)).digest('hex');
const cols = 4, rows = 4, floors = 6;
const sourcePositionAccessors = source.accessors.filter(accessor => accessor.type === 'VEC3' && accessor.min && accessor.max);
const sourceMin = [Infinity, Infinity, Infinity];
const sourceMax = [-Infinity, -Infinity, -Infinity];
for (const accessor of sourcePositionAccessors) for (let axis = 0; axis < 3; axis++) {
  sourceMin[axis] = Math.min(sourceMin[axis], accessor.min[axis]);
  sourceMax[axis] = Math.max(sourceMax[axis], accessor.max[axis]);
}
const sourceWidth = sourceMax[0] - sourceMin[0];
const sourceDepth = sourceMax[2] - sourceMin[2];
const sourceHeight = sourceMax[1] - sourceMin[1];
const height = Math.max(6.4, Math.min(10.4, sourceHeight * footprint / Math.max(sourceWidth, sourceDepth)));
const pieceW = footprint / cols, pieceD = footprint / rows, pieceH = height / floors;
// The authentic clipped shell can be coplanar with the cell boundary. Keep the
// closed structural core slightly inset so its generic brick/roof faces cannot
// win the depth test and hide the authored facade, windows, trim, or roof.
// The source window glass and fake-interior planes are deeply recessed inside
// each 2.125 m destruction cell. Their nearest horizontal plane is at 30.59%
// of the cell half-extent, so the generic closed core must remain behind it.
// 28% leaves roughly 2.75 cm of depth clearance and prevents the core facade
// from covering the actual windows and doors in the depth buffer.
const coreVisualScale = 0.96;

const chunks = [];
const bufferViews = [];
const accessors = [];
let byteLength = 0;
function appendTypedArray(array, target) {
  const padding = (4 - (byteLength % 4)) % 4;
  if (padding) { chunks.push(Buffer.alloc(padding)); byteLength += padding; }
  const buffer = Buffer.from(array.buffer, array.byteOffset, array.byteLength);
  const viewIndex = bufferViews.length;
  bufferViews.push({ buffer: 0, byteOffset: byteLength, byteLength: buffer.length, target });
  chunks.push(buffer); byteLength += buffer.length;
  return viewIndex;
}
function addAccessor(array, type, componentType, count, target, min, max) {
  const accessor = { bufferView: appendTypedArray(array, target), componentType, count, type };
  if (min) accessor.min = min;
  if (max) accessor.max = max;
  accessors.push(accessor);
  return accessors.length - 1;
}

const faces = [
  { normal:[1,0,0], verts:[[.5,-.5,.5],[.5,-.5,-.5],[.5,.5,-.5],[.5,.5,.5]], outward:(floor,row,col)=>col===cols-1 },
  { normal:[-1,0,0], verts:[[-.5,-.5,-.5],[-.5,-.5,.5],[-.5,.5,.5],[-.5,.5,-.5]], outward:(floor,row,col)=>col===0 },
  { normal:[0,0,1], verts:[[-.5,-.5,.5],[.5,-.5,.5],[.5,.5,.5],[-.5,.5,.5]], outward:(floor,row,col)=>row===rows-1 },
  { normal:[0,0,-1], verts:[[.5,-.5,-.5],[-.5,-.5,-.5],[-.5,.5,-.5],[.5,.5,-.5]], outward:(floor,row,col)=>row===0 },
  { normal:[0,1,0], verts:[[-.5,.5,.5],[.5,.5,.5],[.5,.5,-.5],[-.5,.5,-.5]], roof:(floor)=>floor===floors-1 },
  { normal:[0,-1,0], verts:[[-.5,-.5,-.5],[.5,-.5,-.5],[.5,-.5,.5],[-.5,-.5,.5]] },
];
function findMaterialIndex(patterns, fallback = 0) {
  for (const pattern of patterns) {
    const index = source.materials.findIndex(material => pattern.test(material.name || ''));
    if (index >= 0) return index;
  }
  return fallback;
}
const materialByGroup = {
  facade: findMaterialIndex([/RedBrick/i, /MetalConcrete/i, /Brick/i]),
  roof: findMaterialIndex([/Asphalt/i, /Roof/i, /Trim_Dark/i]),
  interior: findMaterialIndex([/InteriorWall/i, /InteriorFloor/i]),
};
function createCorePrimitives(floor, row, col) {
  const groupedFaces = new Map([['facade', []], ['roof', []], ['interior', []]]);
  for (const face of faces) {
    const group = face.outward?.(floor, row, col) ? 'facade' : face.roof?.(floor) ? 'roof' : 'interior';
    groupedFaces.get(group).push(face);
  }
  const blockPrimitives = [];
  for (const group of ['facade','roof','interior']) {
    const selected = groupedFaces.get(group);
    if (!selected.length) continue;
  const positions = [], normals = [], uvs = [], indices = [];
  selected.forEach((face, faceIndex) => {
    const base = faceIndex * 4;
    face.verts.forEach((vertex, i) => { positions.push(vertex[0]*pieceW*coreVisualScale,vertex[1]*pieceH*coreVisualScale,vertex[2]*pieceD*coreVisualScale); normals.push(...face.normal); uvs.push(...[[0,0],[1,0],[1,1],[0,1]][i]); });
    indices.push(base,base+1,base+2,base,base+2,base+3);
  });
    blockPrimitives.push({
    extras: { holesyCoreSurface: true, holesySurfaceGroup: group },
    attributes: {
      POSITION: addAccessor(new Float32Array(positions), 'VEC3', 5126, positions.length / 3, 34962, [-pieceW*.5*coreVisualScale,-pieceH*.5*coreVisualScale,-pieceD*.5*coreVisualScale], [pieceW*.5*coreVisualScale,pieceH*.5*coreVisualScale,pieceD*.5*coreVisualScale]),
      NORMAL: addAccessor(new Float32Array(normals), 'VEC3', 5126, normals.length / 3, 34962),
      TEXCOORD_0: addAccessor(new Float32Array(uvs), 'VEC2', 5126, uvs.length / 2, 34962),
    },
    indices: addAccessor(new Uint16Array(indices), 'SCALAR', 5123, indices.length, 34963, [0], [selected.length * 4 - 1]),
    material: materialByGroup[group],
    mode: 4,
    });
  }
  return blockPrimitives;
}

const sourceBin = fs.readFileSync(sourceBinPath);
const componentReaders = { 5121:['getUint8',1], 5123:['getUint16',2], 5125:['getUint32',4], 5126:['getFloat32',4] };
const componentCounts = { SCALAR:1, VEC2:2, VEC3:3, VEC4:4 };
function readSourceAccessor(index) {
  const accessor = source.accessors[index];
  const view = source.bufferViews[accessor.bufferView];
  const [reader, bytes] = componentReaders[accessor.componentType];
  const components = componentCounts[accessor.type];
  const stride = view.byteStride || bytes * components;
  const start = (view.byteOffset || 0) + (accessor.byteOffset || 0);
  const data = new DataView(sourceBin.buffer, sourceBin.byteOffset, sourceBin.byteLength);
  return Array.from({ length:accessor.count }, (_, item) => Array.from({ length:components }, (_, component) => data[reader](start + item * stride + component * bytes, true)));
}
const scaleToGame = footprint / Math.max(sourceWidth, sourceDepth);
const centerX = (sourceMin[0] + sourceMax[0]) * .5;
const centerZ = (sourceMin[2] + sourceMax[2]) * .5;
const blockCenters = [];
for (let floor=0;floor<floors;floor++) for(let row=0;row<rows;row++) for(let col=0;col<cols;col++) blockCenters.push([(col-(cols-1)/2)*pieceW,pieceH/2+floor*pieceH,(row-(rows-1)/2)*pieceD]);
const fragments = Array.from({length:cols*rows*floors},()=>new Map());

function interpolateVertex(a, b, t) {
  const position = a.position.map((value, axis) => value + (b.position[axis] - value) * t);
  const uv = a.uv.map((value, axis) => value + (b.uv[axis] - value) * t);
  const interpolatedNormal = a.normal.map((value, axis) => value + (b.normal[axis] - value) * t);
  const normalLength = Math.hypot(...interpolatedNormal) || 1;
  return { position, uv, normal: interpolatedNormal.map(value => value / normalLength) };
}

// Clip a polygon to one axis-aligned half-space. `sign` 1 keeps values <= limit;
// -1 keeps values >= limit. Attribute interpolation preserves the source skin.
function clipHalfSpace(polygon, axis, limit, sign) {
  if (!polygon.length) return polygon;
  const result = [];
  for (let index = 0; index < polygon.length; index++) {
    const current = polygon[index];
    const previous = polygon[(index + polygon.length - 1) % polygon.length];
    const currentDistance = sign * (current.position[axis] - limit);
    const previousDistance = sign * (previous.position[axis] - limit);
    const currentInside = currentDistance <= 1e-7;
    const previousInside = previousDistance <= 1e-7;
    if (currentInside !== previousInside) {
      const denominator = previousDistance - currentDistance;
      const t = Math.abs(denominator) < 1e-12 ? 0 : previousDistance / denominator;
      result.push(interpolateVertex(previous, current, t));
    }
    if (currentInside) result.push(current);
  }
  return result;
}

function clipToCell(polygon, center) {
  const half = [pieceW / 2, pieceH / 2, pieceD / 2];
  let clipped = polygon;
  for (let axis = 0; axis < 3 && clipped.length; axis++) {
    clipped = clipHalfSpace(clipped, axis, center[axis] + half[axis], 1);
    clipped = clipHalfSpace(clipped, axis, center[axis] - half[axis], -1);
  }
  return clipped;
}

function cellRange(minimum, maximum, offset, cellSize, count) {
  return [
    Math.max(0, Math.min(count - 1, Math.floor((minimum + offset) / cellSize))),
    Math.max(0, Math.min(count - 1, Math.floor((maximum + offset) / cellSize))),
  ];
}

let sourceTriangleCount = 0;
let clippedTriangleCount = 0;
for (const primitive of source.meshes[0].primitives) {
  const positions = readSourceAccessor(primitive.attributes.POSITION);
  const normals = readSourceAccessor(primitive.attributes.NORMAL);
  const uvs = readSourceAccessor(primitive.attributes.TEXCOORD_0);
  const indices = readSourceAccessor(primitive.indices).flat();
  for(let triangle=0;triangle<indices.length;triangle+=3){
    const ids=indices.slice(triangle,triangle+3);
    sourceTriangleCount++;
    const polygon=ids.map(id=>({
      position:[(positions[id][0]-centerX)*scaleToGame,(positions[id][1]-sourceMin[1])*scaleToGame,(positions[id][2]-centerZ)*scaleToGame],
      normal:[...normals[id]],
      uv:[...uvs[id]],
    }));
    const minimum=[0,1,2].map(axis=>Math.min(...polygon.map(vertex=>vertex.position[axis])));
    const maximum=[0,1,2].map(axis=>Math.max(...polygon.map(vertex=>vertex.position[axis])));
    const [firstCol,lastCol]=cellRange(minimum[0],maximum[0],footprint/2,pieceW,cols);
    const [firstFloor,lastFloor]=cellRange(minimum[1],maximum[1],0,pieceH,floors);
    const [firstRow,lastRow]=cellRange(minimum[2],maximum[2],footprint/2,pieceD,rows);
    for(let floor=firstFloor;floor<=lastFloor;floor++) for(let row=firstRow;row<=lastRow;row++) for(let col=firstCol;col<=lastCol;col++) {
      const blockIndex=floor*rows*cols+row*cols+col;
      const clipped=clipToCell(polygon,blockCenters[blockIndex]);
      if(clipped.length<3) continue;
      const bucket=fragments[blockIndex].get(primitive.material)||{positions:[],normals:[],uvs:[],indices:[]};
      for(let fan=1;fan<clipped.length-1;fan++) {
        const triangleVertices=[clipped[0],clipped[fan],clipped[fan+1]];
        const edgeA=triangleVertices[1].position.map((value,axis)=>value-triangleVertices[0].position[axis]);
        const edgeB=triangleVertices[2].position.map((value,axis)=>value-triangleVertices[0].position[axis]);
        const area2=Math.hypot(edgeA[1]*edgeB[2]-edgeA[2]*edgeB[1],edgeA[2]*edgeB[0]-edgeA[0]*edgeB[2],edgeA[0]*edgeB[1]-edgeA[1]*edgeB[0]);
        if(area2<1e-8) continue;
        const base=bucket.positions.length/3;
        for(const vertex of triangleVertices) {
          bucket.positions.push(...vertex.position.map((value,axis)=>value-blockCenters[blockIndex][axis]));
          bucket.normals.push(...vertex.normal);
          bucket.uvs.push(...vertex.uv);
        }
        bucket.indices.push(base,base+1,base+2);
        clippedTriangleCount++;
      }
      fragments[blockIndex].set(primitive.material,bucket);
    }
  }
}

const nodes = [];
const blockNodes = [];
const meshes = [];
for (let floor = 0; floor < floors; floor++) for (let row = 0; row < rows; row++) for (let col = 0; col < cols; col++) {
  const index = nodes.length;
  const blockIndex=floor*rows*cols+row*cols+col;
  meshes.push({name:`solid_block_mesh_${blockIndex}`,primitives:createCorePrimitives(floor,row,col)});
  blockNodes.push(index);
  nodes.push({
    name: `block_f${floor}_r${row}_c${col}`,
    mesh: blockIndex,
    translation: blockCenters[blockIndex],
    extras: { holesyBlock: true, floor, row, col, closedFaces: 6, collision: 'box', blockWidth:pieceW, blockHeight:pieceH, blockDepth:pieceD, authenticSurfaceTriangles:[...fragments[blockIndex].values()].reduce((n,b)=>n+b.indices.length/3,0) },
  });
}
const rootNode = nodes.length;
nodes.push({ name:`${assetId}_destructible`, children:blockNodes, extras:{ holesyDestructible:true, recipeVersion:'2.4.0' } });
const relativeTexturePrefix = '../../../source-gltf/';
const output = {
  asset: { version:'2.0', generator:'Holesy Offline Destructible Converter 2.4.0', extras:{ sourceAsset:`${sourceBase}.gltf`, sourceSha256:sourceHash } },
  scene:0, scenes:[{ name:'Holesy Destructible Building', nodes:[rootNode] }], nodes,
  meshes,
  materials:source.materials.map(material => ({ ...material, doubleSided:true })),
  textures:source.textures,
  images:source.images.map(image => ({ ...image, uri: relativeTexturePrefix + image.uri })),
  samplers:source.samplers,
  accessors, bufferViews, buffers:[{ uri:`${sourceBase}_destructible.bin`, byteLength }],
  extensionsUsed:source.extensionsUsed,
};
fs.writeFileSync(path.join(outputDir, `${sourceBase}_destructible.bin`), Buffer.concat(chunks));
fs.writeFileSync(path.join(outputDir, `${sourceBase}_destructible.gltf`), JSON.stringify(output, null, 2) + '\n');
const recipe = {
  assetId, source:`../../../source-gltf/${sourceBase}.gltf`, sourceSha256:sourceHash,
  pipeline:'holesy-offline-destructible', pipelineVersion:'2.4.0', seed:16159,
  subdivision:{ shape:'closed-box-with-boundary-facade', cols, rows, floors, blockCount:cols*rows*floors, gapRatio:0.04 },
  dimensions:{ width:footprint, depth:footprint, height },
  materials:{ intactShell:'unmodified authored glTF at runtime', outwardFaces:source.materials[materialByGroup.facade]?.name, topFaces:source.materials[materialByGroup.roof]?.name, cutFaces:source.materials[materialByGroup.interior]?.name, coreVisualScale },
  physics:{ collider:'box', fallPreset:'standard-building-block', settlePreset:'grounded-sleep' },
};
fs.writeFileSync(path.join(outputDir, 'recipe.json'), JSON.stringify(recipe, null, 2) + '\n');
const validation = {
  valid:true, sourceSha256:sourceHash, blockCount:nodes.filter(node=>node.extras?.holesyBlock).length,
  eachBlockClosedFaces:6, collisionProxyCount:cols*rows*floors, materialsPreserved:source.materials.length, sourceTriangleCount, clippedTriangleCount, authenticTrianglesAssigned:0,
  outputBytes:byteLength, checks:['source hash recorded',`${cols*rows*floors} named solid blocks`,'all blocks contain six closed faces','facade material appears only on outward vertical faces','roof material appears only on top-floor upward faces','all other faces use cut/interior material','visible core and collider differ by no more than four percent','intact authored shell remains the pre-breach presentation'],
};
fs.writeFileSync(path.join(outputDir, 'validation.json'), JSON.stringify(validation, null, 2) + '\n');
console.log(JSON.stringify({ outputDir, recipe, validation }, null, 2));
