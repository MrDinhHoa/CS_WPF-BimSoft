    using CSiAPIv1;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Lesson12_EtabsCOM.Model
{
    [TestFixture]
    class Test

    {
        [Test]
        public static void KhoiDongEtabs()
        {
            Etabs.Connect();    
        }
    }

    public class Etabs
    {
        public static void Connect()
        {
            try
            {
                cOAPI myETABOject = null;
                cHelper myHelper = new Helper();
                //myETABOject = myHelper.CreateObjectProgID("CSI.ETABS.API.ETABSObject");
                int ret = 0;
                //string ModelPath = @"C:\\CSi_ETABS_API_Example\K003.edb";
                //System.IO.Directory.CreateDirectory(@"C:\\CSi_ETABS_API_Example\");
                //ret = myETABOject.ApplicationStart();
                myETABOject = myHelper.GetObject("CSI.ETABS.API.ETABSObject");
                //cSapModel mySapModel = default(cSapModel);
                cSapModel mySapModel = myETABOject.SapModel;
                Console.WriteLine(mySapModel);
                ret = mySapModel.Results.Setup.DeselectAllCasesAndCombosForOutput();
                ret = mySapModel.Results.Setup.SetComboSelectedForOutput("ULS01");
                
                string Name = "1";
                eItemTypeElm ItemTypeElm;
                int NumberResult = 0;
                string[] Obj = new string[] {};
                double[] ObjSta = new double[] { };

                string[] Elm = new string[] { };

                double[] ElmSta = new double[] { };

                string[] LoadCase = new string[] { };

                string[] StepType = new string[] { };

                double[] StepNum = new double[] { };

                double[] P = new double[] { };

                double[] V2 = new double[] { };

                double[] V3 = new double[] { };

                double[] T = new double[] { };

                double[] M2 = new double[] { };

                double[] M3 = new double[] { };

                ret = mySapModel.Results.FrameForce(Name, eItemTypeElm.Element, ref NumberResult, ref Obj, ref ObjSta, ref Elm, ref ElmSta, ref LoadCase, ref StepType,
                    ref StepNum, ref P, ref V2, ref V3, ref T, ref M2, ref M3);

            }
            catch { }
        }
    }

}
