using System;
using System.Windows.Forms;

namespace OOP_PrisonDatabase_Coursework_Dovhal
{
    public class PrisonerEditForm : Form
    {
        private TextBox txtName;
        private TextBox txtCrime;
        private TextBox txtCell;
        private Button btnOk;
        private Button btnCancel;

        private Prisoner prisoner;

        public PrisonerEditForm(Prisoner p)
        {
            prisoner = p;
            InitializeComponent();

            txtName.Text = prisoner.FullName;
            txtCrime.Text = prisoner.Crime;
            txtCell.Text = prisoner.Cell;
        }

        private void InitializeComponent()
        {
            this.txtName = new TextBox() { Left = 100, Top = 20, Width = 200 };
            this.txtCrime = new TextBox() { Left = 100, Top = 60, Width = 200 };
            this.txtCell = new TextBox() { Left = 100, Top = 100, Width = 200 };
            this.btnOk = new Button() { Text = "ОК", Left = 100, Width = 80, Top = 140, DialogResult = DialogResult.OK };
            this.btnCancel = new Button() { Text = "Скасувати", Left = 220, Width = 80, Top = 140, DialogResult = DialogResult.Cancel };

            this.btnOk.Click += BtnOk_Click;

            this.Controls.Add(new Label() { Text = "ФИО:", Left = 20, Top = 20, Width = 80 });
            this.Controls.Add(txtName);
            this.Controls.Add(new Label() { Text = "Преступление:", Left = 20, Top = 60, Width = 80 });
            this.Controls.Add(txtCrime);
            this.Controls.Add(new Label() { Text = "Камерa:", Left = 20, Top = 100, Width = 80 });
            this.Controls.Add(txtCell);
            this.Controls.Add(btnOk);
            this.Controls.Add(btnCancel);

            this.Text = "Редактор ув'язненого";
            this.AcceptButton = btnOk;
            this.CancelButton = btnCancel;
            this.ClientSize = new System.Drawing.Size(340, 190);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.StartPosition = FormStartPosition.CenterParent;
        }

        private void BtnOk_Click(object sender, EventArgs e)
        {
            prisoner.FullName = txtName.Text.Trim();
            prisoner.Crime = txtCrime.Text.Trim();
            prisoner.Cell = txtCell.Text.Trim();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
