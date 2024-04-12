using app.Domain.Adapter;
using app.Domain.Extensions;
using app.Domain.Models;
using app.Domain.Models.Excel;
using app.Domain.Repositories;
using app.Domain.Resources;
using app.ExcelAdapter.Extensions;
using app.ExcelAdapter.Models;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Localization;
using OfficeOpenXml;
using OfficeOpenXml.DataValidation;
using OfficeOpenXml.Style;
using System.Drawing;

namespace app.ExcelAdapter
{
    public class ExcelAdapter : IExcelAdapter
    {
        private readonly IMapper _mapper;
        private readonly IStringLocalizer<CustomMessages> _localizer;
        private readonly IPermissionRepository _permissionRepository;

        public ExcelAdapter(IMapper mapper, IStringLocalizer<CustomMessages> localizer,
            IPermissionRepository permissionRepository)
        {
            _mapper = mapper;
            _localizer = localizer;
            _permissionRepository = permissionRepository;
        }

        public List<Permission> ReadPermission(IFormFile file)
        {
            var excelPackage = new ExcelPackage(file.OpenReadStream());
            var sheet = excelPackage.Workbook.Worksheets.First();

            var excelResults = sheet.ImportExcelToList<PermissionExcel>();
            excelResults.ThrowIfInvalid(new PermissionExcelValidation());

            var excelResultsMapped = _mapper.Map<IEnumerable<Permission>>(excelResults);

            foreach (var item in excelResultsMapped)
            {
                var existingPermission = _permissionRepository.GetByName(item.Name);

                if (existingPermission != null)
                {
                    item.Id = existingPermission.Id;
                }
            }

            return excelResultsMapped.ToList();
        }

        public Excel GetPermission(IEnumerable<Permission> permissions)
        {
            using var excel = new ExcelPackage();
            var sheetName = CustomMessages.SheetNameExcelPermission;
            var sheet = excel.Workbook.Worksheets.Add(sheetName);
            int lastColumn = 2;
            List<string> headers = new List<string>
            {
                "Name",
                "Description"
            };

            ConfigExcelGeneralSettings(sheet, lastColumn, _localizer, sheetName);
            ConfigExcelNameHeader(sheet, _localizer, headers, false);

            #region data
            int i = 6;

            foreach (var item in permissions)
            {
                sheet.Cells[i, 1].Value = item.Name;
                sheet.Cells[i, 2].Value = item.Description;

                i++;
            }
            #endregion

            ConfigExcelAutoFilter(sheet, 5, lastColumn);
            ConfigExcelStyleHeader(sheet, lastColumn);
            ConfigExcelStyleData(6 + (permissions.Count() - 1), sheet, lastColumn);

            return new Excel($"{sheetName}.xlsx", excel.GetAsByteArray());
        }

        public Excel GetPermissionModel()
        {
            using var excel = new ExcelPackage();
            var sheetName = CustomMessages.SheetNameImportExcelPermission;
            var sheet = excel.Workbook.Worksheets.Add(sheetName);
            int lastColumn = 2;
            List<string> headers = new List<string>
            {
                "Name",
                "Description"
            };

            ConfigExcelGeneralSettings(sheet, headers.Count(), _localizer, sheetName);
            ConfigExcelNameHeader(sheet, _localizer, headers, true);
            ConfigExcelAutoFilter(sheet, 4, lastColumn);
            ConfigExcelStyleHeader(sheet, headers.Count());
            ConfigExcelStyleData(6, sheet, lastColumn);

            return new Excel($"{sheetName}.xlsx", excel.GetAsByteArray());
        }

        private static void ConfigExcelAutoFilter(ExcelWorksheet sheet, int row, int lastColumn)
        {
            using (ExcelRange autoFilterRange = sheet.Cells[row, 1, row, lastColumn])
            {
                autoFilterRange.AutoFilter = true;
            }
        }

        private static void ConfigExcelStyleHeader(ExcelWorksheet sheet, int lastColumn)
        {
            using (ExcelRange tableHeaderRange = sheet.Cells[4, 1, 4, lastColumn])
            {
                tableHeaderRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                tableHeaderRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                tableHeaderRange.Style.WrapText = true;
                tableHeaderRange.Style.Fill.PatternType = ExcelFillStyle.Solid;
                tableHeaderRange.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(210, 224, 255));
            }
        }

        private static void ConfigExcelStyleData(int lastRow, ExcelWorksheet sheet, int lastColumn)
        {
            using (ExcelRange cellsRange = sheet.Cells[4, 1, lastRow, lastColumn])
            {
                cellsRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
                cellsRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
                cellsRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
                cellsRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
            }

            using (ExcelRange tableRange = sheet.Cells[5, 1, lastRow, lastColumn])
            {
                tableRange.Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
                tableRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                tableRange.Style.WrapText = true;
            }
        }

        private static void ConfigExcelGeneralSettings(ExcelWorksheet sheet, int lastColumn, IStringLocalizer<CustomMessages> localizer, string title)
        {
            sheet.DefaultColWidth = 15;
            sheet.Cells.Style.Font.Name = "Arial Narrow";
            sheet.Cells.Style.Font.Size = 12;
            sheet.PrinterSettings.Orientation = eOrientation.Landscape;

            sheet.PrinterSettings.TopMargin = (decimal)0.5;
            sheet.PrinterSettings.BottomMargin = (decimal)0.5;
            sheet.PrinterSettings.LeftMargin = (decimal)0.5;
            sheet.PrinterSettings.RightMargin = (decimal)0.5;

            sheet.Cells[1, 1, 1, lastColumn].Merge = true;
            sheet.Cells[3, 1, 3, lastColumn].Merge = true;

            sheet.Cells[2, 1, 2, lastColumn].Merge = true;
            sheet.Cells[2, 1, 2, lastColumn].Value = localizer[title].Value;
            sheet.Cells[2, 1, 2, lastColumn].Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;
            sheet.Cells[2, 1, 2, lastColumn].Style.Font.Bold = true;
            sheet.Cells[2, 1, 2, lastColumn].Style.Font.Size = 14;
        }

        private static void ConfigExcelNameHeader(ExcelWorksheet sheet, IStringLocalizer<CustomMessages> localizer,
            List<string> headers, bool isModel)
        {
            int i = 1;

            if (isModel)
            {
                foreach (var header in headers)
                {
                    sheet.Cells[4, i, 4, i].Value = header;
                    i++;
                }
            }

            else
            {
                foreach (var header in headers)
                {
                    sheet.Cells[4, i, 5, i].Merge = true;
                    sheet.Cells[4, i, 5, i].Value = localizer[header].Value;
                    i++;
                }
            }
        }

        private static void BuildColumnWithSelectionList(ExcelWorksheet sheet, List<string> items, int column)
        {
            using (ExcelPackage p = new ExcelPackage())
            {
                var val = sheet.Cells[5, column, 5, column].DataValidation.AddListDataValidation() as ExcelDataValidationList;
                // val.AllowBlank = true;

                foreach (var item in items)
                {
                    val.Formula.Values.Add(item);
                }
            }
        }
    }
}