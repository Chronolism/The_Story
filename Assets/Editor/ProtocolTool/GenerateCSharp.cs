using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using UnityEngine;

public class GenerateCSharp
{
    //协议保存路径
    private string SAVE_PATH = Application.dataPath + "/Scripts/Net/Protocol/";

    //生成数据结构类
    public void GenerateData(XmlNodeList nodes)
    {
        string namespaceStr = "";
        string classNameStr = "";
        string fieldStr = "";
        string writingStr = "";
        string readingStr = "";

        foreach (XmlNode dataNode in nodes)
        {
            //命名空间
            namespaceStr = dataNode.Attributes["namespace"].Value;
            //类名
            classNameStr = dataNode.Attributes["name"].Value;
            //读取所有字段节点
            XmlNodeList fields = dataNode.SelectNodes("field");
            //通过这个方法进行成员变量声明的拼接 返回拼接结果
            fieldStr = GetFieldStr(fields);
            //通过某个方法 对Writing函数中的字符串内容进行拼接 返回结果
            writingStr = GetWritingStr(fields);
            //通过某个方法 对Reading函数中的字符串内容进行拼接 返回结果
            readingStr = GetReadingStr(fields);

            string dataStr = "using System;\r\n" +
                             "using System.Collections.Generic;\r\n" +
                             "using System.Text;\r\n" +
                             "using Mirror;\r\n" +
                             $"namespace {namespaceStr}\r\n" +
                              "{\r\n";
            if(dataNode.Attributes["type"].Value == "Inside")
            {
                dataStr += $"\tpublic class {classNameStr}\r\n" +
                            "\t{\r\n" +
                                  $"{fieldStr}" +
                            "\t}\r\n" +
                            "\r\n";
            }

            dataStr +=        $"\tpublic static class {classNameStr}ReadWrite\r\n" +
                              "\t{\r\n" +
                                    $"\t\tpublic static void Write{classNameStr}(this NetworkWriter writer, {classNameStr} value)\r\n" +
                                    "\t\t{\r\n" +
                                        $"{writingStr}" +
                                    "\t\t}\r\n" +
                                    $"\t\tpublic static {classNameStr} Read{classNameStr}(this NetworkReader reader)\r\n" +
                                    "\t\t{\r\n" +
                                        $"\t\t\t{classNameStr} value = new {classNameStr}();\r\n" +
                                        $"{readingStr}" +
                                        "\t\t\treturn value;\r\n" +
                                    "\t\t}\r\n" +
                              "\t}\r\n" +
                              "}";

            //保存为 脚本文件
            //保存文件的路径
            string path = SAVE_PATH + namespaceStr + "/DataReadWrite/";
            //如果不存在这个文件夹 则创建
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            //字符串保存 存储为枚举脚本文件
            File.WriteAllText(path + classNameStr + "ReadWrite.cs", dataStr);

        }
    }

    //生成消息类
    public void GenerateMsg(XmlNodeList nodes)
    {
        string namespaceStr = "";
        string classNameStr = "";
        string fieldStr = "";
        string msgOwn = "";
        string msgType = "";

        string registerStr = "";
        List<string> names = new List<string>();
        List<string> nameSpaces = new List<string>();

        foreach (XmlNode dataNode in nodes)
        {
            //命名空间
            namespaceStr = dataNode.Attributes["namespace"].Value;
            //记录所有命名空间
            if (!nameSpaces.Contains(namespaceStr))
                nameSpaces.Add(namespaceStr);
            //类名
            classNameStr = dataNode.Attributes["name"].Value;
            //记录所有类名辨别同名消息
            if (!names.Contains(classNameStr))
                names.Add(classNameStr);
            else
                Debug.LogError("存在同名的消息" + classNameStr + ",建议即使在不同命名空间中也不要有同名消息");
            //获取消息发送对象
            msgOwn = dataNode.Attributes["own"].Value;
            //获取消息类型
            msgType = dataNode.Attributes["type"].Value;
            //读取所有字段节点
            XmlNodeList fields = dataNode.SelectNodes("field");
            //通过这个方法进行成员变量声明的拼接 返回拼接结果
            fieldStr = GetFieldStr(fields);

            string dataStr = "using System;\r\n" +
                             "using System.Collections.Generic;\r\n" +
                             "using Mirror;\r\n" +
                             "using System.Text;\r\n" +
                             $"namespace {namespaceStr}\r\n" +
                              "{\r\n" +
                              $"\tpublic struct {classNameStr} : NetworkMessage\r\n" +
                              "\t{\r\n" +
                                    $"{fieldStr}" +
                              "\t}\r\n" +
                              "}";

            //保存为 脚本文件
            //保存文件的路径
            string path = SAVE_PATH + namespaceStr + "/Msg/";
            //如果不存在这个文件夹 则创建
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            //字符串保存 存储为枚举脚本文件
            File.WriteAllText(path + classNameStr + ".cs", dataStr);


            //生成处理器脚本
            //判断消息处理器脚本是否存在 如果存在 就不要覆盖了 避免把写过的逻辑处理代码覆盖了
            //如果想要改变 那就直接把没用的删了 它就会自动生成
            //如果不存在这个文件夹 则创建
            string handlerStr;
            if (msgOwn == "Server")
            {
                registerStr += $"\t\tNetworkServer.RegisterHandler<{classNameStr}>({classNameStr}Handler.MsgHandle);\r\n";
                if (!Directory.Exists(path + msgOwn + "/"))
                    Directory.CreateDirectory(path + msgOwn + "/");
                if (File.Exists(path + msgOwn + "/" + classNameStr + "Handler.cs"))
                    continue;
                string handlerContent = "";
                if (msgType != "Common")
                {
                    string msgName = dataNode.Attributes["msg"].Value;
                    if (msgName == "")
                    {
                        Debug.Log(classNameStr + "的回复消息为空");
                    }
                    switch (dataNode.Attributes["type"].Value)
                    {
                        case "Reply":
                            handlerContent = $"\t\t\t{msgName} replyMsg = new {msgName}();\r\n" +
                                             $"\t\t\t//在下面编辑消息处理内容;\r\n" +
                                             $"\t\t\t\r\n" +
                                             $"\t\t\tcon.Send(replyMsg);\r\n";
                            break;
                        case "Broadcast":
                            handlerContent = $"\t\t\t{msgName} broadcastMsg = new {msgName}();\r\n" +
                                             $"\t\t\t//在下面编辑消息处理内容;\r\n" +
                                             $"\t\t\t\r\n" +
                                             $"\t\t\tNetworkServer.SendToAll(broadcastMsg);;\r\n";
                            break;
                    }
                }
                handlerStr =       "using Mirror;\r\n" +
                                  $"namespace {namespaceStr}\r\n" +
                                    "{\r\n" +
                                        $"\tpublic static class {classNameStr}Handler\r\n" +
                                        "\t{\r\n" +
                                            $"\t\tpublic static void MsgHandle(NetworkConnectionToClient con, {classNameStr} msg ,int channelId)\r\n" +
                                            "\t\t{\r\n" +
                                                    handlerContent +
                                            "\t\t}\r\n" +
                                        "\t}\r\n" +
                                    "}\r\n";
            }
            else
            {
                registerStr += $"\t\tNetworkClient.RegisterHandler<{classNameStr}>({classNameStr}Handler.MsgHandle);\r\n";
                if (!Directory.Exists(path + msgOwn + "/"))
                    Directory.CreateDirectory(path + msgOwn + "/");
                if (File.Exists(path + msgOwn + "/" + classNameStr + "Handler.cs"))
                    continue;
                handlerStr = "using Mirror;\r\n" +
                                  $"namespace {namespaceStr}\r\n" +
                                    "{\r\n" +
                                        $"\tpublic static class {classNameStr}Handler\r\n" +
                                        "\t{\r\n" +
                                            $"\t\tpublic static void MsgHandle({classNameStr} msg)\r\n" +
                                            "\t\t{\r\n" +
                                            "\t\t\t\r\n" +
                                            "\t\t}\r\n" +
                                        "\t}\r\n" +
                                    "}\r\n";
            }
            //把消息处理器类的内容保存到本地
            File.WriteAllText(path + msgOwn + "/" + classNameStr + "Handler.cs", handlerStr);
            Debug.Log("消息处理器类生成结束");
        }
        Debug.Log("消息生成结束");

        //获取所有需要引用的命名空间 拼接好
        string nameSpacesStr = "";
        for (int i = 0; i < nameSpaces.Count; i++)
            nameSpacesStr += $"using {nameSpaces[i]};\r\n";
        //消息池对应的类的字符串信息
        string msgPoolStr = "using System;\r\n" +
                            "using System.Collections.Generic;\r\n" +
                            "using Mirror;\r\n" +
                            nameSpacesStr +
                            "public class MsgPool\r\n" +
                            "{\r\n" +
                                "\tpublic MsgPool()\r\n" +
                                "\t{\r\n" +
                                    registerStr +
                                "\t}\r\n" +
                            "}\r\n";

        string poolPath = SAVE_PATH + "/Pool/";
        if (!Directory.Exists(poolPath))
            Directory.CreateDirectory(poolPath);
        //保存到本地
        File.WriteAllText(poolPath + "MsgPool.cs", msgPoolStr);

        Debug.Log("消息池生成结束");
    }

    //生成消息池 主要就是ID和消息类型以及消息处理器类型的对应关系
    public void GenerateMsgPool(XmlNodeList nodes)
    {
        List<string> names = new List<string>();
        List<string> nameSpaces = new List<string>();

        foreach (XmlNode dataNode in nodes)
        {
            //记录所有消息的名字
            string name = dataNode.Attributes["name"].Value;
            if (!names.Contains(name))
                names.Add(name);
            else
                Debug.LogError("存在同名的消息" + name + ",建议即使在不同命名空间中也不要有同名消息");
            //记录所有消息的命名空间
            string msgNamespace = dataNode.Attributes["namespace"].Value;
            if (!nameSpaces.Contains(msgNamespace))
                nameSpaces.Add(msgNamespace);
        }

        //获取所有需要引用的命名空间 拼接好
        string nameSpacesStr = "";
        for (int i = 0; i < nameSpaces.Count; i++)
            nameSpacesStr += $"using {nameSpaces[i]};\r\n";
        //获取所有消息注册相关的内容
        string registerStr = "";
        for (int i = 0; i < names.Count; i++)
            registerStr += $"\t\tNetworkServer.RegisterHandler<{names[i]}>({names[i]}Handler.MsgHandle);\r\n";

        //消息池对应的类的字符串信息
        string msgPoolStr = "using System;\r\n" +
                            "using System.Collections.Generic;\r\n" +
                            "using Mirror;\r\n" +
                            nameSpacesStr +
                            "public class MsgPool\r\n" +
                            "{\r\n" +
                                "\tpublic MsgPool()\r\n" +
                                "\t{\r\n" +
                                    registerStr +
                                "\t}\r\n" +
                            "}\r\n";

        string path = SAVE_PATH + "/Pool/";
        if (!Directory.Exists(path))
            Directory.CreateDirectory(path);
        //保存到本地
        File.WriteAllText(path + "MsgPool.cs", msgPoolStr);

        Debug.Log("消息池生成结束");
    }

    /// <summary>
    /// 获取成员变量声明内容
    /// </summary>
    /// <param name="fields"></param>
    /// <returns></returns>
    private string GetFieldStr(XmlNodeList fields)
    {
        string fieldStr = "";
        foreach (XmlNode field in fields)
        {
            //变量类型
            string type = field.Attributes["type"].Value;
            //变量名
            string fieldName = field.Attributes["name"].Value;
            if(type == "list")
            {
                string T = field.Attributes["T"].Value;
                fieldStr += "\t\tpublic List<" + T + "> ";
            }
            else if(type == "array")
            {
                string data = field.Attributes["data"].Value;
                fieldStr += "\t\tpublic " + data + "[] ";
            }
            else if(type == "dic")
            {
                string Tkey = field.Attributes["Tkey"].Value;
                string Tvalue = field.Attributes["Tvalue"].Value;
                fieldStr += "\t\tpublic Dictionary<" + Tkey +  ", " + Tvalue + "> ";
            }
            else if(type == "enum")
            {
                string data = field.Attributes["data"].Value;
                fieldStr += "\t\tpublic " + data + " ";
            }
            else
            {
                fieldStr += "\t\tpublic " + type + " ";
            }

            fieldStr += fieldName + ";\r\n";
        }
        return fieldStr;
    }


    //拼接 Writing函数的方法
    private string GetWritingStr(XmlNodeList fields)
    {
        string writingStr = "";

        string type = "";
        string name = "";
        foreach (XmlNode field in fields)
        {
            type = field.Attributes["type"].Value;
            name = field.Attributes["name"].Value;
            if (type == "list")
            {
                string T = field.Attributes["T"].Value;
                writingStr += "\t\t\twriter.Write((short)value." + name + ".Count);\r\n";
                writingStr += "\t\t\tfor (int i = 0; i < value." + name + ".Count; ++i)\r\n";
                writingStr += "\t\t\t\t" + GetFieldWritingStr(T, name + "[i]") + "\r\n";
            }
            else if (type == "array")
            {
                string data = field.Attributes["data"].Value;
                writingStr += "\t\t\twriter.Write((short)value." + name + ".Length);\r\n";
                writingStr += "\t\t\tfor (int i = 0; i < value." + name + ".Length; ++i)\r\n";
                writingStr += "\t\t\t\t" + GetFieldWritingStr(data, name + "[i]") + "\r\n";
            }
            else if (type == "dic")
            {
                string Tkey = field.Attributes["Tkey"].Value;
                string Tvalue = field.Attributes["Tvalue"].Value;
                writingStr += "\t\t\twriter.Write((short)value." + name + ".Count);\r\n";
                writingStr += "\t\t\tforeach (" + Tkey + " key in value." + name + ".Keys)\r\n";
                writingStr += "\t\t\t{\r\n";
                writingStr += "\t\t\t\t" + GetFieldWritingStrSingle(Tkey, "key") + "\r\n";
                writingStr += "\t\t\t\t" + GetFieldWritingStr(Tvalue, name + "[key]") + "\r\n";
                writingStr += "\t\t\t}\r\n";
            }
            else
            {
                writingStr += "\t\t\t" + GetFieldWritingStr(type, name) + "\r\n";
            }
        }
        return writingStr;
    }

    private string GetFieldWritingStr(string type, string name)
    {
        switch (type)
        {
            case "byte":
                return "writer.Write(value." + name +");";
            case "int":
                return "writer.Write(value." + name + ");";
            case "short":
                return "writer.Write(value." + name + ");";
            case "long":
                return "writer.Write(value." + name + ");";
            case "float":
                return "writer.Write(value." + name + ");";
            case "bool":
                return "writer.Write(value." + name + ");";
            case "string":
                return "writer.Write(value." + name + ");";
            case "enum":
                return "writer.Write((int)value." + name + ");";
            default:
                return "writer.Write" + type + "(value." + name + ");";
        }
    }

    private string GetFieldWritingStrSingle(string type, string name)
    {
        switch (type)
        {
            case "byte":
                return "writer.Write(" + name + ");";
            case "int":
                return "writer.Write(" + name + ");";
            case "short":
                return "writer.Write(" + name + ");";
            case "long":
                return "writer.Write(" + name + ");";
            case "float":
                return "writer.Write(" + name + ");";
            case "bool":
                return "writer.Write(" + name + ");";
            case "string":
                return "writer.Write(" + name + ");";
            case "enum":
                return "writer.Write((int)" + name + ");";
            default:
                return "writer.Write" + type + "(" + name + ");";
        }
    }

    private string GetReadingStr(XmlNodeList fields)
    {
        string readingStr = "";

        string type = "";
        string name = "";
        foreach (XmlNode field in fields)
        {
            type = field.Attributes["type"].Value;
            name = field.Attributes["name"].Value;
            if (type == "list")
            {
                string T = field.Attributes["T"].Value;
                readingStr += "\t\t\tvalue." + name + " = new List<" + T + ">();\r\n";
                readingStr += "\t\t\tshort " + name + "Count = reader.ReadShort();\r\n";
                readingStr += "\t\t\tfor (int i = 0; i < " + name + "Count; ++i)\r\n";
                readingStr += "\t\t\t\tvalue." + name + ".Add(" + GetFieldReadingStr(T) + ");\r\n";
            }
            else if (type == "array")
            {
                string data = field.Attributes["data"].Value;
                readingStr += "\t\t\tshort " + name + "Length = reader.ReadShort();\r\n";
                readingStr += "\t\t\tvalue." + name + " = new " + data + "[" + name + "Length];\r\n";
                readingStr += "\t\t\tfor (int i = 0; i < value." + name + ".Length; ++i)\r\n";
                readingStr += "\t\t\t\tvalue." + name + "[i] = " + GetFieldReadingStr(data) + ";\r\n";
            }
            else if (type == "dic")
            {
                string Tkey = field.Attributes["Tkey"].Value;
                string Tvalue = field.Attributes["Tvalue"].Value;
                readingStr += "\t\t\tvalue." + name + " = new Dictionary<" + Tkey + ", " + Tvalue + ">();\r\n";
                readingStr += "\t\t\tshort " + name + "Count = reader.ReadShort();\r\n";
                readingStr += "\t\t\tfor (int i = 0; i < " + name + "Count; ++i)\r\n";
                readingStr += "\t\t\t\tvalue." + name + ".Add(" + GetFieldReadingStr(Tkey) + ", " +
                                                            GetFieldReadingStr(Tvalue) + ");\r\n";
            }
            else if (type == "enum")
            {
                string data = field.Attributes["data"].Value;
                readingStr += "\t\t\tvalue." + name + " = (" + data + ")reader.ReadInt();\r\n";
            }
            else
                readingStr += "\t\t\tvalue." + name + " = " + GetFieldReadingStr(type) + ";\r\n";
        }

        return readingStr;
    }

    private string GetFieldReadingStr(string type)
    {
        switch (type)
        {
            case "byte":
                return "reader.ReadByte()";
            case "int":
                return "reader.ReadInt()";
            case "short":
                return "reader.ReadShort()";
            case "long":
                return "reader.ReadLong()";
            case "float":
                return "reader.ReadFloat()";
            case "bool":
                return "reader.ReadBool()";
            case "string":
                return "reader.ReadString()";
            default:
                return "reader.Read" + type + "()";
        }
    }
}
