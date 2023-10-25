using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(BuffDetileAttribute))]
public class BuffDetileDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //var buffDetile = attribute as BuffDetileAttribute;

        var list = BuffDetileAttribute.AllBuff();

        if (property.propertyType == SerializedPropertyType.Integer)
        {
            int index = 0;
            int value = property.intValue;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].id == value)
                {
                    index = i;
                }
            }
            index = EditorGUI.Popup(position, property.displayName, index, BuffDetileAttribute.AllBuffNames());

            property.intValue = list[index].id;
        }
        else
        {
            base.OnGUI(position, property, label);
        }
    }
}
#endif