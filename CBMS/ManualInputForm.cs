// 位置：E:\cbms\CBMS\Forms\ManualInputForm.cs
using System;
using System.Windows.Forms;
using BookManagementSystem.Database;
using BookManagementSystem.Models;
using BookManagementSystem.Services;

namespace BookManagementSystem.Forms
{
    // 注意：去掉了 partial
    public class ManualInputForm : Form 
    {
        // === 手动声明控件 ===
        private TextBox txtISBNField;
        private TextBox txtTitle;
        private TextBox txtAuthor;
        private TextBox txtPublisher;
        private DateTimePicker dtpPublicationDate;
        private Button btnSave;
        private Button btnCancel;

        private DatabaseService _databaseService;

        public ManualInputForm()
        {
            InitializeComponent();
            _databaseService = new DatabaseService();
        }

        // === 手动创建界面 ===
        private void InitializeComponent()
        {
            this.Text = "手动录入图书";
            this.Size = new System.Drawing.Size(400, 400);

            // ISBN
            txtISBNField = new TextBox();
            txtISBNField.Location = new System.Drawing.Point(100, 20);
            this.Controls.Add(new Label() { Text = "ISBN:", Location = new System.Drawing.Point(20, 20) });
            this.Controls.Add(txtISBNField);

            // Title
            txtTitle = new TextBox();
            txtTitle.Location = new System.Drawing.Point(100, 60);
            this.Controls.Add(new Label() { Text = "书名:", Location = new System.Drawing.Point(20, 60) });
            this.Controls.Add(txtTitle);

            // Author
            txtAuthor = new TextBox();
            txtAuthor.Location = new System.Drawing.Point(100, 100);
            this.Controls.Add(new Label() { Text = "作者:", Location = new System.Drawing.Point(20, 100) });
            this.Controls.Add(txtAuthor);

            // Publisher
            txtPublisher = new TextBox();
            txtPublisher.Location = new System.Drawing.Point(100, 140);
            this.Controls.Add(new Label() { Text = "出版社:", Location = new System.Drawing.Point(20, 140) });
            this.Controls.Add(txtPublisher);

            // Date
            dtpPublicationDate = new DateTimePicker();
            dtpPublicationDate.Location = new System.Drawing.Point(100, 180);
            this.Controls.Add(new Label() { Text = "出版日期:", Location = new System.Drawing.Point(20, 180) });
            this.Controls.Add(dtpPublicationDate);

            // Save Button
            btnSave = new Button();
            btnSave.Text = "保存";
            btnSave.Location = new System.Drawing.Point(100, 230);
            btnSave.Click += btnSave_Click;
            this.Controls.Add(btnSave);

            // Cancel Button
            btnCancel = new Button();
            btnCancel.Text = "取消";
            btnCancel.Location = new System.Drawing.Point(200, 230);
            btnCancel.Click += btnCancel_Click;
            this.Controls.Add(btnCancel);
        }

        // === 业务逻辑 ===
        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtISBNField.Text) || !IsValidISBN(txtISBNField.Text))
            {
                MessageBox.Show("请输入有效的ISBN号码");
                return;
            }
            if (string.IsNullOrEmpty(txtTitle.Text))
            {
                MessageBox.Show("请输入书名");
                return;
            }

            var bookRecord = new BookRecord
            {
                ISBN = txtISBNField.Text,
                Title = txtTitle.Text,
                Author = txtAuthor.Text,
                Publisher = txtPublisher.Text,
                PublicationDate = dtpPublicationDate.Value
            };

            var verificationResult = new BookVerificationResult
            {
                IsVerified = true,
                VerificationDate = DateTime.Now,
                NationalLibraryRecord = bookRecord
            };

            if (_databaseService.UpdateBook(verificationResult))
            {
                MessageBox.Show("保存成功！");
                this.Close();
            }
            else
            {
                MessageBox.Show("保存失败");
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private bool IsValidISBN(string isbn)
        {
            if (string.IsNullOrWhiteSpace(isbn)) return false;
            string cleaned = isbn.Replace("-", "").Replace(" ", "");
            if (cleaned.Length != 10 && cleaned.Length != 13) return false;
            for (int i = 0; i < cleaned.Length; i++)
            {
                char c = cleaned[i];
                if (i == 9 && cleaned.Length == 10 && (c == 'X' || c == 'x')) continue;
                if (!char.IsDigit(c)) return false;
            }
            return true;
        }
    }
}