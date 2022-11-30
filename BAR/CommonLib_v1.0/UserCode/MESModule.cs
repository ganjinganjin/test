using BAR.Commonlib;
using Newtonsoft.Json.Linq;
using RestSharp;
using Spire.Xls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using TensionDetectionDLL;

namespace BAR
{
    public class MESModule
    {
        Act g_act = Act.GetInstance();
        Config g_config = Config.GetInstance();
        
        public bool GetNumByLotSN_XWD(string strSN,out string strResponse)
        {
            XmlDocument document = new XmlDocument();//创建XmlDocument对象

            XmlDeclaration declaration = document.CreateXmlDeclaration("1.0", "utf-8", "");//xml文档的声明部分
            document.AppendChild(declaration);

            XmlElement root = document.CreateElement("soap","Envelope", "http://schemas.xmlsoap.org/soap/envelope/");
            root.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            root.SetAttribute("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
            document.AppendChild(root);

            XmlElement soap_elem = document.CreateElement("soap","Body", "http://schemas.xmlsoap.org/soap/envelope/");
            root.AppendChild(soap_elem);

            XmlElement api_elem = document.CreateElement("GetNumByLotSN");
            api_elem.SetAttribute("xmlns", "WWW.SUNWODA.COM");
            soap_elem.AppendChild(api_elem);
            
            XmlElement elem = document.CreateElement("LotSN");
            elem.InnerText = strSN;
            api_elem.AppendChild(elem);

            string Url = string.Format("http://{0:s}:{1:s}/MESInterface.asmx", Mes.IP, Mes.Port);
            var client = new RestSharp.RestClient(Url);
            var requestPost = new RestRequest(Method.POST);
            requestPost.Timeout = 5000;
            requestPost.AddHeader("Content-Type", "text/xml");
            requestPost.AddParameter("text/xml", document.InnerXml, ParameterType.RequestBody);
            IRestResponse responsePost = client.Execute(requestPost);
            strResponse = responsePost.Content;
            if (!responsePost.IsSuccessful)
            {
                string str = "获取Mes数据错误!!!\r\n错误信息：" + responsePost.ErrorMessage;
                g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, str, "Error");
                MessageBox.Show(str, "提示：", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return responsePost.IsSuccessful;
        }

        /// <summary>
        /// 解析MES返回的数据
        /// </summary>
        /// <param name="strResponse">MES返回的数据</param>
        /// <returns></returns>
        public bool AnalyseLotSNResp_XWD(int type, string strResponse, string httpRoot1, string httpRoot2)
        {
            XmlDocument doc = new XmlDocument();//创建XmlDocument对象
            doc.LoadXml(strResponse);

            string json = Newtonsoft.Json.JsonConvert.SerializeXmlNode(doc);
            JObject obj = Newtonsoft.Json.Linq.JObject.Parse(json);
            string resMag = obj["soap:Envelope"]["soap:Body"][httpRoot1][httpRoot2].ToString();
            if (resMag.IndexOf("FALSE") > -1)
            {
                g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, resMag, "Error");
                MessageBox.Show(resMag, "错误提示：", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            else if(type == 0)
            {
                obj = Newtonsoft.Json.Linq.JObject.Parse(resMag);
                Mes.ItemCode = obj["ItemCode"].ToString();
                Mes.Count = Convert.ToInt32(obj["Count"].ToString());
                if (Mes.ItemCode == "")
                {
                    g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "批次号获取异常!!!", "Error");
                    MessageBox.Show("批次号获取异常!!!", "错误提示：", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                else
                {
                    return true;
                }
            }
            return true;
            
        }

        /// <summary>
        /// 检索烧录文档
        /// </summary>
        /// <returns></returns>
        public bool CheckProgFiles_XWD()
        {
            //访问Excel器件表
            if (Directory.Exists(g_config.StrAppDir))
            {
                string filepath = g_config.StrAppDir + "\\" + "DeviceType.xls";//一定要在路径和文件名之间加
                if (!File.Exists(filepath))
                {
                    string str = string.Format("在{0:s}无法找到文件：{1:s}", g_config.StrAppDir, "DeviceType.xls");
                    g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, str, "Warning");
                    MessageBox.Show(str, "提示：", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                else
                {
                    Workbook workbook = new Workbook();
                    workbook.LoadFromFile(filepath);
                    Worksheet sheet = workbook.Worksheets[0];
                    CellRange[] range = sheet.FindAllString(Mes.ItemCode, false, false);
                    if (range.Length == 1)
                    {
                        Mes.Checksum_Mes = sheet.Range[range[0].Row, range[0].Column + 1].Value;
                        //Mes.MD5 = sheet.Range[range[0].Row, range[0].Column + 2].Value;
                    }
                    else if(range.Length > 1)
                    {
                        string str = string.Format("DeviceType.xls文件中存在多个料号为：{0:s}的匹配项", Mes.ItemCode);
                        g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, str, "Warning");
                        MessageBox.Show(str, "提示：", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                    else
                    {
                        string str = string.Format("DeviceType.xls文件中不存在料号为：{0:s}的匹配项", Mes.ItemCode);
                        g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, str, "Warning");
                        MessageBox.Show(str, "提示：", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return false;
                    }
                }
                
            }

            string file = (Mes.ItemCode + "_" + Mes.Version_File + "_" + Mes.Checksum_Mes + ".apr");
            if (!Directory.Exists(Mes.ProgFilePath))
            {
                string str = string.Format("文件夹{0:s}不存在", Mes.ProgFilePath);
                g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, str, "Warning");
                MessageBox.Show(str, "提示：", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else
            {
                DirectoryInfo dir = new DirectoryInfo(Mes.ProgFilePath);
                FileInfo[] fi = dir.GetFiles(file);
                if (fi.Length < 0)
                {
                    string str = string.Format("在{0:s}下无法找到烧录档案，料号为：{1:s}，版本为：{2:s}\r\n", Mes.ProgFilePath, Mes.ItemCode, Mes.Version_File);
                    g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, str, "Warning");
                    MessageBox.Show(str, "提示：", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                else if(fi.Length > 1)
                {
                    string str = string.Format("在{0:s}下找到多个匹配的烧录档案，料号为：{1:s}，版本为：{2:s}\r\n", Mes.ProgFilePath, Mes.ItemCode, Mes.Version_File);
                    g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, str, "Warning");
                    MessageBox.Show(str, "提示：", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                else
                {
                    Mes.ProgFileName = Path.Combine(Mes.ProgFilePath, file);
                    string str = string.Format("匹配到一个烧录档案{0:s}\r\n", fi[0].Name);
                    g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, str, "Flow");
                    return true;
                }
            }
        }

        /// <summary>
        /// 获取烧录器数据
        /// </summary>
        public void GetBurnData()
        {
            byte[] byteResult = new byte[20000];
            if (AC_API.AC_GetStatus_Json(byteResult, 20000))
            {
                string str = Encoding.Default.GetString(byteResult);
                JObject obj = Newtonsoft.Json.Linq.JObject.Parse(str);
                BurnInfo.AllPass = obj["Pass"].ToString();
                BurnInfo.AllFail = obj["Fail"].ToString();
            }
            else
            {
                BurnInfo.AllPass = UserTask.OKDoneC.ToString();
                BurnInfo.AllFail = UserTask.NGDoneC.ToString();

                string str = "获取烧录器数据失败！！！";
                g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, str, "Error");
            }
        }

        /// <summary>
        /// 解析昂科烧录器产出信息
        /// </summary>
        public void AnalyseYieldChangeData(string MsgData)
        {
            string buffer = null;
            JObject obj = Newtonsoft.Json.Linq.JObject.Parse(MsgData);
            BurnInfo.SiteSN = obj["SiteSN"].ToString();
            BurnInfo.SiteAlias = obj["SiteAlias"].ToString();
            BurnInfo.Total = obj["Total"].ToString();
            BurnInfo.Fail = obj["Fail"].ToString();
            BurnInfo.Pass = obj["Pass"].ToString();
            BurnInfo.CurTotal = obj["CurTotal"].ToString();
            BurnInfo.CurFail = obj["CurFail"].ToString();
            BurnInfo.CurPass = obj["CurPass"].ToString();
            int index = Convert.ToInt32(BurnInfo.SiteAlias.Substring(BurnInfo.SiteAlias.Length - 1, 1));

            var result = obj["FailReason"];
            for (int i = 0; i < 8; i++)
            {
                BurnInfo.FailReason[i].SKTIDX = "0";
                BurnInfo.FailReason[i].ErrMsg = "0";
                BurnInfo.Group[index - 1].unit[i].Status = "OK";
            }
            if (result != null)
            {
                int length = result.Count();
                for (int i = 0; i < length; i++)
                {
                    string ind = obj["FailReason"][i]["SKTIDX"].ToString();
                    BurnInfo.FailReason[Convert.ToInt32(ind) - 1].SKTIDX = ind;
                    BurnInfo.FailReason[Convert.ToInt32(ind) - 1].ErrMsg = obj["FailReason"][i]["ErrMsg"].ToString();
                    BurnInfo.Group[index - 1].unit[Convert.ToInt32(ind) - 1].Status = "NG";
                }
            }
            Mes.DataValue_XC = string.Format("ACTION={0:s},WO={1:s},QTY={2:d},PRODUCT={3:s},ChipName={4:s},", Mes.LotSN,
                                                    Mes.BuildVersion, Mes.Count, Mes.DeviceSN, Mes.Device);
            for (int i = 0; i < 8; i++)
            {
                if (i < 7)
                {
                    buffer += string.Format("UID_{0:d}={1:s}:{2:s},", i + 1, BurnInfo.Group[index - 1].unit[i].ID,
                                                    BurnInfo.Group[index - 1].unit[i].Status);
                }
                else
                {
                    buffer += string.Format("UID_{0:d}={1:s}:{2:s},END", i + 1, BurnInfo.Group[index - 1].unit[i].ID,
                                                    BurnInfo.Group[index - 1].unit[i].Status);
                }
                
            }
            Mes.DataValue_XC += buffer;
            buffer = null;//清除数据缓存
        }

        /// <summary>
        /// 解析昂科烧录器UID信息
        /// </summary>
        public void AnalyseUIDFetchedData(string MsgData)
        {
            int groupIndex;
            JObject obj = Newtonsoft.Json.Linq.JObject.Parse(MsgData);
            BurnInfo.SiteSN = obj["SiteSN"].ToString();
            BurnInfo.SiteAlias = obj["SiteAlias"].ToString();
            groupIndex = Convert.ToInt32(BurnInfo.SiteAlias.Substring(BurnInfo.SiteAlias.Length - 1, 1));

            var result = obj["UIDInfo"];
            if (result != null)
            {
                int length = result.Count();
                for (int i = 0; i < length; i++)
                {
                    int ind = Convert.ToInt32(obj["UIDInfo"][i]["SKTIDX"].ToString());
                    BurnInfo.Group[groupIndex - 1].unit[ind - 1].ID = obj["UIDInfo"][i]["UID"].ToString();
                }
            }
        }

        /// <summary>
        /// 开启MES时重置昂科烧录器UID信息
        /// </summary>
        public void RstUID()
        {
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    BurnInfo.Group[i].unit[j].ID = "nonsupport";
                    BurnInfo.Group[i].unit[j].Status = "---";
                }
            }
        }

        /// <summary>
        /// 保存烧录测试信息到欣旺达MES
        /// </summary>
        public void SaveSmtBurnLog_XWD()
        {
            XmlDocument document = new XmlDocument();//创建XmlDocument对象
            XmlDeclaration declaration = document.CreateXmlDeclaration("1.0", "utf-8", "");//xml文档的声明部分
            document.AppendChild(declaration);

            XmlElement soap_root = document.CreateElement("soap", "Envelope", "http://schemas.xmlsoap.org/soap/envelope/");
            soap_root.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            soap_root.SetAttribute("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
            document.AppendChild(soap_root);

            XmlElement soap_elem = document.CreateElement("soap", "Body", "http://schemas.xmlsoap.org/soap/envelope/");
            soap_root.AppendChild(soap_elem);

            XmlElement root = document.CreateElement("SaveSmtBurnLog");
            root.SetAttribute("xmlns", "WWW.SUNWODA.COM");
            soap_elem.AppendChild(root);

            XmlElement devRoot = document.CreateElement("deviceTestInfo");
            root.AppendChild(devRoot);

            XmlElement elem;
            elem = document.CreateElement("LotSN");
            elem.InnerText = Mes.LotSN;
            devRoot.AppendChild(elem);

            elem = document.CreateElement("DeviceSN");
            elem.InnerText = Mes.DeviceSN;
            devRoot.AppendChild(elem);

            elem = document.CreateElement("DeviceDes");
            elem.InnerText = Mes.DeviceDes;
            devRoot.AppendChild(elem);

            elem = document.CreateElement("Device");
            elem.InnerText = Mes.Device;
            devRoot.AppendChild(elem);

            elem = document.CreateElement("BuildVersion");
            elem.InnerText = Mes.BuildVersion;
            devRoot.AppendChild(elem);

            elem = document.CreateElement("Version");
            elem.InnerText = Mes.Version;
            devRoot.AppendChild(elem);

            elem = document.CreateElement("Operator");
            elem.InnerText = StaticInfo.TDUser;
            devRoot.AppendChild(elem);

            elem = document.CreateElement("Description");
            elem.InnerText = Mes.MD5;
            devRoot.AppendChild(elem);

            XmlElement info_list = document.CreateElement("DetailsInfo");
            devRoot.AppendChild(info_list);

            string[] str_items = { "Version", "ProgSN", "ProgAlias", "Checksum", "EraseDone", "TotalPass", "TotalFail" };
            string[] str_items_value = {Mes.Version_File, BurnInfo.SiteSN, BurnInfo.SiteAlias, Mes.Checksum_File, "1",
                BurnInfo.AllPass, BurnInfo.AllFail};

            for (int i = 0; i < 7; i++)
            {
                XmlElement detail_elem = document.CreateElement("DeviceDetailsInfo");
                info_list.AppendChild(detail_elem);

                elem = document.CreateElement("ItemCode");
                elem.InnerText = str_items[i];
                detail_elem.AppendChild(elem);

                elem = document.CreateElement("Result");
                elem.InnerText = "";
                detail_elem.AppendChild(elem);

                elem = document.CreateElement("Value");
                elem.InnerText = str_items_value[i];
                detail_elem.AppendChild(elem);

                elem = document.CreateElement("Remark");
                elem.InnerText = "";
                detail_elem.AppendChild(elem);

                elem = document.CreateElement("Limit");
                elem.InnerText = "";
                detail_elem.AppendChild(elem);

                elem = document.CreateElement("Upper");
                elem.InnerText = "";
                detail_elem.AppendChild(elem);
            }

            ///烧录结果数据
            int site_num = Convert.ToInt32(BurnInfo.CurTotal);
            int fail_num = Convert.ToInt32(BurnInfo.CurFail);
            for (int i = 0; i < site_num; i++)
            {
                XmlElement detail_elem = document.CreateElement("DeviceDetailsInfo");
                info_list.AppendChild(detail_elem);
                int ind = i + 1;

                elem = document.CreateElement("ItemCode");
                string str_sp = string.Format("SKTPos{0:d}", ind);
                elem.InnerText = str_sp;
                detail_elem.AppendChild(elem);

                elem = document.CreateElement("Result");
                elem.InnerText = "";
                detail_elem.AppendChild(elem);

                elem = document.CreateElement("Value");
                int s_ind = Convert.ToInt32(BurnInfo.FailReason[i].SKTIDX) - 1;
                if (s_ind == i)
                {
                    elem.InnerText = BurnInfo.FailReason[i].ErrMsg;
                }
                else
                {
                    elem.InnerText = "Pass";
                }
                detail_elem.AppendChild(elem);

                elem = document.CreateElement("Remark");
                elem.InnerText = "";
                detail_elem.AppendChild(elem);

                elem = document.CreateElement("Limit");
                elem.InnerText = "";
                detail_elem.AppendChild(elem);

                elem = document.CreateElement("Upper");
                elem.InnerText = "";
                detail_elem.AppendChild(elem);
            }
            elem = document.CreateElement("Result");
            elem.InnerText = "true";
            devRoot.AppendChild(elem);

            elem = document.CreateElement("TestTime");
            elem.InnerText = DateTime.Now.ToString("yyyy-MM-dd");
            devRoot.AppendChild(elem);

            elem = document.CreateElement("EmpNo");
            elem.InnerText = StaticInfo.TDUser;
            devRoot.AppendChild(elem);

            string Url = string.Format("http://{0:s}:{1:s}/MESInterface.asmx", Mes.IP, Mes.Port);
            var client = new RestSharp.RestClient(Url);
            var requestPost = new RestRequest(Method.POST);
            requestPost.AddHeader("Content-Type", "text/xml");
            requestPost.AddParameter("text/xml", document.InnerXml, ParameterType.RequestBody);
            IRestResponse responsePost = client.Execute(requestPost);
            string strResponse = responsePost.Content;
            if (!responsePost.IsSuccessful)
            {
                
                string str = "保存烧录测试信息失败！！！\r\n错误信息：" + responsePost.ErrorMessage;
                g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, str, "Error");
                return;
            }
            else
            {
                if (!AnalyseLotSNResp_XWD(1, strResponse, "SaveSmtBurnLogResponse", "SaveSmtBurnLogResult"))
                {
                    return;
                }
                else
                {
                    string str = "保存烧录测试信息成功！！！";
                    g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, str, "Flow");
                }
            }
        }

        /// <summary>
        /// 保存烧录测试信息到快捷达MES
        /// </summary>
        public void SaveSmtBurnLog_KJD()
        {
            float temp;
            if (UserTask.OKAllC + UserTask.NGAllC > 0)
            {
                temp = (float)UserTask.OKAllC / (UserTask.OKAllC + UserTask.NGAllC);
            }
            else
            {
                temp = 0;
            }
            
            Mes.record_KJD.ErrorQuantity = Convert.ToInt32(BurnInfo.AllFail);//不良数量
            Mes.record_KJD.PassQuantity = Convert.ToInt32(BurnInfo.AllPass); //良品数量
            Mes.record_KJD.UPH = Efficiency.value;                           //每小时产能
            Mes.record_KJD.UpdateTime = DateTime.Now;                        //更新时间
            Mes.record_KJD.Yield = (temp * 100).ToString() + "%";            //烧录良品率
            Mes.record_KJD.UpdateBy = Mes.record_KJD.UpdateBy++;             //更新次数 

            if (Mes.Exit == GlobConstData.ST_MESEXIT_WITHOK)
            {
                if (Mes.record_KJD.PassQuantity >= Mes.Count)
                {
                    Mes.record_KJD.JobProductionEndTime = DateTime.Now;              //烧录结束时间
                }
            }
            else
            {
                if ((Mes.record_KJD.PassQuantity + Mes.record_KJD.ErrorQuantity) >= Mes.Count)
                {
                    Mes.record_KJD.JobProductionEndTime = DateTime.Now;              //烧录结束时间
                }
            }

            if (!Mes._dll_KJD.Save_Record(Mes.record_KJD, out string err))
            {
                string Msg = "更新数据到MES失败" + err;
                g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, Msg, "Error");
                return;
            }
            else
            {
                g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, "提交数据成功");
            }
        }

        /// <summary>
        /// 保存烧录测试信息到协创MES
        /// </summary>
        public void SaveSmtBurnLog_XC()
        {
            XmlDocument document = new XmlDocument();//创建XmlDocument对象
            XmlDeclaration declaration = document.CreateXmlDeclaration("1.0", "utf-8", "");//xml文档的声明部分
            document.AppendChild(declaration);

            XmlElement soap_root = document.CreateElement("soap", "Envelope", "http://schemas.xmlsoap.org/soap/envelope/");
            soap_root.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            soap_root.SetAttribute("xmlns:xsd", "http://www.w3.org/2001/XMLSchema");
            document.AppendChild(soap_root);

            XmlElement soap_elem = document.CreateElement("soap", "Body", "http://schemas.xmlsoap.org/soap/envelope/");
            soap_root.AppendChild(soap_elem);

            XmlElement root = document.CreateElement("AteCallMes");
            root.SetAttribute("xmlns", "http://tempuri.org/");
            soap_elem.AppendChild(root);

            XmlElement elem;
            elem = document.CreateElement("Line");
            elem.InnerText = "";
            root.AppendChild(elem);

            elem = document.CreateElement("MyGroup");
            elem.InnerText = "";
            root.AppendChild(elem);

            elem = document.CreateElement("DataValue");
            elem.InnerText = Mes.DataValue_XC;
            root.AppendChild(elem);

            elem = document.CreateElement("EMP");
            elem.InnerText = "";
            root.AppendChild(elem);

            string Url = string.Format("http://{0:s}/soap/mes.php", Mes.IP_XC);
            var client = new RestSharp.RestClient(Url);
            var requestPost = new RestRequest(Method.POST);
            requestPost.AddHeader("Content-Type", "text/xml");
            requestPost.AddParameter("text/xml", document.InnerXml, ParameterType.RequestBody);
            //requestPost.Timeout = 5000;//连接超时时间
            IRestResponse responsePost = client.Execute(requestPost);
            string strResponse = responsePost.Content;
            if (!responsePost.IsSuccessful)
            {
                string str = "保存烧录测试信息失败！！！\r\n错误信息：" + responsePost.ErrorMessage;
                g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, str, "Error");
                return;
            }
            else
            {
                string str = "保存烧录测试信息成功！！！" + Mes.DataValue_XC;
                g_act.GenMesLogMessage(GlobConstData.ST_LOG_PRINTANDRECORD, str, "Flow");
            }
        }
    }
}
