using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;

namespace Lesson09_AcadNET
{
    public class Lesson09_AcadNET
    {
        [CommandMethod("TKThepDam")]

        public void TKThepDam()
        {
           
            var adoc = Application.DocumentManager.MdiActiveDocument;
            Editor editor = adoc.Editor;

            //Create the Prompt;
            PromptDoubleOptions Dia = new PromptDoubleOptions("Đường kính: ");
            PromptDoubleResult DiaResult = editor.GetDouble(Dia);

            PromptDoubleOptions Num = new PromptDoubleOptions("Số lượng: ");
            PromptDoubleResult NumResult = editor.GetDouble(Num);

            PromptDoubleOptions NumEle = new PromptDoubleOptions("Số cấu kiện: ");
            PromptDoubleResult NumEleResult = editor.GetDouble(NumEle);


        }


    }
}
