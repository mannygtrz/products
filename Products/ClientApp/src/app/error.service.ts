import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable()
export class ErrorService {

  private _error$: BehaviorSubject<any> = new BehaviorSubject<any>([]);
  public error$: Observable<any> = this._error$.asObservable();

  constructor(private router: Router) { }

  handleError(error: any) {
    this._error$.next(error['error']);
    this.router.navigate(['error']);
  }
}
