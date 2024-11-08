// This file can be replaced during build by using the `fileReplacements` array.
// `ng build` replaces `environment.ts` with `environment.prod.ts`.
// The list of file replacements can be found in `angular.json`.

export const environment = {
  production: false,
  basedUrl:'https://localhost:7126/api',
  basedUrlResource:'https://localhost:7126/resources',
  limitRowCount:5,
  userTokenName:'userJwtToken',
  STATUS_UNAUTHORIZED:401,
  googleClientId:'359190181437-h0ppaoeval47vl6bhg5m6vmldskepah8.apps.googleusercontent.com',
  facebookClientId:'971651790786067'
};

/*
 * For easier debugging in development mode, you can import the following file
 * to ignore zone related error stack frames such as `zone.run`, `zoneDelegate.invokeTask`.
 *
 * This import should be commented out in production mode because it will have a negative impact
 * on performance if an error is thrown.
 */
// import 'zone.js/plugins/zone-error';  // Included with Angular CLI.
