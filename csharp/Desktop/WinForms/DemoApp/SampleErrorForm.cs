using System;
using System.Windows.Forms;
using Coderr.Client;
using Coderr.Client.ContextCollections;
using Coderr.Client.WinForms;

namespace DemoApp
{
    public partial class SampleErrorForm : Form
    {
        private readonly FormFactoryContext _context;

        public SampleErrorForm(FormFactoryContext context)
        {
            _context = context;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(tbDescription.Text) && string.IsNullOrWhiteSpace(tbEmail.Text))
                return;

            Err.LeaveFeedback(_context.Report.ReportId, new UserSuppliedInformation(tbDescription.Text, tbEmail.Text));
            Close();
        }
    }
}
