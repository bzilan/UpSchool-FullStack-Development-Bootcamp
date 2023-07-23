import { useEffect, useState } from "react";
import { Grid, Segment, List, Icon } from "semantic-ui-react";
import "../App.css";
import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";

const BASE_SIGNALR_URL = import.meta.env.VITE_API_SIGNALR_URL;

let token = "";

function CrawlerLogsPage() {
  const [logs, setLogs] = useState<string[]>([]);

  const [orderHubConnection, setOrderHubConnection] =
    useState<HubConnection | null>(null);

  const [seleniumLogHubConnection, setSeleniumLogHubConnection] =
    useState<HubConnection | null>(null);

  const [notificationHubConnection, setNotificationHubConnection] =
    useState<HubConnection | null>(null);

  useEffect(() => {
    const createHubConnections = async () => {
      const orderHub = new HubConnectionBuilder()
        .withUrl(`${BASE_SIGNALR_URL}Hubs/OrderHub?access_token=${token}`)
        .build();
      const seleniumLogHubConnection = new HubConnectionBuilder()
        .withUrl(`${BASE_SIGNALR_URL}Hubs/SeleniumLogHub?access_token=${token}`)
        .build();
      const notificationHub = new HubConnectionBuilder()
        .withUrl(`${BASE_SIGNALR_URL}Hubs/NotificationHub`)
        .build();

      if (orderHub.state === "Disconnected") {
        await orderHub.start();
      }

      if (seleniumLogHubConnection.state === "Disconnected") {
        await seleniumLogHubConnection.start();
      }

      if (notificationHub.state === "Disconnected") {
        await notificationHub.start();
      }

      setOrderHubConnection(orderHub);
      setSeleniumLogHubConnection(seleniumLogHubConnection);
      setNotificationHubConnection(notificationHub);
    };

    createHubConnections();

    return () => {
      orderHubConnection?.stop();
      seleniumLogHubConnection?.stop();
      notificationHubConnection?.stop();
    };
  }, []);

  const hubConnection = new HubConnectionBuilder()
    .withUrl("https://localhost:7217/Hubs/SeleniumLogHub") 
    .withAutomaticReconnect()
    .build();

  const startConnection = async () => {
    try {
      await hubConnection.start();
      console.log("Connected to Selenium Log Hub");
    } catch (error) {
      console.error("Error connecting to Selenium Log Hub:", error);
    }
  };

  startConnection();

  return (
    <Grid centered columns={2} style={{ height: "80vh" }}>
      <Grid.Column>
        <Segment.Group raised>
          <Segment>
            <div className="top">
              <div className="btns">
                <Icon name="circle" color="red" />
                <Icon name="circle" color="yellow" />
                <Icon name="circle" color="green" />
              </div>
              <div className="title">Crawler Logs</div>
            </div>
          </Segment>
          <Segment>
            <List relaxed divided>
              {logs.map((log, index) => (
                <List.Item key={index}>
                  <List.Content>{log.message} some text</List.Content>
                </List.Item>
              ))}
            </List>
          </Segment>
        </Segment.Group>
      </Grid.Column>
    </Grid>
  );
}

export default CrawlerLogsPage;
