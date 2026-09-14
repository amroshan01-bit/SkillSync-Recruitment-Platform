export interface Notification {
  id: string;
  type: string;
  message: string;
  isRead: boolean;
  createdAtUtc: string;
}