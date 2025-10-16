export interface CreateReservationRequest {
  reservableObjectId: number;
  startDateTime: string; // ISO 8601 format
  endDateTime: string;   // ISO 8601 format
}

export interface Reservation {
  id: number;
  reservableObjectId: number;
  objectName: string;
  locationName: string;
  startDateTime: string;
  endDateTime: string;
  createdAt: string;
}
