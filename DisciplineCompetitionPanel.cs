using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FISCA.Presentation;
using CefSharp.WinForms;

namespace Ischool.discipline_competition
{
    public partial class DisciplineCompetitionPanel : BlankPanel
    {
        public CefSharp.WinForms.ChromiumWebBrowser browser;

        public DisciplineCompetitionPanel()
        {
            InitializeComponent();

            Group = "秩序競賽";

            browser = new ChromiumWebBrowser("https://sites.google.com/ischool.com.tw/facilities-service/%E9%A6%96%E9%A0%81");
            browser.Dock = DockStyle.Fill;
            ContentPanePanel.Controls.Add(browser);

        }

        private static DisciplineCompetitionPanel _DisciplineCompetitionPanel;

        public static DisciplineCompetitionPanel Instance
        {
            get
            {
                if (_DisciplineCompetitionPanel == null)
                {
                    _DisciplineCompetitionPanel = new DisciplineCompetitionPanel();
                }
                return _DisciplineCompetitionPanel;
            }
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // ContentPanePanel
            // 
            this.ContentPanePanel.Location = new System.Drawing.Point(0, 163);
            this.ContentPanePanel.Size = new System.Drawing.Size(870, 421);
            // 
            // DisciplineCompetitionPanel
            // 
            this.Name = "DisciplineCompetitionPanel";
            this.ResumeLayout(false);

        }
    }
}
