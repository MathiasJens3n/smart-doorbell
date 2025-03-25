import {
  deleteToken,
  getMessagingInWindow,
  getToken,
  isWindowSupported,
  onMessage
} from "./chunk-TYGWYUQM.js";
import {
  FirebaseApp,
  FirebaseApps,
  VERSION,
  ɵAngularFireSchedulers,
  ɵgetAllInstancesOf,
  ɵgetDefaultInstanceOf,
  ɵzoneWrap
} from "./chunk-JYQRG4X3.js";
import {
  registerVersion
} from "./chunk-QN2KVYTI.js";
import {
  isPlatformServer
} from "./chunk-RLI5KEP3.js";
import {
  InjectionToken,
  Injector,
  NgModule,
  NgZone,
  Optional,
  PLATFORM_ID,
  makeEnvironmentProviders,
  setClassMetadata,
  ɵɵdefineInjector,
  ɵɵdefineNgModule
} from "./chunk-N2APCJKI.js";
import {
  concatMap,
  distinct,
  from,
  timer
} from "./chunk-P6U2JBMQ.js";
import "./chunk-WDMUDEB6.js";

// node_modules/@angular/fire/fesm2022/angular-fire-messaging.mjs
var Messaging = class {
  constructor(messaging) {
    return messaging;
  }
};
var MESSAGING_PROVIDER_NAME = "messaging";
var MessagingInstances = class {
  constructor() {
    return ɵgetAllInstancesOf(MESSAGING_PROVIDER_NAME);
  }
};
var messagingInstance$ = timer(0, 300).pipe(concatMap(() => from(ɵgetAllInstancesOf(MESSAGING_PROVIDER_NAME))), distinct());
var PROVIDED_MESSAGING_INSTANCES = new InjectionToken("angularfire2.messaging-instances");
function defaultMessagingInstanceFactory(provided, defaultApp, platformId) {
  if (isPlatformServer(platformId)) {
    return null;
  }
  const defaultMessaging = ɵgetDefaultInstanceOf(MESSAGING_PROVIDER_NAME, provided, defaultApp);
  return defaultMessaging && new Messaging(defaultMessaging);
}
function messagingInstanceFactory(fn) {
  return (zone, injector, platformId) => {
    if (isPlatformServer(platformId)) {
      return null;
    }
    const messaging = zone.runOutsideAngular(() => fn(injector));
    return new Messaging(messaging);
  };
}
var MESSAGING_INSTANCES_PROVIDER = {
  provide: MessagingInstances,
  deps: [[new Optional(), PROVIDED_MESSAGING_INSTANCES]]
};
var DEFAULT_MESSAGING_INSTANCE_PROVIDER = {
  provide: Messaging,
  useFactory: defaultMessagingInstanceFactory,
  deps: [[new Optional(), PROVIDED_MESSAGING_INSTANCES], FirebaseApp, PLATFORM_ID]
};
var MessagingModule = class _MessagingModule {
  constructor() {
    registerVersion("angularfire", VERSION.full, "fcm");
  }
  static ɵfac = function MessagingModule_Factory(__ngFactoryType__) {
    return new (__ngFactoryType__ || _MessagingModule)();
  };
  static ɵmod = ɵɵdefineNgModule({
    type: _MessagingModule
  });
  static ɵinj = ɵɵdefineInjector({
    providers: [DEFAULT_MESSAGING_INSTANCE_PROVIDER, MESSAGING_INSTANCES_PROVIDER]
  });
};
(() => {
  (typeof ngDevMode === "undefined" || ngDevMode) && setClassMetadata(MessagingModule, [{
    type: NgModule,
    args: [{
      providers: [DEFAULT_MESSAGING_INSTANCE_PROVIDER, MESSAGING_INSTANCES_PROVIDER]
    }]
  }], () => [], null);
})();
function provideMessaging(fn, ...deps) {
  registerVersion("angularfire", VERSION.full, "fcm");
  return makeEnvironmentProviders([DEFAULT_MESSAGING_INSTANCE_PROVIDER, MESSAGING_INSTANCES_PROVIDER, {
    provide: PROVIDED_MESSAGING_INSTANCES,
    useFactory: messagingInstanceFactory(fn),
    multi: true,
    deps: [NgZone, Injector, PLATFORM_ID, ɵAngularFireSchedulers, FirebaseApps, ...deps]
  }]);
}
var deleteToken2 = ɵzoneWrap(deleteToken, true, 2);
var getMessaging = ɵzoneWrap(getMessagingInWindow, true);
var getToken2 = ɵzoneWrap(getToken, true);
var isSupported = ɵzoneWrap(isWindowSupported, false);
var onMessage2 = ɵzoneWrap(onMessage, false);
export {
  Messaging,
  MessagingInstances,
  MessagingModule,
  deleteToken2 as deleteToken,
  getMessaging,
  getToken2 as getToken,
  isSupported,
  messagingInstance$,
  onMessage2 as onMessage,
  provideMessaging
};
//# sourceMappingURL=@angular_fire_messaging.js.map
