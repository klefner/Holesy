# QA Review - Master 16.117 Capacitor Native Shell

Date: 2026-07-09

## Scope

User approved proceeding with whatever is needed to enable iPhone haptics after browser-only iPhone Chrome failed `navigator.vibrate` and the iOS switch fallback.

## Changes Reviewed

- Root project
  - Adds `package.json`, `package-lock.json`, and `capacitor.config.json`.
  - Adds `node_modules/` to `.gitignore`.
  - Installs Capacitor 8 packages plus `@capacitor/haptics`.
- Native shells
  - Generates `android/`.
  - Generates `ios/`.
  - Points Capacitor `webDir` to `40_RELEASE/Website_Publish_Package/holesy`.
  - Adds Android `VIBRATE` permission.
- Documentation
  - Adds `00_ADMIN/Requirements/NATIVE_APP_CAPACITOR_WORKFLOW.md`.

## Verification

- `npm install @capacitor/core@latest @capacitor/haptics@latest @capacitor/cli@latest @capacitor/ios@latest @capacitor/android@latest --save-exact` completed with zero vulnerabilities reported.
- `npx cap add android` completed and detected `@capacitor/haptics`.
- `npx cap add ios` completed and detected `@capacitor/haptics`.
- `npm run cap:sync` completed and copied the governed modular release package into both native shells.
- `node --check 10_SOURCE/Masters/Master 16/js/main.js` passed.
- `node --check 10_SOURCE/Masters/Master 16/js/build-info.js` passed.

## Notes

- This prepares the native shell required for real iPhone haptics.
- Installing to an iPhone still requires Xcode on macOS or an iOS-capable cloud build service.
- Browser-only iPhone Chrome is still not expected to provide physical haptics.
