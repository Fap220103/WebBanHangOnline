using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace test_Xunit
{
    public class Config
    {
        public void ConfigureEPPlus()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
        }
    }
}
