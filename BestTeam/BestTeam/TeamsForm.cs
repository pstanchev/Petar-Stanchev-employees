using BestTeam.BusinessLogic;
using BestTeam.BusinessObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BestTeam
{
    public partial class TeamsForm : Form
    {
        public TeamsForm()
        {
            InitializeComponent();
        }

        private string DisplayFileDialog()
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "CSV files | *.csv";
            dialog.Multiselect = false;
            dialog.Title = "Select teams data CSV file in order to find the best team:";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = dialog.FileName;
                return filePath;
            }

            return string.Empty;
        }

        private void buttonSearch_Click(object sender, EventArgs e)
        {
            string csvDataFilePath = DisplayFileDialog();
            if (!string.IsNullOrEmpty(csvDataFilePath))
            {
                BestTeamSearcher searcher = new BestTeamSearcher();
                Team bestTeam = searcher.Search(csvDataFilePath);

                dataGridView1.DataSource = (new List<Team>() { bestTeam });
            }
        }

        private void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var dataGridView1 = (DataGridView)sender;
            if (e.RowIndex < 0 || e.RowIndex == dataGridView1.NewRowIndex)
            {
                return;
            }

            if (e.ColumnIndex == 2)
            {
                var bestTeam = dataGridView1.Rows[e.RowIndex].DataBoundItem as Team;
                if (bestTeam != null)
                {
                    e.Value = string.Format("{0}", string.Join(",", bestTeam.Projects.Keys));
                }
            }
        }
    }
}
