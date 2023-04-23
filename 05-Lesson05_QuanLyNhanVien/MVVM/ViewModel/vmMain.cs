using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using _05_Lesson05_QuanLyNhanVien.MVVM.View;
using BIMSoftLib;
using BIMSoftLib.MVVM;
using Microsoft.Xaml.Behaviors.Core;

namespace _05_Lesson05_QuanLyNhanVien.MVVM.ViewModel
{
    class vmMain: PropertyChangedBase
    {
        public static vmMain DcMain = new vmMain();
        private ActionCommand _cmdDonVi_vm = null;

        public ICommand CmdDonVi
        {
            get
            {
                if (_cmdDonVi_vm == null)
                {
                    _cmdDonVi_vm = new ActionCommand(PerformCmdDonVi);
                }

                return _cmdDonVi_vm;
            }
        }
        private void PerformCmdDonVi()
        {
            try
            {
                var win = new vDonVi();
                win.Show();
            }
            catch
            {

            }
        }

        // ReSharper disable once InconsistentNaming
        private ActionCommand _clickLeft_vm = null;
        public ICommand ClickLeft
        {
            get
            {
                if (_clickLeft_vm == null)
                {
                    _clickLeft_vm = new ActionCommand(PerformClickLeft);
                }

                return _clickLeft_vm;
            }
        }
        private void PerformClickLeft()
        {
            MessageBox.Show("Bấm 1 lần");
        }

        private ActionCommand _clickRight_vm = null;
        public ICommand ClickRight
        {
            get
            {
                if (_clickRight_vm == null)
                {
                    _clickRight_vm = new ActionCommand(PerformClickRight);
                }

                return _clickRight_vm;
            }
        }
        private void PerformClickRight()
        {
            MessageBox.Show("Bấm 2 lần");
        }
    }
}
