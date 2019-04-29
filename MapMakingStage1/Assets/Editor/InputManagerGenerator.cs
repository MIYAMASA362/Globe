﻿using UnityEngine;
using UnityEditor;
using System;
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;

public class InputManagerGenerator : AssetPostprocessor
{
    private enum AxisType
    {
        KeyOrMouseButton = 0,
        MouseMovement = 1,
        JoystickAxis = 2
    };


    private class InputAxis
    {
        public string name;
        public string descriptiveName;
        public string descriptiveNegativeName;
        public string negativeButton;
        public string positiveButton;
        public string altNegativeButton;
        public string altPositiveButton;
        public float gravity;
        public float dead;
        public float sensitivity;
        public bool snap = false;
        public bool invert = false;
        public AxisType type;
        public int axis;
        public int joyNum;
    };

    // InputManagerクラステンプレート
    private const string TEMPLATE = @"// This file is generated by InputManagerGenerator.
using UnityEngine;
using System.Collections;
 
public static class InputManager {{

    public static int Num = {0};

    //Input Name String
    {1}

    public enum AxisType {{
        KeyOrMouseButton = 0,
        MouseMovement = 1,
        JoystickAxis = 2
    }};

    public class InputAxis {{
        public string name;
        public string descriptiveName;
        public string descriptiveNegativeName;
        public string negativeButton;
        public string positiveButton;
        public string altNegativeButton;
        public string altPositiveButton;
        public float gravity;
        public float dead;
        public float sensitivity;
        public bool snap = false;
        public bool invert = false;
        public AxisType type;
        public int axis;
        public int joyNum;
    }};
 
    public static InputAxis[] Config = new InputAxis[] {{
{2}    }};

}}
";
    // Axis定義
    private const string INPUT_AXIS_ELEM = @"        
        new InputAxis {{
            name = ""{0}"",
            descriptiveName = ""{1}"",
            descriptiveNegativeName = ""{2}"",
            negativeButton = ""{3}"",
            positiveButton = ""{4}"",
            altNegativeButton = ""{5}"",
            altPositiveButton = ""{6}"",
            gravity = {7}f,
            dead = {8}f,
            sensitivity = {9}f,
            snap = {10},
            invert = {11},
            type = AxisType.{12},
            axis = {13},
            joyNum = {14},
        }},
";

    private const string INPUT_NAME = @"
    public const string {0} = ""{1}"";";

    private static SerializedProperty GetChildProperty(SerializedProperty parent, string name)
    {
        SerializedProperty child = parent.Copy();
        child.Next(true);
        do
        {
            if (child.name == name) return child;
        }
        while (child.Next(false));

        return null;
    }

    static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
    {
                // InputManagerの変更チェック
        var inputManagerPath = Array.Find(importedAssets, path => Path.GetFileName(path) == "InputManager.asset");
        if ( inputManagerPath == null ) {
            return;
        }

        Debug.Log(Application.dataPath);

        // InputManagerの設定情報読み込み
        var serializedObject = new SerializedObject(AssetDatabase.LoadAllAssetsAtPath(inputManagerPath)[0]);
        var axesProperty = serializedObject.FindProperty("m_Axes");
 
        var axes = new InputAxis[axesProperty.arraySize];
        
        for ( int i = 0 ; i < axesProperty.arraySize ; ++i ) {
            var axis = new InputAxis();
            var axisProperty = axesProperty.GetArrayElementAtIndex(i);
 
            axis.name                       = GetChildProperty(axisProperty, "m_Name").stringValue;
            axis.descriptiveName            = GetChildProperty(axisProperty, "descriptiveName").stringValue;
            axis.descriptiveNegativeName    = GetChildProperty(axisProperty, "descriptiveNegativeName").stringValue;
            axis.negativeButton             = GetChildProperty(axisProperty, "negativeButton").stringValue;
            axis.positiveButton             = GetChildProperty(axisProperty, "positiveButton").stringValue;
            axis.altNegativeButton          = GetChildProperty(axisProperty, "altNegativeButton").stringValue;
            axis.altPositiveButton          = GetChildProperty(axisProperty, "altPositiveButton").stringValue;
            axis.gravity                    = GetChildProperty(axisProperty, "gravity").floatValue;
            axis.dead                       = GetChildProperty(axisProperty, "dead").floatValue;
            axis.sensitivity                = GetChildProperty(axisProperty, "sensitivity").floatValue;
            axis.snap                       = GetChildProperty(axisProperty, "snap").boolValue;
            axis.invert                     = GetChildProperty(axisProperty, "invert").boolValue;
            axis.type                       = (AxisType)GetChildProperty(axisProperty, "type").intValue;
            axis.axis                       = GetChildProperty(axisProperty, "axis").intValue;
            axis.joyNum                     = GetChildProperty(axisProperty, "joyNum").intValue;
 
            axes[i] = axis;
        }

        // InputManagerクラスを生成
        string inputAxis = "";
        string NameIndex = "";

        foreach (var axis in axes)
        {
            inputAxis += string.Format(INPUT_AXIS_ELEM,
                axis.name,
                axis.descriptiveName,
                axis.descriptiveNegativeName,
                axis.negativeButton,
                axis.positiveButton,
                axis.altNegativeButton,
                axis.altPositiveButton,
                axis.gravity.ToString(),
                axis.dead.ToString(),
                axis.sensitivity.ToString(),
                axis.snap.ToString().ToLower(),
                axis.invert.ToString().ToLower(),
                axis.type.ToString(),
                axis.axis.ToString(),
                axis.joyNum.ToString()
            );

            string name = String.Format(INPUT_NAME,axis.name.Replace(" ","_"),axis.name);

            //被りをなくす
            NameIndex = NameIndex.Replace(name, "");
            NameIndex += name;
            
        }

        var result = string.Format(TEMPLATE, axesProperty.arraySize, NameIndex,inputAxis);

        // ファイルに保存
        var inputManagerResult = Application.dataPath + "/Features/Scritps/InputManager.cs";
        var sr = new StreamWriter(inputManagerResult);
        sr.Write(result);
        sr.Close();

        // 生成したInputManager.csをインポート
        AssetDatabase.ImportAsset(inputManagerResult, ImportAssetOptions.ForceUpdate);
    }
}