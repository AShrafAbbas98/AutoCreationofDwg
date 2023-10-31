using Autodesk.Revit.Creation;
using Autodesk.Revit.DB;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Document = Autodesk.Revit.DB.Document;

namespace AutoCreationofDwg.Helper
{
    internal class HelperDB
    {
        //Fields

        public static Document Doc = null;
        public static string selectedLayerName = null;
                
        public static Level baseLevel = null;
        public static Level topLevel = null;
                
        public static FamilySymbol SelectedColumnType = null;
                
        public static int noOfColumns_TextBlock = 0;
                
        public static List<PolyLine> columnsPolyLineList = new List<PolyLine>();
        //public static List<XYZ> ColumnsBaseCoord = new List<XYZ>();
        //public static List<Line> ColumnsBaseCoordLine = new List<Line>();

        public string name = "Amr";
        //Constructor
        public HelperDB() { }


        //Methods
        public static XYZ GetMidPoint(XYZ maxPoint, XYZ minPoint)
        {
            double x = (maxPoint.X + minPoint.X) / 2;
            double y = (maxPoint.Y + minPoint.Y) / 2;
            double z = (maxPoint.Z + minPoint.Z) / 2;

            XYZ midPoint = new XYZ(x, y, z);

            return midPoint;
        }


    }
}
