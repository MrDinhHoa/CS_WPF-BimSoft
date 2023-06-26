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
        private ElementId _id = null;
        public ElementId Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        private string _sheetName = null;
        public string SheetName
        {
            get
            {
                return _sheetName;
            }
            set
            {
                _sheetName = value;
                OnPropertyChanged("SheetName");
            }
        }


        private string _newSheetName = null;
        public string NewSheetName
        {
            get
            {
                return _newSheetName;
            }
            set
            {
                _newSheetName = value;
                OnPropertyChanged("NewSheetName");
            }
        }
    }
}
