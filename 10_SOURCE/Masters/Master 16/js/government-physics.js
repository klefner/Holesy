export const GOVERNMENT_PHYSICS_CONFIG = Object.freeze({
  fixedStep: 1 / 60,
  maxSubSteps: 3,
  gravity: 24,
  maxFallSpeed: 34,
  maxHorizontalSpeed: 18,
  maxAngularSpeed: 12,
  cellSize: 3.2,
  solverIterations: 6,
  restitution: 0.44,
  groundRestitution: 0.26,
  friction: 0.72,
  airDamping: 0.992,
  groundDamping: 0.82,
  angularDamping: 0.9,
  sleepSpeed: 0.08,
  sleepAngular: 0.1,
  sleepDelay: 0.72,
  contactSlop: 0.012,
  activationImpulseMin: 5.6,
  activationImpulseMax: 11.4,
  activationLiftMin: 2.1,
  activationLiftMax: 7.4,
  activationJitter: 1.9,
  activationColumnBoost: 2.25,
  activationNeighborBoost: 0.95,
  activationHeightLift: 1.1,
  activationShockJitter: 1.25,
  shakeDuration: 0.58,
  shakeDistance: 0.34,
  toppleAngleMin: 0.28,
  toppleAngleMax: 0.84,
  toppleOffset: 0.9,
  releaseDelayBase: 0.08,
  releaseDelayGrid: 0.11,
  releaseDelayFloor: 0.14,
  releaseImpulseTopple: 4.3,
  columnLeanImpulse: 3.4,
  blastSeparation: 0.42,
  sideImpactHop: 1.05,
  sideImpactScatter: 0.62,
  impactSpin: 5.8,
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
      released: options.released ?? !!options.active,
      sleeping: false,
      sleepTimer: 0,
      onGround: false,
      basePosition: {
        x: object.mesh.position.x,
        y: object.mesh.position.y,
        z: object.mesh.position.z,
      },
      baseRotation: {
        x: object.mesh.rotation.x,
        y: object.mesh.rotation.y,
        z: object.mesh.rotation.z,
      },
      releaseDelay: options.releaseDelay || 0,
      releaseDelayStart: options.releaseDelay || 0,
      shakeTimer: 0,
      toppleAngle: 0,
      toppleDir: { x: 0, z: 0 },
      pendingVelocity: { x: 0, y: 0, z: 0 },
      pendingAngular: { x: 0, y: 0, z: 0 },
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
      body.released = false;
      body.sleeping = false;
      body.sleepTimer = 0;
      body.object.govPhysicsActive = true;
      body.object.govPhysicsReleased = false;
      body.basePosition.x = body.position.x;
      body.basePosition.y = body.position.y;
      body.basePosition.z = body.position.z;
      body.baseRotation.x = body.object.mesh.rotation.x;
      body.baseRotation.y = body.object.mesh.rotation.y;
      body.baseRotation.z = body.object.mesh.rotation.z;
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
      const lift = randomBetween(this.config.activationLiftMin, this.config.activationLiftMax) * (directColumn ? 1.28 : 0.86) + floorRatio * this.config.activationHeightLift;
      body.pendingVelocity.x = body.velocity.x + shockDir.x * impulse + randomBetween(-jitter, jitter);
      body.pendingVelocity.z = body.velocity.z + shockDir.z * impulse + randomBetween(-jitter, jitter);
      body.pendingVelocity.y = body.velocity.y + lift;
      const spinScale = directColumn ? 1.18 : (neighbor ? 0.95 : 0.7);
      body.pendingAngular.x = body.angular.x + randomBetween(-this.config.impactSpin, this.config.impactSpin) * spinScale;
      body.pendingAngular.y = body.angular.y + randomBetween(-this.config.impactSpin, this.config.impactSpin) * spinScale;
      body.pendingAngular.z = body.angular.z + randomBetween(-this.config.impactSpin, this.config.impactSpin) * spinScale;
      body.toppleDir = normalize2(shockDir.x * 0.72 + awayFromColumn.x * 0.28, shockDir.z * 0.72 + awayFromColumn.z * 0.28);
      body.toppleAngle = randomBetween(this.config.toppleAngleMin, this.config.toppleAngleMax) * (0.55 + floorRatio) * (directColumn ? 1.18 : 0.82);
      const delay = this.config.releaseDelayBase
        + gridDist * this.config.releaseDelayGrid
        + floorRatio * this.config.releaseDelayFloor
        + randomBetween(0, directColumn ? 0.055 : 0.14);
      body.releaseDelay = Math.max(0, directColumn ? delay * 0.55 : delay);
      body.releaseDelayStart = Math.max(0.001, body.releaseDelay);
      body.shakeTimer = this.config.shakeDuration;
      body.position.x += shockDir.x * this.config.blastSeparation * (directColumn ? 1 : 0.45);
      body.position.z += shockDir.z * this.config.blastSeparation * (directColumn ? 1 : 0.45);
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
    if (!body.released) {
      this.updateStagedActivation(body, dt);
      return;
    }
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

  updateStagedActivation(body, dt) {
    body.releaseDelay = Math.max(0, body.releaseDelay - dt);
    body.shakeTimer = Math.max(0, body.shakeTimer - dt);
    const obj = body.object || {};
    const floorRatio = Math.max(0, (obj.govFloor || 0) / Math.max(1, (obj.govFloorsY || 1) - 1));
    const progress = clamp(1 - body.releaseDelay / body.releaseDelayStart, 0, 1);
    const shakePhase = (body.shakeTimer / Math.max(0.001, this.config.shakeDuration));
    const shakeAmp = this.config.shakeDistance * shakePhase * (1.1 + floorRatio);
    const wave = Math.sin((body.releaseDelayStart - body.releaseDelay + body.mass) * 72);
    const sideWave = Math.cos((body.releaseDelayStart - body.releaseDelay + body.mass) * 49);
    const leanOffset = this.config.toppleOffset * progress * progress * (0.25 + floorRatio);
    body.position.x = body.basePosition.x + body.toppleDir.x * leanOffset + body.toppleDir.z * wave * shakeAmp;
    body.position.y = body.basePosition.y + Math.abs(wave) * shakeAmp * 0.42;
    body.position.z = body.basePosition.z + body.toppleDir.z * leanOffset - body.toppleDir.x * sideWave * shakeAmp;
    const mesh = body.object.mesh;
    const leanAngle = body.toppleAngle * progress;
    mesh.rotation.x = body.baseRotation.x + body.toppleDir.z * leanAngle + sideWave * shakeAmp * 0.55;
    mesh.rotation.y = body.baseRotation.y + wave * shakeAmp * 0.35;
    mesh.rotation.z = body.baseRotation.z - body.toppleDir.x * leanAngle + wave * shakeAmp * 0.55;
    if (body.releaseDelay <= 0) this.releaseBody(body, progress);
  }

  releaseBody(body, progress = 1) {
    body.released = true;
    body.sleeping = false;
    body.sleepTimer = 0;
    body.velocity.x = body.pendingVelocity.x + body.toppleDir.x * this.config.releaseImpulseTopple * progress;
    body.velocity.y = body.pendingVelocity.y + progress * 0.9;
    body.velocity.z = body.pendingVelocity.z + body.toppleDir.z * this.config.releaseImpulseTopple * progress;
    body.angular.x = body.pendingAngular.x + body.toppleDir.z * this.config.releaseImpulseTopple;
    body.angular.y = body.pendingAngular.y + randomBetween(-this.config.impactSpin, this.config.impactSpin) * 0.42;
    body.angular.z = body.pendingAngular.z - body.toppleDir.x * this.config.releaseImpulseTopple;
    body.object.govPhysicsReleased = true;
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
    if (Math.abs(body.angular.x) + Math.abs(body.angular.z) > 0.45) {
      const slide = Math.min(0.16, (Math.abs(body.angular.x) + Math.abs(body.angular.z)) * 0.015);
      body.velocity.x += Math.sign(body.angular.z || randomBetween(-1, 1)) * slide;
      body.velocity.z -= Math.sign(body.angular.x || randomBetween(-1, 1)) * slide;
    }
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
      if (!body.active || !body.released || body.sleeping || body.object?.falling || body.object?.consumed) continue;
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

    const spin = clamp(impulse * 0.16, 0, this.config.impactSpin);
    const tangentKick = tangentLen > 0.0001 ? clamp(tangentLen * 0.08, 0, spin) : spin * 0.35;
    if (Math.abs(nx) > 0) {
      a.angular.z -= nx * spin * a.invMass;
      b.angular.z += nx * spin * b.invMass;
    } else if (Math.abs(nz) > 0) {
      a.angular.x += nz * spin * a.invMass;
      b.angular.x -= nz * spin * b.invMass;
    } else {
      a.angular.x += randomBetween(-spin, spin);
      a.angular.z += randomBetween(-spin, spin);
      b.angular.x += randomBetween(-spin, spin);
      b.angular.z += randomBetween(-spin, spin);
    }
    a.angular.y += randomBetween(-tangentKick, tangentKick);
    b.angular.y += randomBetween(-tangentKick, tangentKick);
    if (ny === 0 && impulse > 0.85) {
      const hop = Math.min(this.config.sideImpactHop, impulse * 0.045);
      a.velocity.y += hop * a.invMass;
      b.velocity.y += hop * b.invMass;
      a.velocity.x -= nx * this.config.sideImpactScatter * a.invMass;
      a.velocity.z -= nz * this.config.sideImpactScatter * a.invMass;
      b.velocity.x += nx * this.config.sideImpactScatter * b.invMass;
      b.velocity.z += nz * this.config.sideImpactScatter * b.invMass;
      a.angular.x += nz * this.config.columnLeanImpulse * 0.18;
      a.angular.z -= nx * this.config.columnLeanImpulse * 0.18;
      b.angular.x -= nz * this.config.columnLeanImpulse * 0.18;
      b.angular.z += nx * this.config.columnLeanImpulse * 0.18;
    }
    a.sleeping = false;
    b.sleeping = false;
    a.sleepTimer = 0;
    b.sleepTimer = 0;
  }

  updateSleep(body, dt) {
    if (!body.active || !body.released || body.object?.falling || body.object?.consumed) return;
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
    obj.govPhysicsReleased = body.released;
    obj.govPhysicsSleeping = body.sleeping;
  }
}
