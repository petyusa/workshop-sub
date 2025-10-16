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
}

export const ReservableObjectTypes = {
  Desk: 'Desk',
  ParkingSpace: 'ParkingSpace',
  MeetingRoom: 'MeetingRoom'
} as const;

export type ReservableObjectType = typeof ReservableObjectTypes[keyof typeof ReservableObjectTypes];
