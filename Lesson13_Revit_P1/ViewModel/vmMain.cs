using Autodesk.Revit.UI;
using DevExpress.Data.XtraReports.Native;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BIMSoftLib.MVVM;
using Lesson13_Revit_P1.Object;
using Microsoft.Xaml.Behaviors.Core;

namespace Lesson13_Revit_P1.ViewModel
{

        class vmMain : PropertyChangedBase
        {
            public static vmMain DcMain = new vmMain();

            public static UIControlledApplication RevitCtrlApp;
            public static UIApplication RevitApp;
            public static string Apploc = Assembly.GetExecutingAssembly().Location;


            private ObservableRangeCollection<RevitSheetInfo> _gcSource = new ObservableRangeCollection<RevitSheetInfo>();
            public ObservableRangeCollection<RevitSheetInfo> gcSource
            {
                get
                {
                    return _gcSource;
                }
                set
                {
                    _gcSource = value;
                    OnPropertyChanged("gcSource");
                }
            }



            private ActionCommand doiTenCmd;

            public ICommand DoiTenCmd
            {
                get
                {
                    if (doiTenCmd == null)
                    {
                        doiTenCmd = new ActionCommand(PerformDoiTenCmd);
                    }

                    return doiTenCmd;
                }
            }

            private async void PerformDoiTenCmd()
            {

                try
                {
                    DoiTenSheet.sList = DcMain.gcSource.ToList();
                    DoiTenSheetEvent.Raise();
                }
                catch (Exception)
                {
                }

            }
        }
}
