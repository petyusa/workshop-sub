import { ChangeDetectionStrategy, Component, input, output, signal, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ReservableObjectDetail } from '../../models/reservable-object';
import { ReservationService } from '../../services/reservation.service';
import { CreateReservationRequest } from '../../models/reservation';

@Component({
  selector: 'app-reservation-dialog',
  templateUrl: './reservation-dialog.html',
  styleUrls: ['./reservation-dialog.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [FormsModule],
})
export class ReservationDialogComponent {
  readonly reservableObject = input.required<ReservableObjectDetail>();
  readonly reserved = output<void>();
  readonly cancelled = output<void>();

  private readonly reservationService = inject(ReservationService);

  protected readonly startDateTime = signal('');
  protected readonly endDateTime = signal('');
  protected readonly isSubmitting = signal(false);
  protected readonly successMessage = signal<string | null>(null);
  protected readonly errorMessage = signal<string | null>(null);

  protected onStartDateChange(value: string): void {
    this.startDateTime.set(value);
    this.errorMessage.set(null);
  }

  protected onEndDateChange(value: string): void {
    this.endDateTime.set(value);
    this.errorMessage.set(null);
  }

  protected async onSubmit(): Promise<void> {
    this.errorMessage.set(null);
    this.successMessage.set(null);

    const start = this.startDateTime();
    const end = this.endDateTime();

    if (!start || !end) {
      this.errorMessage.set('Please select both start and end date/time');
      return;
    }

    this.isSubmitting.set(true);

    try {
      const request: CreateReservationRequest = {
        reservableObjectId: this.reservableObject().id,
        startDateTime: new Date(start).toISOString(),
        endDateTime: new Date(end).toISOString()
      };

      await this.reservationService.createReservation(request);
      
      this.successMessage.set('Reservation created successfully!');
      
      setTimeout(() => {
        this.reserved.emit();
      }, 1500);
    } catch (error: any) {
      const message = error?.error?.title || error?.error || error?.message || 'Failed to create reservation';
      this.errorMessage.set(message);
    } finally {
      this.isSubmitting.set(false);
    }
  }

  protected onCancel(): void {
    this.cancelled.emit();
  }
}
