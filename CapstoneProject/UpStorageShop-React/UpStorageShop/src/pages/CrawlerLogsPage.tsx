import { useEffect, useState } from "react";
import { Grid, Segment, List, Icon } from "semantic-ui-react";
import "./CrawlerLogsPage.css";
import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";
import { toast, ToastContainer } from "react-toastify";

const CrawlerLogsPage: React.FC = () => {
  const [logs, setLogs] = useState<string[]>([]);
  const [hubConnection, setHubConnection] = useState<HubConnection | null>(
    null
  );

  useEffect(() => {
    const newHubConnection = new HubConnectionBuilder()
      .withUrl("https://localhost:7217/Hubs/SeleniumLogHub")
      .build();

    newHubConnection.on("SendLogNotificationAsync", (log: string) => {
      setLogs((prevLogs) => [...prevLogs, log]);

      if (log.includes("Error")) {
        toast.error(log, {
          position: toast.POSITION.TOP_CENTER,
        });
      }
    });

    newHubConnection
      .start()
      .then(() => console.log("SignalR connected."))
      .catch((error) => console.error("SignalR connection error: ", error));

    setHubConnection(newHubConnection);

    return () => {
      if (hubConnection) {
        hubConnection.stop();
      }
    };
  }, []);

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
                  <List.Content>{log}</List.Content>
                </List.Item>
              ))}
            </List>
          </Segment>
        </Segment.Group>
      </Grid.Column>
      <ToastContainer />
    </Grid>
  );
};

export default CrawlerLogsPage;
