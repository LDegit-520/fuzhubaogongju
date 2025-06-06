using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 辅助包工具.core
{
    /// <summary>
    /// 全局静态数据类
    /// </summary>
    internal class Data
    {
        /// <summary>
        /// exe所在目录
        /// </summary>
        public static string exepath { get; set; } = Application.StartupPath;
    }
}
