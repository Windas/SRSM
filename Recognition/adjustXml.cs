using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;         //引入xml操作的名称空间

namespace Recognition
{
    //该公有类用于操作Xml语法文件
    public class adjustXml
    {
        //创建xml文件对象，并初始化
        XmlDocument grammar = new XmlDocument();
        
        //用于添加子节点的函数，参数为添加的子节点内容
        public void addNode(string detail)
        {
            grammar.Load(".\\grammar.xml");    //读取xml文件
            XmlNode root = grammar.SelectSingleNode("GRAMMAR");    //查询根节点
            XmlNode fc =  root.FirstChild;      //查找root的第一个子节点
            XmlElement xe = grammar.CreateElement("O");     //创建节点O
            xe.InnerText = detail;        //给定该节点的内容
            fc.AppendChild(xe);         //将该节点连接到fc节点上作为其孩子节点
            grammar.Save(".\\grammar.xml");       //保存xml修改信息
        }

        public void delNode(string detail)
        {
        }
    }
}
