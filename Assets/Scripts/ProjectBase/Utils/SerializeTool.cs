using System;
using System.Text;

public class SerializeTool 
{
    /// <summary>
    /// 对象序列化
    /// </summary>
    /// <param name="data">序列化数据</param>
    /// <param name="obj">对象</param>
    /// <param name="index">数据位置指针</param>
    /// <returns></returns>
    public static bool SerializeObject(byte[] data, object obj, ref int index)
    {

        if (obj != null)
        {
            if (obj is string str)
            {
                byte[] strBytes = Encoding.UTF8.GetBytes(str);
                BitConverter.GetBytes((int)obj).CopyTo(data, index);
                index += sizeof(int);
                strBytes.CopyTo(data, index);
                index += strBytes.Length;
            }
            else if (obj is byte by)
            {
                data[index] = by;
                index += sizeof(byte);
            }
            else if (obj is bool b)
            {
                BitConverter.GetBytes(b).CopyTo(data, index);
                index += sizeof(bool);
            }
            else if (obj is short s)
            {
                BitConverter.GetBytes(s).CopyTo(data, index);
                index += sizeof(short);
            }
            else if (obj is int)
            {
                BitConverter.GetBytes((int)obj).CopyTo(data, index);
                index += sizeof(int);
            }
            else if (obj is long l)
            {
                BitConverter.GetBytes(l).CopyTo(data, index);
                index += sizeof(long);
            }
            else if (obj is float f)
            {
                BitConverter.GetBytes(f).CopyTo(data, index);
                index += sizeof(float);
            }
            else if (obj is double d)
            {
                BitConverter.GetBytes(d).CopyTo(data, index);
                index += sizeof(double);
            }
            else
            {
                return false;
            }
            return true;
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// 对象反序列化
    /// </summary>
    /// <param name="data">数据</param>
    /// <param name="type">类型</param>
    /// <param name="index">数据位置指针</param>
    /// <param name="obj">返回对象</param>
    /// <returns></returns>
    public static bool DeserializeObject(byte[] data, Type type, ref int index, out object obj)
    {
        if (type.Equals(typeof(string)))
        {
            int lenth = BitConverter.ToInt32(data, index);
            index += sizeof(int);
            index += lenth;
            obj = BitConverter.ToString(data, index, lenth);
            return true;

        }
        else if (type.Equals(typeof(byte)))
        {
            obj = data[index];
            index += sizeof(byte);
            return true;
        }
        else if (type.Equals(typeof(bool)))
        {
            obj = BitConverter.ToBoolean(data, index);
            index += sizeof(bool);
            return true;
        }
        else if (type.Equals(typeof(short)))
        {
            obj = BitConverter.ToInt16(data, index);
            index += sizeof(short);
            return true;
        }
        else if (type.Equals(typeof(int)))
        {
            obj = BitConverter.ToInt32(data, index);
            index += sizeof(int);
            return true;
        }
        else if (type.Equals(typeof(long)))
        {
            obj = BitConverter.ToInt64(data, index);
            index += sizeof(long);
            return true;
        }
        else if (type.Equals(typeof(float)))
        {
            obj = BitConverter.ToSingle(data, index);
            index += sizeof(float);
            return true;
        }
        else if (type.Equals(typeof(double)))
        {
            obj = BitConverter.ToDouble(data, index);
            index += sizeof(double);
            return true;
        }
        else
        {
            obj = null;
            return false;
        }
    }

}
