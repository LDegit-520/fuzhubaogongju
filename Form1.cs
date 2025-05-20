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
using 辅助包工具.core;

namespace 辅助包工具
{
    public partial class Form1 : Form
    {
        Dictionary<string,RulesNode> RulesTest=new Dictionary<string,RulesNode>();
        public static RichTextBox Richtextbox;
        public Form1()
        {
            InitializeComponent();
            Richtextbox = RText;
            Rulesmod.TabPages.Clear();
            if (rulesmod.strings.Count!=0)//存在数据
            {
                RulesTest = rulesmod.DisRules();
                RText.Text = string.Join(Environment.NewLine, rulesmod.strings);
                foreach (var node in RulesTest)
                {
                    TabPage tabPage = new TabPage()
                    {
                        BackColor = Color.White,
                    };
                    tabPage.Text = node.Key;
                    tabPage.Controls.Add(RulesPanel.AllPanel(node.Value));
                    Rulesmod.TabPages.Add(tabPage);
                }
            }
            else
            {
                button5.Enabled = false;
                baocun.Enabled = false;
                fuzhubaobenti.Enabled = false;
            }

            richTextBox2.Text = "在修改完成后请必须点击保存否则不会产生作用\r\n\r\n" +
                "本程序运行时必须存在rulesmod.ini否则全部功能均会被禁用\r\n" +
                "只有存在本程序适配的rulesmod.ini时本程序才会起作用\r\n" +
                "程序运行中出现崩溃一般来说不会影响rulesmod.ini\r\n" +
                "如果产生影响请使用同目录下的gongju_rulesmod.ini替换rulesmod.ini";

            richTextBox1.Text = "本程序为起源辅助包的设置程序\r\n" +
                "本程序为开源项目<>\r\n" +
                "起源辅助包作者为\r\n" +
                "作者：Aaron_Kka(红警diy论坛昵称)\r\n不止黑色（QQ昵称）\r\n作者QQ：3177567813\r\n交流Q群:420102344\r\n" +
                "本项目作者为\r\n" +
                "作者QQ:2472934389";
        }


        private void button5_Click(object sender, EventArgs e)
        {
            rulesmod.Save();
            Richtextbox.Text= string.Join(Environment.NewLine, rulesmod.strings);
            MessageBox.Show("保存成功","提示");
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            File.WriteAllText(Path.Combine(Data.exepath,"rulesmod.ini"),Richtextbox.Text);
            splitContainer1.SuspendLayout();//禁止刷新
            splitContainer1.Enabled = false;//禁用操作
            rulesmod.strings = File.ReadAllLines(Path.Combine(Data.exepath, "rulesmod.ini")).ToList();
            RulesTest = rulesmod.DisRules();
            Rulesmod.TabPages.Clear();
            RText.Text = string.Join(Environment.NewLine, rulesmod.strings);
            foreach (var node in RulesTest)
            {
                TabPage tabPage = new TabPage()
                {
                    BackColor = Color.White,
                };
                tabPage.Text = node.Key;
                tabPage.Controls.Add(RulesPanel.AllPanel(node.Value));
                Rulesmod.TabPages.Add(tabPage);
            }
            splitContainer1.ResumeLayout();//启用刷新
            splitContainer1.Enabled = true;//启用操作
            splitContainer1.Refresh();//刷新
            MessageBox.Show("保存成功", "提示");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            rulesmo.JIN();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            rulesmo.Q_OriginAI();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            rulesmo.Q_PartOriginAI();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            rulesmod.xianzhiAll();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            rulesmod.JiechuAll();
        }
    }

    /// <summary>
    /// 根据数据生成panel
    /// </summary>
    class RulesPanel
    {
        public static Panel AllPanel(RulesNode rulesNode)
        {
            FlowLayoutPanel mainPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(10),
                Padding = new Padding(5),
                BorderStyle = BorderStyle.FixedSingle,
                FlowDirection = FlowDirection.LeftToRight,
                AutoScroll = true,
            };
            for (int i = 0; i < rulesNode.Values.Count; i++)
            {
                mainPanel.Controls.Add(GetPanel(rulesNode.Values[i]));
            }
            return mainPanel;
        }
        public static Panel GetPanel(RulesNode.KeyValue keyValue)
        {
            string test = $"{keyValue.Test}";
            string kv_test = $"键值对{keyValue.Key}  {keyValue.Value}";
            string Raw_test = $"原始行:    {keyValue.Raw_int}    {keyValue.Raw_string}";
            Color R1 = Color.FromArgb(255, 200, 200);
            Color G1 = Color.FromArgb(200, 255, 200);
            Color B1= Color.FromArgb(200, 200, 255);
            Color color = B1;

            if (keyValue.Type == RulesNode.AN || keyValue.Type == RulesNode.AN1)
            {
                if (!keyValue.Exist)
                {
                    color = R1;
                }
                else
                {
                    color = G1;
                }
            }
            if (keyValue.Type == RulesNode.ONE||keyValue.Type==RulesNode.TF||keyValue.Type==RulesNode.YN)
            {
                color = R1;
                if (keyValue.Value.IndexOf("yes")!=-1||keyValue.Value.IndexOf("true")!=-1||keyValue.Value.IndexOf("1")!=-1)
                {
                    color=G1;
                }
            }
            // 创建主面板
            Panel mainPanel = new Panel
            {
                Width = 350,
                Height =90,
                Margin = new Padding(10),
                Padding = new Padding(0),
                BorderStyle = BorderStyle.FixedSingle
            };

            // 顶部标题区域
            Panel topPanel = CreateSectionPanel(BackgroundColor: Color.Transparent);
            Label titleLabel = new Label
            {
                Text = test,
                Font = new Font("微软雅黑", 10, FontStyle.Bold),
                ForeColor = Color.DarkBlue,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            topPanel.Controls.Add(titleLabel);

            // 中间内容区域
            Panel middlePanel = CreateSectionPanel(BackgroundColor: color);
            middlePanel.Name = "middlePanel";
            // 文本
            Label contentLabel = new Label
            {
                Name= "contentLabel",
                Text = kv_test,
                Dock = DockStyle.Fill,
                Font = new Font("微软雅黑", 10),
                TextAlign = ContentAlignment.MiddleCenter,
            };

            // 将控件添加到中间面板
            middlePanel.Controls.Add(contentLabel);
            

            // 底部信息区域
            Panel bottomPanel = CreateSectionPanel(BackgroundColor: Color.Transparent);
            bottomPanel.Name = "bottomPanel";
            Label infoLabel = new Label
            {
                Name="infoLabel",
                Text = Raw_test,
                Font = new Font("微软雅黑", 8, FontStyle.Italic),
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
            };
            bottomPanel.Controls.Add(infoLabel);


            // 组装主面板

            mainPanel.Controls.Add(bottomPanel);
            mainPanel.Controls.Add(middlePanel);
            mainPanel.Controls.Add(topPanel);
            
            if(keyValue.Type==RulesNode.AN)
            {
                contentLabel.Click += (s,e) =>
                {
                    keyValue.Exist=!keyValue.Exist;//取反
                    //改值
                    if (keyValue.Value.StartsWith(";"))
                    {
                        keyValue.Value = keyValue.Value.Substring(1);
                        keyValue.Raw_string=keyValue.Raw_string.Substring(1);
                        rulesmod.SetRules(keyValue.Raw_int,keyValue.Raw_string);
                    }
                    else
                    {
                        keyValue.Value = $";{keyValue.Value}";
                        keyValue.Raw_string = $";{keyValue.Raw_string}";
                        rulesmod.SetRules(keyValue.Raw_int, keyValue.Raw_string);
                    }
                    //更新界面
                    if (middlePanel.BackColor == R1)//切换颜色
                    {
                        middlePanel.BackColor = G1;
                    }
                    else
                    {
                        middlePanel.BackColor = R1;
                    }
                    contentLabel.Text = $"键值对{keyValue.Key}  {keyValue.Value}";
                    infoLabel.Text= $"原始行:    {keyValue.Raw_int}    {keyValue.Raw_string}";

                };
            }
            if(keyValue.Type==RulesNode.AN1)
            {
                keyValue.pair_object = mainPanel;
                contentLabel.Click += (s, e) =>
                {
                    keyValue.Exist = !keyValue.Exist;//取反
                    keyValue.pair.Exist =!keyValue.pair.Exist;
                    //改值
                    if (keyValue.Value.StartsWith(";"))
                    {
                        keyValue.Value = keyValue.Value.Substring(1);
                        keyValue.Raw_string = keyValue.Raw_string.Substring(1);
                        rulesmod.SetRules(keyValue.Raw_int, keyValue.Raw_string);

                        keyValue.pair.Value = keyValue.pair.Value.Substring(1);
                        keyValue.pair.Raw_string = keyValue.pair.Raw_string.Substring(1);
                        rulesmod.SetRules(keyValue.pair.Raw_int, keyValue.pair.Raw_string);
                    }
                    else
                    {
                        keyValue.Value = $";{keyValue.Value}";
                        keyValue.Raw_string = $";{keyValue.Raw_string}";
                        rulesmod.SetRules(keyValue.Raw_int, keyValue.Raw_string);

                        keyValue.pair.Value = $";{keyValue.pair.Value}";
                        keyValue.pair.Raw_string = $";{keyValue.pair.Raw_string}";
                        rulesmod.SetRules(keyValue.pair.Raw_int, keyValue.pair.Raw_string);
                    }
                    //更新界面
                    if (middlePanel.BackColor == R1)//切换颜色
                    {
                        middlePanel.BackColor = G1;
                    }
                    else
                    {
                        middlePanel.BackColor = R1;
                    }
                    contentLabel.Text = $"键值对{keyValue.Key}  {keyValue.Value}";
                    infoLabel.Text = $"原始行:    {keyValue.Raw_int}    {keyValue.Raw_string}";
                    var pairpanel = ((Panel)keyValue.pair.pair_object);
                    if (pairpanel.Controls["middlePanel"].BackColor == R1)
                    {
                        pairpanel.Controls["middlePanel"].BackColor = G1;
                    }
                    else
                    {
                        pairpanel.Controls["middlePanel"].BackColor = R1;
                    }
                    pairpanel.Controls["middlePanel"].Controls["contentLabel"].Text = $"键值对{keyValue.pair.Key}  {keyValue.pair.Value}";
                    pairpanel.Controls["bottomPanel"].Controls["infoLabel"].Text = $"原始行:    {keyValue.pair.Raw_int}    {keyValue.pair.Raw_string}";
                };
            }
            if(keyValue.Type==RulesNode.ONE)
            {
                contentLabel.Click += (sender, e) =>
                {
                    //改值
                    if (keyValue.Value.IndexOf("1")!=-1)
                    {
                        keyValue.Value = keyValue.Value.Replace("1","0");
                        //定位
                        int dengyu =keyValue.Raw_string.IndexOf("=");//找到值的开始
                        int fenge=keyValue.Raw_string.IndexOf(";");//找到标记的开始
                        string ss1=keyValue.Raw_string.Substring(0,dengyu);
                        string ss2=keyValue.Raw_string.Substring(dengyu,fenge-dengyu).Replace("1","0");
                        string ss3 = keyValue.Raw_string.Substring(fenge);
                        keyValue.Raw_string=ss1+ ss2+ss3;
                        //
                        rulesmod.SetRules(keyValue.Raw_int, keyValue.Raw_string);
                    }
                    else
                    {
                        keyValue.Value = keyValue.Value.Replace("0","1");
                        //定位
                        int dengyu = keyValue.Raw_string.IndexOf("=");//找到值的开始
                        int fenge = keyValue.Raw_string.IndexOf(";");//找到标记的开始
                        string ss1 = keyValue.Raw_string.Substring(0, dengyu);
                        string ss2 = keyValue.Raw_string.Substring(dengyu, fenge - dengyu).Replace("0", "1");
                        string ss3 = keyValue.Raw_string.Substring(fenge);
                        keyValue.Raw_string = ss1 + ss2 + ss3;
                        //
                        rulesmod.SetRules(keyValue.Raw_int, keyValue.Raw_string);
                    }
                    //更新界面
                    if (middlePanel.BackColor == R1)//切换颜色
                    {
                        middlePanel.BackColor = G1;
                    }
                    else
                    {
                        middlePanel.BackColor = R1;
                    }
                    contentLabel.Text = $"键值对{keyValue.Key}  {keyValue.Value}";
                    infoLabel.Text = $"原始行:    {keyValue.Raw_int}    {keyValue.Raw_string}";
                };
            }
            if (keyValue.Type == RulesNode.YN)
            {
                contentLabel.Click += (sender, e) =>
                {
                    //改值
                    if (keyValue.Value.IndexOf("yes") != -1)
                    {
                        keyValue.Value = keyValue.Value.Replace("yes", "no");
                        //定位
                        int dengyu = keyValue.Raw_string.IndexOf("=");//找到值的开始
                        int fenge = keyValue.Raw_string.IndexOf(";");//找到标记的开始
                        string ss1 = keyValue.Raw_string.Substring(0, dengyu);
                        string ss2 = keyValue.Raw_string.Substring(dengyu, fenge - dengyu).Replace("yes", "no");
                        string ss3 = keyValue.Raw_string.Substring(fenge);
                        keyValue.Raw_string = ss1 + ss2 + ss3;
                        //
                        rulesmod.SetRules(keyValue.Raw_int, keyValue.Raw_string);
                    }
                    else
                    {
                        keyValue.Value = keyValue.Value.Replace("no", "yes");
                        //定位
                        int dengyu = keyValue.Raw_string.IndexOf("=");//找到值的开始
                        int fenge = keyValue.Raw_string.IndexOf(";");//找到标记的开始
                        string ss1 = keyValue.Raw_string.Substring(0, dengyu);
                        string ss2 = keyValue.Raw_string.Substring(dengyu, fenge - dengyu).Replace("no", "yes");
                        string ss3 = keyValue.Raw_string.Substring(fenge);
                        keyValue.Raw_string = ss1 + ss2 + ss3;
                        //
                        rulesmod.SetRules(keyValue.Raw_int, keyValue.Raw_string);
                    }
                    //更新界面
                    if (middlePanel.BackColor == R1)//切换颜色
                    {
                        middlePanel.BackColor = G1;
                    }
                    else
                    {
                        middlePanel.BackColor = R1;
                    }
                    contentLabel.Text = $"键值对{keyValue.Key}  {keyValue.Value}";
                    infoLabel.Text = $"原始行:    {keyValue.Raw_int}    {keyValue.Raw_string}";
                };
            }
            if (keyValue.Type == RulesNode.TF)
            {
                contentLabel.Click += (sender, e) =>
                {
                    //改值
                    if (keyValue.Value.IndexOf("true") != -1)
                    {
                        keyValue.Value = keyValue.Value.Replace("true", "false");
                        //定位
                        int dengyu = keyValue.Raw_string.IndexOf("=");//找到值的开始
                        int fenge = keyValue.Raw_string.IndexOf(";");//找到标记的开始
                        string ss1 = keyValue.Raw_string.Substring(0, dengyu);
                        string ss2 = keyValue.Raw_string.Substring(dengyu, fenge - dengyu).Replace("true", "false");
                        string ss3 = keyValue.Raw_string.Substring(fenge);
                        keyValue.Raw_string = ss1 + ss2 + ss3;
                        //
                        rulesmod.SetRules(keyValue.Raw_int, keyValue.Raw_string);
                    }
                    else
                    {
                        keyValue.Value = keyValue.Value.Replace("false", "true");
                        //定位
                        int dengyu = keyValue.Raw_string.IndexOf("=");//找到值的开始
                        int fenge = keyValue.Raw_string.IndexOf(";");//找到标记的开始
                        string ss1 = keyValue.Raw_string.Substring(0, dengyu);
                        string ss2 = keyValue.Raw_string.Substring(dengyu, fenge - dengyu).Replace("false", "true");
                        string ss3 = keyValue.Raw_string.Substring(fenge);
                        keyValue.Raw_string = ss1 + ss2 + ss3;
                        //
                        rulesmod.SetRules(keyValue.Raw_int, keyValue.Raw_string);
                    }
                    //更新界面
                    if (middlePanel.BackColor == R1)//切换颜色
                    {
                        middlePanel.BackColor = G1;
                    }
                    else
                    {
                        middlePanel.BackColor = R1;
                    }
                    contentLabel.Text = $"键值对{keyValue.Key}  {keyValue.Value}";
                    infoLabel.Text = $"原始行:    {keyValue.Raw_int}    {keyValue.Raw_string}";
                };
            }
            if(keyValue.Type==RulesNode.INTER||keyValue.Type==RulesNode.BANFEN)
            {
                contentLabel.Click += (s,e) => 
                {
                    using (InputForm inputForm = new InputForm("请输入值", "输入框", keyValue.Value))
                    {
                        if (inputForm.ShowDialog() == DialogResult.OK)
                        {
                            string userInput = inputForm.UserInput;
                            int Ival;
                            if (int.TryParse(userInput, out Ival))
                            {
                                //定位
                                int dengyu = keyValue.Raw_string.IndexOf("=");//找到值的开始
                                int fenge = keyValue.Raw_string.IndexOf(";");//找到标记的开始
                                string ss1 = keyValue.Raw_string.Substring(0, dengyu);
                                string ss2 = keyValue.Raw_string.Substring(dengyu, fenge - dengyu).Replace(keyValue.Value, userInput);
                                string ss3 = keyValue.Raw_string.Substring(fenge);
                                keyValue.Raw_string = ss1 + ss2 + ss3;
                                //
                                keyValue.Value = userInput;
                                rulesmod.SetRules(keyValue.Raw_int, keyValue.Raw_string);
                                contentLabel.Text = $"键值对{keyValue.Key}  {keyValue.Value}";
                                infoLabel.Text = $"原始行:    {keyValue.Raw_int}    {keyValue.Raw_string}";
                            }
                            else
                            {
                                MessageBox.Show("警告：输入的值不是数字","警告");
                            }
                        }
                    }
                };
            }
            infoLabel.Click += (s,e) =>
            {
                ScrollToLine(Form1.Richtextbox, keyValue.Raw_int);
            };
            return mainPanel;
        }

        private static Panel CreateSectionPanel(Color BackgroundColor)
        {
            return new Panel
            {
                Dock = DockStyle.Top,
                Height = 30,
                Margin = new Padding(5),
                Padding = new Padding(0),
                BackColor = BackgroundColor,
                BorderStyle = BorderStyle.None
            };
        }
        private static void ScrollToLine(RichTextBox richTextBox, int lineNumber)
        {
            // 检查行号是否超出范围
            if (lineNumber < 0 || lineNumber >= richTextBox.Lines.Length)
            {
                MessageBox.Show("行号超出范围！");
                return;
            }

            // 获取目标行的起始字符索引
            int firstCharIndex = richTextBox.GetFirstCharIndexFromLine(lineNumber);

            // 设置光标位置到该行的起始位置
            richTextBox.SelectionStart = firstCharIndex;
            richTextBox.SelectionLength = 0; // 清除选中区域

            // 滚动到光标位置
            richTextBox.ScrollToCaret();
        }
    }

    public partial class InputForm : Form
    {
        public string UserInput { get; private set; }

        public InputForm(string prompt, string title, string defaultText = "")
        {

            // 设置窗体标题
            this.Text = title;

            // 创建标签
            Label labelPrompt = new Label
            {
                Text = prompt,
                Left = 20,
                Top = 15,
                Width = 260
            };

            // 创建输入框
            TextBox textBoxInput = new TextBox
            {
                Left = 20,
                Top = 40,
                Width = 260,
                Text = defaultText
            };

            // 创建“确定”按钮
            Button buttonOK = new Button
            {
                Text = "确定",
                Left = 20,
                Top = 75,
                Width = 80
            };

            // 创建“取消”按钮
            Button buttonCancel = new Button
            {
                Text = "取消",
                Left = 130,
                Top = 75,
                Width = 80
            };

            // 按钮点击事件
            buttonOK.Click += (s, e) =>
            {
                UserInput = textBoxInput.Text;
                DialogResult = DialogResult.OK;
                Close();
            };

            buttonCancel.Click += (s, e) =>
            {
                DialogResult = DialogResult.Cancel;
                Close();
            };

            // 添加控件到窗体
            Controls.Add(labelPrompt);
            Controls.Add(textBoxInput);
            Controls.Add(buttonOK);
            Controls.Add(buttonCancel);

            // 设置窗体大小
            this.Width = 300;
            this.Height = 150;
        }
    }
}
