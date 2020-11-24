using Syncfusion.Windows.Controls.Grid;
using System;
using System.Windows;
using System.Windows.Controls;

namespace SiasoftAppExt
{
    //Sia.PublicarPnt(9486,"DaGrid");
    //Sia.TabU(9486);
    public partial class DaGrid : UserControl
    {
        dynamic SiaWin;
        dynamic tabitem;
        int idemp = 0;
        string cnEmp = "";
        public DaGrid(dynamic tabitem1)
        {
            InitializeComponent();
            InitializeComponent();
            SiaWin = Application.Current.MainWindow;
            tabitem = tabitem1;
            tabitem.Logo(9, ".png");
            tabitem.MultiTab = false;
            idemp = SiaWin._BusinessId;
            LoadConfig();
            try
            {
                llenarGrid();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error en : " + e);
            }
        }
        private void LoadConfig()
        {
            try
            {
                System.Data.DataRow foundRow = SiaWin.Empresas.Rows.Find(idemp);
                int idLogo = Convert.ToInt32(foundRow["BusinessLogo"].ToString().Trim());
                idemp = Convert.ToInt32(foundRow["BusinessId"].ToString().Trim());
                cnEmp = foundRow[SiaWin.CmpBusinessCn].ToString().Trim();
                string aliasemp = foundRow["BusinessAlias"].ToString().Trim();
                tabitem.Logo(idLogo, ".png");
                tabitem.Title = "Prueba de grid control(" + aliasemp + ")";
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }
        private void llenarGrid()
        {
            ScrollViewer ScrollViewer = new ScrollViewer();

            ////GridControl defined here
            GridControl gridControl = new GridControl();
            //Specifying row and column count
            gridControl.Model.RowCount = 100;
            gridControl.Model.ColumnCount = 20;

            //Looping through the cells and assigning the values based on row and column index
            for (int i = 0; i < 100; i++)
            {

                for (int j = 0; j < 20; j++)
                {
                    gridControl.Model[i, j].CellValue = string.Format("{0}/{1}", i, j);
                }
            }
            //ScrollViewer defined here


            //GridControl set as the content of the ScrollViewer
            ScrollViewer.Content = gridControl;

            //To bring the Grid control to the view, ScrollViewer should be set as a child of LayoutRoot      
            this.layoutRoot.Children.Add(ScrollViewer);


        }

    }
}
