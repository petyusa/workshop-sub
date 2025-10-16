import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { ReservableObject, ReservableObjectDetail } from '../models/reservable-object';

@Injectable({
  providedIn: 'root'
})
export class ReservableObjectService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = 'http://localhost:5074';

  readonly objects = signal<ReservableObject[]>([]);
  readonly isLoading = signal(false);

  async loadObjects(locationId?: number, type?: string, activeOnly: boolean = true): Promise<void> {
    this.isLoading.set(true);
    
    try {
      const params: Record<string, string> = {
        activeOnly: String(activeOnly)
      };

      if (locationId !== undefined && locationId !== null) {
        params['locationId'] = String(locationId);
      }

      if (type) {
        params['type'] = type;
      }

      const queryString = new URLSearchParams(params).toString();
      const url = `${this.apiUrl}/api/reservable-objects${queryString ? '?' + queryString : ''}`;

      const objects = await this.http.get<ReservableObject[]>(url).toPromise();
      this.objects.set(objects || []);
    } catch (error) {
      console.error('Failed to load reservable objects:', error);
      this.objects.set([]);
    } finally {
      this.isLoading.set(false);
    }
  }

  async getObjectById(id: number): Promise<ReservableObjectDetail | null> {
    try {
      const object = await this.http.get<ReservableObjectDetail>(`${this.apiUrl}/api/reservable-objects/${id}`).toPromise();
      return object || null;
    } catch (error) {
      console.error('Failed to load reservable object:', error);
      return null;
    }
  }
}
