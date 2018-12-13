using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

// Reference: http://www.dispatchertimer.com/tutorial/how-to-create-an-excel-file-in-net-using-openxml-part-2-export-a-collection-to-spreadsheet/

namespace CCSInventory.Utilities
{
    public static class ExcelUtil
    {
        /// <summary>
        /// This method generates an Excel file for the provided IEnumerable<> with the property names as the header row.
        /// </summary>
        /// <param name="output">Stream to write to.  Usually MemoryStream</param>
        /// <param name="content">IEnumerable collection of items to add to the spreadsheet.</param>
        /// <typeparam name="T"></typeparam>
        public static void GenerateExcel<T>(Stream output, IEnumerable<T> content)
        {
            using (SpreadsheetDocument document = SpreadsheetDocument.Create(output, SpreadsheetDocumentType.Workbook))
            {
                // Setup of the document:
                WorkbookPart wbPart = document.AddWorkbookPart();
                wbPart.Workbook = new Workbook();
                WorksheetPart worksheetPart = wbPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet();
                Sheets sheets = wbPart.Workbook.AppendChild(new Sheets());
                Sheet sheet = new Sheet() { Id = wbPart.GetIdOfPart(worksheetPart), SheetId = 1, Name = "Report Data" };
                sheets.Append(sheet);
                wbPart.Workbook.Save();
                SheetData sheetData = worksheetPart.Worksheet.AppendChild(new SheetData());

                var contentProperties = new List<string>();
                foreach (PropertyInfo property in typeof(T).GetProperties(
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty))
                {
                    contentProperties.Add(property.Name);
                }

                // Create header row:
                Row row = new Row();
                foreach (var prop in contentProperties)
                {
                    row.AppendChild(new Cell
                    {
                        CellValue = new CellValue(prop),
                        DataType = CellValues.String
                    });
                }
                sheetData.AppendChild(row);

                // Inserting actual data:
                foreach (var c in content)
                {
                    row = new Row();
                    foreach (string prop in contentProperties)
                    {
                        PropertyInfo propInfo = c.GetType().GetProperty(prop);
                        if (propInfo == null) continue;
                        var cell = new Cell();
                        if (propInfo.PropertyType == typeof(string))
                        {
                            cell.CellValue = new CellValue((string)propInfo.GetValue(c, null));
                            cell.DataType = CellValues.String;
                        }
                        else if (propInfo.PropertyType == typeof(Int16))
                        {
                            cell.CellValue = new CellValue(((short)propInfo.GetValue(c, null)).ToString());
                            cell.DataType = CellValues.Number;
                        }
                        else if (propInfo.PropertyType == typeof(Int32))
                        {
                            cell.CellValue = new CellValue(((int)propInfo.GetValue(c, null)).ToString());
                            cell.DataType = CellValues.Number;
                        }
                        else if (propInfo.PropertyType == typeof(Int64))
                        {
                            cell.CellValue = new CellValue(((long)propInfo.GetValue(c, null)).ToString());
                            cell.DataType = CellValues.Number;
                        }
                        else if (propInfo.PropertyType == typeof(decimal))
                        {
                            cell.CellValue = new CellValue(((decimal)propInfo.GetValue(c, null)).ToString());
                            cell.DataType = CellValues.Number;
                        }
                        else if (propInfo.PropertyType == typeof(DateTime))
                        {
                            cell.CellValue = new CellValue(((DateTime)propInfo.GetValue(c, null)).ToString());
                            cell.DataType = CellValues.String;
                        }
                        else if (propInfo.PropertyType == typeof(bool))
                        {
                            cell.CellValue = new CellValue((bool)propInfo.GetValue(c, null) ? "true" : "false");
                            cell.DataType = CellValues.Boolean;
                        } else {
                            cell.CellValue = new CellValue("Unsupported Data Type");
                            cell.DataType = CellValues.String;
                        }
                        row.AppendChild(cell);
                    }
                    sheetData.AppendChild(row);
                }
                worksheetPart.Worksheet.Save();
                wbPart.Workbook.Save();
            }
        }
    }
}
