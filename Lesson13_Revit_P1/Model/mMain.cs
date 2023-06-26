using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Lesson13_Revit_P1.Object;

namespace Lesson13_Revit_P1.Model
{
    class mMain
    {
        public static void ChangeSheetName(Document doc, List<RevitSheetInfo> sList)
        {
            try
            {
                foreach (var info in sList)
                {
                    if (!string.IsNullOrEmpty(info.NewSheetName) && !string.IsNullOrWhiteSpace(info.NewSheetName))
                    {
                        var sht = doc.GetElement(info.Id) as ViewSheet;

                        Transaction trans = new Transaction(doc, "Sửa tên Sheet");
                        trans.Start();

                        sht.Name = info.NewSheetName;

                        trans.Commit();
                    }
                }


            }
            catch (Exception)
            {
            }

        }
    }
}
