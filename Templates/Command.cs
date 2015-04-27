using System;
using Eto.Drawing;
using Eto.Forms;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.UI;

namespace ${namespace}
{
  /// <summary>
  /// Command logic
  /// </summary>
  public class ${EscapedIdentifier} : Rhino.Commands.Command
  {
    public override string EnglishName
    {
      get { return "${EscapedIdentifier}"; }
    }

    protected override Result RunCommand(Rhino.RhinoDoc doc, RunMode mode)
    {
      var model = new ${EscapedIdentifier}Model { Point = Rhino.Geometry.Point3d.Origin };

      var result = mode == RunMode.Interactive ? RunInteractive(doc, model) : RunScript(model);
      if (result != Result.Success)
        return result;
      if (System.Guid.Empty == doc.Objects.AddPoint(model.Point))
        return Result.Failure;
      doc.Views.Redraw();
      return Result.Success;
    }

    Result RunScript(${EscapedIdentifier}Model model)
    {
      Point3d point;
      var result = Rhino.Input.RhinoGet.GetPoint("Location for new point", false, out point);
      if (result == Result.Success)
        model.Point = point;
      return result;
    }

    Result RunInteractive(Rhino.RhinoDoc doc, ${EscapedIdentifier}Model model)
    {
      var form = new ${EscapedIdentifier}Form { DataContext = model };

      // save and restore position
      form.RestorePosition();
      form.UnLoad += (sender, e) => form.SavePosition();

      return form.ShowModal(RhinoEtoApp.MainWindow);
    }
  }

  /// <summary>
  /// View model
  /// </summary>
  class ${EscapedIdentifier}Model : Rhino.UI.ViewModel
  {
    Point3d m_point;

    public Point3d Point
    {
      get { return m_point; }
      set
      { 
        if (m_point == value)
          return;
        m_point = value; 
        RaisePropertyChanged(() => Point);
        RaisePropertyChanged(() => X);
        RaisePropertyChanged(() => Y);
        RaisePropertyChanged(() => Z);
      }
    }

    public double X
    {
      get { return Point.X; }
      set { Point = new Point3d(value, Point.Y, Point.Z); }
    }

    public double Y
    {
      get { return Point.Y; }
      set { Point = new Point3d(Point.X, value, Point.Z); }
    }

    public double Z
    {
      get { return Point.Z; }
      set { Point = new Point3d(Point.X, Point.Y, value); }
    }
  }

  /// <summary>
  /// User interface
  /// </summary>
  class ${EscapedIdentifier}Form : Dialog<Result>
  {
    public ${EscapedIdentifier}Form()
    {
      Title = "${EscapedIdentifier}";
      Resizable = true;
      Result = Rhino.Commands.Result.Cancel;

      // Input controls
      var x_point = new NumericUpDown { DecimalPlaces = 5, Increment = 0.1 };
      x_point.ValueBinding.BindDataContext((${EscapedIdentifier}Model r) => r.X);

      var y_point = new NumericUpDown { DecimalPlaces = 5, Increment = 0.1 };
      y_point.ValueBinding.BindDataContext((${EscapedIdentifier}Model r) => r.Y);

      var z_point = new NumericUpDown { DecimalPlaces = 5, Increment = 0.1 };
      z_point.ValueBinding.BindDataContext((${EscapedIdentifier}Model r) => r.Z);

      var pickPointButton = new Button { Text = "Pick Point >>" };
      pickPointButton.Click += (sender, e) => this.PushPickButton(PickButtonCallback);

      // Table of options
      var options = new TableLayout
      {
        Spacing = new Size(5, 5),
        Rows =
        {
          new TableRow("X:", x_point),
          new TableRow("Y:", y_point),
          new TableRow("Z:", z_point),
          new TableRow(new TableCell(), pickPointButton)
        }
      };

      // Buttons
      DefaultButton = new Button { Text = "Apply" };
      DefaultButton.Click += (sender, e) => Close(Result.Success);

      AbortButton = new Button { Text = "C&ancel" };
      AbortButton.Click += (sender, e) => Close(Result.Cancel);

      var buttons = new StackLayout
      {
        Orientation = Orientation.Horizontal,
        Spacing = 5,
        Items = { null, AbortButton, DefaultButton }
      };

      // Top layout
      Content = new StackLayout
      {
        Padding = new Padding(10),
        Spacing = 5,
        HorizontalContentAlignment = HorizontalAlignment.Stretch,
        Items =
        {
          new StackLayoutItem(options, expand: true),
          buttons
        }
      };
    }

    void PickButtonCallback(object sender, EventArgs e)
    {
      Point3d point;
      if (Result.Success == Rhino.Input.RhinoGet.GetPoint("Location for new point", false, out point))
        ((${EscapedIdentifier}Model)DataContext).Point = point;
    }
  }
}

