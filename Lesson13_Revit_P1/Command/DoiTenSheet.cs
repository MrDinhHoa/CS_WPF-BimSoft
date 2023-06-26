using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Lesson13_Revit_P1.Object;

namespace Lesson13_Revit_P1.Command
{
    class DoiTenSheet: IExternalEventHandler
    {
        public static List<RevitSheetInfo> sList = new List<RevitSheetInfo>();
        public static ExternalEvent DoiTenSheetEvent = null;

        void IExternalEventHandler.Execute(UIApplication app)
        {
            try
            {
                UIDocument uiDoc = app.ActiveUIDocument;
                Document doc = uiDoc.Document;
                foreach (var info in sList)
                {
                    if (!string.IsNullOrEmpty(info.NewSheetName) && !string.IsNullOrWhiteSpace(info.NewSheetName))
                    {
                        var sht = doc.GetElement(info.Id) as ViewSheet;

                        using (Transaction trans = new Transaction(doc, "Sửa tên Sheet"))
                        {
                            trans.Start();
                            sht.Name = info.NewSheetName;
                            trans.Commit();
                        }
                    }
                }
            }
            catch (Exception)
            {
            }

        }

        string IExternalEventHandler.GetName()
        {
            return "DoiTenSheet Event Handler";
        }
    }
}

