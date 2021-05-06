using System.Collections.Generic;
using System.IO;
using System.Reflection;
using KOTRLibrary;
using UnityEditor;
using UnityEngine;

namespace World.Editor
{
    public class RESFileTest : EditorWindow, ISerializationCallbackReceiver
    {
        [MenuItem("KOTR/Run RES Test")]
        private static void ShowWindow()
        {
            var window = GetWindow<RESFileTest>();
            window.titleContent = new GUIContent("RES Tester");
            window.Show();
        }

        private static string FilePath;
        private RESFile file;
        private bool IsOpen = false;

        private List<PaletteGUI> PaletteGuis = new List<PaletteGUI>();

        private void OnGUI()
        {
            GUILayout.BeginHorizontal();
            FilePath = EditorGUILayout.TextField(FilePath);
            if (GUILayout.Button("..."))
            {
                FilePath = EditorUtility.OpenFilePanel("Open RES file!",
                    string.IsNullOrEmpty(FilePath) ? Application.dataPath : FilePath, "res");


            }

            GUILayout.EndHorizontal();

            if (GUILayout.Button("Load RES!"))
            {
                file = RESFile.OpenFile(FilePath);

                PaletteGuis.Clear();

                for (int i = 0; i < file.Palettes.Count; i++)
                {
                    PaletteGuis.Add(new PaletteGUI(file.Palettes[i]));
                }

                IsOpen = true;
            }

            if (file == null && !IsOpen)
            {
                EditorGUILayout.LabelField("RES file not loaded!");
                return;
            }

            if (GUILayout.Button("Close"))
            {

                for (int i = 0; i < PaletteGuis.Count; i++)
                {
                    PaletteGuis[i] = null;
                }

                PaletteGuis.Clear();
                IsOpen = false;

                file = null;

                return;
            }

            SoundFilesOpen = EditorGUILayout.Foldout(SoundFilesOpen, "SOUNDFILES");

            if (SoundFilesOpen)
            {
                SoundScrollPos = EditorGUILayout.BeginScrollView(SoundScrollPos);
                EditorGUILayout.BeginVertical();
                EditorGUI.indentLevel++;
                for (int i = 0; i < file.SoundFiles.Count; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    if (GUILayout.Button($"Play {file.SoundFiles[i].Name}"))
                    {
                        AudioClip clip = ((SoundFile) file.SoundFiles[i].Item).CreateAudioClip();
                    }

                    if (GUILayout.Button($"Save", GUILayout.Width(50)))
                    {
                        File.WriteAllBytes(Application.dataPath + $"\\{file.SoundFiles[i].Name}", ((SoundFile)file.SoundFiles[i].Item).GetBytes());
                    }
                    EditorGUILayout.EndHorizontal();
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndScrollView();
                EditorGUI.indentLevel--;
            }

            PaletteFilesOpen = EditorGUILayout.Foldout(PaletteFilesOpen,"PALETTEFILES");

            if (PaletteFilesOpen)
            {
                PaletteScrollPos = EditorGUILayout.BeginScrollView(PaletteScrollPos);
                EditorGUILayout.BeginVertical();
                EditorGUI.indentLevel++;
                for (int i = 0; i < PaletteGuis.Count; i++)
                {
                    PaletteGuis[i].OnGUI();
                }

                EditorGUI.indentLevel--;
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndScrollView();
            }

            TextureFilesOpen = EditorGUILayout.Foldout(TextureFilesOpen, "TEXTUREFILES");

            if (TextureFilesOpen)
            {
                TextureFilesScrollPos = EditorGUILayout.BeginScrollView(TextureFilesScrollPos);
                EditorGUILayout.BeginVertical();
                EditorGUI.indentLevel++;
                for (int i = 0; i < file.TextureFiles.Count; i++)
                {
                    EditorGUILayout.BeginVertical(EditorStyles.helpBox, GUILayout.MaxHeight(128));
                    EditorGUILayout.BeginHorizontal();
                    float width = EditorGUIUtility.currentViewWidth;
                    EditorGUILayout.BeginVertical(GUILayout.MaxWidth((width / 2) - 40));

                    KOTRTexture tex = (KOTRTexture) file.TextureFiles[i].Item;

                    EditorGUILayout.LabelField($"Texture: {file.TextureFiles[i].Name}", EditorStyles.boldLabel);
                    EditorGUILayout.LabelField($"Dimensions: {tex.Texture.width}x{tex.Texture.height}", EditorStyles.boldLabel);
                    EditorGUILayout.LabelField($"Format: {tex.Texture.format:G}", EditorStyles.boldLabel);
                    EditorGUILayout.EndVertical();
                    EditorGUILayout.BeginHorizontal(GUILayout.MaxWidth((width / 2) + 20));
                    Rect layoutRect = EditorGUILayout.GetControlRect(GUILayout.Height(128));
                    EditorGUI.DrawTextureTransparent(layoutRect, tex.Texture,ScaleMode.ScaleToFit);
                    //EditorGUILayout.HelpBox(new GUIContent(tex.Texture));
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.EndVertical();
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.EndScrollView();
                EditorGUI.indentLevel--;
            }
        }

        private bool SoundFilesOpen = false;
        private Vector2 SoundScrollPos = Vector2.zero;


        private bool PaletteFilesOpen = false;
        private Vector2 PaletteScrollPos = Vector2.zero;

        private bool TextureFilesOpen = false;
        private Vector2 TextureFilesScrollPos = Vector2.zero;

        private class PaletteGUI
        {
            private RESFile.SectionInfo<Palette> Info;

            private bool IsOpen;
            private bool IsColorsOpen;

            public PaletteGUI(RESFile.SectionInfo<Palette> info)
            {
                Info = info;
            }

            public void OnGUI()
            {
                IsOpen = EditorGUILayout.Foldout(IsOpen, Info.Name);

                if (IsOpen)
                {
                    EditorGUI.indentLevel++;
                    Palette palette = Info.Item;

                    if (palette.PaletteColors != null)
                    {
                        IsColorsOpen = EditorGUILayout.Foldout(IsColorsOpen,"Show Colors");

                        if (IsColorsOpen)
                        {
                            var oldColor = GUI.color;
                            GUILayout.BeginHorizontal();
                            GUILayout.Space(EditorGUI.indentLevel * 20);
                            EditorGUILayout.BeginVertical();
                            for (int i = 0; i < 16; i++)
                            {
                                EditorGUILayout.BeginHorizontal();
                                for (int j = 0; j < 16; j++)
                                {
                                    GUI.color = palette.PaletteColors[(i * 16) + j];
                                    GUILayout.Box(GUIContent.none,GUILayout.Width(10), GUILayout.Height(10));
                                }
                                EditorGUILayout.EndHorizontal();
                            }
                            EditorGUILayout.EndVertical();
                            EditorGUILayout.EndHorizontal();

                            GUI.color = oldColor;
                        }
                    }

                    EditorGUI.indentLevel--;
                }
            }
        }

        public static void PlayClip(AudioClip clip, int startSample = 0, bool loop = false)
        {
            System.Reflection.Assembly unityEditorAssembly = typeof(AudioImporter).Assembly;
            System.Type audioUtilClass = unityEditorAssembly.GetType("UnityEditor.AudioUtil");
            System.Reflection.MethodInfo method = audioUtilClass.GetMethod(
                "PlayClip",
                System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public,
                null,
                new System.Type[] { typeof(AudioClip), typeof(int), typeof(bool) },
                null
            );
            method.Invoke(
                null,
                new object[] { clip, startSample, loop }
            );
        }

        /// <inheritdoc />
        public void OnBeforeSerialize()
        {
            
        }

        /// <inheritdoc />
        public void OnAfterDeserialize()
        {
            file = null;
        }
    }
}