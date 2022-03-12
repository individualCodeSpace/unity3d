using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using System.Threading;
using UnityEngine.SceneManagement;

public class LoadAlphaMap : MonoBehaviour
{


    [MenuItem("GameObject/Alpha/Import All -- 3")]
    public static void LoadALL()
    {
        foreach(var item in Selection.transforms)
        {
            LoadAll(item);
        }
      


    }

    [MenuItem("GameObject/Alpha/Import Grass -- optional")]
    public static void LoadGrass()
    {


        foreach (var item in Selection.transforms)
        {
            LoadGrass(item);
        }

    }
 
    [MenuItem("GameObject/Alpha/Import Tree -- optional")]
    public static void LoadTree()
    {


        foreach (var item in Selection.transforms)
        {
            LoadTree(item);
        }

    }

    [MenuItem("GameObject/Alpha/Import Image -- optional")]
    public static void LoadImage()
    {


        foreach (var item in Selection.transforms)
        {
            LoadImage(item);
        }

    }
    [MenuItem("GameObject/Alpha/Import Raw -- optional")]
    public static void LoadRaw()
    {
       foreach(var item in Selection.transforms)
        {
            LoadRaw(item);
        }
           
       

           
    }
    //保存图片操作的作用是按索引MapMagic内置Layer渲染之后，对该实例地形进行备份Layer，之后才能执行Ctr + S保存地形。
    [MenuItem("GameObject/Alpha/Save Terrain -- 4")]
    public static void SaveImage()
    {
        foreach (var item in Selection.gameObjects)
        {
            SaveImage(item.transform);
        }
        //SaveImage(Selection.activeTransform);

    }
       


    //}
    [MenuItem("GameObject/Alpha/Clone a fetus Terrain -- 2")]
    //制作地形毛坯，以便各类数据的导入，毛坯与源地形名称，规模，分辨率，位置，渲染范围等一致。
    public static void Copy()
    {
        foreach (var item in Selection.gameObjects)
        {
            //var go = Selection.activeTransform;
            var terrain = item.GetComponent<Terrain>();
            var terrdate = new TerrainData();
            var terr = Terrain.CreateTerrainGameObject(terrdate);
           
            var url = "Assets/myTerrainData/";
            if (!Directory.Exists(url))
            {
                Directory.CreateDirectory(url);
                return;
            }
            object o = terrdate as object;
            AssetDatabase.CreateAsset((UnityEngine.Object)o, url + terrain.name + ".asset");
            terr.name = item.gameObject.name;
            terr.transform.position = item.transform.position;
            terr.GetComponent<Terrain>().terrainData = terrdate;

            terr.GetComponent<Terrain>().terrainData.heightmapResolution = terrain.terrainData.heightmapResolution;
            terr.GetComponent<Terrain>().terrainData.size = terrain.terrainData.size;
           
            terr.GetComponent<Terrain>().terrainData.SetDetailResolution(terrain.terrainData.detailResolution, terrain.terrainData.detailResolutionPerPatch);
            terr.GetComponent<Terrain>().terrainData.baseMapResolution = terrain.terrainData.baseMapResolution;
            terr.GetComponent<Terrain>().terrainData.alphamapResolution = terrain.terrainData.alphamapResolution;
            terr.GetComponent<Terrain>().terrainData.wavingGrassTint = terrain.terrainData.wavingGrassTint;
            terr.GetComponent<Terrain>().shadowCastingMode = terrain.GetComponent<Terrain>().shadowCastingMode;
            terr.GetComponent<Terrain>().reflectionProbeUsage = terrain.GetComponent<Terrain>().reflectionProbeUsage;
            terr.GetComponent<Terrain>().heightmapPixelError = terrain.GetComponent<Terrain>().heightmapPixelError;
            terr.GetComponent<Terrain>().bakeLightProbesForTrees = terrain.GetComponent<Terrain>().bakeLightProbesForTrees;
            terr.GetComponent<Terrain>().detailObjectDistance = terrain.GetComponent<Terrain>().detailObjectDistance;
            terr.GetComponent<Terrain>().treeBillboardDistance = terrain.GetComponent<Terrain>().treeBillboardDistance;
            terr.GetComponent<Terrain>().treeMaximumFullLODCount = terrain.GetComponent<Terrain>().treeMaximumFullLODCount;

            GameObject[] Childdetial = new GameObject[terrain.transform.childCount];
            for (int i = 0; i < terrain.transform.childCount; i++)
            {
                Childdetial[i] = GameObject.Instantiate<GameObject>(terrain.transform.GetChild(i).gameObject);
                Childdetial[i].transform.position = terrain.transform.GetChild(i).transform.position;

            }
            foreach (var children in Childdetial)
            {
                children.transform.parent = terr.transform;

            }
        }
    }
   
    [InitializeOnLoadMethod]
    static void OverrideHierarchy()
    {
        EditorApplication.hierarchyWindowItemOnGUI += OnHGUI;
    }

    private static void OnHGUI(int instanceID, Rect selectionRect)
    {
        if (Event.current != null && selectionRect.Contains(Event.current.mousePosition)
            && Event.current.button == 1 && Event.current.type <= EventType.MouseUp)
        {
            GameObject sobj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (sobj.GetComponentInChildren<Terrain>() != null)
            {
                var mousepos = Event.current.mousePosition;
                EditorUtility.DisplayPopupMenu(new Rect(mousepos.x, mousepos.y, 0, 0), "GameObject/Alpha", null);
                Event.current.Use();
            }
        }
    }

    public static void LoadAll(Transform tf)
    {


        var terrain = tf.GetComponent<Terrain>();
        if (terrain != null)
        {
            LoadLayers(tf);
            LoadGrassProto(tf);
            LoadTreeProto(tf);
            LoadImage(terrain);
            LoadRaw(tf);
            LoadGrass(terrain);
            LoadTree(terrain);
            SaveImage(tf);
        }

        foreach (Transform t in tf)
        {
            var terr = t.GetComponent<Terrain>();
            if (!terr)
                continue;
            LoadLayers(t);
            LoadGrassProto(t);
            LoadTreeProto(t);
            LoadAll(t);
            LoadRaw(t);
            LoadGrass(terr);
            LoadTree(terr);
            SaveImage(t);
        
        }
    }

    public static void LoadTree(Transform tf)
    {
        var terrain = tf.GetComponent<Terrain>();
        if(terrain != null)
        {
            LoadTreeProto(tf);
            LoadTree(terrain);
        }
        foreach (Transform t in tf)
        {
            var terr = t.GetComponent<Terrain>();
            if (!terr)
                continue;
            LoadTreeProto(tf);
            LoadTree(t);
            LoadTree(terrain);

        }
    }
    public static void LoadGrass(Transform tf)
    {
        var terrain = tf.GetComponent<Terrain>();
        if (terrain != null)
        {
            LoadGrassProto(tf);
            LoadGrass(terrain);
        }
        foreach (Transform t in tf)
        {
            var terr = t.GetComponent<Terrain>();
            if (!terr)
                continue;
            LoadGrassProto(tf);
            LoadGrass(t);
            LoadGrass(terrain);


        }
    }
     public static void LoadImage(Transform tf)
    {
        var terrain = tf.GetComponent<Terrain>();
        if (terrain != null)
        {
            LoadLayers(tf);
            LoadImage(terrain);
            SaveImage(tf);
        }
        foreach (Transform t in tf)
        {
            var terr = t.GetComponent<Terrain>();
            if (!terr)
                continue;
            LoadLayers(tf);
            LoadImage(t);
            SaveImage(tf);
        }
    }
    public static void SaveImage(Transform tf)
    {
        var terrain = tf.GetComponent<Terrain>();
        if (terrain != null)
        {
            SaveMyLayers(tf);
            LoadMyLayer(tf);
           // LoadImage(terrain);
        }
        foreach (Transform t in tf)
        {
            var terr = t.GetComponent<Terrain>();
            if (!terr)
                continue;
            SaveMyLayers(tf);
            LoadMyLayer(tf);
            //LoadImage(t);

        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="terr"></param>
    private static void LoadTree(Terrain terr)
    {
        Scene CurrentScene = SceneManager.GetActiveScene();
        var dir = Application.dataPath + "/../" + CurrentScene.name + "/" + terr.gameObject.name + "/tree.bin";
        var res = Deserialize<List<MyTreeInstance>>(dir);
        terr.terrainData.SetTreeInstances(res.Select(x => x.ToTree()).ToArray(), false);
    }

    private static void LoadRaw(Transform transform)
    {
        Scene CurrentScene = SceneManager.GetActiveScene();
        var terr = transform.GetComponent<Terrain>();
        if (!terr)
            return;
        var url = Application.dataPath + "/../" + CurrentScene.name + "/" + terr.gameObject.name + "/heights.bin";
        var data = terr.terrainData;
        float[,] heights = Deserialize<float[,]>(url);
        data.SetHeights(0, 0, heights);
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="url"></param>
    /// <returns></returns>
    private static T Deserialize<T>(string url)
    {
        T heights;
        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream rems = new MemoryStream(File.ReadAllBytes(url));
        heights = (T)formatter.Deserialize(rems);
        return heights;
    }
    private static void SerializeObj(string url, object heights)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream rems = new MemoryStream();
        formatter.Serialize(rems, heights);

        File.WriteAllBytes(url, rems.GetBuffer());
    }

    private static void LoadGrass(Terrain terr)
    {
        Scene CurrentScene = SceneManager.GetActiveScene();
        var url = Application.dataPath + "/../" + CurrentScene.name + "/" + terr.gameObject.name + "/grass.bin";
        List<int[,]> res = Deserialize<List<int[,]>>(url);
        for (int i = 0; i < res.Count; i++)
        {
            terr.terrainData.SetDetailLayer(0, 0, i, res[i]);
            //Debug.Log(res[1].Length);
        }
        //int[,] resArray = res[1];
        //Debug.Log(resArray.Length);
    }
    // Use this for initialization
    public static void LoadImage(Terrain terrain)
    {
        Scene CurrentScene = SceneManager.GetActiveScene();
        var dir = Application.dataPath + "/../" + CurrentScene.name + "/" + terrain.gameObject.name;
        var td = terrain.terrainData;
        var alphas = td.GetAlphamaps(0, 0, td.alphamapWidth, td.alphamapHeight);
        var len = (alphas.GetLength(2));
        List<Texture2D> textures = new List<Texture2D>();
        for (int i = 0; i < len / 4 + 1; i++)
        {
            textures.Add(new Texture2D(td.alphamapWidth, td.alphamapHeight));
            textures[i].LoadImage(File.ReadAllBytes(dir + "/alpha" + i + ".png"));
        }
        for (int i = 0; i < td.alphamapWidth; i++)
        {
            for (int j = 0; j < td.alphamapHeight; j++)
            {

                for (int m = 0; m * 4 < len; m++)
                {
                    var c = textures[m].GetPixel(j, i);
                    if (alphas.GetLength(2) > m * 4)
                        alphas[i, j, m * 4] = c.r;
                    if (alphas.GetLength(2) > m * 4 + 1)
                        alphas[i, j, m * 4 + 1] = c.g;
                    if (alphas.GetLength(2) > m * 4 + 2)
                        alphas[i, j, m * 4 + 2] = c.b;
                    if (alphas.GetLength(2) > m * 4 + 3)
                        alphas[i, j, m * 4 + 3] = c.a;
                }

            }
        }
        td.SetAlphamaps(0, 0, alphas);
    }
    
    public static void LoadGrassProto(Transform tf)
    {
        Scene CurrentScene = SceneManager.GetActiveScene();
        var terr = tf.GetComponent<Terrain>();
        var dir = Application.dataPath + "/../" + CurrentScene.name + "/" + terr.gameObject.name + "/grassproto.bin";
        var list = Deserialize<List<MyDetailPrototype>>(dir);
        terr.terrainData.detailPrototypes = list.Select(x => x.ToDetail()).ToArray();
    }

    public static void LoadTreeProto(Transform tf)
    {
        Scene CurrentScene = SceneManager.GetActiveScene();
        var terr = tf.GetComponent<Terrain>();
        var dir = Application.dataPath + "/../" + CurrentScene.name + "/" + terr.gameObject.name + "/treeproto.bin";
        var list = Deserialize<List<MyTreePrototype>>(dir);
        terr.terrainData.treePrototypes = list.Select(x => x.ToTree()).ToArray();
    }
    public static void LoadMyLayer(Transform tf)
    {
        Scene CurrentScene = SceneManager.GetActiveScene();
        var terr = tf.GetComponent<Terrain>();
        var dir = Application.dataPath + "/../" + CurrentScene.name + "/" + terr.gameObject.name + "/splatproto.bin";
        var list = Deserialize<List<string>>(dir);
        var array = new TerrainLayer[list.Count];
        for(int i = 0; i < list.Count; i++)
        {
            array[i] = AssetDatabase.LoadAssetAtPath<TerrainLayer>(list[i]);
        }
        //var array = list.Select(x => x).ToArray();
        terr.terrainData.terrainLayers = array;
        AssetDatabase.SaveAssets();


    }
    public static void SaveMyLayers(Transform tf)
    {
        Scene CurrentScene = SceneManager.GetActiveScene();
        var terr = tf.GetComponent<Terrain>();
        var dir1 = Application.dataPath + "/../" + CurrentScene.name + "/" + terr.gameObject.name + "/splatproto.bin";
        //将样板地形的terrainlayer文件复制一份生成到指定文件路径，以备导入时调用
        var mydir = "Assets/myTerrainLayers/";
        if (!Directory.Exists(mydir))
        {
            Directory.CreateDirectory(mydir);
            return;
        }
        var layers = new List<string>();
        foreach (var item in terr.terrainData.terrainLayers)
        {
            object o = item as object;
            var name = "mylayer" + o.GetHashCode();
            if (!File.Exists(mydir + name + ".terrainlayer"))
            {

                AssetDatabase.CreateAsset((UnityEngine.Object)o, mydir + name + ".terrainlayer");
                layers.Add(mydir + name + ".terrainlayer");
               // Debug.Log(mydir + name);
            }
            else
            {
                AssetDatabase.DeleteAsset(mydir + name + ".terrainlayer");
                AssetDatabase.CreateAsset((UnityEngine.Object)o, mydir + name + ".terrainlayer");
                layers.Add(mydir + name + ".terrainlayer");
                //    for(int i = 0; i < layers.Count; i++)
                //    {
                //        AssetDatabase.LoadAssetAtPath<TerrainLayer>(layers[i]).diffuseTexture = terrain.terrainData.terrainLayers[i].diffuseTexture;
                //        AssetDatabase.LoadAssetAtPath<TerrainLayer>(layers[i]).maskMapTexture = terrain.terrainData.terrainLayers[i].maskMapTexture;
                //        AssetDatabase.LoadAssetAtPath<TerrainLayer>(layers[i]).normalMapTexture = terrain.terrainData.terrainLayers[i].normalMapTexture;

                //    }

                //    Debug.Log("mydear! you already creat successfully! so we don't creat again for you! but we have finished altering texture");
                //}

            }
            SerializeObj(dir1, layers);
        }
    }    
    public static void LoadLayers(Transform tf)
    {
        Scene CurrentScene = SceneManager.GetActiveScene();
        var terr = tf.GetComponent<Terrain>();
        var dir = Application.dataPath + "/../" + CurrentScene.name + "/" + terr.gameObject.name + "/mylayers.bin";
        var list = Deserialize<List<MyTerrainLayer>>(dir);
        terr.terrainData.terrainLayers = list.Select(x => x.ToLayer()).ToArray();
    }
    
}
