using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static 辅助包工具.core.RulesNode;

namespace 辅助包工具.core
{
    /// <summary>
    /// 节点类（也就是按照;=re=;进行分割的键值对集合）
    /// </summary>
    class RulesNode
    {
        /// <summary>
        /// 标识符END
        /// </summary>
        public static readonly byte END = 0x00;
        /// <summary>
        /// 标识符RE
        /// </summary>
        public static readonly byte RE = 0x01;
        /// <summary>
        /// 标识符AN
        /// </summary>
        public static readonly byte AN = 0x02;
        /// <summary>
        /// 标识符AN1
        /// </summary>
        public static readonly byte AN1 = 0x03;
        /// <summary>
        /// 标识符ONE
        /// </summary>
        public static readonly byte ONE = 0x05;
        /// <summary>
        /// 标识符YN
        /// </summary>
        public static readonly byte YN = 0x06;
        /// <summary>
        /// 标识符TF
        /// </summary>
        public static readonly byte TF = 0x07;
        /// <summary>
        /// 标识符INTER
        /// </summary>
        public static readonly byte INTER = 0x08;
        /// <summary>
        /// 标识符BANFEN
        /// </summary>
        public static readonly byte BANFEN = 0x09;
        /// <summary>
        /// 区域名称（也就是那个选择栏的选项）
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 区域包含值
        /// </summary>
        public List<KeyValue> Values { get; set; } =new List<KeyValue>();
        /// <summary>
        /// 自定义类用于存储键值对
        /// </summary>
        public class KeyValue 
        { 
            /// <summary>
            /// 配对项引用（给AN1使用） 用于引用对应的需要同时关闭的那些
            /// </summary>
            public KeyValue pair {  get; set; }
            /// <summary>
            /// 给配对项使用（这个是用来在界面上存储对应的控件的）
            /// </summary>
            public object pair_object { get; set; }
            /// <summary>
            /// 原始字符串 也就是原本的那行是啥
            /// </summary>
            public string Raw_string { get; set; }
            /// <summary>
            /// 原始文件位置行数 原始位置 给界面的跳转和显示用
            /// </summary>
            public int Raw_int { get; set; }
            /// <summary>
            /// 文件文本（也就是标识符后面的文本）
            /// </summary>
            public string Test {  set; get; }
            /// <summary>
            /// 文件类型，值为RulesNode的静态量
            /// </summary>
            public byte Type { get; set; }
            /// <summary>
            /// 给AN,AN1使用。标记最前面是否为;（AN1的配对此项要相同）
            /// </summary>
            public bool Exist {  get; set; }
            /// <summary>
            /// 给其他几种类型使用标识键（标记符前面的=之前）如果是AN,AN1则存为string.Empty
            /// </summary>
            public string Key { get; set; }
            /// <summary>
            /// 给其他几种使用标记值（标记符前面的=的后面），如果是AN,AN1,AN2则存标记符前面的所有内容
            /// </summary>
            public string Value { get; set; }
        }
    }
    /// <summary>
    /// rulesmod文件的操作类
    /// </summary>
    internal class rulesmod
    {
        /// <summary>
        /// 按行存储的文件文本
        /// </summary>
        public static List<string> strings=new List<string>();
        /// <summary>
        /// 静态构造函数，用于初始化数据
        /// </summary>
        static rulesmod()
        {
            if(File.Exists(Path.Combine(Data.exepath, "rulesmod.ini")))//检查文件是否存在
            {
                if(File.Exists(Path.Combine(Data.exepath, "gongju_rulesmod.ini")))//备份存在
                {
                    File.Delete(Path.Combine(Data.exepath, "gongju_rulesmod.ini"));//删除原备份
                }
                File.Copy(Path.Combine(Data.exepath, "rulesmod.ini"), Path.Combine(Data.exepath, "gongju_rulesmod.ini"));//添加备份
                strings = File.ReadAllLines(Path.Combine(Data.exepath, "rulesmod.ini")).ToList();//读取文件
            }
        }
        /// <summary>
        /// 保存文件
        /// </summary>
        public static void Save()
        {
            File.WriteAllLines(Path.Combine(Data.exepath,"rulesmod.ini"),strings);//保存文件
        }
        #region 辅助包限制区
        /// <summary>
        /// 静态字符串
        /// </summary>
        static string BuildLimit = "BuildLimit=1";//建造限制
        /// <summary>
        /// 静态字符串
        /// </summary>
        static string Cloneable = "Cloneable=no";//克隆限制
        /// <summary>
        /// 静态字符串
        /// </summary>
        static string SW_Shots = "SW.Shots=1";//超武次数限制
        /// <summary>
        /// 解除所有的限制 下面单独的和这个一样
        /// </summary>
        public static void JiechuAll()
        {
            for (int i = 0; i < strings.Count; i++)//遍历
            {
                strings[i] = Regex.Replace( strings[i], $@"(?<!;){Regex.Escape(BuildLimit)}", $";{BuildLimit}");//添加分号，并限制分号最多只会存在一个
                strings[i] = Regex.Replace( strings[i], $@"(?<!;){Regex.Escape(Cloneable)}", $";{Cloneable}" );
                strings[i] = Regex.Replace( strings[i], $@"(?<!;){Regex.Escape(SW_Shots)}", $";{SW_Shots}" );
            }
        }
        /// <summary>
        /// 限制所有 下面单独的和这个一样
        /// </summary>
        public static void xianzhiAll()
        {
            for (int i = 0; i < strings.Count; i++)//遍历
            {
                strings[i] = strings[i].Replace($";{BuildLimit}", BuildLimit);//去除分号
                strings[i] = strings[i].Replace($";{Cloneable}", Cloneable);
                strings[i] = strings[i].Replace($";{SW_Shots}", SW_Shots);
            }
        }
        /// <summary>
        /// 解除建造
        /// </summary>
        public static void jiechuBuild()
        {
            for (int i = 0; i < strings.Count; i++)
            {
                strings[i] = strings[i].Replace(BuildLimit,$";{BuildLimit}");
            }
        }
        /// <summary>
        /// 限制建造
        /// </summary>
        public static void xianzhiBuild()
        {
            for (int i = 0; i < strings.Count; i++)
            {
                strings[i] = strings[i].Replace($";{BuildLimit}", BuildLimit);
            }
        }
        /// <summary>
        /// 解除克隆
        /// </summary>
        public static void jiechuClone()
        {
            for (int i = 0; i < strings.Count; i++)
            {
                strings[i] = strings[i].Replace(Cloneable, $";{Cloneable}");
            }
        }
        /// <summary>
        /// 限制克隆
        /// </summary>
        public static void xianzhiClone()
        {
            for (int i = 0; i < strings.Count; i++)
            {
                strings[i] = strings[i].Replace($";{Cloneable}", Cloneable);
            }
        }
        /// <summary>
        /// 解除超武
        /// </summary>
        public static void jiechuSW()
        {
            for (int i = 0; i < strings.Count; i++)
            {
                strings[i] = strings[i].Replace(SW_Shots, $";{SW_Shots}");
            }
        }
        /// <summary>
        /// 限制超武
        /// </summary>
        public static void xianzhiSW()
        {
            for (int i = 0; i < strings.Count; i++)
            {
                strings[i] = strings[i].Replace($";{SW_Shots}", SW_Shots);
            }
        }
        #endregion
        #region 增援管理局升星
        /// <summary>
        /// 静态字符串
        /// </summary>
        static string zhengyuanjie = "[SUPRTX]";
        /// <summary>
        /// 静态字符串
        /// </summary>
        static string zhengyuan_bubing = "Academy.InfantryVeterancy=2.0";
        /// <summary>
        /// 静态字符串
        /// </summary>
        static string zhengyuan_fexing = "Academy.AircraftVeterancy=2.0";
        /// <summary>
        /// 静态字符串
        /// </summary>
        static string zhengyuan_zaiju = "Academy.VehicleVeterancy=2.0";
        /// <summary>
        /// 静态字符串
        /// </summary>
        static string zhengyuan_jianzhu = "Academy.BuildingVeterancy=2.0";
        /// <summary>
        /// 禁用升星
        /// </summary>
        public static void jin_shengxing()
        {
            for (int i = 0; i < strings.Count; i++)//遍历
            {
                strings[i] = Regex.Replace(strings[i], $@"(?<!;){Regex.Escape(zhengyuan_bubing)}", $";{zhengyuan_bubing}");//添加分号，且限制分号最多只有一个
                strings[i] = Regex.Replace(strings[i], $@"(?<!;){Regex.Escape(zhengyuan_fexing)}", $";{zhengyuan_fexing}");
                strings[i] = Regex.Replace(strings[i], $@"(?<!;){Regex.Escape(zhengyuan_zaiju)}", $";{zhengyuan_zaiju}");
                strings[i] = Regex.Replace(strings[i], $@"(?<!;){Regex.Escape(zhengyuan_jianzhu)}", $";{zhengyuan_jianzhu}");
            }
        }
        /// <summary>
        /// 解除禁用升星
        /// </summary>
        public static void jie_shengxing()
        {
            for (int i = 0; i < strings.Count; i++)//遍历
            {
                strings[i] = strings[i].Replace($";{zhengyuan_bubing}", zhengyuan_bubing);//去除分号
                strings[i] = strings[i].Replace($";{zhengyuan_fexing}", zhengyuan_fexing);
                strings[i] = strings[i].Replace($";{zhengyuan_zaiju}", zhengyuan_zaiju);
                strings[i] = strings[i].Replace($";{zhengyuan_jianzhu}", zhengyuan_jianzhu);
            }
        }
        #endregion
        #region 标记定义
        /// <summary>
        /// 结束标记，方便快速结束，减少性能消耗
        /// </summary>
        static string END = ";=end=;";
        /// <summary>
        /// 区域标记在遇到下个区域前均为一个区域标记
        /// </summary>
        static string RE = ";=re=;";
        /// <summary>
        /// 此标记为启用禁用符，用于给注释启用和注释禁用的项仅标记
        /// </summary>
        static string AN = ";=a=;";
        /// <summary>
        /// 此标记为启用禁用符，这个的上下两部分需同时标记
        /// </summary>
        static string AN1 = ";=a1=;";
        /// <summary>
        /// 此标记为01标记
        /// </summary>
        static string ONE = ";=01=;";
        /// <summary>
        /// 此标记为YesNo标记符
        /// </summary>
        static string YN = ";=yn=;";
        /// <summary>
        /// 此标记为TrueFalse
        /// </summary>
        static string TF = ";=tf=;";
        /// <summary>
        /// 此标记为int数值
        /// </summary>
        static string INTER = ";=i=;";
        /// <summary>
        /// 此标记为百分数数值
        /// </summary> 
        static string BAIFEN = ";=b=;";
        #endregion

        /// <summary>
        /// 处理文件成为字典方便使用
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, RulesNode> DisRules()
        {
            var result = new Dictionary<string, RulesNode>() { { "NULL", new RulesNode() { Name = "NULL" } } };//添加一个NULL节防止在第一个=re=前有键值对
            RulesNode DanQian = result["NULL"];//初始情况下为NULL 当前遍历=re=节
            Dictionary<string, RulesNode.KeyValue> AN1S = new Dictionary<string, RulesNode.KeyValue>();//AN1列表
            for (int i = 0; i < strings.Count; i++)//遍历
            {
                string line = strings[i];//局部存储遍历变量
                if (line.IndexOf(END) != -1)//遇到截止符
                {
                    break;//结束循环
                }
                if (line.IndexOf(RE) != -1)//提取分区名
                {
                    int zuo = line.IndexOf("<");//<位置
                    int you = line.IndexOf(">");//>位置
                    string lname = line.Substring(zuo+1, you - zuo-1);//截取字符串
                    if (!result.TryGetValue(lname, out DanQian))//如果字典存在则使用字典，字典不存在则新建
                    {
                        result.Add(lname, new RulesNode() { Name = lname });//添加进入返回字典
                        DanQian = result[lname];//将本分区设为当前
                    }
                    continue;//结束本次循环
                }
                int BJ = line.IndexOf(AN);//获取AN是否存在，存在返回对应位置，不存在返回-1  下面的与此类同
                if (BJ != -1)//AN存在
                {
                    var _kv = DisRukes_Fuzhu_AN(line, i, AN, RulesNode.AN,out _);//调用处理函数获取处理后键值对
                    DanQian.Values.Add(_kv);//添加进入当前节
                    continue;//结束本次循环 下面类同
                }
                BJ = line.IndexOf(AN1);
                if (BJ != -1)
                {
                    string[] _lines;//定义一个临时数组存储
                    var _kv = DisRukes_Fuzhu_AN(line,i,AN1,RulesNode.AN1,out _lines);//调用处理函数
                    DanQian.Values.Add(_kv);

                    //处理AN1的数字对照
                    int qian = _lines[1].IndexOf("<");
                    int hou = _lines[1].IndexOf(">");
                    string shuzi = _lines[1].Substring(qian+1, hou - qian-1);//提取对应标记符
                                                                             //例如下面的标记0
                                                                             //+=GAREAP               ;=a1=;<0>（东电核污艇）
                                                                             //+=GAREAPL              ;=a1=;<0>（东电核污艇）
                    RulesNode.KeyValue ls;//定义临时键值对
                    if (AN1S.TryGetValue(shuzi, out ls))//如果存在，也就是说已经读取到GAREAPL了这是后一个所以准备结束
                    {
                        _kv.pair = ls;//互相配对
                        ls.pair = _kv;//互相配对
                        AN1S.Remove(shuzi);//在字典中删除
                    }
                    else//如果不存在，也就是说读取到GAREAP了，这个还没有结束
                    {
                        AN1S.Add(shuzi, _kv);//加入字典 也就是上面的AN1字典
                    }
                    continue;
                }
                BJ = line.IndexOf(ONE);
                if (BJ != -1)
                {
                    var _kv = DisRules_Fuzhu(line, i, ONE, RulesNode.ONE);//对应处理函数
                    DanQian.Values.Add(_kv);
                    continue;
                }
                BJ = line.IndexOf(YN);
                if (BJ != -1)
                {
                    var _kv = DisRules_Fuzhu(line, i, YN, RulesNode.YN);
                    DanQian.Values.Add(_kv);
                    continue;
                }
                BJ = line.IndexOf(TF);
                if (BJ != -1)
                {
                    var _kv = DisRules_Fuzhu(line, i, TF, RulesNode.TF);
                    DanQian.Values.Add(_kv);
                    continue;
                }
                BJ = line.IndexOf(INTER);
                if (BJ != -1)
                {
                    var _kv = DisRules_Fuzhu(line,i,INTER,RulesNode.INTER);
                    DanQian.Values.Add(_kv);
                    continue;
                }
                BJ = line.IndexOf(BAIFEN);
                if (BJ != -1)
                {
                    var _kv = DisRules_Fuzhu(line, i, BAIFEN, RulesNode.BANFEN);
                    DanQian.Values.Add(_kv);
                    continue;
                }
            }
            return result;
        }
        /// <summary>
        /// AN处理函数用于AN和AN1
        /// </summary>
        /// <param name="line">当前行内容</param>
        /// <param name="i">当前行号</param>
        /// <param name="seg">标记字符串</param>
        /// <param name="type">标记类型</param>
        /// <param name="lines">返回列表 给AN1用的</param>
        /// <returns></returns>
        private static RulesNode.KeyValue DisRukes_Fuzhu_AN(string line, int i, string seg, byte type,out string[] lines)
        {
            RulesNode.KeyValue result = new RulesNode.KeyValue();//定义键值对
            string[] _lines = line.Split(new[] { seg }, StringSplitOptions.None);//根据标记分割
            
            result.Raw_string = line;//原始字符串
            result.Raw_int = i;//行号
            result.Test = _lines[1];//简要解释文本
            result.Type = type;//类型
            result.Exist = true;//是否被注释，先默认没有被注释
            if (_lines[0].IndexOf(";") != -1)//如果被注释了
            {
                result.Exist = false;//修改为被注释
            }
            result.Key = string.Empty;//对AN和AN1没用，存为默认字符串
            result.Value = _lines[0];//标记符前面的内容//AN和AN1和其他几个不一样
            lines=_lines;//把分割好的字符串返回（给AN1使用）
            return result;//返回键值对
        }
        /// <summary>
        /// 辅助处理
        /// </summary>
        /// <param name="line">行内容</param>
        /// <param name="i">行号</param>
        /// <param name="seg">分隔符</param>
        /// <param name="type">类型</param>
        /// <returns></returns>
        private static RulesNode.KeyValue DisRules_Fuzhu(string line,int i,string seg,byte type)
        {
            var result = new RulesNode.KeyValue();//定义键值对
            string[] _lines = line.Split(new[] { seg }, StringSplitOptions.None);//根据标记分割
            result.Raw_string = line;//原始字符串
            result.Raw_int = i;//行号
            result.Test = _lines[1];//简要解释文本
            result.Type = type;//添加类型
            string[] Keys = _lines[0].Split('=');//分离键值对
            result.Key = Keys[0];//添加键
            result.Value = Keys[1].Trim();//添加值
            if(seg==BAIFEN)//如果是百分号类型 例
                           //Armor=10,ARMOR,yes,1.25          ;=b=;护甲
                           //这个等于号之后只有第一个有用，所以在进行处理
            {
                string[] values = Keys[1].Split(',');//分离值列表
                result.Value = values[0];//添加值
            }
            return result;//返回
        }
        /// <summary>
        /// 设置Rules，也就是修改文件字符串列表
        /// </summary>
        public static void SetRules(int lineNumber,string line)
        {
            strings[lineNumber]=line;
        }
    }
    /// <summary>
    /// rulesmo设置类（主要为起源ai）
    /// </summary>
    class rulesmo
    {
        /// <summary>
        /// 存储文件
        /// </summary>
        public static string[] strings =null;
        static rulesmo()
        {
            if (File.Exists((Path.Combine(Data.exepath, "rulesmo.ini"))))//如何文件存在
            {
                strings = File.ReadAllLines(Path.Combine(Data.exepath, "rulesmo.ini"));//读取文件
            }
        }
        
        /// <summary>thn 
        /// 禁用全部起源
        /// </summary>
        public static void JIN()
        {
            if (strings == null)
            {
                strings = File.ReadAllLines(Path.Combine(Data.exepath, "rulesmo.ini"));
            }
            J_OriginAI();//禁用起源
            J_PartOriginAI();//禁用部分起源
        }
        /// <summary>
        /// 保存文件，在修改后一定需要触发这个否则不会保存
        /// </summary>
        public static void Save()
        {
            File.WriteAllLines(Path.Combine(Data.exepath,"rulesmo.ini"), strings);
        }
        /// <summary>
        /// 启用起源
        /// </summary>
        public static void Q_OriginAI()
        {
            if (strings == null)
            {
                strings = File.ReadAllLines(Path.Combine(Data.exepath, "rulesmo.ini"));
            }
            J_PartOriginAI();//先禁用部分起源
            var file = Path.Combine(Data.exepath, "bak_aimo.ini");//bak_aimo文件路径
            if (File.Exists(file))//bak_aimo存在
            {
                var newfile = Path.Combine(Data.exepath, "aimo.ini");//构造aimo路径
                File.Move(file, newfile);//替换文件
            }
            for (int i = 0; i < strings.Length; i++)//遍历
            {
                string line = strings[i];
                if (line.IndexOf(";3=OriginAI.ini")!=-1)//包含指定值
                {
                    strings[i] =line.Replace(";3=OriginAI.ini", "3=OriginAI.ini");//解除注释
                }
                if(line.IndexOf("[General]") !=-1)//及时停止，防止性能损耗，这里是根据文件结构写死的
                {
                    break;
                }
            }
        }
        /// <summary>
        /// 启用部分起源
        /// </summary>
        public static void Q_PartOriginAI()
        {
            if (strings == null)
            {
                strings = File.ReadAllLines(Path.Combine(Data.exepath, "rulesmo.ini"));
            }
            J_OriginAI();//先禁用起源
            for (int i = 0; i < strings.Length; i++)//遍历
            {
                string line = strings[i];
                if (line.IndexOf(";4=PartOriginAI.ini") != -1)
                {
                    strings[i] = line.Replace(";4=PartOriginAI.ini", "4=PartOriginAI.ini");//解除注释
                }
                if (line.IndexOf("[General]") != -1)//及时停止，防止性能损耗
                {
                    break;
                }
            }
        }
        /// <summary>
        /// 禁用起源
        /// </summary>
        public static void J_OriginAI()
        {
            if (strings == null)
            {
                strings = File.ReadAllLines(Path.Combine(Data.exepath, "rulesmo.ini"));
            }
            var file = Path.Combine(Data.exepath, "aimo.ini");//构造aimo路径
            if (File.Exists(file))//将aimo.ini取消
            {
                var newfile = Path.Combine(Data.exepath, "bak_aimo.ini");//bak_aimo路径
                File.Move(file, newfile);
            }
            for (int i = 0; i < strings.Length; i++)//遍历
            {
                string line = strings[i];
                if (line.IndexOf("3=OriginAI.ini") != -1&&line.IndexOf(";3=OriginAI.ini") ==-1)//防止多添加分号
                {
                    strings[i] = line.Replace("3=OriginAI.ini", ";3=OriginAI.ini");//添加注释
                }
                if (line.IndexOf("[General]") != -1)//及时停止，防止性能损耗
                {
                    break;
                }
            }
        }
        /// <summary>
        /// 禁用部分起源
        /// </summary>
        public static void J_PartOriginAI()
        {
            if (strings == null)
            {
                strings = File.ReadAllLines(Path.Combine(Data.exepath, "rulesmo.ini"));
            }
            for (int i = 0; i < strings.Length; i++)
            {
                string line = strings[i];
                if (line.IndexOf("4=PartOriginAI.ini") != -1&&line.IndexOf(";4=PartOriginAI.ini") ==-1)
                {
                    strings[i] = line.Replace("4=PartOriginAI.ini", ";4=PartOriginAI.ini");//添加注释
                }
                if (line.IndexOf("[General]") != -1)//及时停止，防止性能损耗
                {
                    break;
                }
            }
        }
    }
}
