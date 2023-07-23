import React, { useState, useEffect, useContext } from "react";
import { Table, Header } from "semantic-ui-react";
import { OrderGetByIdDto, OrderGetByIdQuery } from "../types/OrderTypes";
import api from "../utils/axiosInstance.ts";
import { AppUserContext } from "../context/StateContext.tsx";

const BASE_URL = import.meta.env.VITE_API_URL;

function OrdersPage() {
  const { appUser } = useContext(AppUserContext);

  const [orderList, setOrderList] = useState<OrderGetByIdDto[]>([]);

  useEffect(() => {
    const fetchOrders = async () => {
      const orderGetByIdQuery: OrderGetByIdQuery = {
        userId: appUser ? appUser.id ?? "" : "",
      };

      try {
        const response = await api.post("/Orders/GetById", orderGetByIdQuery);
        setOrderList(response.data);
        console.log(response.data);
      } catch (error) {
        console.log(error);
      }
    };

    fetchOrders();
  }, []);

  return (
    <React.Fragment>
      <Header as="h2" color="blue" style={{ alignSelf: "flex-start" }}>
        Orders
      </Header>
      <Table size="small">
        <Table.Header>
          <Table.Row>
            <Table.HeaderCell>Id</Table.HeaderCell>
            <Table.HeaderCell>Date</Table.HeaderCell>
            <Table.HeaderCell>CrawlType</Table.HeaderCell>
            <Table.HeaderCell>Requested Amount</Table.HeaderCell>
            <Table.HeaderCell>Total Found Amount</Table.HeaderCell>
          </Table.Row>
        </Table.Header>
        <Table.Body>
          {orderList.map((row) => (
            <Table.Row
              key={row.id}
              style={{ "&:last-child td, &:last-child th": { border: 0 } }}
            >
              <Table.Cell>{row.id}</Table.Cell>
              <Table.Cell align="right">{row.createdOn.toString()}</Table.Cell>
              <Table.Cell align="right">{row.productCrawlType}</Table.Cell>
              <Table.Cell align="right">{row.requestedAmount}</Table.Cell>
              <Table.Cell align="right">{row.totalFoundAmount}</Table.Cell>
            </Table.Row>
          ))}
        </Table.Body>
      </Table>
    </React.Fragment>
  );
}

export default OrdersPage;
