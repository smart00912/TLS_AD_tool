using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Schema;

namespace TLS_AD_tool
{
    class  dt2xml
    {
        // Xml结构的文件读到DataTable中
      public  static DataTable XmlToDataTableByFile()
        {
            string fileName = "c:\\adaccount.xml";
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);

            DataTable dt = new DataTable("song");
            //以第一个元素song的子元素建立表结构
            XmlNode songNode = doc.SelectSingleNode("/music/song[1]");
            string colName;
            if (songNode != null)
            {
                for (int i = 0; i < songNode.ChildNodes.Count; i++)
                {
                    colName = songNode.ChildNodes.Item(i).Name;
                    dt.Columns.Add(colName);
                }
            }
            DataSet ds = new DataSet("music");
            ds.Tables.Add(dt);

            //Xml所有song元素的子元素读到表song中,当然用dt也可以读。
            ds.ReadXml(fileName);
            return dt;
        }

        // Xml结构的字符中读到DataTable中
     public   static void XmlToDataTableByString()
        {
            string fileName = "c:\\adaccount.xml";
            XmlDocument doc = new XmlDocument();
            doc.Load(fileName);

            DataTable dt = new DataTable("song");
            //以第一个元素song的子元素建立表结构
            XmlNode songNode = doc.SelectSingleNode("/music/song[1]");
            string colName;
            if (songNode != null)
            {
                for (int i = 0; i < songNode.ChildNodes.Count; i++)
                {
                    colName = songNode.ChildNodes.Item(i).Name;
                    dt.Columns.Add(colName);
                }
            }
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);

            //获取Xml字串
            string xmlString = doc.InnerXml;
            StringReader sr = new StringReader(xmlString);
            XmlTextReader xr = new XmlTextReader(sr);
            //Xml所有song元素的子元素读到表song中,当然用dt也可以读。
            ds.ReadXml(xr);
        }

        // DataTable转换成Xml结构的文本
      public  static void DataTableToXml()
        {
            //dt的名为song,ds的名为music
            DataTable dt = XmlToDataTableByFile();

            //保存Xml验证架构
            dt.WriteXmlSchema("c:\\xmlsample.xsd");

            //dt写成Xml结构
            System.IO.TextWriter tw = new System.IO.StringWriter();
            dt.WriteXml(tw);
            string xml = tw.ToString();
        }

        //验证Xml结构
      public  static void VaildationXmlSchema()
        {
            XmlSchemaSet set = new XmlSchemaSet();
            set.Add("", "c:\\xmlsample.xsd");

            XmlDocument doc = new XmlDocument();
            doc.Schemas = set;

            try
            {
                doc.Load("c:\\adaccount.xml");
                doc.Validate(new ValidationEventHandler(Vaildation));
            }
            catch (Exception e)
            {
                Console.Write(e.Message);
            }
        }
      public  static void Vaildation(object sender, ValidationEventArgs e)
        {
            switch (e.Severity)
            {
                case XmlSeverityType.Error:
                    throw e.Exception;
                case XmlSeverityType.Warning:
                    throw e.Exception;
            }
        }
    }
}
