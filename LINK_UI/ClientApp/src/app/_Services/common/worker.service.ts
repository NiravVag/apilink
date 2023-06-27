import { Injectable, Inject } from "@angular/core";
import { HttpClient } from "@angular/common/http";
import { map, first } from "rxjs/operators";
import { PushWorkerStatus } from "../../_Models/worker/worker.model";
import { AuthenticationService } from "../user/authentication.service";

//Injectable({
//  providedIn: 'root'
//})
@Injectable()
export class WorkerMiddlewareService {

  public pushWorkerStatus: PushWorkerStatus;
  public swRegistration: any;
  private userId: number;
  private static eventsOnGetTask: Array<any> = [];
  private static eventsOnGetNotification: Array<any> = [];
  private static isInit: boolean = false;
  private static i: number = 0;

  constructor(private http: HttpClient, @Inject('BASE_URL') private baseUrl: string, @Inject('APP_PUBLIC_KEY') private pubKey: string, private authService: AuthenticationService) {
    this.pushWorkerStatus = new PushWorkerStatus();
    WorkerMiddlewareService.i++;
   // console.log("i=" + WorkerMiddlewareService.i);
  }

  public subscribeOnGetTask = (func: any) => {

    if (!WorkerMiddlewareService.eventsOnGetTask.some(x => x.toString() == func.toString()))
      WorkerMiddlewareService.eventsOnGetTask.push(func);

  //  for (let item of WorkerMiddlewareService.eventsOnGetTask)
    //  console.log(item.toString());

    //console.log(WorkerMiddlewareService.eventsOnGetTask.length);

  }

  public unSubscribeOnGetTask = (func: any) => {

    var index = WorkerMiddlewareService.eventsOnGetTask.indexOf(func);

    if (index > -1)
      WorkerMiddlewareService.eventsOnGetTask.splice(index, 1);
  }

  public subscribeOnGetNotification = (func: any) => {

    if (!WorkerMiddlewareService.eventsOnGetNotification.some(x => x.toString() == func.toString()))
      WorkerMiddlewareService.eventsOnGetNotification.push(func);
  }

  public unSubscribeOnGetNotification = (func: any) => {

    var index = WorkerMiddlewareService.eventsOnGetNotification.indexOf(func);

    if (index > -1)
      WorkerMiddlewareService.eventsOnGetNotification.splice(index, 1);
  }

  unregister() {
    WorkerMiddlewareService.isInit = false;;
    if (navigator.serviceWorker != null) {
      navigator.serviceWorker.getRegistrations().then(function (registrations) {
      //  console.log("ici");
        for (let registration of registrations) {
        //  console.log(registration);
          registration.unregister()
        }
      })
    }
  }

  init() {
    let user = this.authService.getCurrentUser();

    if (WorkerMiddlewareService.isInit) {
      if (user)
        this.userId = user.id;
    }
    else if ('serviceWorker' in navigator && 'PushManager' in window) {

      if (user) {
        WorkerMiddlewareService.isInit = true;
        this.userId = user.id;

        navigator.serviceWorker.register('assets/sw.js')
          .then(swReg => {
            this.swRegistration = swReg;
            this.checkSubscription();

          })
          .catch(error => {
            console.error('Service Worker Error', error);
          });
        this.pushWorkerStatus.isSupported = true;

        var options = { tag: 'user_alerts' };
        navigator.serviceWorker.addEventListener('message', (event) => {
         // console.log(event);
          if (event.data.TypeId == "Task") {
          //  console.log(WorkerMiddlewareService.eventsOnGetTask.length);
            for (let item of WorkerMiddlewareService.eventsOnGetTask)
              item(event.data);
          }
          else {
            for (let item of WorkerMiddlewareService.eventsOnGetNotification)
              item(event.data);
          }
        });
        navigator.serviceWorker.ready.then(function (registration) {
          registration.getNotifications(options).then(function (notifications) {
          })
        });
      }
      else {
        console.log("No User is found in local storage");
        this.pushWorkerStatus.isSupported = false;
      }


    } else {
      console.log("service worker  is not supported")
      this.pushWorkerStatus.isSupported = false;
    }
  }

  checkSubscription() {
    this.swRegistration.pushManager.getSubscription()
      .then(subscription => {
        this.pushWorkerStatus.isSubscribed = !(subscription === null);
        this.subscribe();
      });
  }

  subscribe() {
    this.pushWorkerStatus.isInProgress = true;
    //check the source code to get the method below
   // console.log("key:" + this.pubKey);
    const applicationServerKey = this.urlBase64ToUint8Array(this.pubKey);
    this.swRegistration.pushManager.subscribe({
      userVisibleOnly: true,
      applicationServerKey: applicationServerKey
    }).then(subscription => {
   //   console.log("subscription");
    //  console.log(JSON.stringify(subscription));

      var newSub = JSON.parse(JSON.stringify(subscription));

      this.http.post<PushSubscription>(`${this.baseUrl}api/user/subscribe/${this.userId}`, {
        auth: newSub.keys.auth,
        p256Dh: newSub.keys.p256dh,
        endPoint: newSub.endpoint
      }).pipe()
        .subscribe(response => {
          this.pushWorkerStatus.isSubscribed = true;
        },
          error => {
            console.log(error);
          });
    })
      .catch(err => {
        console.log('Failed to subscribe the user: ', err);
      })
      .then(() => {
        this.pushWorkerStatus.isInProgress = false;
      });
  }

  /**
     * urlBase64ToUint8Array
     * 
     * @param {string} base64String a public vavid key
 */
  urlBase64ToUint8Array(base64String) {
    var padding = '='.repeat((4 - base64String.length % 4) % 4);
    var base64 = (base64String + padding)
      .replace(/\-/g, '+')
      .replace(/_/g, '/');

    var rawData = window.atob(base64);
    var outputArray = new Uint8Array(rawData.length);

    for (var i = 0; i < rawData.length; ++i) {
      outputArray[i] = rawData.charCodeAt(i);
    }
    return outputArray;
  }

  unsubscribe() {
    this.pushWorkerStatus.isInProgress = true;
    var sub: any;
    this.swRegistration.pushManager.getSubscription()
      .then(function (subscription) {
        if (subscription) {
          sub = JSON.parse(JSON.stringify(subscription));
          return subscription.unsubscribe();
        }
      })
      .catch(function (error) {
        console.log('Error unsubscribing', error);
      })
      .then(() => {

        this.http.post<PushSubscription>(`${this.baseUrl}/api/user/unsubscribe/${this.userId}`, {
          auth: sub.keys.auth,
          p256Dh: sub.keys.p256dh,
          endPoint: sub.endpoint
        }).pipe()
          .subscribe(response => {
            this.pushWorkerStatus.isSubscribed = false;
            this.pushWorkerStatus.isInProgress = false;
          },
            error => {
              console.log(error);
            });
      });
  }

}
