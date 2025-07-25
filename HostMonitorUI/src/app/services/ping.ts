import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, timer, switchMap } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class PingService {
  constructor(private http: HttpClient) {}

  getPings(): Observable<any[]> {
    return this.http.get<any[]>('http://localhost:5000/api/pings');
  }

  getPingsPeriodically(intervalMs: number = 3000): Observable<any[]> {
    return timer(0, intervalMs).pipe(
      switchMap(() => this.getPings())
    );
  }
}

