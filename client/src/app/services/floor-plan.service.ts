import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FloorPlan } from '../models/floor-plan';

@Injectable({
  providedIn: 'root'
})
export class FloorPlanService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = 'http://localhost:5074';

  readonly floorPlan = signal<FloorPlan | null>(null);
  readonly isLoading = signal(false);

  async loadFloorPlan(locationId: number): Promise<void> {
    this.isLoading.set(true);
    
    try {
      const floorPlan = await this.http.get<FloorPlan>(
        `${this.apiUrl}/api/floor-plan/${locationId}`
      ).toPromise();
      
      this.floorPlan.set(floorPlan || null);
    } catch (error) {
      console.error('Failed to load floor plan:', error);
      this.floorPlan.set(null);
    } finally {
      this.isLoading.set(false);
    }
  }

  clearFloorPlan(): void {
    this.floorPlan.set(null);
  }
}
