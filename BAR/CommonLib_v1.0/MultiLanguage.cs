using BAR.Commonlib;
using BAR.Properties;
using CCWin.SkinControl;
using LBSoft.IndustrialCtrls.Leds;
using Spire.Xls.Core.Converter.General.Word;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BAR.CommonLib_v1._0
{
    class MultiLanguage
    {
        /// <summary>
        /// 语言
        /// </summary>
        public static string DefaultLanguage = "en";

        /// <summary>
        /// 修改默认语言
        /// </summary>
        /// <param name="lang">待设置默认语言</param>
        public static void SetDefaultLanguage()
        {
            DefaultLanguage = Properties.Settings.Default.DefaultLanguage;
            //if (DefaultLanguage != "en-US") 
            //    return;
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(DefaultLanguage);
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(DefaultLanguage);
        }

        public static void SaveDefaultLanguage()
        {
            Properties.Settings.Default.DefaultLanguage = DefaultLanguage;//【在Properties.Settings中添加DefaultLanguage】
            Properties.Settings.Default.Save();
        }

        public static bool IsEnglish()
        {
            if (DefaultLanguage == GlobConstData.ST_English)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 加载语言
        /// </summary>
        /// <param name="form">加载语言的窗口</param>
        /// <param name="formType">窗口的类型</param>
        public static void LoadLanguage(Form form, Type formType)
        {
            if (DefaultLanguage != "en-US")
                return;
            if (form != null)
            {
                //ComponentResourceManager resources = new ComponentResourceManager(formType);
                UpDataLanguageOfType(typeof(LBLed), form.Controls);
                //resources.ApplyResources(form, "$this");
                //Loading(form, resources);
            }
        }

        public static void UpDataLanguageOfType(Type type, Control.ControlCollection formControls)
        {
            foreach (Control control in formControls)
            {
                if (control.GetType() == type)
                {
                    (control as LBLed).Label = GetString(control.Name);
                    (control as LBLed).Font = new Font("Arial", control.Font.Size, control.Font.Style);
                }
                    
                if (control.Controls.Count > 0)
                    UpDataLanguageOfType(type, control.Controls);
            }
        }

        /// <summary>
        /// 获取资源文件对应名称字符串
        /// </summary>
        /// <param name="Name">名称</param>
        /// <param name="formType">资源文件位置</param>
        /// <returns></returns>
        public static string GetString(string Name, Type formType)
        {
            if (DefaultLanguage != "en-US")
                return Name;
            ComponentResourceManager resources = new ComponentResourceManager(formType);
            string strGet= resources.GetString(Name);
            if (!strGet.IsNull())
            {
                Name = strGet;
            }
            return Name;
        }

        /// <summary>
        /// 获取强类型资源文件对应名称字符串
        /// </summary>
        /// <param name="Name">名称</param>
        /// <returns></returns>
        public static string GetString(string Name)
        {
            if (DefaultLanguage != "en-US")
                return Name;
            string[] strTemp = Name.Split('[', ']');
            Name = null;
            string strGet;
            for (int i = 0; i < strTemp.Length; i++)
            {
                strGet = CommonRes.ResourceManager.GetString(strTemp[i]);
                if (!strGet.IsNull())
                {
                    Name += strGet;
                }
                else
                {
                    Name += strTemp[i];
                }
            }
            
            return Name;
        }

        /// <summary>
        /// 加载语言
        /// </summary>
        /// <param name="control">控件</param>
        /// <param name="resources">语言资源</param>
        private static void Loading(Control control, ComponentResourceManager resources)
        {
            if (control is MenuStrip)
            {
                //将资源与控件对应
                resources.ApplyResources(control, control.Name);
                MenuStrip ms = (MenuStrip)control;
                if (ms.Items.Count > 0)
                {
                    foreach (ToolStripMenuItem c in ms.Items)
                    {
                        //遍历菜单
                        Loading(c, resources);
                    }
                }
            }

            if (control is StatusStrip)
            {
                //将资源与控件对应
                resources.ApplyResources(control, control.Name);
                StatusStrip ts = (StatusStrip)control;

                foreach (ToolStripItem c in ts.Items)
                {
                    //遍历菜单
                    resources.ApplyResources(c, c.Name);
                }
            }

            //需要大家去完善


            foreach (Control c in control.Controls)
            {
                //c.Font = new Font("Arial", c.Font.Size, c.Font.Style);
                resources.ApplyResources(c, c.Name);
                Loading(c, resources);
            }
        }

        /// <summary>
        /// 遍历菜单
        /// </summary>
        /// <param name="item">菜单项</param>
        /// <param name="resources">语言资源</param>
        private static void Loading(ToolStripMenuItem item, ComponentResourceManager resources)
        {
            if (item is ToolStripMenuItem)
            {
                resources.ApplyResources(item, item.Name);
                ToolStripMenuItem tsmi = (ToolStripMenuItem)item;
                if (tsmi.DropDownItems.Count > 0)
                {
                    foreach (ToolStripMenuItem c in tsmi.DropDownItems)
                    {
                        Loading(c, resources);
                    }
                }
            }
        }
    }
}
