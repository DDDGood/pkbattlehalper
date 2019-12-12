using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

public class Parser : MonoBehaviour
{
    public TextAsset source;


    // Start is called before the first frame update
    void Start()
    {

        // ProcessImage();

    }



    void ProcessImage()
    {
        string path = Path.Combine(Application.dataPath, "Images");
        path = Path.Combine(path, "pokedex");

        if (Directory.Exists(path))
        {

            List<int> indexes = new List<int>();

            Debug.Log("ImgProcessing");
            foreach (string filePath in Directory.GetFiles(path, "*.png"))
            {
                string assetPath = filePath.Substring(filePath.IndexOf("Assets"));

                string fileName = Path.GetFileNameWithoutExtension(filePath);
                Debug.Log(fileName);

                string[] splits = fileName.Split(']');
                if (splits.Length < 3)
                {
                    Debug.LogWarning("SPLITTTS");
                    continue;
                }
                if (splits[2].Length > 8)
                {
                    Debug.LogWarning(assetPath);
                    AssetDatabase.DeleteAsset(assetPath);
                    continue;
                }
                int index;
                try
                {
                    index = int.Parse(splits[2].Substring(0, 4));
                    if (indexes.Contains(index))
                    {
                        Debug.LogWarning(index + " already exist");
                    }
                    else
                    {
                        indexes.Add(index);
                        AssetDatabase.RenameAsset(assetPath, index.ToString());
                    }
                    Debug.Log(index);
                }
                catch
                {
                    try
                    {
                        index = int.Parse(splits[2].Substring(0, 3));
                        if (indexes.Contains(index))
                        {
                            Debug.LogWarning(index + " already exist: " + filePath);
                        }
                        else
                        {
                            indexes.Add(index);
                            AssetDatabase.RenameAsset(assetPath, index.ToString());
                        }
                        Debug.Log(index);
                    }
                    catch
                    {
                        Debug.LogError("wrong index");
                    }
                }
            }
        }
    }





    void _Localization(string text)
    {
        LocalizationDataList localizationDataList = new LocalizationDataList();

        using (System.IO.StringReader reader = new System.IO.StringReader(text))
        {
            while (reader.Peek() != -1)
            {
                string line = reader.ReadLine();

                if (line.Length < 3)
                    continue;

                string sId = line.Substring(0, 3);
                int id;
                try
                {
                    id = int.Parse(sId);
                }
                catch
                {
                    continue;
                }

                string[] split = line.Split('	');
                if (split.Length < 4)
                {
                    Debug.LogError(line);
                }

                Debug.Log(id + ": " + split[2] + " " + split[3]);

                LocalizationData data = new LocalizationData();
                data.id = id;
                data.eng = split[2];
                data.cht = split[3];

                localizationDataList.list.Add(data);
            }
        }

        string localText = JsonUtility.ToJson(localizationDataList);
        string path = Path.Combine(Application.dataPath, "Data");
        File.WriteAllText(path + "localization.json", localText);
    }

}
