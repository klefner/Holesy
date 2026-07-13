# QA Review - Master 16.115 Native Haptics Bridge

Date: 2026-07-09

## Scope

User requested whatever is needed to enable haptics after confirming mobile testing is on Chrome for iPhone.

## Finding

Chrome on iPhone does not give Holesy a browser vibration API path. iPhone haptics require a native or hybrid app shell that exposes native haptic APIs to the game.

## Changes Reviewed

- `10_SOURCE/Masters/Master 16/js/main.js`
  - Adds native haptics bridge detection before browser vibration.
  - Supports Capacitor Haptics at `Capacitor.Plugins.Haptics`.
  - Supports an optional custom bridge at `window.HolesyNativeHaptics`.
  - Maps existing gameplay haptic event types to native impact, notification, or vibrate calls.
  - Keeps Android/web `navigator.vibrate` fallback when no native bridge exists.
- `00_ADMIN/Requirements/NATIVE_HAPTICS_BRIDGE.md`
  - Documents the native bridge contract and validation expectations.

## Verification

- `node --check 10_SOURCE/Masters/Master 16/js/main.js` passed.
- `node --check 10_SOURCE/Masters/Master 16/js/build-info.js` passed.
- Source files were mirrored to `40_RELEASE/Website_Publish_Package/holesy/`.
- Fresh full upload copy created at `C:\Users\KentLefner\Downloads\holesy-godaddy-upload-master-16.115\holesy\`.

## Notes

- This enables the game-side integration needed for iPhone haptics.
- It does not make browser-only iPhone Chrome vibrate; a Capacitor/native shell is still required.
