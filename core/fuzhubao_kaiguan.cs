using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 辅助包工具.core
{
    internal class fuzhubao_kaiguan
    {
        /// <summary>
        /// 辅助包基础文件列表
        /// </summary>
        public static List<string> fuzhubao = new List<string>()
        {
            "artmo.ini" ,
            "artmod.ini" ,
            "expandmo88.mix" ,
            "rulesmo.ini" ,
            "rulesmod.ini" ,
            "soundmo.ini" ,
            "stringtable02.csf" ,
            "uimd.ini",
        };
        /// <summary>
        /// 辅助包全部文件
        /// </summary>
        public static List<string> fuzhubao_ALL = new List<string>()
        {
            "artmo.ini" ,
            "artmod.ini" ,
            "bak_aimo.ini" ,
            "bbk_rulesmod.ini" ,
            "emigdal.map" ,
            "expandmo84.mix" ,
            "expandmo85.mix" ,
            "expandmo86.mix" ,
            "expandmo87.mix" ,
            "expandmo88.mix" ,
            "game.fnt" ,
            "GetShortcutTarget.vbs" ,
            "Phobos.dll" ,
            "pips.shp" ,
            "Ren_rulesmo.ini" ,
            "rulesmo.ini" ,
            "rulesmod.ini" ,
            "soundmo.ini" ,
            "stringtable02.csf" ,
            "stringtable10.csf" ,
            "stroph.map" ,
            "uimd.ini" ,
            "更新说明.txt" ,
            "鸣谢列表.txt",
            "使用前必看！必须！.txt",
        };
        public static List<string> fuzhubao_qiyuan_0 = new List<string>()
        {
            "bak_aimo.ini",
            "rulesmo.ini",
            "Ren_rulesmo.ini",
        };
        public static List<string> fuzhubao_qiyuan_1 = new List<string>()
        {
            "aimo.ini",
            "bbk_rulesmo.ini",
            "rulesmo.ini",
        };
        public static List<string> fuzhubao_qiyuan_2 = new List<string>()
        {
            "aimo.ini",
            "rulesmo.ini",
            "bbk_rulesmo.ini",
        };
        public static List<string> fuzhubao_qiyuan_3 = new List<string>()
        {
            "bak_aimo.ini",
            "Ren_rulesmo.ini",
            "rulesmo.ini",
        };
        public static List<string> fuzhubao_xianzhi_0 = new List<string>()
        {
            "rulesmod.ini",
            "bbk_rulesmod.ini",
        };
        public static List<string> fuzhubao_xianzhi_1 = new List<string>()
        {
            "bck_rulesmod.ini",
            "rulesmod.ini",
        };
        public static List<string> fuzhubao_xianzhi_2 = new List<string>()
        {
            "rulesmod.ini",
            "bck_rulesmod.ini",
        };
        public static List<string> fuzhubao_xianzhi_3 = new List<string>()
        {
            "bbk_rulesmod.ini",
            "rulesmod.ini",
        };
        /// <summary>
        /// 禁用辅助包
        /// </summary>
        public static void Jin_fuzhubao()
        {
            for (int i = 0; i < fuzhubao.Count; i++)
            {
                string file = Path.Combine(Data.exepath, fuzhubao[i]);
                if (File.Exists(file))
                {
                    File.Move(file, "bak_" + file);
                }
            }
        }
        /// <summary>
        /// 完全禁用辅助包
        /// </summary>
        public static void JinAll_fuzhubao()
        {
            for (int i = 0; i < fuzhubao.Count; i++)
            {
                {
                    string file = Path.Combine(Data.exepath, fuzhubao[i]);
                    if (File.Exists(file))
                    {
                        File.Move(file, "bak_" + file);
                    }
                }
            }
            string phobos = Path.Combine(Data.exepath, "Phobos.dll");
            if (File.Exists(phobos))
            {
                File.Move(phobos, $"bak_{phobos}");
            }
        }
        /// <summary>
        /// 启用辅助包
        /// </summary>
        public static void qi_duzhubao()
        {
            for(int i = 0;i < fuzhubao.Count;i++)
            {
                string file= Path.Combine(Data.exepath,$"bak_{fuzhubao[i]}");
                if (File.Exists(file))
                {
                    string newName = file.Substring(4);
                    File.Move(file, newName);
                }
            }
        }
        /// <summary>
        /// 删除辅助包
        /// </summary>
        public static void delete_fuzhubao()
        {
            for(int i=0;i<fuzhubao_ALL.Count;i++)
            {
                string file = Path.Combine(Data.exepath, fuzhubao_ALL[i]);
                if(File.Exists(file))
                {
                    File.Delete(file);
                }
            }
        }
        /// <summary>
        /// 启用起源ai
        /// </summary>
        public static void qi_qiyuan()
        {
            for(int i=0;i<fuzhubao_qiyuan_0.Count;i++)
            {
                string file=Path.Combine(Data.exepath,fuzhubao_qiyuan_0[i]);
                string newName = Path.Combine(Data.exepath, fuzhubao_qiyuan_1[i]);
                if(File.Exists(file))
                {
                    File.Move (file, newName);
                }
            }
        }
        /// <summary>
        /// 禁用起源ai
        /// </summary>
        public static void jin_qiyuan()
        {
            for (int i = 0; i < fuzhubao_qiyuan_1.Count; i++)
            {
                string file = Path.Combine(Data.exepath, fuzhubao_qiyuan_2[i]);
                string newName = Path.Combine(Data.exepath, fuzhubao_qiyuan_3[i]);
                if (File.Exists(file))
                {
                    File.Move(file, newName);
                }
            }
        }
        /// <summary>
        /// 取消限制
        /// </summary>
        public static void qu_xianzhi()
        {
            for (int i = 0; i < fuzhubao_xianzhi_0.Count; i++)
            {
                string file = Path.Combine(Data.exepath, fuzhubao_xianzhi_0[i]);
                string newName = Path.Combine(Data.exepath, fuzhubao_xianzhi_1[i]);
                if (File.Exists(file))
                {
                    File.Move(file, newName);
                }
            }
        }
        /// <summary>
        /// 恢复限制
        /// </summary>
        public static void hui_xianzhi()
        {
            for (int i = 0; i < fuzhubao_xianzhi_2.Count; i++)
            {
                string file = Path.Combine(Data.exepath, fuzhubao_xianzhi_2[i]);
                string newName = Path.Combine(Data.exepath, fuzhubao_xianzhi_3[i]);
                if (File.Exists(file))
                {
                    File.Move(file, newName);
                }
            }
        }
    }

}
