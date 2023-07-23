import { useState } from "react";

const SettingsPage = () => {
  const [notificationType, setNotificationType] = useState("");

  const handleNotificationChange = (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    setNotificationType(event.target.value);
  };

  return (
    <div>
      <h1>Settings</h1>
      <select value={notificationType} onChange={handleNotificationChange}>
        <option value="None">None</option>
        <option value="Email">Email</option>
        <option value="InApp">In-App</option>
      </select>
    </div>
  );
};

export default SettingsPage;
