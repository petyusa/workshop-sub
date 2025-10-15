import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LocationService } from '../../services/location.service';

@Component({
  selector: 'app-location-selector',
  templateUrl: './location-selector.html',
  styleUrls: ['./location-selector.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LocationSelectorComponent {
  protected readonly locationService = inject(LocationService);

  selectLocation(event: Event): void {
    const select = event.target as HTMLSelectElement;
    const id = parseInt(select.value, 10);
    if (id) {
      this.locationService.selectLocation(id);
    }
  }

  changeLocation(): void {
    this.locationService.clearSelection();
  }
}
