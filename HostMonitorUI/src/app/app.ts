import { Component, inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { TrackedHost } from './models/tracked-host';
import { PingResult } from './models/ping-result';
import { PingService } from './services/ping';
import { DatePipe } from '@angular/common';
import { NgFor } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [NgFor, DatePipe],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App implements OnInit {
  protected title = 'HostMonitorUI';

  protected pingResults: PingResult[] = [];
  private readonly maxResults = 50;
  protected hosts: TrackedHost[] = [];
  private http = inject(HttpClient);

  constructor(private pingService: PingService) {}

  ngOnInit(): void {
    //this.loadHosts();
    this.loadPings()
  }

  loadPings(): void {
    this.pingService.getPingsPeriodically(3000).subscribe({
      next: data => this.pingResults = data,
      error: err => console.error('Failed to fetch pings:', err)
    });
  }

  loadHosts(): void {
    this.http.get<TrackedHost[]>('http://localhost:5000/api/hosts')  // Update port if needed
      .subscribe({
        next: (data) => this.hosts = data,
        error: (err) => console.error('Failed to load hosts', err)
      });
  }
}

