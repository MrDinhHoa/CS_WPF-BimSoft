using DevExpress.Internal.WinApi.Windows.UI.Notifications;
using DevExpress.Xpf.Editors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.UI;

namespace Lesson13_Revit_P1
{
    class App : IExternalApplication
    {
        Result IExternalApplication.OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        Result IExternalApplication.OnStartup(UIControlledApplication a)
        {
            try
            {
                RevitCtrlApp = a;

                DoiTenSheetEvent = ExternalEvent.Create(new DoiTenSheet());


                RevitCtrlApp.CreateRibbonTab("K003 AddIns");

                var panel = RevitCtrlApp.CreateRibbonPanel("K003 AddIns", "Ứng dụng ví dụ");

                var btnData = new PushButtonData("OpenWindow", "Mở cửa sổ", Apploc, "K00Revit.LoadSheet");
                btnData.ToolTip = "Mở cửa sổ";
                btnData.LargeImage = PNGtoImageSource.Convert("K00Revit.Resources.Photo.Icon.Crane_32.png");
                btnData.Image = PNGtoImageSource.Convert("K00Revit.Resources.Photo.Icon.Crane_16.png");

                panel.AddItem(btnData);

            }
            catch
            {
                return Result.Failed;
            }

            return Result.Succeeded;
        }
    }
}
