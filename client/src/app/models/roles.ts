export const Roles = {
  Employee: 'employee',
  Admin: 'admin'
} as const;

export type Role = typeof Roles[keyof typeof Roles];
