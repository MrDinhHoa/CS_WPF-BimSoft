using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Lesson13_Revit_P1.ViewModel.vmMain;
using Lesson13_Revit_P1.Object;
using Lesson13_Revit_P1.ViewModel;
using Lesson13_Revit_P1.View;

namespace Lesson13_Revit_P1
{
    [Transaction(TransactionMode.Manual)]
    class LoadSheet
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            return Execute(commandData.Application);
        }

        public Result Execute(UIApplication uiApp)
        {
            RevitApp = uiApp;

            DcMain.gcSource.Clear();

            UIDocument uiDoc = RevitApp.ActiveUIDocument;
            Document doc = uiDoc.Document;

            FilteredElementCollector collector = new FilteredElementCollector(doc);
            ICollection<Element> sheets = collector.OfClass(typeof(ViewSheet)).ToElements();

            var sList = new List<RevitSheetInfo>();

            foreach (Element sheet in sheets)
            {
                var s = new RevitSheetInfo
                {
                    Id = sheet.Id,
                    SheetName = sheet.Name,
                };

                sList.Add(s);
            }


            DcMain.gcSource.AddRange(sList);


            var win = new vMain();
            win.Show();

            return Result.Succeeded;
        }
    }
}
