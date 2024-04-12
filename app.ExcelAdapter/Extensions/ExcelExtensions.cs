using app.Domain.Exceptions;
using app.Domain.Resources;
using OfficeOpenXml;
using OfficeOpenXml.Table;
using System.Reflection;
using System.Text.RegularExpressions;

namespace app.ExcelAdapter.Extensions
{
    public class ExcelNavigator
    {
        public Object Value { get; set; }
        public string Address { get; set; }
        public int Index { get; set; }
    }

    internal static class ExcelExtensions
    {
        /// <summary>
        /// Read all rows of excel data to bind in T collection
        /// </summary>
        /// <typeparam name="T">The type of each row data</typeparam>
        /// <param name="worksheet">A excel worksheet that contains data</param>
        /// <returns>A collection data of T</returns>
        public static List<T> ImportExcelToList<T>(this ExcelWorksheet worksheet) where T : new()
        {
            //DateTime Conversion
            Func<double, DateTime> convertDateTime = new Func<double, DateTime>(excelDate =>
            {
                DateTime dateOfReference = ExcelDateValidation(ref excelDate);

                return dateOfReference.AddDays(excelDate);
            });

            ExcelTable table = null;

            if (worksheet.Tables.Any())
            {
                table = worksheet.Tables.FirstOrDefault();
            }
            else
            {
                table = worksheet.Tables.Add(worksheet.Dimension, "Table1");

                ExcelAddressBase newaddy = new ExcelAddressBase(table.Address.Start.Row, table.Address.Start.Column, table.Address.End.Row + 1, table.Address.End.Column);

                //Edit the raw XML by searching for all references to the old address
                table.TableXml.InnerXml = table.TableXml.InnerXml.Replace(table.Address.ToString(), newaddy.ToString());
            }

            //Get the cells based on the table address
            List<IGrouping<int, ExcelRangeBase>> groups = table.WorkSheet.Cells[4, table.Address.Start.Column, table.Address.End.Row, table.Address.End.Column]
                .GroupBy(cell => cell.Start.Row)
                .ToList();

            //Get the properties of T
            List<PropertyInfo> modelProperties = new T().GetType().GetProperties().ToList();
            List<Type> types = modelProperties.Select(x => x.PropertyType).ToList();

            //Assume first row has the column names
            var colnames = groups.FirstOrDefault()
            .Select((hcell, idx) => new
            {
                Name = hcell.Value.ToString().Replace(" ", string.Empty),
                Address = hcell.Address.Substring(0, 1),
                index = idx
            })
            .Where(o => modelProperties.Select(p => p.Name.ToLower()).Contains(o.Name.ToLower()))
            .ToList();

            //Everything after the header is data
            List<List<ExcelNavigator>> rowvalues = groups
                .Skip(1) //Exclude header
                .Select(cg => cg.Select(c => new ExcelNavigator { Value = c.Value, Address = c.Address }).ToList()).ToList();

            //Create the collection container
            List<T> collection = new List<T>();
            int currentRow = 5; // header + first row
            var readErrors = new List<(string, int)>();

            foreach (List<ExcelNavigator> row in rowvalues)
            {
                string currentColumn = string.Empty;

                if (!row.Any() || row.First() is null) continue;

                T tnew = new T();
                foreach (var colname in colnames)
                {
                    currentColumn = colname.Name.ToLower();
                    try
                    {
                        //This is the real wrinkle to using reflection - Excel stores all numbers as double including int
                        object val = row.FirstOrDefault(x => x.Address.Substring(0, 1) == colname.Address)?.Value;

                        Type type = types[colname.index];
                        PropertyInfo prop = modelProperties.FirstOrDefault(p => p.Name.ToLower() == colname.Name.ToLower());

                        if (!prop.CanWrite) continue;
                        ConvertType(convertDateTime, tnew, val, type, prop, currentRow);
                    }

                    catch (DomainException)
                    {
                        throw;
                    }

                    catch
                    {
                        readErrors.Add((currentColumn, currentRow));
                    }
                }

                PropertyInfo propLineNumber = modelProperties.FirstOrDefault(p => p.Name == "line_number");
                propLineNumber.SetValue(tnew, (long)currentRow);

                collection.Add(tnew);
                currentRow += 1;
            }

            if (readErrors.Any())
            {
                throw new ReadExcelException(readErrors);
            }

            return collection;
        }

        private static DateTime ExcelDateValidation(ref double excelDate)
        {
            if (excelDate < 1)
            {
                throw new ArgumentException("Excel dates cannot be smaller than 0.");
            }

            DateTime dateOfReference = new DateTime(1900, 1, 1);

            if (excelDate > 60d)
            {
                excelDate = excelDate - 2;
            }
            else
            {
                excelDate = excelDate - 1;
            }

            return dateOfReference;
        }

        private static void ConvertType<T>(Func<double, DateTime> convertDateTime, T tnew, object val, Type type, PropertyInfo prop, int currentRow) where T : new()
        {
            //If it is numeric it is a double since that is how excel stores all numbers
            if (type != typeof(string))
            {
                if (val != null)
                {
                    var value = val.ToString();
                    value = String.Join("", value.Where(c => !char.IsWhiteSpace(c)));

                    if (value != "")
                    {
                        var regex = Regex.IsMatch(value, @"^[a-zA-Z]+$");

                        if (regex)
                        {
                            throw new DomainException(String.Format(CustomMessages.StringNotAllowed, prop.Name, currentRow));
                        }
                    }

                    else
                    {
                        val = null;
                    }
                }

                //Unbox it
                double? unboxedVal = (double?)val;

                //FAR FROM A COMPLETE LIST!!!
                if (unboxedVal == null)
                {
                    prop.SetValue(tnew, null);
                }

                else
                {
                    if (prop.PropertyType == typeof(int))
                    {
                        prop.SetValue(tnew, (int)unboxedVal);
                    }
                    else if (prop.PropertyType == typeof(long))
                    {
                        prop.SetValue(tnew, (long)unboxedVal);
                    }
                    else if (prop.PropertyType == typeof(short))
                    {
                        prop.SetValue(tnew, (short)unboxedVal);
                    }
                    else if (prop.PropertyType == typeof(double))
                    {
                        prop.SetValue(tnew, unboxedVal);
                    }
                    else if (prop.PropertyType == typeof(decimal))
                    {
                        prop.SetValue(tnew, (decimal)unboxedVal);
                    }
                    else if (prop.PropertyType == typeof(DateTime))
                    {
                        prop.SetValue(tnew, convertDateTime(unboxedVal.Value));
                    }
                    else if (prop.PropertyType == typeof(string))
                    {
                        prop.SetValue(tnew, val.ToString());
                    }
                    else
                    {
                        throw new NotImplementedException(string.Format("Type '{0}' not implemented yet!", prop.PropertyType.Name));
                    }
                }
            }
            //Its a string
            else
            {
                if (val == null)
                {
                    prop.SetValue(tnew, "");
                }

                else
                {
                    prop.SetValue(tnew, val.ToString());
                }
            }
        }
    }
}