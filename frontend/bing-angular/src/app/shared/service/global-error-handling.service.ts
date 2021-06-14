import {ErrorHandler, Injectable, Injector, NgZone} from '@angular/core';
import {Router} from "@angular/router";

@Injectable({
  providedIn: 'root'
})
export class GlobalErrorHandlingService implements ErrorHandler{

  private router: Router;
  constructor(private injector : Injector,
              private ngZone: NgZone) {}
  /*
  error.status gets the code.
  error.message gets the errormessage
   */

  handleError(error: any) {
    console.log(error)
    if (error.status == 400 ) {
      this.router = this.injector.get(Router);
      this.ngZone.run(() => {this.router.navigate(['/error'], {fragment: '400'})});
    }
    else if (error.status == 401 ) {
      this.router = this.injector.get(Router);
      this.ngZone.run(() => {this.router.navigate(['/login'])});
    }
    else if (error.status == 403 ) {
      this.router = this.injector.get(Router);
      this.ngZone.run(() => {this.router.navigate(['/error'], {fragment: '403'})});
    }
    else if (error.status == 404 ) {
      this.router = this.injector.get(Router);
      this.ngZone.run(() => {this.router.navigate(['/error'], {fragment: '404'})});
    }
    else if (error.status == 406 ) {
      this.router = this.injector.get(Router);
      this.ngZone.run(() => {this.router.navigate(['/error'], {fragment: '406'})});
    }
    else if (error.status == 408 ) {
      this.router = this.injector.get(Router);
      this.ngZone.run(() => {this.router.navigate(['/error'], {fragment: '408'})});
    }
    else if (error.status == 409 ) {
      this.router = this.injector.get(Router);
      this.ngZone.run(() => {this.router.navigate(['/error'], {fragment: '409'})});
    }
    else if (error.status == 410 ) {
      this.router = this.injector.get(Router);
      this.ngZone.run(() => {this.router.navigate(['/error'], {fragment: '410'})});
    }
    else if (error.status == 500 ) {
      this.router = this.injector.get(Router);
      this.ngZone.run(() => {this.router.navigate(['/error'], {fragment: '500'})});
    }
    else if (error.status == 503 ) {
      this.router = this.injector.get(Router);
      this.ngZone.run(() => {this.router.navigate(['/error'], {fragment: '503'})});
    }
  }
}
