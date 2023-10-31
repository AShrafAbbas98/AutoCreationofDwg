using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AutoCreationofDwg.Helper
{
    internal class MyExternalEventHandeler : IExternalEventHandler
    {
        public void Execute(UIApplication app)
        {
            using (Transaction trans = new Transaction(HelperDB.Doc, "Create Columns"))
            {
                trans.Start();

                try
                {
                    int count = 0;
                    foreach (var poly in HelperDB.columnsPolyLineList)
                    {
                        var layer = HelperDB.Doc.GetElement(poly.GraphicsStyleId) as GraphicsStyle;

                        string layerName = layer.GraphicsStyleCategory.Name;

                        if (layerName == HelperDB.selectedLayerName)
                        {
                            var columnOutline = poly.GetOutline();
                            XYZ maxPoint = columnOutline.MaximumPoint;
                            XYZ minPoint = columnOutline.MinimumPoint;

                            XYZ midPoint = HelperDB.GetMidPoint(maxPoint, minPoint);
                            //ColumnsBaseCoord.Add(midPoint);

                            XYZ columnLineBasePoint = new XYZ(midPoint.X, midPoint.Y, HelperDB.baseLevel.Elevation);
                            XYZ columnLineTopPoint = new XYZ(midPoint.X, midPoint.Y, HelperDB.topLevel.Elevation);

                            Line columnLine = Line.CreateBound(columnLineBasePoint, columnLineTopPoint);
                            //ColumnsBaseCoordLine.Add(columnLine);
                            count++;
                            HelperDB.Doc.Create.NewFamilyInstance(columnLine, HelperDB.SelectedColumnType, HelperDB.baseLevel, Autodesk.Revit.DB.Structure.StructuralType.Column);
                        }
                    }
                    HelperDB.noOfColumns_TextBlock = count;
                    trans.Commit();

                }
                catch (Exception)
                {

                    TaskDialog.Show("Error", "Error in creating Columns");
                    trans.Dispose();
                }
            }

        }

        public string GetName()
        {
            return "MyExternalEventHandeler";
        }
    }
}
