import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class UrlService {
  escape(str: string): string {
    // JS escape() - deprecated; JS encodeURIComponent() doesn't escapes '(' and ')' chars,
    // that exist in some titles passed to the url
    return str.replace('(', '%28').replace(')', '%29');
  }
}
