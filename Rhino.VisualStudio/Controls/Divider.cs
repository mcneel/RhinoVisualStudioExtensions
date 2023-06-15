using Eto.Drawing;
using Eto.Forms;
using System;

namespace Rhino.VisualStudio.Controls
{
  /// <summary>
  /// Control to draw a horizontal or vertical line on the form
  /// </summary>
  /// <remarks>
  /// The orientation of the line is based on the size of the control. 
  /// e.g. if the width is greater than the height, then it will draw a horizontal line.
  /// </remarks>
  public class Divider : Drawable
  {
    private Color m_color = UnsetColor;

    /// <summary>
    /// Set Color to UnsetColor to use default system color for line drawing.
    /// </summary>
    /// <value>The unset color.</value>
    public static Color UnsetColor => Color.FromArgb(unchecked((int)0xFFEDEDED));

    /// <summary>
    /// When running in Windows 2 lines are drawn, the first is a shadow color
    /// (Color), the second a highlight (WindowsHighlightColor) color.
    /// 
    /// When Running on Mac a single line will be drawing using Color.
    /// </summary>
    public Color Color
    {
      get { return m_color; }
      set
      {
        if (m_color == value)
          return;
        m_color = value;
        Invalidate();
      }
    }

    /// <summary>
    /// If true will always draw a horizontal line otherwise; will draw a
    /// vertical line if the height is greater than the width.
    /// </summary>
    public bool ForceHorizontalLine
    {
      get => _forceHorizontalLine;
      set
      {
        if (value == _forceHorizontalLine)
          return;
        _forceHorizontalLine = value;
        Invalidate();
      }
    }
    private bool _forceHorizontalLine;

    /// <summary>
    /// Gets the color to use when drawing the line.
    /// </summary>
    /// <value>
    /// If Color is set to UnsetColor then system colors are returned otherwise;
    /// the Color value is returned.
    /// </value>
    virtual protected Color DrawColor
    {
      get
      {
        if (m_color != UnsetColor)
          return m_color;
        if (Platform.IsMac)
          return new Color(SystemColors.ControlText, 0.125f);
        return Color.FromArgb(88, 88, 88);
      }
    }

    public Orientation Orientation => !ForceHorizontalLine && Width < Height ? Orientation.Vertical : Orientation.Horizontal;

    public Divider()
    {
      Size = new Size(3, 3);
    }

    protected override void OnSizeChanged(EventArgs e)
    {
      base.OnSizeChanged(e);
      Invalidate();
    }

    protected override void OnLoadComplete(EventArgs e)
    {
      base.OnLoadComplete(e);
      Invalidate();
    }

    protected override void OnPaint(PaintEventArgs e)
    {
      base.OnPaint(e);
      var middle = new PointF(Size / 2);
      e.Graphics.FillRectangle(
        DrawColor,
        Orientation == Orientation.Horizontal
          ? new RectangleF(0f, middle.Y, ClientSize.Width, 1)
          : new RectangleF(middle.X, 0f, 1, ClientSize.Height));
    }
  }
}
