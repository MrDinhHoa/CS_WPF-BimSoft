using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BIMSoftLib.MVVM;

namespace _04.Lesson04_UserManager.Object
{
    class UserInfor: PropertyChangedBase
    {
        private string _ten_vm = null;

        public string Ten
        {
            get
            {
                return _ten_vm;
            }
            set
            {
                _ten_vm = value;
                OnPropertyChanged("Ten");
            }
        }

        private string _diachi_vm = null;

        public string Diachi
        {
            get
            {
                return _diachi_vm;
            }
            set
            {
                _diachi_vm = value;
                OnPropertyChanged("DiaChi");
            }
        }

        private string _email_vm = null;

        public string Email
        {
            get
            {
                return _email_vm;
            }
            set
            {
                _email_vm = value;
                OnPropertyChanged("Email");
            }
        }

        private string _dienthoai_vm = null;

        public string DienThoai
        {
            get
            {
                return _dienthoai_vm;
            }
            set
            {
                _dienthoai_vm = value;
                OnPropertyChanged("DienThoai");
            }
        }
    }
}
