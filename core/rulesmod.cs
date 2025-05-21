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
    class RulesNode
    {
        public static readonly byte END = 0x00;
        public static readonly byte RE = 0x01;
        public static readonly byte AN = 0x02;
        public static readonly byte AN1 = 0x03;
        public static readonly byte ONE = 0x05;
        public static readonly byte YN = 0x06;
        public static readonly byte TF = 0x07;
        public static readonly byte INTER = 0x08;
        public static readonly byte BANFEN = 0x09;
        /// <summary>
        /// 区域名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 区域包含值
        /// </summary>
        public List<KeyValue> Values { get; set; } =new List<KeyValue>();
        public class KeyValue 
        { 
            /// <summary>
            /// 配对项引用（给AN1使用）
            /// </summary>
            public KeyValue pair {  get; set; }
            /// <summary>
            /// 给配对项使用
            /// </summary>
            public object pair_object { get; set; }
            /// <summary>
            /// 原始字符串
            /// </summary>
            public string Raw_string { get; set; }
            /// <summary>
            /// 原始文件位置行数
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
    internal class rulesmod
    {
        public static List<string> strings=new List<string>();
        /// <summary>
        /// 静态构造函数，用于初始化数据
        /// </summary>
        static rulesmod()
        {
            if(File.Exists(Path.Combine(Data.exepath, "rulesmod.ini")))
            {
                if(File.Exists(Path.Combine(Data.exepath, "gongju_rulesmod.ini")))//删除原备份
                {
                    File.Delete(Path.Combine(Data.exepath, "gongju_rulesmod.ini"));
                }
                File.Copy(Path.Combine(Data.exepath, "rulesmod.ini"), Path.Combine(Data.exepath, "gongju_rulesmod.ini"));//添加备份
                strings = File.ReadAllLines(Path.Combine(Data.exepath, "rulesmod.ini")).ToList();
            }
        }
        /// <summary>
        /// 保存
        /// </summary>
        public static void Save()
        {
            File.WriteAllLines(Path.Combine(Data.exepath,"rulesmod.ini"),strings);
        }
        #region 辅助包限制区
        static string BuildLimit = "BuildLimit=1";//建造限制
        static string Cloneable = "Cloneable=no";//克隆限制
        static string SW_Shots = "SW.Shots=1";//超武次数限制
        /// <summary>
        /// 解除所有
        /// </summary>
        public static void JiechuAll()
        {
            for (int i = 0; i < strings.Count; i++)
            {
                strings[i] = Regex.Replace( strings[i], $@"(?<!;){Regex.Escape(BuildLimit)}", $";{BuildLimit}");
                strings[i] = Regex.Replace( strings[i], $@"(?<!;){Regex.Escape(Cloneable)}", $";{Cloneable}" );
                strings[i] = Regex.Replace( strings[i], $@"(?<!;){Regex.Escape(SW_Shots)}", $";{SW_Shots}" );
            }
        }
        /// <summary>
        /// 限制所有
        /// </summary>
        public static void xianzhiAll()
        {
            for (int i = 0; i < strings.Count; i++)
            {
                strings[i] = strings[i].Replace($";{BuildLimit}", BuildLimit);
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
        static string zhengyuanjie = "[SUPRTX]";
        static string zhengyuan_bubing = "Academy.InfantryVeterancy=2.0";
        static string zhengyuan_fexing = "Academy.AircraftVeterancy=2.0";
        static string zhengyuan_zaiju = "Academy.VehicleVeterancy=2.0";
        static string zhengyuan_jianzhu = "Academy.BuildingVeterancy=2.0";
        /// <summary>
        /// 禁用升星
        /// </summary>
        public static void jin_shengxing()
        {
            for (int i = 0; i < strings.Count; i++)
            {
                strings[i] = Regex.Replace(strings[i], $@"(?<!;){Regex.Escape(zhengyuan_bubing)}", $";{zhengyuan_bubing}");
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
            for (int i = 0; i < strings.Count; i++)
            {
                strings[i] = strings[i].Replace($";{zhengyuan_bubing}", zhengyuan_bubing);
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


        public static Dictionary<string, RulesNode> DisRules()
        {
            var result = new Dictionary<string, RulesNode>() { { "NULL", new RulesNode() { Name = "NULL" } } };
            RulesNode DanQian = result["NULL"];//初始情况下为NULL
            Dictionary<string, RulesNode.KeyValue> AN1S = new Dictionary<string, RulesNode.KeyValue>();//AN1列表
            for (int i = 0; i < strings.Count; i++)
            {
                string line = strings[i];
                if (line.IndexOf(END) != -1)//遇到截止符
                {
                    break;
                }
                if (line.IndexOf(RE) != -1)//提取分区名
                {
                    int zuo = line.IndexOf("<");
                    int you = line.IndexOf(">");
                    string lname = line.Substring(zuo+1, you - zuo-1);
                    if (!result.TryGetValue(lname, out DanQian))//如果字典存在则使用字典，字典不存在则新建
                    {
                        result.Add(lname, new RulesNode() { Name = lname });
                        DanQian = result[lname];
                    }
                    continue;
                }
                int BJ = line.IndexOf(AN);
                if (BJ != -1)
                {
                    var _kv = DisRukes_Fuzhu_AN(line, i, AN, RulesNode.AN,out _);
                    DanQian.Values.Add(_kv);
                    continue;
                }
                BJ = line.IndexOf(AN1);
                if (BJ != -1)
                {
                    string[] _lines;
                    var _kv = DisRukes_Fuzhu_AN(line,i,AN1,RulesNode.AN1,out _lines);
                    DanQian.Values.Add(_kv);

                    //处理AN1的数字对照
                    int qian = _lines[1].IndexOf("<");
                    int hou = _lines[1].IndexOf(">");
                    string shuzi = _lines[1].Substring(qian+1, hou - qian-1);
                    RulesNode.KeyValue ls;
                    if (AN1S.TryGetValue(shuzi, out ls))//如果存在
                    {
                        _kv.pair = ls;
                        ls.pair = _kv;//互相配对
                        AN1S.Remove(shuzi);//在字典中删除
                    }
                    else//如果不存在
                    {
                        AN1S.Add(shuzi, _kv);//加入字典
                    }
                    continue;
                }
                BJ = line.IndexOf(ONE);
                if (BJ != -1)
                {
                    var _kv = DisRules_Fuzhu(line, i, ONE, RulesNode.ONE);
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
        private static RulesNode.KeyValue DisRukes_Fuzhu_AN(string line, int i, string seg, byte type,out string[] lines)
        {
            RulesNode.KeyValue result = new RulesNode.KeyValue();
            string[] _lines = line.Split(new[] { seg }, StringSplitOptions.None);//分割
            
            result.Raw_string = line;
            result.Raw_int = i;
            result.Test = _lines[1];
            result.Type = type;
            result.Exist = true;
            if (_lines[0].IndexOf(";") != -1)
            {
                result.Exist = false;
            }
            result.Key = string.Empty;
            result.Value = _lines[0];
            lines=_lines;
            return result;
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
            var result = new RulesNode.KeyValue();
            string[] _lines = line.Split(new[] { seg }, StringSplitOptions.None);//分割
            result.Raw_string = line;//添加原始字符串
            result.Raw_int = i;//添加原始位置
            result.Test = _lines[1];//添加名称
            result.Type = type;//添加类型
            string[] Keys = _lines[0].Split('=');//分离键值对
            result.Key = Keys[0];//添加键
            result.Value = Keys[1].Trim();//添加值
            if(seg==BAIFEN)
            {
                string[] values = Keys[1].Split(',');//分离值列表
                result.Value = values[0];//添加值
            }
            return result;
        }
        /// <summary>
        /// 设置Rules
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
        public static string[] strings =null;
        static rulesmo()
        {
            if (File.Exists((Path.Combine(Data.exepath, "rulesmo.ini"))))
            {
                strings = File.ReadAllLines(Path.Combine(Data.exepath, "rulesmo.ini"));
            }
        }
        
        /// <summary>
        /// 禁用全部起源
        /// </summary>
        public static void JIN()
        {
            if (strings == null)
            {
                strings = File.ReadAllLines(Path.Combine(Data.exepath, "rulesmo.ini"));
            }
            J_OriginAI();
            J_PartOriginAI();
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
            var file = Path.Combine(Data.exepath, "bak_aimo.ini");
            if (File.Exists(file))//添加aimo.ini
            {
                var newfile = Path.Combine(Data.exepath, "aimo.ini");
                File.Move(file, newfile);
            }
            for (int i = 0; i < strings.Length; i++)
            {
                string line = strings[i];
                if (line.IndexOf(";3=OriginAI.ini")!=-1)
                {
                    strings[i] =line.Replace(";3=OriginAI.ini", "3=OriginAI.ini");//解除注释
                }
                if(line.IndexOf("[General]") !=-1)//及时停止，防止性能损耗
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
            for (int i = 0; i < strings.Length; i++)
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
            var file = Path.Combine(Data.exepath, "aimo.ini");
            if (File.Exists(file))//将aimo.ini取消
            {
                var newfile = Path.Combine(Data.exepath, "bak_aimo.ini");
                File.Move(file, newfile);
            }
            for (int i = 0; i < strings.Length; i++)
            {
                string line = strings[i];
                if (line.IndexOf("3=OriginAI.ini") != -1&&line.IndexOf(";3=OriginAI.ini") ==-1)
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
