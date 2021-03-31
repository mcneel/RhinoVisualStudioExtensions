using Eto.Drawing;
using Eto.Forms;

namespace Rhino.VisualStudio
{
  public class BaseDialog : Dialog<bool>
  {
    Panel content;
    public new Control Content
    {
      get { return content.Content; }
      set { content.Content = value; }
    }

    public BaseDialog()
    {
      //ClientSize = new Size(800, 400);
      Style = "rhino.themed";
      Resizable = true;
      BackgroundColor = Global.Theme.ProjectDialogBackground;

      content = new Panel();

      AbortButton = new Button { Text = "C&ancel" };
      AbortButton.Click += (sender, e) => Close(false);

      DefaultButton = new Button { Text = "Finish" };
      DefaultButton.Click += (sender, e) => Close(true);
      DefaultButton.BindDataContext(c => c.Enabled, (BaseWizardViewModel m) => m.IsValid);

      var buttons = new StackLayout
      {
        Orientation = Orientation.Horizontal,
        Spacing = 5,
        Padding = new Padding(10),
        Items =
        {
          null,
          AbortButton,
          DefaultButton
        }
      };

      base.Content = new StackLayout
      {
        HorizontalContentAlignment = HorizontalAlignment.Stretch,
        Items =
        {
          new StackLayoutItem(content, true),
          buttons
        }
      };
    }
  }
}
