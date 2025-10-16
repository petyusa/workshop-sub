export interface ReservableObject {
  id: number;
  name: string;
  type: string;
  locationName: string;
  isAvailable: boolean;
}

export interface ReservableObjectDetail {
  id: number;
  name: string;
  type: string;
  locationName: string;
  isActive: boolean;
  description?: string;
  openingHours?: OpeningHours[];
}

export interface OpeningHours {
  dayOfWeek: number; // 0 = Sunday, 1 = Monday, etc.
  openTime: string;  // Format: "HH:mm"
  closeTime: string; // Format: "HH:mm"
}

export const ReservableObjectTypes = {
  Desk: 'Desk',
  ParkingSpace: 'ParkingSpace',
  MeetingRoom: 'MeetingRoom'
} as const;

export type ReservableObjectType = typeof ReservableObjectTypes[keyof typeof ReservableObjectTypes];
