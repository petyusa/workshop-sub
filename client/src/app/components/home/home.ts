import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { RouterLink } from '@angular/router';
import { LocationSelectorComponent } from '../location-selector/location-selector';
import { LocationService } from '../../services/location.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.html',
  styleUrls: ['./home.css'],
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [LocationSelectorComponent, RouterLink],
})
export class HomeComponent {
  protected readonly locationService = inject(LocationService);
}
