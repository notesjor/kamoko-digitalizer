using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Diagrams;
using Telerik.Windows.Diagrams.Core;

namespace CorpusExplorer.Tool4.KAMOKO.FlowViz
{
  /// <summary>
  ///   Interaktionslogik für WpfDiagram.xaml
  /// </summary>
  public partial class KamokoFlowDiagram : UserControl
  {
    [NonSerialized] private readonly Dictionary<Guid, IShape> _nodes = new Dictionary<Guid, IShape>();

    public KamokoFlowDiagram()
    {
      StyleManager.ApplicationTheme = new Windows8TouchTheme();
      InitializeComponent();
    }

    public void CallAddConnection(Guid nodeSource, Guid nodeTarget)
    {
      var con = diagram.AddConnection(_nodes[nodeSource], _nodes[nodeTarget]);
      con.TargetCapSize = new Size(10, 10);
      con.TargetCapType = CapType.Arrow1;
    }

    public Guid CallAddNode(string node, FlowVizVote[] votes, bool isOriginal)
    {
      var shape = new RadDiagramShape
      {
        Width = 180,
        Height = 100,
        Position = new Point {X = 100, Y = 100},
        Content = MakeComplexNodeContent(node, votes),
        Geometry = ShapeFactory.GetShapeGeometry(CommonShapeType.RectangleShape),
        Background = isOriginal ? new SolidColorBrush(Color.FromRgb(180, 255, 180)) : new SolidColorBrush(Colors.White)
      };

      diagram.AddShape(shape);

      var res = Guid.NewGuid();
      _nodes.Add(res, shape);

      return res;
    }

    public void CallLayoutAsHorizontalTree()
    {
      diagram.Layout(
                     LayoutType.Tree,
                     new TreeLayoutSettings
                     {
                       TreeLayoutType = TreeLayoutType.TreeRight,
                       TipOverTreeStartLevel = int.MaxValue,
                       HorizontalSeparation = 50,
                       VerticalSeparation = 30
                     });
    }

    public void CallNew()
    {
      diagram.Clear();
      _nodes.Clear();
    }

    private object MakeComplexNodeContent(string node, FlowVizVote[] votes)
    {
      var grid = new Grid();
      grid.RowDefinitions.Add(new RowDefinition());
      grid.RowDefinitions.Add(new RowDefinition {Height = new GridLength(24)});

      // TEXT
      var text = new TextBlock {Text = node, Foreground = new SolidColorBrush(Colors.Black)};
      grid.Children.Add(text);
      Grid.SetRow(text, 0);

      // VOTES
      var vpanel = new WrapPanel {Height = 24, HorizontalAlignment = HorizontalAlignment.Stretch};
      for (var i = 0; i < votes.Length; i++)
      {
        var canvas = new Canvas {Width = 24, Height = 24};
        var ellipse = new Ellipse {Width = 24, Height = 24, Stroke = new SolidColorBrush(Colors.Black)};
        switch (votes[i])
        {
          case FlowVizVote.None:
            ellipse.Fill = new SolidColorBrush(Colors.White);
            break;
          case FlowVizVote.Accept:
            ellipse.Fill = new SolidColorBrush(Colors.Green);
            break;
          case FlowVizVote.PossibleIf:
            ellipse.Fill = new SolidColorBrush(Colors.Yellow);
            break;
          case FlowVizVote.Denied:
            ellipse.Fill = new SolidColorBrush(Colors.Red);
            break;
          default:
            throw new ArgumentOutOfRangeException();
        }

        canvas.Children.Add(ellipse);
        var ngrid = new Grid {Margin = new Thickness(0, 3, 0, 0)};
        ngrid.Children.Add(
                           new TextBlock
                           {
                             Width = 24,
                             Height = 21,
                             TextAlignment = TextAlignment.Center,
                             Text = (i + 1).ToString(),
                             Foreground = new SolidColorBrush(Colors.Black)
                           });
        canvas.Children.Add(ngrid);
        vpanel.Children.Add(canvas);
      }

      grid.Children.Add(vpanel);
      Grid.SetRow(vpanel, 1);

      return grid;
    }
  }
}