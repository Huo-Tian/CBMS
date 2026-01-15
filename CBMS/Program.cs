using System;
using System.Windows.Forms;
using BookManagementSystem.Forms;
using System.Drawing;

namespace BookManagementSystem
{
    
    internal static class Program
    {
        /// <summary>
        ///  应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
    {
        // --- 新增代码：开启高 DPI 支持 (解决模糊和压缩) ---
        static void Main()
{
    Application.SetHighDpiMode(HighDpiMode.PerMonitorV2); 
    Application.EnableVisualStyles();
    Application.SetCompatibleTextRenderingDefault(false);
    
    // --- 新增这一行：设置字体变大 ---
    Application.SetDefaultFont(new Font("Microsoft Sans Serif", 72f));

    Application.Run(new MainForm());
}
            // 设置应用程序上下文的高 DPI 行为
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            
            // 启用视觉样式（让界面看起来像 Windows 风格）
            Application.EnableVisualStyles();
            
            // 设置默认的文本渲染方式
            Application.SetCompatibleTextRenderingDefault(false);
            
            // 启动主窗体
            // 注意：这里的 "MainForm" 必须和你项目中主窗体类的名称完全一致
            Application.Run(new MainForm());
        }
    }
}