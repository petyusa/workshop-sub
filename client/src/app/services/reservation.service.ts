import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { CreateReservationRequest, Reservation } from '../models/reservation';

@Injectable({
  providedIn: 'root'
})
export class ReservationService {
  private readonly http = inject(HttpClient);
  private readonly apiUrl = 'http://localhost:5074';

  readonly reservations = signal<Reservation[]>([]);
  readonly isLoading = signal(false);

  async createReservation(request: CreateReservationRequest): Promise<Reservation> {
    try {
      const reservation = await this.http.post<Reservation>(
        `${this.apiUrl}/api/reservations`,
        request
      ).toPromise();
      
      if (!reservation) {
        throw new Error('Failed to create reservation');
      }
      
      return reservation;
    } catch (error) {
      console.error('Failed to create reservation:', error);
      throw error;
    }
  }

  async loadMyReservations(): Promise<void> {
    this.isLoading.set(true);
    
    try {
      const reservations = await this.http.get<Reservation[]>(
        `${this.apiUrl}/api/reservations/my`
      ).toPromise();
      
      this.reservations.set(reservations || []);
    } catch (error) {
      console.error('Failed to load reservations:', error);
      this.reservations.set([]);
    } finally {
      this.isLoading.set(false);
    }
  }
}
