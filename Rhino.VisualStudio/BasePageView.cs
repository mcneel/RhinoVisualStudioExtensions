using Eto.Drawing;
using Eto.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rhino.VisualStudio
{
    public class BasePageView : Panel
    {
        Panel content = new Panel();
        Panel information = new Panel();

        public new Control Content
        {
            get { return content.Content; }
            set { content.Content = value; }
        }

        public Control Information
        {
            get { return information.Content; }
            set { information.Content = value; }
        }

        public BasePageView()
        {
            if (Platform.IsWpf)
            {
                BackgroundColor = Global.Theme.ProjectBackground;
                MinimumSize = new Size(700, 400);
            }

            content.Padding = new Padding(50, 10, 20, 10);

            base.Content = new StackLayout
            {
                Orientation = Orientation.Horizontal,
                VerticalContentAlignment = VerticalAlignment.Stretch,
                Items =
                {
                    new StackLayoutItem(content, VerticalAlignment.Center, expand: true),
                    new Panel { BackgroundColor = Global.Theme.SummaryBackground, Size = new Size(280, 200), Content = information, Padding = new Padding(20) }
                }
            };
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                if (DataContext is BaseWizardViewModel m)
                {
                    m.Continue();
                    e.Handled = true;
                }
            }
            if (e.KeyData == Keys.Escape)
            {
                if (DataContext is BaseWizardViewModel m)
                {
                    m.Abort();
                    e.Handled = true;
                }
            }
            base.OnKeyUp(e);
        }



    }
}
