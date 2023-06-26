using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using BIMSoftLib.MVVM;
using Lesson13_Revit_P1.Object;
using Microsoft.Xaml.Behaviors.Core;
using System.Windows.Input;
using Lesson13_Revit_P1.Command;
using static Lesson13_Revit_P1.Command.DoiTenSheet;

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
            get => _gcSource;
            set
            {
                _gcSource = value; 
                OnPropertyChanged("gcSource");
            }
        }
        private ActionCommand _doiTenCmd;

        

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
        public ICommand DoiTenCmd
        {
            get
            {
                if (_doiTenCmd == null)
                {
                    _doiTenCmd = new ActionCommand(PerformDoiTenCmd);
                }
                return _doiTenCmd;
            }
        }
    }
}
