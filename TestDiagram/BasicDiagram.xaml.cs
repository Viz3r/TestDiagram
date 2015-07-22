using DevExpress.Diagram.Core;
using DevExpress.Mvvm.Native;
using DevExpress.Mvvm.UI.Interactivity;
using DevExpress.Xpf.Bars;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Core.Native;
using DevExpress.Xpf.Diagram.Native;
using DevExpress.Xpf.PropertyGrid;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DevExpress.Xpf.Diagram.Tests.Samples {
    /// <summary>
    /// Interaction logic for BasicDiagram.xaml
    /// </summary>
    public partial class BasicDiagram : UserControl {
        Random random = new Random();
        public BasicDiagram() {
            InitializeComponent();
            diagramControl.Items.CollectionChanged += Items_CollectionChanged;
            tools.ItemLinksSource = new DiagramTool[] {
                diagramControl.ActiveTool,
                new PredefinedShapeTool(PredefinedShapeKind.Rectangle),
                new PredefinedShapeTool(PredefinedShapeKind.Ellipse),
                new ContainerTool(),
            };
            fontSizes.ItemsSource = new double[] {
                6, 8, 9, 10, 11, 12, 14, 16, 18, 24, 30, 36, 48, 60,
            };
            textAlignments.ItemLinksSource = Enum.GetValues(typeof(TextAlignment));
            verticalAlignments.ItemLinksSource = Enum.GetValues(typeof(VerticalAlignment)).Cast<VerticalAlignment>().Except(new[] { VerticalAlignment.Stretch });
            measureUnits.ItemsSource = new[] {
                MeasureUnits.Pixels,
                MeasureUnits.Inches,
                MeasureUnits.Millimeters,
            };
        }

        

        private void BarButtonItem_ItemClick(object sender, ItemClickEventArgs e) {
            propertyGrid.ClearValue(PropertyGridControl.SelectedObjectProperty);
        }

        private void leftMargin_EditValueChanged(object sender, RoutedEventArgs e) {
            var margin = diagramControl.ScrollMargin;
            margin.Left = (double)leftMargin.EditValue;
            diagramControl.ScrollMargin = margin;
        }

        private void topMargin_EditValueChanged(object sender, RoutedEventArgs e) {
            var margin = diagramControl.ScrollMargin;
            margin.Top = (double)topMargin.EditValue;
            diagramControl.ScrollMargin = margin;
        }

        private void rightMargin_EditValueChanged(object sender, RoutedEventArgs e) {
            var margin = diagramControl.ScrollMargin;
            margin.Right = (double)rightMargin.EditValue;
            diagramControl.ScrollMargin = margin;

        }

        private void bottomMargin_EditValueChanged(object sender, RoutedEventArgs e) {
            var margin = diagramControl.ScrollMargin;
            margin.Bottom = (double)bottomMargin.EditValue;
            diagramControl.ScrollMargin = margin;
        }

        private void BestFitTreeLayout_Click(object sender, ItemClickEventArgs e) {
            DevExpress.Diagram.Core.Layout.TreeLayout layout = new DevExpress.Diagram.Core.Layout.TreeLayout();
            layout.BestFit(diagramControl.RootItem());
        }
        void Items_CollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e) {
            verticesCore = diagramControl.Items.Where(item =>!(item is DiagramConnector)).ToList();
            edgesCore = diagramControl.Items.Where(edge => edge is DiagramConnector).Cast<DiagramConnector>().ToList();
        }
        List<DiagramItem> verticesCore;
        public List<DiagramItem> Vertices { get { if(verticesCore == null) Items_CollectionChanged(null, null); return verticesCore; } }
        List<DiagramConnector> edgesCore;
        public List<DiagramConnector> Edges { get { if(edgesCore == null) Items_CollectionChanged(null, null); return edgesCore; } }
        private void GenerateRandom_Click(object sender, ItemClickEventArgs e) {
            for(int i = 0; i < 15; i++) AddRandomItem();
            for(int i = 0; i < 10; i++) GenerateVertices();
        }

        private void GenerateVertices() {
            if(Vertices.Count < 2) return;
            int id1 = random.Next(0, Vertices.Count);
            int id2 = random.Next(0, Vertices.Count);
            while(id1 == id2) {
                id2 = random.Next(0, Vertices.Count);
            }
            diagramControl.Items.Add(new DiagramConnector() { BeginShape = Vertices[id1], EndShape = Vertices[id2], CanMove = false});
        }

        private void AddRandomItem() {
            diagramControl.Items.Add(new DiagramShape() { Bounds = new Rect(random.Next(0, 100), random.Next(0, 100), random.Next(10, 100), random.Next(10, 100)), Background = Brushes.Red, Stroke = Brushes.Blue });
        }

        private void Clear_Click(object sender, ItemClickEventArgs e) {
            diagramControl.Items.Clear();
        }
    }

    public static class Converter {
        public static double SizeToDouble(Size size) {
            return size.Width;
        }
        public static Size DoubleToSize(double val) {
            return new Size(Math.Round(val), Math.Round(val));
        }
        public static bool ResizingModeToBool(ResizingMode mode) {
            return mode == ResizingMode.Live;
        }
        public static ResizingMode BoolToResizingMode(bool allowLiveResizing) {
            return allowLiveResizing ? ResizingMode.Live : ResizingMode.Preview;
        }
        public static ImageSource TextAlignmentToImage(TextAlignment alignment) {
            return GetImageByName(string.Format("Align{0}_16x16.png", alignment));
        }
        public static ImageSource VerticalAlignmentToImage(VerticalAlignment alignment) {
            return GetImageByName(string.Format("AlignHorizontal{0}_16x16.png", alignment));
        }
        static ImageSource GetImageByName(string name) {
            
            return (ImageSource)new DXImageExtension() { Image = (DXImageInfo)new DXImageConverter().ConvertFromString(name) }.ProvideValue(null);
        }
    }
}
