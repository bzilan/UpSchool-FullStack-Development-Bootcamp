export type ApiResponse<T> = {
  message?: string;
  data?: T;
  errors?: string[];
};

export type PaginatedList<T> = {
  items: T[];
  pageNumber: number;
  totalPages: number;
  totalCount: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
};

export type LogDto = {
  Message: string;
  SentOn: string;
};

// export type ExcelData = {
//   id: number;
//   date: string;
//   crawltype: string;
//   requested: number;
//   total: number;
// };

// export type ExcelExportProps = {
//   data: ExcelData[];
// };
