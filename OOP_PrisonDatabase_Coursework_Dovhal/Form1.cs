using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace OOP_PrisonDatabase_Coursework_Dovhal
{
    public partial class Form1 : Form
    {
        private BindingList<Prisoner> prisoners = new BindingList<Prisoner>();
        private string dataFile = "prisoners.xml";

        public Form1()
        {
            InitializeComponent();

            dataGridView1.DataSource = prisoners;

            btnLoad.Click += BtnLoad_Click;
            btnSave.Click += BtnSave_Click;
            btnAdd.Click += BtnAdd_Click;
            btnDelete.Click += BtnDelete_Click;
            btnEdit.Click += BtnEdit_Click;
            btnSearch.Click += BtnSearch_Click;

            // sample columns if none
            if (dataGridView1.Columns.Count == 0)
            {
                dataGridView1.AutoGenerateColumns = true;
            }

            // load existing data if present
            if (File.Exists(dataFile))
            {
                LoadData();
            }
            else
            {
                // add sample data
                prisoners.Add(new Prisoner { Id = 1, FullName = "Владислав Зелючко", Crime = "Корупцiя", Cell = "A1" });
                prisoners.Add(new Prisoner { Id = 2, FullName = "Микола Базаров", Crime = "Терроризм", Cell = "B2" });
            }
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            var q = txtSearch.Text.Trim().ToLower();
            if (string.IsNullOrEmpty(q))
            {
                dataGridView1.DataSource = prisoners;
                return;
            }

            var filtered = prisoners.Where(p => (p.FullName ?? "").ToLower().Contains(q)
                || (p.Crime ?? "").ToLower().Contains(q)
                || (p.Cell ?? "").ToLower().Contains(q)).ToList();

            dataGridView1.DataSource = new BindingList<Prisoner>(filtered);
        }

        private void BtnEdit_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;

            var p = dataGridView1.CurrentRow.DataBoundItem as Prisoner;
            if (p == null) return;

            using (var dlg = new PrisonerEditForm(p))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    // refresh grid
                    dataGridView1.Refresh();
                }
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null) return;
            var p = dataGridView1.CurrentRow.DataBoundItem as Prisoner;
            if (p == null) return;

            var res = MessageBox.Show("Видалити обраного ув'язненого?", "Підтвердити", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (res == DialogResult.Yes)
            {
                prisoners.Remove(p);
            }
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            var newP = new Prisoner { Id = prisoners.Any() ? prisoners.Max(x => x.Id) + 1 : 1 };
            using (var dlg = new PrisonerEditForm(newP))
            {
                if (dlg.ShowDialog() == DialogResult.OK)
                {
                    prisoners.Add(newP);
                }
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            SaveData();
            MessageBox.Show("Даннi збережено", "Збережено", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void SaveData()
        {
            try
            {
                var serializer = new XmlSerializer(typeof(List<Prisoner>));
                using (var fs = File.Create(dataFile))
                {
                    serializer.Serialize(fs, prisoners.ToList());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при збереженнi данних: " + ex.Message);
            }
        }

        private void LoadData()
        {
            try
            {
                var serializer = new XmlSerializer(typeof(List<Prisoner>));
                using (var fs = File.OpenRead(dataFile))
                {
                    var list = serializer.Deserialize(fs) as List<Prisoner> ?? new List<Prisoner>();
                    prisoners = new BindingList<Prisoner>(list);
                    dataGridView1.DataSource = prisoners;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при завантаженнi данних: " + ex.Message);
            }
        }
    }

    public class Prisoner
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Crime { get; set; }
        public string Cell { get; set; }
    }
}
