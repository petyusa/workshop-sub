import { ChangeDetectionStrategy, Component, inject, signal, effect } from '@angular/core';
import { Router } from '@angular/router';
import { FloorPlanService } from '../../services/floor-plan.service';
import { LocationService } from '../../services/location.service';
import { ReservableObjectService } from '../../services/reservable-object.service';
import { FloorPlanObject } from '../../models/floor-plan';
import { ReservableObjectDetail, ReservableObjectTypes } from '../../models/reservable-object';
import { ReservationDialogComponent } from '../reservation-dialog/reservation-dialog';

@Component({
  selector: 'app-floor-plan',
  templateUrl: './floor-plan.html',
  styleUrls: ['./floor-plan.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [ReservationDialogComponent],
})
export class FloorPlanComponent {
  protected readonly floorPlanService = inject(FloorPlanService);
  protected readonly locationService = inject(LocationService);
  protected readonly objectService = inject(ReservableObjectService);
  private readonly router = inject(Router);

  protected readonly selectedObject = signal<ReservableObjectDetail | null>(null);
  protected readonly showReservationDialog = signal(false);
  protected readonly showSuccessToast = signal(false);

  constructor() {
    // Load floor plan when location changes
    effect(() => {
      const locationId = this.locationService.selectedLocationId();
      if (locationId !== null) {
        this.floorPlanService.loadFloorPlan(locationId);
      }
    });

    // Initial load
    const locationId = this.locationService.selectedLocationId();
    if (locationId !== null) {
      this.floorPlanService.loadFloorPlan(locationId);
    }
  }

  protected goBack(): void {
    this.router.navigate(['/']);
  }

  protected getObjectClass(obj: FloorPlanObject): string {
    const baseClasses = 'floor-plan-object';
    
    if (!obj.isAvailable && obj.currentReservationUser) {
      // Reserved
      return `${baseClasses} reserved`;
    } else if (!obj.isAvailable) {
      // Inactive
      return `${baseClasses} inactive`;
    } else {
      // Available
      return `${baseClasses} available`;
    }
  }

  protected getTypeIcon(type: string): string {
    switch (type) {
      case ReservableObjectTypes.Desk:
        return '🪑';
      case ReservableObjectTypes.ParkingSpace:
        return '🚗';
      case ReservableObjectTypes.MeetingRoom:
        return '🏢';
      default:
        return '📦';
    }
  }

  protected getTypeName(type: string): string {
    switch (type) {
      case ReservableObjectTypes.Desk:
        return 'Desk';
      case ReservableObjectTypes.ParkingSpace:
        return 'Parking';
      case ReservableObjectTypes.MeetingRoom:
        return 'Meeting Room';
      default:
        return type;
    }
  }

  protected async onObjectClick(obj: FloorPlanObject): Promise<void> {
    if (!obj.isAvailable) {
      return; // Don't allow clicking on reserved or inactive objects
    }

    // Fetch full object details
    const objectDetail = await this.objectService.getObjectById(obj.id);
    if (objectDetail) {
      this.selectedObject.set(objectDetail);
      this.showReservationDialog.set(true);
    }
  }

  protected onReservationComplete(): void {
    this.showReservationDialog.set(false);
    this.selectedObject.set(null);
    this.showSuccessToast.set(true);
    
    // Reload floor plan to show updated availability
    const locationId = this.locationService.selectedLocationId();
    if (locationId !== null) {
      this.floorPlanService.loadFloorPlan(locationId);
    }
    
    setTimeout(() => {
      this.showSuccessToast.set(false);
    }, 3000);
  }

  protected onReservationCancelled(): void {
    this.showReservationDialog.set(false);
    this.selectedObject.set(null);
  }
}
