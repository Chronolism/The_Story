using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UIElements;

public class NetWorkAttribute : Attribute { }

public class NetworkBehaviour : MonoBehaviour
{
    //实体唯一id
    public int unitId;
    //实体资源数据唯一id
    public int assetId;
    [NetWork]
    public int UnitId { get { return unitId; }set { unitId = value; }}
    [NetWork]
    public int AssetId { get { return assetId; } set { assetId = value; } }

    //属性列表（内存换速率）
    Dictionary<string, List<PropertyInfo>> propertyInfosDic = new Dictionary<string, List<PropertyInfo>>();
    //发送数据（内存换GC频率）
    byte[] bytes = new byte[1024 * 150];
    int index = 0;

    /// <summary>
    /// 发送属性获取
    /// </summary>
    /// <param name="type"></param>
    private void GetProperty(Type type)
    {
        string typeName = type.Name;
        if (propertyInfosDic.ContainsKey(typeName))
        {
            return;
        }
        List<PropertyInfo> piList;
        piList = new List<PropertyInfo>();
        propertyInfosDic.Add(typeName, piList);
        PropertyInfo[] pi;
        pi = type.GetProperties();
        foreach (PropertyInfo pi2 in pi)
        {
            if (pi2.GetCustomAttribute<NetWorkAttribute>() != null)
            {
                piList.Add(pi2);
                if (pi2.GetValue(this).GetType().IsClass)
                {
                    GetProperty(pi2.GetValue(this).GetType());
                }
            };
        }

    }

    /// <summary>
    /// 同步数据发送
    /// </summary>
    /// <returns></returns>
    public byte[] SendObj()
    {
        index = 0;
        SerializeObject(this);
        byte[] buffer = new byte[index];
        Array.Copy(bytes, buffer, index);
        return buffer;
    }
    /// <summary>
    /// 同步数据接收
    /// </summary>
    /// <param name="data"></param>
    public void ReceiveObj(byte[] data)
    {
        int index = 0;
        Deserialize(this, data,ref index);
    }

    /// <summary>
    /// 序列化
    /// </summary>
    /// <param name="obj">对象</param>
    private void SerializeObject(object obj)
    {
        List<PropertyInfo> piList;
        if (!propertyInfosDic.TryGetValue(obj.GetType().Name, out piList))
        {
            GetProperty(obj.GetType());
            piList = propertyInfosDic[obj.GetType().Name];
        }
        Debug.Log(piList.Count);
        foreach (PropertyInfo field in piList)
        {
            Handle(field.GetValue(this));
        }
         
        void Handle(object obj)
        {
            if (!SerializeTool.SerializeObject(bytes, obj, ref index))
            {
                if (obj is IList list)
                {
                    BitConverter.GetBytes(list.Count).CopyTo(bytes, index);
                    index += sizeof(int);
                    foreach (object obj2 in list)
                    {
                        Handle(obj2);
                    }
                }
                else if (obj.GetType().IsClass)
                {
                    SerializeObject(obj);
                }
            }
        }
    }
    /// <summary>
    /// 反序列化
    /// </summary>
    /// <param name="obj">对象</param>
    /// <param name="data">数据</param>
    /// <param name="index">数据位置指针</param>
    private void Deserialize(object obj , byte[] data , ref int index)
    {
        List<PropertyInfo> piList;
        if (!propertyInfosDic.TryGetValue(obj.GetType().Name, out piList))
        {
            GetProperty(obj.GetType());
            piList = propertyInfosDic[obj.GetType().Name];
        }

        foreach (PropertyInfo propertyInfo in piList)
        {

            if (propertyInfo.PropertyType.Equals(typeof(string)))
            {
                int lenth = BitConverter.ToInt32(data, index);
                index += sizeof(int);
                propertyInfo.SetValue(obj, BitConverter.ToString(data, index, lenth));
                index += lenth;
            }
            else if (propertyInfo.PropertyType.Equals(typeof(byte)))
            {
                propertyInfo.SetValue(obj, data[index]);
                index += sizeof(byte);
            }
            else if (propertyInfo.PropertyType.Equals(typeof(bool)))
            {
                propertyInfo.SetValue(obj, BitConverter.ToBoolean(data, index));
                index += sizeof(bool);
            }
            else if (propertyInfo.PropertyType.Equals(typeof(short)))
            {
                propertyInfo.SetValue(obj, BitConverter.ToInt16(data, index));
                index += sizeof(short);
            }
            else if (propertyInfo.PropertyType.Equals(typeof(int)))
            {
                propertyInfo.SetValue(obj, BitConverter.ToInt32(data, index));
                index += sizeof(int);
            }
            else if (propertyInfo.PropertyType.Equals(typeof(long)))
            {
                propertyInfo.SetValue(obj, BitConverter.ToInt64(data, index));
                index += sizeof(long);
            }
            else if (propertyInfo.PropertyType.Equals(typeof(float)))
            {
                propertyInfo.SetValue(obj, BitConverter.ToSingle(data, index));
                index += sizeof(float);
            }
            else if (propertyInfo.PropertyType.Equals(typeof(double)))
            {
                propertyInfo.SetValue(obj, BitConverter.ToDouble(data, index));
                index += sizeof(double);
            }
            else
            {
                object obj2 = propertyInfo.GetValue(obj);
                if (propertyInfo.PropertyType.IsArray)
                {
                    int length = BitConverter.ToInt32(data, index);
                    index += sizeof(int);
                    Type El_type = propertyInfo.PropertyType.GetElementType();
                    Array ilist = Array.CreateInstance(El_type, length);

                    for (int i = 0; i < length; i++)
                    {
                        if (SerializeTool.DeserializeObject(data, El_type, ref index, out object obj3))
                        {
                            ilist.SetValue(obj3, i);
                        }
                        else
                        {
                            object obj4 = Activator.CreateInstance(El_type);
                            Deserialize(obj4, data, ref index);
                            ilist.SetValue(obj4, i);
                        }
                    }
                    propertyInfo.SetValue(obj, ilist);
                }
                else if (propertyInfo.PropertyType.IsGenericType)
                {
                    int length = BitConverter.ToInt32(data, index);
                    index += sizeof(int);

                    IList ilist = Activator.CreateInstance(obj2.GetType()) as IList;

                    for (int i = 0; i < length; i++)
                    {
                        if (SerializeTool.DeserializeObject(data, propertyInfo.PropertyType.GetGenericArguments()[0], ref index, out object obj3))
                        {
                            ilist.Add(obj3);
                        }
                        else
                        {
                            object obj4 = Activator.CreateInstance(propertyInfo.PropertyType.GetGenericArguments()[0]);
                            Deserialize(obj4, data, ref index);
                            ilist.Add(obj4);
                        }
                    }
                    propertyInfo.SetValue(obj, ilist);
                }
                else if (obj2.GetType().IsClass)
                {
                    Deserialize(obj2, data, ref index);
                }
            }
        }
        
    }

    //private void Handle(byte[] data,PropertyInfo propertyInfo, object obj , ref int index)
    //{

    //}

    
}
