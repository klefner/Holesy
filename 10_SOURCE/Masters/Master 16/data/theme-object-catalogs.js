const slug = value => value.toLowerCase().replace(/[^a-z0-9]+/g, '_').replace(/^_|_$/g, '');

const RAILGATE_GROUPS = Object.freeze({
  rail: [
    'Straight Rail Section', 'Curved Rail Section', 'Switch Points', 'Rail Crossing Panel',
    'Buffer Stop', 'Signal Mast', 'Dwarf Signal', 'Milepost', 'Trackside Relay Box',
    'Crossing Gate', 'Crossing Bell', 'Rail Warning Sign', 'Loose Rail Length',
    'Sleeper Tie Stack', 'Spike Bucket', 'Coupling Hook', 'Brake Hose Coil',
    'Wheel Chock', 'Detached Train Wheelset', 'Broken Axle',
  ],
  station: [
    'Platform Bench', 'Station Clock', 'Ticket Kiosk', 'Ticket Validator', 'Timetable Board',
    'Luggage Trolley', 'Suitcase', 'Travel Trunk', 'Parcel Sack', 'Newspaper Bundle',
    'Platform Trash Bin', 'Platform Lamp', 'Station Payphone', 'Bicycle Rack',
    'Parked Bicycle', 'Station Canopy Section', 'Platform Barrier', 'Vending Machine',
    'Lost And Found Crate', 'Station Loudspeaker',
  ],
  freight: [
    'Closed Freight Container', 'Open Freight Container', 'Coal Bin', 'Cargo Wagon',
    'Empty Flat Wagon', 'Open Gondola Wagon', 'Coal Hopper', 'Tanker Wagon', 'Boxcar',
    'Caboose', 'Cargo Pallet', 'Timber Bundle', 'Pipe Bundle', 'Steel Coil', 'Oil Drum',
    'Cable Spool', 'Loading Ramp', 'Warehouse Handcart', 'Yard Forklift', 'Yard Generator',
  ],
  transit: [
    'Steam Locomotive', 'Coal Tender', 'Passenger Coach', 'High Speed Cab',
    'High Speed Coach', 'Commuter Train', 'City Bus', 'School Bus', 'Railgate Taxi',
    'Railgate Ambulance', 'Rail Police Car', 'Maintenance Pickup', 'Rail Inspection Cart',
    'Station Shuttle Van', 'Rail Tow Truck',
  ],
  crossing: [
    'Rail Traffic Cone', 'Crossing Traffic Light', 'Crossing Stop Sign', 'Crossing Yield Sign',
    'Rail Crossing Crossbuck', 'Rail Detour Sign', 'Crossing Bollard', 'Concrete Rail Barrier',
    'Rail Yard Hydrant', 'Rail Yard Manhole Cover', 'Trackside Storm Drain',
    'Railgate Streetlamp', 'Bus Stop Pole', 'Railgate Bus Shelter', 'Station Parking Meter',
  ],
  landmark: [
    'Station House', 'Freight Depot', 'Signal Cabin', 'Rail Warehouse', 'Railway Corner Shop',
    'Railway Apartment Block', 'Railgate Bank', 'Railgate Hospital', 'Railway Hotel',
    'Maintenance Shed',
  ],
});

const IMPORTED_ASSET_LABELS = new Set([
  'Straight Rail Section', 'Curved Rail Section', 'Closed Freight Container',
  'Open Freight Container', 'Cargo Wagon', 'Empty Flat Wagon', 'Open Gondola Wagon',
  'Coal Hopper', 'Steam Locomotive', 'Coal Tender', 'Passenger Coach', 'High Speed Cab',
  'High Speed Coach', 'Commuter Train', 'City Bus', 'School Bus', 'Railgate Taxi',
  'Railgate Ambulance', 'Rail Police Car', 'Rail Traffic Cone', 'Crossing Traffic Light',
  'Railway Corner Shop', 'Railway Apartment Block', 'Railgate Bank', 'Railgate Hospital',
]);

const FAMILY_DEFAULTS = Object.freeze({
  rail: { role: 'infrastructure', zones: ['rail_corridor', 'yard'], sizeClass: 'small', routeAffinity: 'rail' },
  station: { role: 'passenger', zones: ['platform', 'station'], sizeClass: 'small', routeAffinity: 'platform' },
  freight: { role: 'freight', zones: ['yard', 'warehouse'], sizeClass: 'medium', routeAffinity: 'rail' },
  transit: { role: 'vehicle', zones: ['rail_corridor', 'street'], sizeClass: 'large', routeAffinity: 'route' },
  crossing: { role: 'street', zones: ['crossing', 'street'], sizeClass: 'small', routeAffinity: 'crossing' },
  landmark: { role: 'building', zones: ['station', 'commercial'], sizeClass: 'structure', routeAffinity: 'site' },
});

export const RAILGATE_OBJECT_CATALOG = Object.freeze(
  Object.entries(RAILGATE_GROUPS).flatMap(([family, labels]) => labels.map((label, index) => {
    const defaults = FAMILY_DEFAULTS[family];
    const imported = IMPORTED_ASSET_LABELS.has(label);
    return Object.freeze({
      id: `railgate_${slug(label)}`,
      label,
      themeIds: Object.freeze(['railgate']),
      family,
      role: defaults.role,
      sourceAssetKey: imported ? `railgate/${slug(label)}` : null,
      factoryId: imported ? 'imported_glb' : family === 'landmark' ? 'compound_structure' : 'procedural_compound',
      variantRecipe: slug(label),
      sizeClass: defaults.sizeClass,
      radius: family === 'landmark' ? 5.5 : family === 'transit' ? 2.8 : family === 'freight' ? 1.8 : 0.8,
      value: family === 'landmark' ? 240 : family === 'transit' ? 110 : family === 'freight' ? 58 : 24,
      weight: index < 4 ? 1.25 : 1,
      minCount: index < 2 ? 2 : 0,
      maxCount: family === 'landmark' ? 2 : family === 'transit' ? 5 : 12,
      zones: Object.freeze(defaults.zones),
      routeAffinity: defaults.routeAffinity,
      mobility: family === 'transit' ? 'route' : 'static',
      destructibleProfile: family === 'landmark' ? 'imported_building_stack' : family === 'transit' ? 'articulated_vehicle' : 'whole_object',
      objectiveTags: Object.freeze([family, defaults.role]),
      eraTags: Object.freeze(['industrial', 'modern']),
      perfCost: imported || family === 'landmark' ? 'medium' : 'low',
      lodPolicy: imported ? 'theme_distance_lod' : 'shared_geometry',
      audioProfile: family === 'transit' ? 'rail_vehicle' : family === 'rail' ? 'rail_metal' : 'theme_object',
    });
  }))
);

if (RAILGATE_OBJECT_CATALOG.length !== 100) {
  throw new Error(`Railgate catalog must contain exactly 100 distinct objects; found ${RAILGATE_OBJECT_CATALOG.length}.`);
}

const SHARED_THEME_LABELS = Object.freeze([
  'Compact Car', 'Family Sedan', 'Sport Utility Vehicle', 'Taxi Cab', 'Police Cruiser',
  'Sports Car', 'Ambulance', 'City Bus', 'School Bus', 'Bicycle', 'Traffic Cone',
  'Traffic Light', 'Fire Hydrant', 'Streetlamp', 'Mailbox', 'Trash Bin', 'Wood Pallet',
  'Utility Box', 'Concrete Barrier', 'Portable Generator',
]);

export const SHARED_THEME_OBJECT_CATALOG = Object.freeze(SHARED_THEME_LABELS.map(label => Object.freeze({
  id: `shared_${slug(label)}`,
  label,
  themeIds: Object.freeze(['classic', 'megakitDowntown', 'railgate']),
  family: label.includes('Car') || label.includes('Vehicle') || label.includes('Taxi') || label.includes('Cruiser')
    ? 'vehicle'
    : 'street',
  sourceAssetKey: null,
  factoryId: 'shared_existing',
  weight: 1,
  minCount: 0,
  maxCount: 10,
  objectiveTags: Object.freeze(['shared']),
})));

export function selectThemeObjectPool(themeId, rng = Math.random, limit = 36) {
  const owned = themeId === 'railgate' ? RAILGATE_OBJECT_CATALOG : [];
  const shared = SHARED_THEME_OBJECT_CATALOG.filter(entry => entry.themeIds.includes(themeId));
  const eligible = [...owned, ...shared];
  const required = eligible.filter(entry => (entry.minCount || 0) > 0);
  const optional = eligible
    .filter(entry => (entry.minCount || 0) === 0)
    .map(entry => ({ entry, key: Math.pow(Math.max(0.0001, rng()), 1 / Math.max(0.01, entry.weight || 1)) }))
    .sort((a, b) => b.key - a.key)
    .map(item => item.entry);
  return Object.freeze([...required, ...optional].slice(0, Math.max(required.length, limit)));
}
