import {
  Grid,
  Segment,
  Form,
  Input,
  Button,
  Checkbox,
} from "semantic-ui-react";
import React, { useState } from "react";
import { DashboardProps, ProductCrawlType } from "../types/OrderTypes";
import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";

const BASE_SIGNALR_URL = import.meta.env.VITE_API_SIGNALR_URL;

function DashboardPage({ onCrawlStart }: DashboardProps) {
  const [productCount, setProductCount] = useState<number>(10);
  const [crawlType, setCrawlType] = useState<ProductCrawlType>(
    ProductCrawlType.All
  );

  const handleProductCountChange = (
    event: React.ChangeEvent<HTMLInputElement>
  ) => {
    const count = Number(event.target.value);
    setProductCount(count);
  };

  const handleCrawlTypeChange = (
    event: React.FormEvent<HTMLInputElement>,
    data: any
  ) => {
    setCrawlType(data.value as ProductCrawlType);
  };

  const handleStartCrawl = async () => {
    onCrawlStart(productCount, crawlType);

    // SignalR bağlantısı oluştur
    const hubConnection = new HubConnectionBuilder()
      .withUrl(`${BASE_SIGNALR_URL}SeleniumLogHub`) // Selenium logları için SignalR hub adresi
      .withAutomaticReconnect()
      .build();

    try {
      // Hub'a bağlan
      await hubConnection.start();

      // Hub üzerinden backend'e startCrawl mesajı gönder
      await hubConnection.invoke("Bot Started", productCount, crawlType);

      console.log("Crawler işlemi başlatıldı.");
    } catch (error) {
      console.error("Crawler işlemi başlatılırken hata oluştu:", error);
    }
  };

  return (
    <Grid centered columns={2} style={{ height: "80vh" }}>
      <Grid.Column>
        <Segment>
          <Form>
            <Form.Field>
              <label>Product Count:</label>
              <Input
                type="number"
                value={productCount}
                onChange={handleProductCountChange}
                placeholder="Enter product count..."
              />
            </Form.Field>
            <Form.Field>
              <label>Crawl Type:</label>
              <Form.Group>
                <Form.Field>
                  <Checkbox
                    radio
                    label="All"
                    name="crawlType"
                    value={ProductCrawlType.All}
                    checked={crawlType === ProductCrawlType.All}
                    onChange={handleCrawlTypeChange}
                  />
                </Form.Field>
                <Form.Field>
                  <Checkbox
                    radio
                    label="On Discount"
                    name="crawlType"
                    value={ProductCrawlType.OnDiscount}
                    checked={crawlType === ProductCrawlType.OnDiscount}
                    onChange={handleCrawlTypeChange}
                  />
                </Form.Field>
                <Form.Field>
                  <Checkbox
                    radio
                    label="Non Discount"
                    name="crawlType"
                    value={ProductCrawlType.NonDiscount}
                    checked={crawlType === ProductCrawlType.NonDiscount}
                    onChange={handleCrawlTypeChange}
                  />
                </Form.Field>
              </Form.Group>
            </Form.Field>
            <Button primary onClick={handleStartCrawl}>
              Start Crawl
            </Button>
          </Form>
        </Segment>
      </Grid.Column>
    </Grid>
  );
}

export default DashboardPage;
