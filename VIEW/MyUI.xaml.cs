using AutoCreationofDwg.Helper;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace AutoCreationofDwg.VIEW
{
    /// <summary>
    /// Interaction logic for MyUI.xaml
    /// </summary>
    public partial class MyUI : Window
    {

        #region Field

        Document Doc;
        List<ImportInstance> DwgsList = new List<ImportInstance>();
        List<FamilySymbol> columnTypesList = new List<FamilySymbol>();

        List<Level> levelsList = new List<Level>();

        List<string> LayerNameList = new List<string>();

        List<PolyLine> columnsPolyLineList = new List<PolyLine>();

        List<XYZ> ColumnsBaseCoord = new List<XYZ>();
        List<Line> ColumnsBaseCoordLine = new List<Line>();

        MyExternalEventHandeler myHandeler;
        ExternalEvent external;


        //HelperDB helper = new HelperDB();

        #endregion


        // Constructor
        public MyUI(Document doc)
        {
            InitializeComponent();
            Doc = doc;
            HelperDB.Doc = doc;

            myHandeler = new MyExternalEventHandeler();
            external = ExternalEvent.Create(myHandeler);    
        }

        //Methods
        private void CollectBtn_Click(object sender, RoutedEventArgs e)
        {
            DwgsList.Clear();
            this.DWGsBox.Items.Clear();


            //Collect All DWG
            var DwgImports = new FilteredElementCollector(Doc)
                .OfClass(typeof(ImportInstance))
                .WhereElementIsNotElementType()
                .ToElementIds();

            foreach (var dwgId in DwgImports)
            {
                var Dwg = Doc.GetElement(dwgId) as ImportInstance;
                DwgsList.Add(Dwg);

                var DwgCategory = Dwg.Category;
                string DwgName = DwgCategory.Name;

                this.DWGsBox.Items.Add(DwgName);
            }


            //Get Column types
            var columnsIds = new FilteredElementCollector(Doc)
                .OfCategory(BuiltInCategory.OST_StructuralColumns)
                .WhereElementIsElementType()
                .ToElementIds();

            foreach (var colId in columnsIds)
            {
                var columnType = Doc.GetElement(colId) as FamilySymbol;
                columnTypesList.Add(columnType);

                this.ColumnTypesBox.Items.Add(columnType.Name);
            }


            //Get all levels in Revit 
            levelsList.Clear();

            levelsList = new FilteredElementCollector(Doc)
                .OfClass(typeof(Level))
                .WhereElementIsNotElementType()
                .Cast<Level>()
                .ToList();


            foreach (var level in levelsList)
            {
                string levelName = level.Name;

                this.TopLevelBox.Items.Add(levelName);
                this.BottomLevelBox.Items.Add(levelName);
            }

        }

        private void RefreshBtn_Click(object sender, RoutedEventArgs e)
        {
            this.LayersBox.Items.Clear();
            HelperDB.columnsPolyLineList.Clear();

            var DwgName = this.DWGsBox.SelectedItem.ToString();

            var Dwg = DwgsList.Where(x => x.Category.Name == DwgName).FirstOrDefault() as ImportInstance;

            var DwgGeo = Dwg.get_Geometry(new Options());

            foreach (var dwgObj in DwgGeo)
            {
                if (dwgObj is GeometryInstance)
                {
                    var DwgInst = dwgObj as GeometryInstance;
                    GeometryElement GeoElement = DwgInst.GetInstanceGeometry();

                    foreach (var ele in GeoElement)
                    {
                        if (ele is PolyLine)
                        {
                            HelperDB.columnsPolyLineList.Add((PolyLine)ele);
                            var layer = Doc.GetElement(ele.GraphicsStyleId) as GraphicsStyle;

                            string layerName = layer.GraphicsStyleCategory.Name;
                            LayerNameList.Add(layerName);

                        }
                    }
                }
            }

            foreach(var layer in LayerNameList.Distinct())
            {
                this.LayersBox.Items.Add(layer);

            }
        }

        private void GenerateBtn_Click(object sender, RoutedEventArgs e)
        {
            //FrontEnd data
            // Get user selections
            var SelectedColumnTypeName = this.ColumnTypesBox.SelectedItem.ToString();
            HelperDB.SelectedColumnType = columnTypesList.Where(x => x.Name == SelectedColumnTypeName).FirstOrDefault();

            HelperDB.selectedLayerName = this.LayersBox.SelectedItem.ToString();

            var baseLevelName = this.BottomLevelBox.SelectedItem.ToString();
            var topLevelName = this.TopLevelBox.SelectedItem.ToString();

            var topLevel = HelperDB.baseLevel = levelsList.Where(x => x.Name == baseLevelName).FirstOrDefault();
            var baseLevel =HelperDB.topLevel = levelsList.Where(x => x.Name == topLevelName).FirstOrDefault();

            double height = Math.Abs(topLevel.Elevation - baseLevel.Elevation);
            //Backend Actions
            //Get Mid Points from PolyLine of selected layer


            external.Raise();


            this.noOfColumns_TextBlock.Text = HelperDB.noOfColumns_TextBlock.ToString();

        }
    }
}
