import { useEffect, useState } from "react";
import { Grid, Segment, List, Icon } from "semantic-ui-react";
import "../App.css";
import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";
import { LogDto } from "../types/GenericTypes";

function CrawlerLogsPage() {
  const [logs, setLogs] = useState<LogDto[]>([]);
  const [hubConnection, setHubConnection] = useState<HubConnection | null>(
    null
  );

  useEffect(() => {
    const url = "https://localhost:7217/Hubs/SeleniumLogHub";
    const connection = new HubConnectionBuilder()
      .withUrl(url)
      .withAutomaticReconnect()
      .build();

    connection.on("UpStorageLogAdded", (logDto: LogDto) => {
      setLogs((prevLogs) => [...prevLogs, logDto]);
      console.log(logDto.Message);
    });

    async function startConnection() {
      try {
        await connection.start();
        setHubConnection(connection);
      } catch (err) {
        console.error("Error while establishing connection: ", err);
      }
    }

    startConnection();

    return () => {
      if (hubConnection) {
        hubConnection.off("UpStorageLogAdded");
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
                  <List.Content>{log.Message} some text</List.Content>
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
