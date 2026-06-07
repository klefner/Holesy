export const GOVERNMENT_PHYSICS_CONFIG = Object.freeze({
  fixedStep: 1 / 60,
  maxSubSteps: 3,
  gravity: 24,
  maxFallSpeed: 28,
  maxHorizontalSpeed: 13,
  maxAngularSpeed: 7,
  cellSize: 3.2,
  solverIterations: 4,
  restitution: 0.28,
  groundRestitution: 0.18,
  friction: 0.82,
  airDamping: 0.992,
  groundDamping: 0.86,
  angularDamping: 0.91,
  sleepSpeed: 0.08,
  sleepAngular: 0.1,
  sleepDelay: 0.72,
  contactSlop: 0.012,
  activationImpulseMin: 3.4,
  activationImpulseMax: 7.2,
  activationLiftMin: 1.0,
  activationLiftMax: 3.8,
  activationJitter: 1.4,
  activationColumnBoost: 1.55,
  activationNeighborBoost: 0.9,
  activationHeightLift: 0.42,
  activationShockJitter: 0.85,
  impactSpin: 2.8,
});

function randomBetween(min, max) {
  return min + Math.random() * (max - min);
}

function clamp(value, min, max) {
  return Math.max(min, Math.min(max, value));
}

function length3(x, y, z) {
  return Math.hypot(x, y, z);
}

function normalize2(x, z) {
  const len = Math.hypot(x, z);
  if (len < 0.0001) {
    const a = Math.random() * Math.PI * 2;
    return { x: Math.cos(a), z: Math.sin(a) };
  }
  return { x: x / len, z: z / len };
}

export class GovernmentPhysicsWorld {
  constructor(config = GOVERNMENT_PHYSICS_CONFIG) {
    this.config = config;
    this.bodies = [];
    this.bodyByObject = new WeakMap();
    this.accumulator = 0;
  }

  clear() {
    this.bodies.length = 0;
    this.bodyByObject = new WeakMap();
    this.accumulator = 0;
  }

  registerPiece(object, options) {
    const half = options.half;
    const body = {
      object,
      buildingId: options.buildingId,
      half: { x: half.x, y: half.y, z: half.z },
      mass: options.mass || 1,
      invMass: 1 / Math.max(0.001, options.mass || 1),
      position: {
        x: object.mesh.position.x,
        y: object.mesh.position.y,
        z: object.mesh.position.z,
      },
      velocity: {
        x: options.vx || 0,
        y: options.vy || 0,
        z: options.vz || 0,
      },
      angular: {
        x: options.avx || 0,
        y: options.avy || 0,
        z: options.avz || 0,
      },
      active: !!options.active,
      sleeping: false,
      sleepTimer: 0,
      onGround: false,
    };
    this.bodies.push(body);
    this.bodyByObject.set(object, body);
    this.syncObject(body);
    return body;
  }

  removeObject(object) {
    const body = this.bodyByObject.get(object);
    if (!body) return;
    const index = this.bodies.indexOf(body);
    if (index >= 0) this.bodies.splice(index, 1);
    this.bodyByObject.delete(object);
  }

  getBody(object) {
    return this.bodyByObject.get(object) || null;
  }

  isActiveObject(object) {
    const body = this.getBody(object);
    return !!body?.active;
  }

  activateBuilding(buildingId, source = null) {
    const sourceX = source?.x ?? 0;
    const sourceZ = source?.z ?? 0;
    let touched = null;
    let touchedDist = Infinity;
    for (const body of this.bodies) {
      if (body.buildingId !== buildingId || body.object?.consumed) continue;
      const dx = body.position.x - sourceX;
      const dz = body.position.z - sourceZ;
      const dist = dx * dx + dz * dz;
      if (dist < touchedDist) {
        touched = body;
        touchedDist = dist;
      }
    }
    const touchedObj = touched?.object || null;
    const touchedColX = touchedObj?.govColX ?? null;
    const touchedRowZ = touchedObj?.govRowZ ?? null;
    const touchedX = touched?.position.x ?? sourceX;
    const touchedZ = touched?.position.z ?? sourceZ;
    let activated = 0;
    for (const body of this.bodies) {
      if (body.buildingId !== buildingId || body.object?.consumed) continue;
      body.active = true;
      body.sleeping = false;
      body.sleepTimer = 0;
      body.object.govPhysicsActive = true;
      const obj = body.object || {};
      const gridDx = touchedColX == null ? 0 : Math.abs((obj.govColX ?? touchedColX) - touchedColX);
      const gridDz = touchedRowZ == null ? 0 : Math.abs((obj.govRowZ ?? touchedRowZ) - touchedRowZ);
      const gridDist = gridDx + gridDz;
      const directColumn = gridDist === 0;
      const neighbor = gridDist <= 2;
      const awayFromHole = normalize2(body.position.x - sourceX, body.position.z - sourceZ);
      const awayFromColumn = normalize2(body.position.x - touchedX, body.position.z - touchedZ);
      const shockDir = directColumn
        ? awayFromHole
        : normalize2(awayFromHole.x * 0.58 + awayFromColumn.x * 0.42, awayFromHole.z * 0.58 + awayFromColumn.z * 0.42);
      const floorRatio = Math.max(0, (obj.govFloor || 0) / Math.max(1, (obj.govFloorsY || 1) - 1));
      const distanceFalloff = directColumn ? this.config.activationColumnBoost : Math.max(0.34, 1 - gridDist * 0.18);
      const neighborBoost = neighbor ? this.config.activationNeighborBoost : 0.56;
      const impulse = randomBetween(this.config.activationImpulseMin, this.config.activationImpulseMax) * distanceFalloff * neighborBoost;
      const jitter = this.config.activationJitter + this.config.activationShockJitter * (directColumn ? 1.25 : 0.85);
      body.velocity.x += shockDir.x * impulse + randomBetween(-jitter, jitter);
      body.velocity.z += shockDir.z * impulse + randomBetween(-jitter, jitter);
      body.velocity.y += randomBetween(this.config.activationLiftMin, this.config.activationLiftMax) * (directColumn ? 1.2 : 0.85) + floorRatio * this.config.activationHeightLift;
      const spinScale = directColumn ? 1.18 : (neighbor ? 0.95 : 0.7);
      body.angular.x += randomBetween(-this.config.impactSpin, this.config.impactSpin) * spinScale;
      body.angular.y += randomBetween(-this.config.impactSpin, this.config.impactSpin) * spinScale;
      body.angular.z += randomBetween(-this.config.impactSpin, this.config.impactSpin) * spinScale;
      activated++;
    }
    return activated;
  }

  step(dt) {
    this.accumulator += Math.min(0.05, Math.max(0, dt));
    let steps = 0;
    while (this.accumulator >= this.config.fixedStep && steps < this.config.maxSubSteps) {
      this.stepFixed(this.config.fixedStep);
      this.accumulator -= this.config.fixedStep;
      steps++;
    }
    if (steps >= this.config.maxSubSteps) this.accumulator = 0;
    for (const body of this.bodies) this.syncObject(body);
  }

  stepFixed(dt) {
    for (const body of this.bodies) this.integrate(body, dt);
    for (let i = 0; i < this.config.solverIterations; i++) {
      this.solveContacts();
    }
    for (const body of this.bodies) this.updateSleep(body, dt);
  }

  integrate(body, dt) {
    if (!body.active || body.sleeping || body.object?.falling || body.object?.consumed) return;
    body.onGround = false;
    body.velocity.y -= this.config.gravity * dt;
    body.velocity.y = Math.max(body.velocity.y, -this.config.maxFallSpeed);
    const planarSpeed = Math.hypot(body.velocity.x, body.velocity.z);
    if (planarSpeed > this.config.maxHorizontalSpeed) {
      const scale = this.config.maxHorizontalSpeed / planarSpeed;
      body.velocity.x *= scale;
      body.velocity.z *= scale;
    }
    const angularSpeed = length3(body.angular.x, body.angular.y, body.angular.z);
    if (angularSpeed > this.config.maxAngularSpeed) {
      const scale = this.config.maxAngularSpeed / angularSpeed;
      body.angular.x *= scale;
      body.angular.y *= scale;
      body.angular.z *= scale;
    }
    body.velocity.x *= this.config.airDamping;
    body.velocity.z *= this.config.airDamping;
    body.angular.x *= this.config.angularDamping;
    body.angular.y *= this.config.angularDamping;
    body.angular.z *= this.config.angularDamping;
    body.position.x += body.velocity.x * dt;
    body.position.y += body.velocity.y * dt;
    body.position.z += body.velocity.z * dt;
    const mesh = body.object.mesh;
    mesh.rotation.x += body.angular.x * dt;
    mesh.rotation.y += body.angular.y * dt;
    mesh.rotation.z += body.angular.z * dt;
    this.solveGround(body);
  }

  solveGround(body) {
    const floorY = body.half.y;
    if (body.position.y >= floorY) return;
    body.position.y = floorY;
    body.onGround = true;
    if (body.velocity.y < 0) body.velocity.y = -body.velocity.y * this.config.groundRestitution;
    body.velocity.x *= this.config.groundDamping;
    body.velocity.z *= this.config.groundDamping;
    if (Math.abs(body.velocity.y) < 0.28) body.velocity.y = 0;
    body.angular.x *= this.config.groundDamping;
    body.angular.y *= this.config.groundDamping;
    body.angular.z *= this.config.groundDamping;
  }

  solveContacts() {
    const grid = this.buildGrid();
    const tested = new Set();
    for (const bucket of grid.values()) {
      for (let i = 0; i < bucket.length; i++) {
        for (let j = i + 1; j < bucket.length; j++) {
          const a = bucket[i];
          const b = bucket[j];
          const key = a._gridId < b._gridId ? `${a._gridId}:${b._gridId}` : `${b._gridId}:${a._gridId}`;
          if (tested.has(key)) continue;
          tested.add(key);
          this.solvePair(a, b);
        }
      }
    }
  }

  buildGrid() {
    const grid = new Map();
    let id = 1;
    for (const body of this.bodies) {
      body._gridId = id++;
      if (!body.active || body.sleeping || body.object?.falling || body.object?.consumed) continue;
      const minX = Math.floor((body.position.x - body.half.x) / this.config.cellSize);
      const maxX = Math.floor((body.position.x + body.half.x) / this.config.cellSize);
      const minZ = Math.floor((body.position.z - body.half.z) / this.config.cellSize);
      const maxZ = Math.floor((body.position.z + body.half.z) / this.config.cellSize);
      for (let gx = minX; gx <= maxX; gx++) {
        for (let gz = minZ; gz <= maxZ; gz++) {
          const key = `${gx},${gz}`;
          if (!grid.has(key)) grid.set(key, []);
          grid.get(key).push(body);
        }
      }
    }
    return grid;
  }

  solvePair(a, b) {
    const dx = b.position.x - a.position.x;
    const dy = b.position.y - a.position.y;
    const dz = b.position.z - a.position.z;
    const overlapX = a.half.x + b.half.x - Math.abs(dx);
    const overlapY = a.half.y + b.half.y - Math.abs(dy);
    const overlapZ = a.half.z + b.half.z - Math.abs(dz);
    if (overlapX <= 0 || overlapY <= 0 || overlapZ <= 0) return;

    let nx = Math.sign(dx) || 1;
    let ny = 0;
    let nz = 0;
    let penetration = overlapX;
    if (overlapY < penetration) {
      nx = 0; ny = Math.sign(dy) || 1; nz = 0; penetration = overlapY;
    }
    if (overlapZ < penetration) {
      nx = 0; ny = 0; nz = Math.sign(dz) || 1; penetration = overlapZ;
    }

    const totalInv = a.invMass + b.invMass;
    if (totalInv <= 0) return;
    const correction = Math.max(0, penetration - this.config.contactSlop) / totalInv;
    a.position.x -= nx * correction * a.invMass;
    a.position.y -= ny * correction * a.invMass;
    a.position.z -= nz * correction * a.invMass;
    b.position.x += nx * correction * b.invMass;
    b.position.y += ny * correction * b.invMass;
    b.position.z += nz * correction * b.invMass;

    const rvx = b.velocity.x - a.velocity.x;
    const rvy = b.velocity.y - a.velocity.y;
    const rvz = b.velocity.z - a.velocity.z;
    const velAlongNormal = rvx * nx + rvy * ny + rvz * nz;
    if (velAlongNormal > 0) return;
    const impulse = -(1 + this.config.restitution) * velAlongNormal / totalInv;
    const ix = impulse * nx;
    const iy = impulse * ny;
    const iz = impulse * nz;
    a.velocity.x -= ix * a.invMass;
    a.velocity.y -= iy * a.invMass;
    a.velocity.z -= iz * a.invMass;
    b.velocity.x += ix * b.invMass;
    b.velocity.y += iy * b.invMass;
    b.velocity.z += iz * b.invMass;

    const tangentX = rvx - velAlongNormal * nx;
    const tangentY = rvy - velAlongNormal * ny;
    const tangentZ = rvz - velAlongNormal * nz;
    const tangentLen = length3(tangentX, tangentY, tangentZ);
    if (tangentLen > 0.0001) {
      const frictionImpulse = Math.min(impulse * this.config.friction, tangentLen / totalInv);
      const tx = tangentX / tangentLen;
      const ty = tangentY / tangentLen;
      const tz = tangentZ / tangentLen;
      a.velocity.x += tx * frictionImpulse * a.invMass;
      a.velocity.y += ty * frictionImpulse * a.invMass;
      a.velocity.z += tz * frictionImpulse * a.invMass;
      b.velocity.x -= tx * frictionImpulse * b.invMass;
      b.velocity.y -= ty * frictionImpulse * b.invMass;
      b.velocity.z -= tz * frictionImpulse * b.invMass;
    }

    const spin = clamp(impulse * 0.075, 0, this.config.impactSpin);
    a.angular.x += randomBetween(-spin, spin);
    a.angular.z += randomBetween(-spin, spin);
    b.angular.x += randomBetween(-spin, spin);
    b.angular.z += randomBetween(-spin, spin);
    a.sleeping = false;
    b.sleeping = false;
    a.sleepTimer = 0;
    b.sleepTimer = 0;
  }

  updateSleep(body, dt) {
    if (!body.active || body.object?.falling || body.object?.consumed) return;
    const speed = length3(body.velocity.x, body.velocity.y, body.velocity.z);
    const angular = length3(body.angular.x, body.angular.y, body.angular.z);
    if (body.onGround && speed < this.config.sleepSpeed && angular < this.config.sleepAngular) {
      body.sleepTimer += dt;
      if (body.sleepTimer >= this.config.sleepDelay) {
        body.sleeping = true;
        body.velocity.x = 0; body.velocity.y = 0; body.velocity.z = 0;
        body.angular.x = 0; body.angular.y = 0; body.angular.z = 0;
      }
    } else {
      body.sleepTimer = 0;
      body.sleeping = false;
    }
  }

  syncObject(body) {
    const obj = body.object;
    if (!obj?.mesh || obj.consumed || obj.falling) return;
    obj.mesh.position.set(body.position.x, body.position.y, body.position.z);
    obj.x = body.position.x;
    obj.z = body.position.z;
    obj.govVx = body.velocity.x;
    obj.govVy = body.velocity.y;
    obj.govVz = body.velocity.z;
    obj.govAvx = body.angular.x;
    obj.govAvy = body.angular.y;
    obj.govAvz = body.angular.z;
    obj.govPhysicsActive = body.active;
    obj.govPhysicsSleeping = body.sleeping;
  }
}
