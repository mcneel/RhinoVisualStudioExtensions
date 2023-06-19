using System;
using Eto.Drawing;
using Eto.Forms;

namespace Rhino.VisualStudio.Controls
{
    /// <summary>
    /// Text and horizontal line intended to be used to visually separate control
    /// sections in Rhino panels and properties pages.
    /// 
    /// On Windows will appear as follows:
    ///     Label text ---------------------------------------------------
    /// 
    /// On Mac will appear as follows:
    ///     --------------------------------------------------------------
    ///     Label text
    /// 
    /// On Mac with atTopOfPanel set to true
    ///     Label text
    /// </summary>
    class PanelSeparator : Panel
    {
        /// <summary>
        /// Seporator styles
        /// </summary>
        public enum SeparatorStyle
        {
            /// <summary>
            /// Will draw line above label on Mac and label to left of the line on
            ///  Windows
            /// </summary>
            Normal,
            /// <summary>
            /// Will suppress the line above the label on Mac, does nothing on Windows
            /// </summary>
            Top,
            /// <summary>
            /// Draws lable to the left of the line on both Mac and Windows.
            /// </summary>
            ForceSameRow
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="T:PanelSeparator"/> class.
        /// </summary>
        /// <param name="text">
        /// Label text
        /// </param>
        /// <param name="style">
        /// Controls how the label and line appear.
        /// </param>
        public PanelSeparator(string text, SeparatorStyle style = SeparatorStyle.Normal)
        {
            Label = new Label { Text = text, Font = SystemFonts.Bold() };
            Label.Wrap = WrapMode.None;
            Label.VerticalAlignment = VerticalAlignment.Center;
            Line = new Divider { ForceHorizontalLine = true };
            Padding = 0;
            var mac = Platform.IsMac;
            var layout = new TableLayout
            {
                Padding = 0,
                Spacing = new Size(4, 4)
            };
            var line = new TableCell(Line) { ScaleWidth = true };
            if (mac && style != SeparatorStyle.ForceSameRow)
            {
                // Mac formatted as folows
                // -------------------------------------------------------------
                // Text
                Line.Size = new Size(20, 2);
                // https://mcneel.myjetbrains.com/youtrack/issue/RH-52544
                // This works on Windows because the line is to the right of the text
                // label and has a height of 20.  The default height seems to truncate
                // descending characters on Mac but setting the label height fixes the 
                // problem.
                Label.Height = 16;
                if (style != SeparatorStyle.Top) // Suppress horizontal line if true
                    layout.Rows.Add(new TableRow(line));
                layout.Rows.Add(Label);
            }
            else
            {
                // Windows formatted as follows
                // Text --------------------------------------------------------
                Line.Size = new Size(20, 20);
                layout.Rows.Add(new TableRow(Label, line));
            }
            layout.Rows.Add(null);
            Content = layout;
        }

        /// <summary>
        /// Label used to display text
        /// </summary>
        public Label Label { get; }

        /// <summary>
        /// Control used to draw the horizontal line
        /// </summary>
        public Divider Line { get; }
    }
}
