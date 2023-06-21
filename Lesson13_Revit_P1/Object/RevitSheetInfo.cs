using Autodesk.Revit.DB;
using BIMSoftLib.MVVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson13_Revit_P1.Object
{
    public class RevitSheetInfo : PropertyChangedBase
    {
        private ElementId _Id = null;
        public ElementId Id
        {
            get
            {
                return _Id;
            }
            set
            {
                _Id = value;
                OnPropertyChanged("Id");
            }
        }

        private string _SheetName = null;
        public string SheetName
        {
            get
            {
                return _SheetName;
            }
            set
            {
                _SheetName = value;
                OnPropertyChanged("SheetName");
            }
        }


        private string _NewSheetName = null;
        public string NewSheetName
        {
            get
            {
                return _NewSheetName;
            }
            set
            {
                _NewSheetName = value;
                OnPropertyChanged("NewSheetName");
            }
        }
    }
}
