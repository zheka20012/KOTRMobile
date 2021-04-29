﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using KOTRLibrary;
using KOTRLibrary.B3DNodes;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;


public class KOTRLibraryTest : EditorWindow
{
    [MenuItem("KOTR/Run Test")]
    static void OpenWindow()
    {
        EditorWindow window = GetWindow(typeof(KOTRLibraryTest), false, "Test");

        window.Show();
    }

    private class EditorBlockUI
    {
        private BaseNode Node;

        private List<EditorBlockUI> ChildNodes;

        private bool IsOpened;
        private bool IsShownValues;

        public EditorBlockUI(BaseNode node)
        {
            if (node.GetType().IsSubclassOf(typeof(BaseGroupNode)))
            {
                BaseGroupNode childRoot = node as BaseGroupNode;

                ChildNodes = new List<EditorBlockUI>();

                for (int i = 0; i < childRoot.ChildNodes.Count; i++)
                {
                    ChildNodes.Add(new EditorBlockUI(childRoot.ChildNodes[i]));
                }
            }

            Node = node;

            var values = Node.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            for (int i = 0; i < values.Length; i++)
            {
                ArraysFoldouts.Add(false);
            }
        }

        public void OnGUI()
        {
            float viewWidth = EditorGUIUtility.currentViewWidth-30;

            EditorGUILayout.BeginVertical(GUILayout.MaxWidth(viewWidth));
            if (Node.GetType().IsSubclassOf(typeof(BaseGroupNode)))
            {
                EditorGUILayout.BeginHorizontal();
                IsOpened = EditorGUILayout.Foldout(IsOpened, $"{Node.Name} ({Node.GetType().Name})");
                IsShownValues = EditorGUILayout.ToggleLeft("Show Data", IsShownValues, GUILayout.MaxWidth(viewWidth/4));
                EditorGUILayout.EndHorizontal();

                if (IsShownValues)
                {
                    ShowData();
                }

                if (IsOpened)
                {
                    EditorGUI.indentLevel++;

                    EditorGUILayout.LabelField($"Size: {ChildNodes.Count}", EditorStyles.boldLabel);

                    for (int i = 0; i < ChildNodes.Count; i++)
                    {
                        ChildNodes[i].OnGUI();
                    }


                    EditorGUI.indentLevel--;
                }
            }
            else
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"{Node.Name} ({Node.GetType().Name})", GUILayout.MaxWidth(viewWidth * 3/4));
                IsShownValues = EditorGUILayout.ToggleLeft("Show Data", IsShownValues, GUILayout.MaxWidth(viewWidth / 4));
                EditorGUILayout.EndHorizontal();

                if (IsShownValues)
                {
                    ShowData();
                }
            }


            EditorGUILayout.EndVertical();
        }

        private List<bool> ArraysFoldouts = new List<bool>();

        protected void ShowData()
        {
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("Block Data:");
            var values = Node.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            for (int i = 0; i < values.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField($"{values[i].Name} ", EditorStyles.boldLabel);

                if (values[i].PropertyType.IsArray)
                {
                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.BeginHorizontal();
                    ArraysFoldouts[i] =  EditorGUILayout.Foldout(ArraysFoldouts[i], $"{values[i].GetValue(Node).ToString()}");
                    EditorGUILayout.EndHorizontal();

                    if (ArraysFoldouts[i])
                    {
                        Array array = (Array) values[i].GetValue(Node);
                        for (int j = 0; j < array.Length; j++)
                        {
                            EditorGUILayout.LabelField(array.GetValue(i).ToString());
                        }
                    }
                    EditorGUILayout.EndVertical();
                }
                else if (values[i].PropertyType.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IList<>)))
                {
                    EditorGUILayout.BeginVertical();
                    EditorGUILayout.BeginHorizontal();
                    ArraysFoldouts[i] = EditorGUILayout.Foldout(ArraysFoldouts[i], $"{values[i].GetValue(Node).ToString()}");
                    EditorGUILayout.EndHorizontal();

                    if (ArraysFoldouts[i])
                    {
                        var array = (IEnumerable)values[i].GetValue(Node, null);
                        foreach (var o in array)
                        {
                            EditorGUILayout.LabelField(o.ToString());
                        }
                    }
                    EditorGUILayout.EndVertical();
                }
                else
                {
                    EditorGUILayout.LabelField($"{values[i].GetValue(Node).ToString()}");
                }

                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }
    }

    private static string FilePath = String.Empty;
    private B3DFile file;

    private bool MaterialsOpened = false;
    private Vector2 scrollPos = Vector2.zero;

    private bool DataOpened = false;
    private Vector2 scrollData = Vector2.zero;

    private List<EditorBlockUI> BlockUis;

    private void OnGUI()
    {
        GUILayout.BeginHorizontal();
        FilePath = EditorGUILayout.TextField(FilePath);
        if (GUILayout.Button("..."))
        {
            FilePath = EditorUtility.OpenFilePanel("Open b3d file!",
                string.IsNullOrEmpty(FilePath) ? Application.dataPath : FilePath, "b3d");
        }
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Load b3d!"))
        {
            file = B3DFile.OpenFile(FilePath);

            BlockUis = new List<EditorBlockUI>();

            for (int i = 0; i < file.Nodes.Count; i++)
            {
                BlockUis.Add(new EditorBlockUI(file.Nodes[i]));
            }
        }

        if (file == null || BlockUis == null)
        {
            EditorGUILayout.LabelField("b3d file not loaded!");
            return;
        }

        MaterialsOpened = EditorGUILayout.Foldout(MaterialsOpened, "Materials");

        if (MaterialsOpened)
        {
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            EditorGUILayout.BeginVertical();
            for (int i = 0; i < file.MaterialLibrary.Count; i++)
            {
                EditorGUILayout.LabelField(file.MaterialLibrary[i]);
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }

        DataOpened = EditorGUILayout.Foldout(DataOpened, "Data");

        if (DataOpened)
        {
            scrollData = EditorGUILayout.BeginScrollView(scrollData);
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField($"Size: {file.Nodes.Count}", EditorStyles.boldLabel);
            for (int i = 0; i < BlockUis.Count; i++)
            {
                BlockUis[i].OnGUI();
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndScrollView();
        }
    }


}