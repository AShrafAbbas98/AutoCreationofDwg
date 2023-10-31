using Autodesk.Revit.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace AutoCreationofDwg.Register
{
    internal class RevitApp : IExternalApplication
    {
        public Result OnShutdown(UIControlledApplication application)
        {
            return Result.Succeeded;
        }

        public Result OnStartup(UIControlledApplication application)
        {
            string tabName = "CAD2Revit Tab";
            string panelName = "Modeling";
            string pushButtonName = "Convert Cad 2 Revit";
            string externalCommandClassName = "AutoCreationofDwg.Register.Commands";
            Uri C2R_ImgURL = new Uri("pack://application:,,,/AutoCreationofDwg;component/Images/icons8-modeling-32.png");


            application.CreateRibbonTab(tabName);
            RibbonPanel panelModeling = application.CreateRibbonPanel(tabName, panelName);
            string path = Assembly.GetExecutingAssembly().Location;
            PushButtonData buttonCad2REvit = new PushButtonData("pushButtonName", pushButtonName, path, externalCommandClassName);
            BitmapImage imageC2R= new BitmapImage(C2R_ImgURL);


            PushButton pushButtonCad2Revit = panelModeling.AddItem(buttonCad2REvit) as PushButton;
            pushButtonCad2Revit.LargeImage = imageC2R;


            return Result.Succeeded;
        }
    }
}
