using Microsoft.Xaml.Behaviors.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BIMSoftLib.MVVM;
using System.Windows.Input;
using _04.Lesson04_UserManager.Object;
using _04.Lesson04_UserManager.MVVM.View;

namespace _04_Lesson04_UserManager.MVVM.ViewModel
{
    class vmMain : PropertyChangedBase
    {
        public static vmMain DcMain = new vmMain();

        private ObservableRangeCollection<UserInfor> _gcSource_vm = new ObservableRangeCollection<UserInfor>();

        public ObservableRangeCollection<UserInfor> GcSource
        {
            get
            {
                return _gcSource_vm;
            }
            set
            {
                _gcSource_vm = value;
                OnPropertyChanged("GcSource");
            }
        }

        private ActionCommand _cmdAddUser_vm;

        public ICommand CmdAddUser
        {
            get
            {
                if (_cmdAddUser_vm == null)
                {
                    _cmdAddUser_vm = new ActionCommand(PerformCmdAddUser);
                }

                return _cmdAddUser_vm;
            }
        }

        private void PerformCmdAddUser()
        {
            var win = new vUserInfor();
            win.Show();
        }

        private ActionCommand _cmdEditUser_vm;

        public ICommand CmdEditUser
        {
            get
            {
                if (_cmdEditUser_vm == null)
                {
                    _cmdEditUser_vm = new ActionCommand(PerformCmdEditUser);
                }

                return _cmdEditUser_vm;
            }
        }

        private void PerformCmdEditUser()
        {
        }

        private ActionCommand _cmdRemoveUser_vm;

        public ICommand CmdRemoveUser
        {
            get
            {
                if (_cmdRemoveUser_vm == null)
                {
                    _cmdRemoveUser_vm = new ActionCommand(PerformCmdRemoveUser);
                }

                return _cmdRemoveUser_vm;
            }
        }

        private void PerformCmdRemoveUser()
        {
        }
    }
}
