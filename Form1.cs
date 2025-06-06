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
using static 辅助包工具.core.RulesNode;

namespace 辅助包工具
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// 分类完成的键值对区
        /// </summary>
        Dictionary<string,RulesNode> RulesTest=new Dictionary<string,RulesNode>();
        /// <summary>
        /// 文件文件显示区也就是下面那一大块文本的控件在这里写成公开方便其他调用 （这种写法不是规范写法不可取）（注：这种写法在大项目不可取，虽然方便，但是不安全，但这个项目无所谓安全，毕竟纯单机软件）
        /// </summary>
        public static RichTextBox Richtextbox;
        public Form1()
        {
            InitializeComponent();//程序默认构造器函数
            Richtextbox = RText;//将文本显示区对应出去
            Rulesmod.TabPages.Clear();//清空选项卡集合
            if (rulesmo.strings == null)//如果rulesmo文件不存在，则禁用掉对应的按钮，防止点击报错
            {
                button1.Enabled = false;//禁用按钮 （这里按钮名称很抽象是因为我是之间托放的控件，控件名称均是自然生成的）
                button2.Enabled = false;
                button6.Enabled = false;
            }
            if (rulesmod.strings.Count != 0)//存在数据
            {
                RulesTest = rulesmod.DisRules();//处理字典并存贮
                RText.Text = string.Join(Environment.NewLine, rulesmod.strings);//把文件显示出来
                foreach (var node in RulesTest)//遍历字典生成选项卡
                {
                    TabPage tabPage = new TabPage()//定义一个新的选项卡
                    {
                        BackColor = Color.White,//背景色设为白色
                    };
                    tabPage.Text = node.Key;//选项卡标题=re=的分区名
                    tabPage.Controls.Add(RulesPanel.AllPanel(node.Value));//添加所以键值对
                    Rulesmod.TabPages.Add(tabPage);//添加进选项卡集合控件
                }
            }
            else//不存在数据，禁用按钮
            {
                button5.Enabled = false;
                baocun.Enabled = false;
                fuzhubaobenti.Enabled = false;
            }
            //文本显示区显示的文本
            richTextBox2.Text =
                "本程序为起源辅助包的设置程序\r\n" +
                "本程序为开源项目[GitHub地址](https://github.com/LDegit-520/fuzhubaogongju)\r\n" +
                "详细介绍请前往github产看\r\n" +
                "起源辅助包作者为\r\n" +
                "作者：Aaron_Kka(红警diy论坛昵称)\r\n不止黑色（QQ昵称）\r\n作者QQ：3177567813\r\n交流Q群:420102344\r\n" +
                "本项目作者为\r\n" +
                "作者名称：惊鸿\r\n" +
                "作者QQ:2472934389\r\n\r\n"
                +"在修改完成后请必须点击保存否则不会产生作用\r\n\r\n" +
                "本程序运行时必须存在rulesmod.ini否则全部功能均会被禁用\r\n" +
                "只有存在本程序适配的rulesmod.ini时本程序才会起作用\r\n" +
                "只有存在rulesmo.ini时禁用和启用起源才可使用\r\n" +
                "程序运行中出现崩溃一般来说不会影响rulesmod.ini\r\n" +
                "如果产生影响请使用同目录下的gongju_rulesmod.ini替换rulesmod.ini\r\n" +
                "备份为每启动一次本程序备份一次";
        }

        /// <summary>
        /// 保存修改按钮的对应事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click(object sender, EventArgs e)
        {
            rulesmod.Save();//保存rulesmod文件
            rulesmo.Save();//保存rulemo文件
            Richtextbox.Text= string.Join(Environment.NewLine, rulesmod.strings);//更新文本区的文本
            MessageBox.Show("保存成功","提示");//弹窗提示
        }
        /// <summary>
        /// 文本区的保存按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button5_Click_1(object sender, EventArgs e)
        {
            File.WriteAllText(Path.Combine(Data.exepath,"rulesmod.ini"),Richtextbox.Text);//将修改写入rulesmod
            splitContainer1.SuspendLayout();//禁止刷新
            splitContainer1.Enabled = false;//禁用操作
            rulesmod.strings = File.ReadAllLines(Path.Combine(Data.exepath, "rulesmod.ini")).ToList();//从新读取
            RulesTest = rulesmod.DisRules();//从新处理
            Rulesmod.TabPages.Clear();//清空选项卡
            RText.Text = string.Join(Environment.NewLine, rulesmod.strings);//从新写入文本
            foreach (var node in RulesTest)//同上面
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
            MessageBox.Show("保存成功", "提示");//弹窗提示
        }
        /// <summary>
        /// 禁用全部起源按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            rulesmo.JIN();
        }
        /// <summary>
        /// 启用起源按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            rulesmo.Q_OriginAI();
        }
        /// <summary>
        /// 启用部分起源按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button6_Click(object sender, EventArgs e)
        {
            rulesmo.Q_PartOriginAI();
        }
        /// <summary>
        /// 解除所有辅助包限制按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button4_Click(object sender, EventArgs e)
        {
            rulesmod.JiechuAll();
        }
        /// <summary>
        /// 限制所有复杂包限制按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button3_Click(object sender, EventArgs e)
        {
            rulesmod.xianzhiAll();
        }
        /// <summary>
        /// 禁用管理局升星
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button8_Click(object sender, EventArgs e)
        {
            rulesmod.jin_shengxing();
        }
        /// <summary>
        /// 启用管理局升星
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button7_Click(object sender, EventArgs e)
        {
            rulesmod.jie_shengxing();
        }
    }

    /// <summary>
    /// 根据数据生成panel（也就是选项卡能看到的那些）
    /// </summary>
    class RulesPanel
    {
        static Color R1 = Color.FromArgb(255, 200, 200);//定义颜色 红
        static Color G1 = Color.FromArgb(200, 255, 200);//定义颜色 绿
        static Color B1 = Color.FromArgb(200, 200, 255);//定义颜色 蓝
        /// <summary>
        /// 生成整个区域方法
        /// </summary>
        /// <param name="rulesNode"></param>
        /// <returns></returns>
        public static Panel AllPanel(RulesNode rulesNode)
        {
            FlowLayoutPanel mainPanel = new FlowLayoutPanel //定义面板
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(10),
                Padding = new Padding(5),
                BorderStyle = BorderStyle.FixedSingle,
                FlowDirection = FlowDirection.LeftToRight,
                AutoScroll = true,
            };//面板属性
            for (int i = 0; i < rulesNode.Values.Count; i++)//遍历添加所有控件
            {
                mainPanel.Controls.Add(GetPanel(rulesNode.Values[i]));
            }
            return mainPanel;
        }
        /// <summary>
        /// 添加键值对对应控件
        /// </summary>
        /// <param name="keyValue"></param>
        /// <returns></returns>
        public static Panel GetPanel(RulesNode.KeyValue keyValue)
        {
            string test = $"{keyValue.Test}";//简要介绍区
            string kv_test = $"键值对{keyValue.Key}  {keyValue.Value}";//键值对区
            string Raw_test = $"原始行:    {keyValue.Raw_int}    {keyValue.Raw_string}";//原始数据区
            Color color = B1;//颜色
            if (keyValue.Type == RulesNode.AN || keyValue.Type == RulesNode.AN1)//根据是否注释来决定显示的颜色
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
            if (keyValue.Type == RulesNode.ONE||keyValue.Type==RulesNode.TF||keyValue.Type==RulesNode.YN)//这个也是显示颜色
            {
                color = R1;
                if (keyValue.Value.IndexOf("yes")!=-1||keyValue.Value.IndexOf("true")!=-1||keyValue.Value.IndexOf("1")!=-1)
                {
                    color=G1;
                }
            }
            #region 页面创建  这里开始都是
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
            #endregion
            //给不同的键值对卡添加功能
            if (keyValue.Type==RulesNode.AN)
            {
                contentLabel.Click += (s,e) =>
                {
                    keyValue.Exist=!keyValue.Exist;//取反
                    //改值
                    if (keyValue.Value.StartsWith(";"))
                    {
                        keyValue.Value = keyValue.Value.Substring(1);//删除;
                        keyValue.Raw_string=keyValue.Raw_string.Substring(1);//删除;
                        rulesmod.SetRules(keyValue.Raw_int,keyValue.Raw_string);//更新值
                    }
                    else
                    {
                        keyValue.Value = $";{keyValue.Value}";
                        keyValue.Raw_string = $";{keyValue.Raw_string}";
                        rulesmod.SetRules(keyValue.Raw_int, keyValue.Raw_string);
                    }
                    jiemiangengxin(keyValue,middlePanel,contentLabel,infoLabel);//更新界面

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
                        //下面是关联值的修改
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
                    jiemiangengxin(keyValue, middlePanel, contentLabel, infoLabel);
                    var pairpanel = ((Panel)keyValue.pair.pair_object);
                    jiemiangengxin(keyValue.pair, (Panel)pairpanel.Controls["middlePanel"], //关联值界面更新
                        (Label)pairpanel.Controls["middlePanel"].Controls["contentLabel"],
                        (Label)pairpanel.Controls["bottomPanel"].Controls["infoLabel"]);
                };
            }
            if(keyValue.Type==RulesNode.ONE)//01的处理
            {
                contentLabel.Click += (sender, e) =>
                {
                    //改值
                    if (keyValue.Value.IndexOf("1")!=-1)
                    {
                        keyValue.Value = keyValue.Value.Replace("1","0");//替换值
                        keyValue.Raw_string = Fuzhu_dingwei(keyValue,"1","0");//修改原始字符串
                        rulesmod.SetRules(keyValue.Raw_int, keyValue.Raw_string);//更新值
                    }
                    else
                    {
                        keyValue.Value = keyValue.Value.Replace("0","1");
                        keyValue.Raw_string = Fuzhu_dingwei(keyValue,"0","1");
                        rulesmod.SetRules(keyValue.Raw_int, keyValue.Raw_string);
                    }
                    jiemiangengxin(keyValue, middlePanel, contentLabel, infoLabel);
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
                        keyValue.Raw_string = Fuzhu_dingwei(keyValue,"yes","no");
                        rulesmod.SetRules(keyValue.Raw_int, keyValue.Raw_string);
                    }
                    else
                    {
                        keyValue.Value = keyValue.Value.Replace("no", "yes");
                        keyValue.Raw_string = Fuzhu_dingwei(keyValue,"no","yes");
                        rulesmod.SetRules(keyValue.Raw_int, keyValue.Raw_string);
                    }
                    jiemiangengxin(keyValue, middlePanel, contentLabel, infoLabel);
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
                        //int dengyu = keyValue.Raw_string.IndexOf("=");//找到值的开始
                        //int fenge = keyValue.Raw_string.IndexOf(";");//找到标记的开始
                        //string ss1 = keyValue.Raw_string.Substring(0, dengyu);
                        //string ss2 = keyValue.Raw_string.Substring(dengyu, fenge - dengyu).Replace("true", "false");
                        //string ss3 = keyValue.Raw_string.Substring(fenge);
                        keyValue.Raw_string = Fuzhu_dingwei(keyValue,"true","false");
                        //
                        rulesmod.SetRules(keyValue.Raw_int, keyValue.Raw_string);
                    }
                    else
                    {
                        keyValue.Value = keyValue.Value.Replace("false", "true");
                        //定位
                        //int dengyu = keyValue.Raw_string.IndexOf("=");//找到值的开始
                        //int fenge = keyValue.Raw_string.IndexOf(";");//找到标记的开始
                        //string ss1 = keyValue.Raw_string.Substring(0, dengyu);
                        //string ss2 = keyValue.Raw_string.Substring(dengyu, fenge - dengyu).Replace("false", "true");
                        //string ss3 = keyValue.Raw_string.Substring(fenge);
                        keyValue.Raw_string = Fuzhu_dingwei(keyValue,"false","true");
                        //
                        rulesmod.SetRules(keyValue.Raw_int, keyValue.Raw_string);
                    }
                    ////更新界面
                    //if (middlePanel.BackColor == R1)//切换颜色
                    //{
                    //    middlePanel.BackColor = G1;
                    //}
                    //else
                    //{
                    //    middlePanel.BackColor = R1;
                    //}
                    //contentLabel.Text = $"键值对{keyValue.Key}  {keyValue.Value}";
                    //infoLabel.Text = $"原始行:    {keyValue.Raw_int}    {keyValue.Raw_string}";
                    jiemiangengxin(keyValue, middlePanel, contentLabel, infoLabel);
                };
            }
            if(keyValue.Type==RulesNode.INTER||keyValue.Type==RulesNode.BANFEN)//数字和百分值修改
            {
                contentLabel.Click += (s,e) => 
                {
                    using (InputForm inputForm = new InputForm("请输入值", "输入框", keyValue.Value))//弹窗
                    {
                        inputForm.StartPosition = FormStartPosition.CenterParent;//弹窗定位
                        if (inputForm.ShowDialog() == DialogResult.OK)//弹窗确认
                        {
                            string userInput = inputForm.UserInput;//获取值
                            int Ival;
                            if (int.TryParse(userInput, out Ival))//转化值为数字，成功进入下面
                            {
                                keyValue.Raw_string = Fuzhu_dingwei(keyValue,keyValue.Value,userInput);//修改原始字符串
                                keyValue.Value = userInput;//修改值
                                rulesmod.SetRules(keyValue.Raw_int, keyValue.Raw_string);//修改文件字符串列表
                                contentLabel.Text = $"键值对{keyValue.Key}  {keyValue.Value}";
                                infoLabel.Text = $"原始行:    {keyValue.Raw_int}    {keyValue.Raw_string}";
                            }
                            else//输入不是数字
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
        /// <summary>
        /// 修改值
        /// </summary>
        /// <param name="keyValue">键值对</param>
        /// <param name="str1">修改前值</param>
        /// <param name="str2">修改后值</param>
        /// <returns></returns>
        private static string Fuzhu_dingwei(RulesNode.KeyValue keyValue,string str1,string str2)
        {
            int dengyu = keyValue.Raw_string.IndexOf("=");//找到值的开始
            int fenge = keyValue.Raw_string.IndexOf(";");//找到标记的开始
            string ss1 = keyValue.Raw_string.Substring(0, dengyu);
            string ss2 = keyValue.Raw_string.Substring(dengyu, fenge - dengyu).Replace(str1, str2);
            string ss3 = keyValue.Raw_string.Substring(fenge);
            return  ss1 + ss2 + ss3;
        }
        /// <summary>
        /// 更新界面，也就是切换颜色和显示文本
        /// </summary>
        /// <param name="keyValue">键值对</param>
        /// <param name="middlePanel">主面板</param>
        /// <param name="contentLabel">中间部分</param>
        /// <param name="infoLabel">下面部分</param>
        private static void jiemiangengxin(RulesNode.KeyValue keyValue, Panel middlePanel,Label contentLabel,Label infoLabel)
        {
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
        }
        /// <summary>
        /// 键值对卡的上中下部分的集中创建
        /// </summary>
        /// <param name="BackgroundColor"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 在文本显示区进行跳转指定行
        /// </summary>
        /// <param name="richTextBox"></param>
        /// <param name="lineNumber"></param>
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
    /// <summary>
    /// 自定义弹窗，给可以修改数值的那些使用
    /// </summary>
    public partial class InputForm : Form
    {
        public string UserInput { get; private set; }

        public InputForm(string prompt, string title, string defaultText = "")
        {

            // 设置窗体标题
            this.Text = title;
            this.ShowIcon = false;
            this.MaximizeBox = false;
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
                Width = 80,
                Height = 30
            };

            // 创建“取消”按钮
            Button buttonCancel = new Button
            {
                Text = "取消",
                Left = 200,
                Top = 75,
                Width = 80,
                Height= 30
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
            this.Width = 320;
            this.Height = 170;
        }
    }
}
