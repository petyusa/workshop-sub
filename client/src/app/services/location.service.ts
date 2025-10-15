import { Injectable, computed, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Location } from '../models/location';

@Injectable({
  providedIn: 'root'
})
export class LocationService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = 'http://localhost:5074';

  readonly locations = signal<Location[]>([]);
  readonly selectedLocationId = signal<number | null>(null);
  
  readonly selectedLocation = computed(() => {
    const id = this.selectedLocationId();
    return this.locations().find(loc => loc.id === id);
  });
  
  readonly hasSelectedLocation = computed(() => this.selectedLocationId() !== null);

  constructor() {
    this.loadLocations();
  }

  async loadLocations(): Promise<void> {
    try {
      const locations = await this.http.get<Location[]>(`${this.apiUrl}/api/locations`).toPromise();
      this.locations.set(locations || []);
    } catch (error) {
      console.error('Failed to load locations:', error);
      this.locations.set([]);
    }
  }

  selectLocation(id: number): void {
    this.selectedLocationId.set(id);
  }

  clearSelection(): void {
    this.selectedLocationId.set(null);
  }
}
