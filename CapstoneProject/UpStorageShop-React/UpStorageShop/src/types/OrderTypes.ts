export type OrderGetAllDto = {
  id: string;
  requestedAmount?: number;
  totalFoundAmount?: number;
  productCrawlType: ProductCrawlType[];
};
export enum ProductCrawlType {
  All = 0,
  OnDiscount = 1,
  NonDiscount = 2,
}

export type OrderGetByIdQuery = {
  userId: string;
};

export type OrderGetByIdDto = {
  id: string;
  userId: string;
  requestedAmount: number;
  totalFoundAmount: number;
  productCrawlType: ProductCrawlType;
  createdOn: Date;
};

export type OrderAddCommand = {
  id: string;
  requestedAmount: number;
  productCrawlType: ProductCrawlType;
};

export type DashboardProps = {
  onCrawlStart: (productCount: number, crawlType: ProductCrawlType) => void;
};
