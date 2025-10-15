import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { LocationSelectorComponent } from '../location-selector/location-selector';
import { LocationService } from '../../services/location.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.html',
  styleUrls: ['./home.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [LocationSelectorComponent],
})
export class HomeComponent {
  protected readonly locationService = inject(LocationService);
}
