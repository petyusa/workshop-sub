import { ChangeDetectionStrategy, Component, effect, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { ReservableObjectService } from '../../services/reservable-object.service';
import { LocationService } from '../../services/location.service';
import { ReservableObjectTypes } from '../../models/reservable-object';

@Component({
  selector: 'app-reservable-objects',
  templateUrl: './reservable-objects.html',
  styleUrls: ['./reservable-objects.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ReservableObjectsComponent {
  protected readonly objectService = inject(ReservableObjectService);
  protected readonly locationService = inject(LocationService);
  private readonly router = inject(Router);

  protected readonly selectedType = signal<string | null>(null);
  protected readonly objectTypes = [
    { value: null, label: 'All Types' },
    { value: ReservableObjectTypes.Desk, label: 'Desks' },
    { value: ReservableObjectTypes.ParkingSpace, label: 'Parking Spaces' },
    { value: ReservableObjectTypes.MeetingRoom, label: 'Meeting Rooms' }
  ];

  constructor() {
    // Load objects when location changes
    effect(() => {
      const locationId = this.locationService.selectedLocationId();
      const type = this.selectedType();
      
      if (locationId !== null) {
        this.objectService.loadObjects(locationId, type || undefined);
      }
    });

    // Initial load
    const locationId = this.locationService.selectedLocationId();
    if (locationId !== null) {
      this.objectService.loadObjects(locationId);
    }
  }

  filterByType(type: string | null): void {
    this.selectedType.set(type);
  }

  getTypeBadgeClass(type: string): string {
    switch (type) {
      case ReservableObjectTypes.Desk:
        return 'bg-blue-100 text-blue-800 border-blue-200';
      case ReservableObjectTypes.ParkingSpace:
        return 'bg-slate-100 text-slate-800 border-slate-200';
      case ReservableObjectTypes.MeetingRoom:
        return 'bg-green-100 text-green-800 border-green-200';
      default:
        return 'bg-gray-100 text-gray-800 border-gray-200';
    }
  }

  getTypeLabel(type: string): string {
    switch (type) {
      case ReservableObjectTypes.Desk:
        return 'Desk';
      case ReservableObjectTypes.ParkingSpace:
        return 'Parking Space';
      case ReservableObjectTypes.MeetingRoom:
        return 'Meeting Room';
      default:
        return type;
    }
  }

  goBack(): void {
    this.router.navigate(['/']);
  }
}
