using CatiaHoleAutomation.Services;
using System;
using System.Windows.Forms;

namespace CatiaHoleAutomation
{
    public partial class MainForm : Form
    {
        private readonly CatiaConnector _catiaConnector;

        public MainForm()
        {
            InitializeComponent();
            _catiaConnector = new CatiaConnector();
            _catiaConnector.Connect();
        }

    }
}
