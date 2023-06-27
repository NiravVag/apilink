/***************************************************************************************************
 * Load `$localize` onto the global scope - used if i18n tags appear in Angular templates.
 */
import '@angular/localize/init';
import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';
import { UserModel } from './app/_Models/user/user.model';
import { AppModule } from './app/app.module';
import { environment } from './environments/environment';

const config = require("./assets/appconfig/appconfig.json");

export function getBaseUrl() {
  return config.APP.APISERVER;
}

export function getFileServerUrl() {
  return config.APP.FILESERVER;
}

export function getDevEnvironment() {
  return config.APP.DEV;
}

export function getCurrentUser() {
  if (localStorage.getItem('currentUser')) {
    var user = JSON.parse(localStorage.getItem('currentUser'));
    return <UserModel>user;
  }
}

export function getPublicKey() {
  //https://passos.com.au/push-notifications-angular-net-core/
  //https://web-push-codelab.glitch.me/
  return "BKJJKiknd7N64gnpwYu9FWvda_5Q7BJgLEMvX8vxJa1DgVlHpz6uT69kyMnYmbtYXyKcQdy4KolGGNhgVODEtWI";
}


const providers = [
  { provide: 'BASE_URL', useFactory: getBaseUrl, deps: [] },
  { provide: 'CURRENT_USER', useFactory: getCurrentUser, deps: [] },
  { provide: 'APP_PUBLIC_KEY', useFactory: getPublicKey, deps: [] },
  { provide: 'FILE_SERVER_URL', useFactory: getFileServerUrl, deps: [] },
  { provide: 'DEV_ENV', useFactory: getDevEnvironment, deps: [] }  
];

if (environment.production) {
  enableProdMode();
}

platformBrowserDynamic(providers).bootstrapModule(AppModule)
  .catch(err => console.log(err));

export { renderModule, renderModuleFactory } from '@angular/platform-server';
