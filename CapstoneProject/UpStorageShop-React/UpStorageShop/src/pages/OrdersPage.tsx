import { createContext, useContext, useEffect, useState } from "react";
import { OrderGetByIdDto, ProductCrawlType } from "../types/OrderTypes";
import { HubConnection, HubConnectionBuilder } from "@microsoft/signalr";
import { useNavigate } from "react-router-dom";
import * as XLSX from "xlsx";
import { saveAs } from "file-saver";
import "../index.css";

interface OrderContextType {
  orderList: OrderGetByIdDto[];
  setOrderList: React.Dispatch<React.SetStateAction<OrderGetByIdDto[]>>;
}

function OrdersPage() {
  const [productCount, setProductCount] = useState<number>(1);
  const [crawlType, setCrawlType] = useState<ProductCrawlType>(
    ProductCrawlType.All
  );
  const [result, setResult] = useState<string>("");
  const [seleniumHubConnection, setSeleniumHubConnection] =
    useState<HubConnection | null>(null);
  const navigate = useNavigate();
  const OrderContext = createContext<OrderContextType>({
    orderList: [],
    setOrderList: () => {
      [];
    },
  });
  const { orderList, setOrderList } = useContext(OrderContext);

  useEffect(() => {
    const startConnection = async () => {
      const connection = new HubConnectionBuilder()
        .withUrl("https://localhost:7217/Hubs/SeleniumLogHub")
        .withAutomaticReconnect()
        .build();

      try {
        await connection.start();
        setSeleniumHubConnection(connection);
      } catch (error) {
        console.error("SignalR connection error:", error);
      }
    };

    if (!seleniumHubConnection) {
      startConnection();
    }
  }, [seleniumHubConnection]);

  const startCrawling = async () => {
    try {
      if (!seleniumHubConnection) {
        setResult("An error occurred while starting crawling.");
        return;
      }

      await seleniumHubConnection.send("StartCrawling", {
        productCount: productCount,
        crawlType: crawlType,
      });

      navigate("/crawlerlogs");
    } catch (error) {
      console.error(error);
      setResult("An error occurred while starting crawling.");
    }
  };

  const handleExportToExcel = () => {
    const tableDataForExport = orderList.map(
      ({
        id,
        createdOn,
        productCrawlType,
        requestedAmount,
        totalFoundAmount,
      }) => ({
        id,
        createdOn: createdOn.toString(),
        productCrawlType,
        requestedAmount,
        totalFoundAmount,
      })
    );

    const worksheet = XLSX.utils.json_to_sheet(tableDataForExport);
    const workbook = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(workbook, worksheet, "Sheet1");
    const excelBuffer = XLSX.write(workbook, {
      bookType: "xlsx",
      type: "array",
    });
    const data = new Blob([excelBuffer], {
      type: "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
    });
    saveAs(data, "orders.xlsx");
  };

  return (
    <div>
      <h1>Product Crawler</h1>
      <label htmlFor="productCount">How many items do you want to crawl?</label>
      <input
        type="number"
        id="productCount"
        name="productCount"
        min={1}
        step={1}
        value={productCount}
        onChange={(e) => setProductCount(Number(e.target.value))}
        required
      />
      <br />
      <label>What products do you want to crawl?</label>
      <br />
      <input
        type="radio"
        id="all"
        name="crawlType"
        value={ProductCrawlType.All}
        checked={crawlType === ProductCrawlType.All}
        onChange={() => setCrawlType(ProductCrawlType.All)}
      />
      <label htmlFor="all">All</label>
      <input
        type="radio"
        id="onSale"
        name="crawlType"
        value={ProductCrawlType.OnDiscount}
        checked={crawlType === ProductCrawlType.OnDiscount}
        onChange={() => setCrawlType(ProductCrawlType.OnDiscount)}
      />
      <label htmlFor="onSale">On Sale</label>
      <input
        type="radio"
        id="nonDiscount"
        name="crawlType"
        value={ProductCrawlType.NonDiscount}
        checked={crawlType === ProductCrawlType.NonDiscount}
        onChange={() => setCrawlType(ProductCrawlType.NonDiscount)}
      />
      <label htmlFor="nonDiscount">Non Discount</label>
      <br />
      <button onClick={startCrawling}>Start Crawling</button>
      <button onClick={handleExportToExcel} className="export-button">
        Export to Excel
      </button>
      <div>{result}</div>
    </div>
  );
}

export default OrdersPage;
