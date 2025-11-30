import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})

export class DeviceInfoService {

  public getDeviceInfo(): string {
    const ua = navigator.userAgent;

    const browser = this.detectBrowser(ua);
    const os = this.detectOS(ua);
    const device = /mobile/i.test(ua) ? 'Mobile' : 'Desktop';

    return `${browser}-${os} $`;
  }

  private detectBrowser(ua: string): string {
    if (/chrome|chromium|crios/i.test(ua)) return 'Chrome';
    if (/firefox|fxios/i.test(ua)) return 'Firefox';
    if (/safari/i.test(ua) && !/chrome/i.test(ua)) return 'Safari';
    if (/edg/i.test(ua)) return 'Edge';
    if (/opr|opera/i.test(ua)) return 'Opera';
    return 'Unknown Browser';
  }

  private detectOS(ua: string): string {
    if (/windows nt/i.test(ua)) return 'Windows';
    if (/android/i.test(ua)) return 'Android';
    if (/mac os x/i.test(ua)) return 'macOS';
    if (/iphone|ipad|ipod/i.test(ua)) return 'iOS';
    if (/linux/i.test(ua)) return 'Linux';
    return 'Unknown OS';
  }
}

