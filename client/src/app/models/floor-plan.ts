export interface FloorPlanObject {
  id: number;
  name: string;
  type: string;
  positionX: number;
  positionY: number;
  isAvailable: boolean;
  currentReservationUser?: string | null;
  reservationStart?: string | null;
  reservationEnd?: string | null;
}

export interface FloorPlan {
  locationId: number;
  locationName: string;
  width: number;
  height: number;
  objects: FloorPlanObject[];
}
