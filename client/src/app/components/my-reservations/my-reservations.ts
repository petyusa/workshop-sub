import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { ReservationService } from '../../services/reservation.service';

@Component({
  selector: 'app-my-reservations',
  templateUrl: './my-reservations.html',
  styleUrls: ['./my-reservations.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MyReservationsComponent {
  protected readonly reservationService = inject(ReservationService);
  private readonly router = inject(Router);

  constructor() {
    this.reservationService.loadMyReservations();
  }

  protected goBack(): void {
    this.router.navigate(['/']);
  }

  protected goToBrowse(): void {
    this.router.navigate(['/reservable-objects']);
  }

  protected formatDateTime(isoString: string): string {
    const date = new Date(isoString);
    return date.toLocaleString('en-US', {
      month: 'short',
      day: 'numeric',
      year: 'numeric',
      hour: 'numeric',
      minute: '2-digit',
      hour12: true
    });
  }

  protected formatDateRange(start: string, end: string): string {
    const startDate = new Date(start);
    const endDate = new Date(end);
    
    const dateStr = startDate.toLocaleString('en-US', {
      month: 'short',
      day: 'numeric',
      year: 'numeric'
    });
    
    const startTime = startDate.toLocaleString('en-US', {
      hour: 'numeric',
      minute: '2-digit',
      hour12: true
    });
    
    const endTime = endDate.toLocaleString('en-US', {
      hour: 'numeric',
      minute: '2-digit',
      hour12: true
    });
    
    return `${dateStr}, ${startTime} - ${endTime}`;
  }
}
