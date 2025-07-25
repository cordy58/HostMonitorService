import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TrackedHost } from '../models/tracked-host';

@Injectable({
  providedIn: 'root'
})
export class HostService {
  private apiUrl = 'http://localhost:5000/api/hosts';

  constructor(private http: HttpClient) {}

  getHosts(): Observable<TrackedHost[]> {
    return this.http.get<TrackedHost[]>(this.apiUrl);
  }
}

