using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using static _03.Lesson03_BindingNangCao.MVVM.Model.TinhTong;

namespace _03.Lesson03_BindingNangCao.MVVM.Model
{
    class NUnitTest
    {
        public void TinhTong()
        {
            int a = 1;
            int b = 5;
            int kq = Cong(a, b);
            Console.WriteLine(kq);
        }
    }

    class TinhTong
    {
        public static int Cong(int a, int b)
        {
            return a + b;
        }
    }
}
