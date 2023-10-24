using AutoCreationofDwg.VIEW;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCreationofDwg.Register
{
    [TransactionAttribute(TransactionMode.Manual)]
    public class Commands : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIDocument uiDoc = commandData.Application.ActiveUIDocument;
            Document doc = uiDoc.Document;

            MyUI ui = new MyUI(doc);
            ui.Show();

            return Result.Succeeded;
        }
    }
}
