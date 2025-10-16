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

  protected getGroupedOpeningHours(): Array<{days: string, openTime: string, closeTime: string}> {
    const openingHours = this.reservableObject().openingHours;
    if (!openingHours || openingHours.length === 0) {
      return [];
    }

    // Group consecutive days with same hours
    const dayNames = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
    const groups: Array<{days: string, openTime: string, closeTime: string}> = [];
    
    // Sort by day of week
    const sorted = [...openingHours].sort((a, b) => a.dayOfWeek - b.dayOfWeek);
    
    let currentGroup: {start: number, end: number, openTime: string, closeTime: string} | null = null;
    
    for (const hours of sorted) {
      if (!currentGroup) {
        currentGroup = {
          start: hours.dayOfWeek,
          end: hours.dayOfWeek,
          openTime: hours.openTime,
          closeTime: hours.closeTime
        };
      } else if (
        hours.dayOfWeek === currentGroup.end + 1 &&
        hours.openTime === currentGroup.openTime &&
        hours.closeTime === currentGroup.closeTime
      ) {
        currentGroup.end = hours.dayOfWeek;
      } else {
        // Save current group and start new one
        const days = currentGroup.start === currentGroup.end
          ? dayNames[currentGroup.start]
          : `${dayNames[currentGroup.start]} - ${dayNames[currentGroup.end]}`;
        groups.push({
          days,
          openTime: this.formatTime(currentGroup.openTime),
          closeTime: this.formatTime(currentGroup.closeTime)
        });
        currentGroup = {
          start: hours.dayOfWeek,
          end: hours.dayOfWeek,
          openTime: hours.openTime,
          closeTime: hours.closeTime
        };
      }
    }
    
    // Don't forget the last group
    if (currentGroup) {
      const days = currentGroup.start === currentGroup.end
        ? dayNames[currentGroup.start]
        : `${dayNames[currentGroup.start]} - ${dayNames[currentGroup.end]}`;
      groups.push({
        days,
        openTime: this.formatTime(currentGroup.openTime),
        closeTime: this.formatTime(currentGroup.closeTime)
      });
    }
    
    return groups;
  }

  protected getClosedDays(): string {
    const openingHours = this.reservableObject().openingHours;
    if (!openingHours || openingHours.length === 0) {
      return '';
    }

    const dayNames = ['Sunday', 'Monday', 'Tuesday', 'Wednesday', 'Thursday', 'Friday', 'Saturday'];
    const openDays = new Set(openingHours.map(h => h.dayOfWeek));
    const closedDays = dayNames.filter((_, index) => !openDays.has(index));
    
    if (closedDays.length === 0) {
      return '';
    }
    
    return closedDays.join(', ');
  }

  private formatTime(time: string): string {
    // Convert "08:00" to "8:00 AM"
    const [hours, minutes] = time.split(':').map(Number);
    const period = hours >= 12 ? 'PM' : 'AM';
    const displayHours = hours > 12 ? hours - 12 : hours === 0 ? 12 : hours;
    return `${displayHours}:${minutes.toString().padStart(2, '0')} ${period}`;
  }
}
