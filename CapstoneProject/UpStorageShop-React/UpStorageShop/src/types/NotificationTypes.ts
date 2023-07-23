export type NotificationTypes = {
  Email: 0;
  InAppNotification: 1;
  None: 2;
};

export type NotificationSettingsDto = {
  pushNotification: boolean;
  emailNotification: boolean;
  email?: string;
};
