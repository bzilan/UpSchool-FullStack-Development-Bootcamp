// import React, { useContext, useEffect, useState } from "react";
// import axios from "axios";
// import XLSX from "xlsx";
// import { ExcelExportProps, ExcelData } from "../types/GenericTypes";

// function ExcelExport({ data, fileName }) {
//   const exportToExcel = () => {
//     const worksheet = XLSX.utils.json_to_sheet(data);
//     const workbook = XLSX.utils.book_new();
//     XLSX.utils.book_append_sheet(workbook, worksheet, "ProductData");
//     XLSX.writeFile(workbook, fileName);
//   };

//   return (
//     <div>
//       <button onClick={exportToExcel}>Export to Excel</button>
//     </div>
//   );
// }

// export default ExcelExport;
