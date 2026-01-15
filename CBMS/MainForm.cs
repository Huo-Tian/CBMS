using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ClosedXML.Excel; // 引用 ClosedXML

namespace BookManagementSystem.Forms
{
    // 图书模型
    public class Book
    {
        public string ISBN { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public string PublicationDate { get; set; }
    }

    public class MainForm : Form
    {
        // === UI 控件 ===
        private DataGridView dataGridView;
        private TextBox txtScannerInput; // 隐藏的扫码输入框
        private Button btnImport, btnExport;

        // 图书数据列表
        private List<Book> bookList;

        public MainForm()
        {
            // 初始化数据
            bookList = new List<Book>();

            // 初始化界面
            InitializeComponent();

            // 窗体加载时，让扫码框获得焦点
            this.Load += (s, e) => txtScannerInput.Focus();
        }

        private void InitializeComponent()
        {
            // =============== 基础设置 ===============
            this.Text = "图书管理系统 (扫码版)";
            this.WindowState = FormWindowState.Maximized; // 默认最大化，看起来更现代
            this.AutoScaleMode = AutoScaleMode.Dpi;
            this.Font = new Font("Microsoft Sans Serif", 12F); // 统一字体

            // =============== 表格 (DataGridView) ===============
            dataGridView = new DataGridView();
            dataGridView.Dock = DockStyle.Fill; // 填满剩余空间
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView.Font = new Font("Microsoft Sans Serif", 11F);
            dataGridView.RowTemplate.Height = 35; // 行高，让表格不那么挤
            dataGridView.AllowUserToAddRows = false;
            this.Controls.Add(dataGridView);

            // =============== 底部工具栏 (Panel) ===============
            var bottomPanel = new Panel();
            bottomPanel.Height = 80;
            bottomPanel.Dock = DockStyle.Bottom;
            bottomPanel.BackColor = Color.FromArgb(240, 240, 240); // 浅灰色背景
            this.Controls.Add(bottomPanel);

            // --- 扫码提示 Label ---
            var lblScanner = new Label();
            lblScanner.Text = "扫码枪输入:";
            lblScanner.Location = new Point(20, 25);
            lblScanner.Size = new Size(120, 30);
            bottomPanel.Controls.Add(lblScanner);

            // --- 扫码输入框 (TextBox) ---
            txtScannerInput = new TextBox();
            txtScannerInput.Location = new Point(150, 25);
            txtScannerInput.Width = 300;
            txtScannerInput.Font = new Font("Consolas", 14F); // 等宽字体，方便看码
            // 关键事件：监听按键
            txtScannerInput.KeyDown += TxtScannerInput_KeyDown;
            bottomPanel.Controls.Add(txtScannerInput);

            // --- 导入按钮 ---
            btnImport = new Button();
            btnImport.Text = "导入 Excel";
            btnImport.Location = new Point(500, 20);
            btnImport.Size = new Size(120, 40);
            btnImport.Click += BtnImport_Click;
            bottomPanel.Controls.Add(btnImport);

            // --- 导出按钮 ---
            btnExport = new Button();
            btnExport.Text = "导出 Excel";
            btnExport.Location = new Point(650, 20);
            btnExport.Size = new Size(120, 40);
            btnExport.Click += BtnExport_Click;
            bottomPanel.Controls.Add(btnExport);

            // 初始化表格列
            SetupDataGridView();
        }

        // 初始化表格列
        private void SetupDataGridView()
        {
            dataGridView.Columns.Clear(); // 防止重复添加
            dataGridView.Columns.Add("ISBN", "ISBN");
            dataGridView.Columns.Add("Title", "书名");
            dataGridView.Columns.Add("Author", "作者");
            dataGridView.Columns.Add("Publisher", "出版社");
            dataGridView.Columns.Add("PublicationDate", "出版日期");
        }

        // =============== 核心逻辑 ===============

        // 监听扫码枪输入
        private void TxtScannerInput_KeyDown(object sender, KeyEventArgs e)
        {
            // 如果按下回车键，说明扫码结束
            if (e.KeyCode == Keys.Enter)
            {
                string rawInput = txtScannerInput.Text.Trim();
                txtScannerInput.Clear(); // 清空输入框，准备下一次扫描

                // 模拟搜索图书 (这里你可以替换为你原来的 NationalLibraryService)
                var book = SimulateSearchBook(rawInput);

                if (book != null)
                {
                    // 添加到列表
                    bookList.Add(book);
                    // 刷新表格
                    RefreshDataGridView();
                    MessageBox.Show($"成功添加: {book.Title}");
                }
                else
                {
                    MessageBox.Show("未找到该ISBN的图书信息");
                }
            }
        }

        // 模拟搜索图书 (替换为你原来的逻辑)
        private Book SimulateSearchBook(string isbn)
        {
            // 这里只是模拟。实际应调用你的 _libraryService.VerifyBookAsync
            // 为了演示，我们返回一个假数据
            return new Book
            {
                ISBN = isbn,
                Title = "C# 高级编程",
                Author = "张三",
                Publisher = "电子工业出版社",
                PublicationDate = "2023-01-01"
            };
        }

        // 刷新表格显示
        private void RefreshDataGridView()
        {
            dataGridView.Rows.Clear();
            foreach (var book in bookList)
            {
                dataGridView.Rows.Add(book.ISBN, book.Title, book.Author, book.Publisher, book.PublicationDate);
            }
        }

        // 导出到 Excel
        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                var workbook = new XLWorkbook();
                var worksheet = workbook.Worksheets.Add("图书列表");

                // 写入表头
                for (int i = 0; i < dataGridView.Columns.Count; i++)
                {
                    worksheet.Cell(1, i + 1).Value = dataGridView.Columns[i].HeaderText;
                }

                // 写入数据
                for (int i = 0; i < dataGridView.Rows.Count; i++)
                {
                    for (int j = 0; j < dataGridView.Columns.Count; j++)
                    {
                        worksheet.Cell(i + 2, j + 1).Value = dataGridView.Rows[i].Cells[j].Value?.ToString();
                    }
                }

                // 弹出保存对话框
                var sfd = new SaveFileDialog();
                sfd.Filter = "Excel 文件|*.xlsx";
                sfd.FileName = "图书数据导出.xlsx";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    workbook.SaveAs(sfd.FileName);
                    MessageBox.Show("导出成功！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("导出失败: " + ex.Message);
            }
        }

        // 从 Excel 导入
        private void BtnImport_Click(object sender, EventArgs e)
        {
            try
            {
                var ofd = new OpenFileDialog();
                ofd.Filter = "Excel 文件|*.xlsx";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    var workbook = new XLWorkbook(ofd.FileName);
                    var worksheet = workbook.Worksheet(1); // 第一个工作表

                    // 从第2行开始读（第1行是表头）
                    foreach (var row in worksheet.RowsUsed())
                    {
                        // 跳过表头
                        if (row.RowNumber() == 1) continue;

                        var book = new Book
                        {
                            ISBN = row.Cell(1).Value.ToString().Trim(),
                            Title = row.Cell(2).Value.ToString().Trim(),
                            Author = row.Cell(3).Value.ToString().Trim(),
                            Publisher = row.Cell(4).Value.ToString().Trim(),
                            PublicationDate = row.Cell(5).Value.ToString().Trim()
                        };
                        bookList.Add(book);
                    }

                    RefreshDataGridView();
                    MessageBox.Show("导入成功！");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("导入失败: " + ex.Message);
            }
        }
    }
}