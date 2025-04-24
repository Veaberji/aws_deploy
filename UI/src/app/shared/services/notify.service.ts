import { Inject, Injectable, Injector } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root',
})
export class NotifyService {
  // Need to get ToastrService from injector rather than constructor injection to avoid cyclic dependency error
  constructor(@Inject(Injector) private injector: Injector) {}

  success(message?: string, title?: string, timeOut: number = 0): void {
    this.toastr.success(message, title, { timeOut, extendedTimeOut: 0, progressBar: !!timeOut });
  }

  info(message?: string, title?: string, timeOut: number = 0): void {
    this.toastr.info(message, title, { timeOut, extendedTimeOut: 0, progressBar: !!timeOut });
  }

  warning(message?: string, title?: string, timeOut: number = 0): void {
    this.toastr.warning(message, title, { timeOut, extendedTimeOut: 0, progressBar: !!timeOut });
  }

  error(message?: string, title?: string, timeOut: number = 0): void {
    this.toastr.error(message, title, { timeOut, extendedTimeOut: 0, progressBar: !!timeOut });
  }

  private get toastr(): ToastrService {
    return this.injector.get(ToastrService);
  }
}
