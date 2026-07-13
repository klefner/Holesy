# QA Review - Master 16.116 iOS Web Haptic Fallback

Date: 2026-07-09

## Scope

User asked to search online for an iPhone Chrome haptics solution after browser vibration and native bridge preparation still produced no haptics in the browser URL.

## Finding

The current public compatibility data still shows iOS browser vibration as unsupported. A nonstandard iOS WebKit switch-control haptic workaround is discussed in public developer threads, but it is not a full replacement for native haptics and may vary by iOS version.

## Changes Reviewed

- `10_SOURCE/Masters/Master 16/js/main.js`
  - Adds a hidden iOS `input type="checkbox"` with the nonstandard `switch` attribute.
  - Attempts that fallback after native haptics and before reporting browser vibration unsupported.
  - Reports `iOS Tick` on the Haptics button when the fallback is triggered.
  - Keeps native Capacitor/custom bridge support and Android/browser vibration fallback intact.

## Verification

- `node --check 10_SOURCE/Masters/Master 16/js/main.js` passed.
- `node --check 10_SOURCE/Masters/Master 16/js/build-info.js` passed.
- Source files were mirrored to `40_RELEASE/Website_Publish_Package/holesy/`.
- Fresh full upload copy created at `C:\Users\KentLefner\Downloads\holesy-godaddy-upload-master-16.116\holesy\`.

## Notes

- If the button reports `iOS Tick` but no physical feedback is felt, that iOS/browser version is not honoring the workaround.
- The reliable commercial iPhone path remains a native shell using the `Master 16.115` native bridge.
