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
            MessageBox.Show("Đã bật được CAD","Lỗi",MessageBoxButtons.OK,MessageBoxIcon.Error);
            var adoc = Application.DocumentManager.MdiActiveDocument;
            Editor editor = adoc.Editor;

            //Create the Prompt;
            PromptKeywordOptions pko = new PromptKeywordOptions("Chọn command");
            pko.Keywords.Add("Command1");
            pko.Keywords.Add("comMand2");
            pko.AllowNone = false;

            //Get the user input
            PromptResult result = editor.GetKeywords(pko);
            if(result.Status == PromptStatus.OK)
            {editor.WriteMessage($"Nhập({ result.StringResult})"); }
            else { editor.WriteMessage("Không nhập gì"); }
        }


    }
}
