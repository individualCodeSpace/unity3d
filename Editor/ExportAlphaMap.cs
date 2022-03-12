using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class ExportAlphaMap
{
    //static List<Color> colors = new List<Color>();
    [MenuItem("GameObject/Alpha/Export -- 1")]
    public static void ExportImage()
    {

        //colors.Add(new Color(1, 0, 0, 0));
        //colors.Add(new Color(0, 1, 0, 0));
        //colors.Add(new Color(0, 0, 1, 0));
        //colors.Add(new Color(0, 0, 0, 1));
        foreach (var item in Selection.gameObjects)
        {
            var terr = item.GetComponent<Terrain>();
            if (!terr)
                continue;
            ExportImage(item.transform);
            ExportRaw(terr);
            ExportTree(terr);
            ExportGrass(terr);
        }

    }

    private static void ExportGrass(Terrain terr)
    {
        Scene CurrentScene = SceneManager.GetActiveScene();
        var dir = Application.dataPath + "/../" + CurrentScene.name  + "/" + terr.gameObject.name + "/grass.bin";
        var res = new List<int[,]>();
        for (int i = 0; i < terr.terrainData.detailPrototypes.Length; i++)
        {
            res.Add(terr.terrainData.GetDetailLayer(0, 0, terr.terrainData.detailWidth, terr.terrainData.detailHeight, i));
        }
        SerializeObj(dir, res);
        dir = Application.dataPath + "/../" + CurrentScene.name + "/" + terr.gameObject.name + "/grassproto.bin";
        var list = new List<MyDetailPrototype>();
        foreach (var item in terr.terrainData.detailPrototypes)
        {
            list.Add(new MyDetailPrototype(item));
        }
        SerializeObj(dir, list);

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="terr">选中场景的地形组件</param>
    private static void ExportTree(Terrain terr)
    {
        Scene CurrentScene = SceneManager.GetActiveScene();
        var dir = Application.dataPath + "/../" + CurrentScene.name + "/" + terr.gameObject.name + "/tree.bin";
        var res = new List<MyTreeInstance>();
        for (int i = 0; i < terr.terrainData.treeInstanceCount; i++)
        {
            var instance = terr.terrainData.GetTreeInstance(i);
            res.Add(new MyTreeInstance(instance));
        }
        SerializeObj(dir, res);
        dir = Application.dataPath + "/../" + CurrentScene.name + "/" + terr.gameObject.name + "/treeproto.bin";
        var list = new List<MyTreePrototype>();
        foreach (var item in terr.terrainData.treePrototypes)
        {
            list.Add(new MyTreePrototype(item));
        }
        SerializeObj(dir, list);
       
    }
    /// <summary>
    /// 导出高度数据并保存为二进制文件
    /// </summary>
    /// <param name="terr">选中场景的地形组件</param>
    private static void ExportRaw(Terrain terr)
    {
        Scene CurrentScene = SceneManager.GetActiveScene();
        var data = terr.terrainData;
        var heights = data.GetHeights(0, 0, data.heightmapWidth, data.heightmapHeight);
        var dir = Application.dataPath + "/../" + CurrentScene.name + "/" + terr.gameObject.name + "/heights.bin";
        SerializeObj(dir, heights);
    }
    /// <summary>
    /// 在指定路径下将对象转化成二进制文件
    /// </summary>
    /// <param name="url">路径名</param>
    /// <param name="heights">高度数据，类型为对象</param>
    private static void SerializeObj(string url, object heights)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        MemoryStream rems = new MemoryStream();
        formatter.Serialize(rems, heights);

        File.WriteAllBytes(url, rems.GetBuffer());
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

    /// <summary>
    /// 重载的多态方法，用递归循环遍历子物体的坐标变换组件
    /// </summary>
    /// <param name="tf">地形对象的坐标组件</param>
    public static void ExportImage(Transform tf)
    {
        var terrain = tf.GetComponent<Terrain>();
        if (terrain != null)
            ExportImage(terrain);
        foreach (Transform t in tf)
        {
            ExportImage(t);
        }
    }


    // Use this for initialization
    public static void ExportImage(Terrain terrain)
    {
        Scene CurrentScene = SceneManager.GetActiveScene();
        var td = terrain.terrainData;
        var dir = Application.dataPath + "/../" + CurrentScene.name + "/" + terrain.gameObject.name;
        if (!Directory.Exists(dir))
            Directory.CreateDirectory(dir);
        var alphas = td.GetAlphamaps(0, 0, td.alphamapWidth, td.alphamapHeight);
        var textures = new List<Texture2D>();
        //RGBA通道来存储地形材质球，所以需要生成的图片的数量为alphas.length(2）/4 +1；
        for (int i = 0; i < alphas.GetLength(2) / 4 + 1; i++)
        {
            textures.Add(new Texture2D(td.alphamapWidth, td.alphamapHeight));
        }
        for (int i = 0; i < td.alphamapWidth; i++)
        {
            for (int j = 0; j < td.alphamapHeight; j++)
            {
                for (int m = 0; m < alphas.GetLength(2) / 4 + 1; m++)
                {
                    Color c = new Color(0, 0, 0, 0);
                    if (alphas.GetLength(2) > m * 4)
                        c.r = alphas[i, j, m * 4];
                    if (alphas.GetLength(2) > m * 4 + 1)
                        c.g = alphas[i, j, m * 4 + 1];
                    if (alphas.GetLength(2) > m * 4 + 2)
                        c.b = alphas[i, j, m * 4 + 2];
                    if (alphas.GetLength(2) > m * 4 + 3)
                        c.a = alphas[i, j, m * 4 + 3];
                    textures[m].SetPixel(j, i, c);
                }

            }
        }
        for (int i = 0; i < textures.Count; i++)
        {
            File.WriteAllBytes(dir + "/alpha" + i + ".png", textures[i].EncodeToPNG());
        }
        var dir_layer = Application.dataPath + "/../" + CurrentScene.name + "/" + terrain.gameObject.name + "/mylayers.bin";
        var list_layer = new List<MyTerrainLayer>();
        foreach (var item in terrain.terrainData.terrainLayers)
        {
            list_layer.Add(new MyTerrainLayer(item));
        }
        SerializeObj(dir_layer, list_layer);
        
    }


}
[Serializable]
public struct MyTreeInstance
{
    //
    // 摘要:
    //     Position of the tree.
    public MyVec position;
    //
    // 摘要:
    //     Width scale of this instance (compared to the prototype's size).
    public float widthScale;
    //
    // 摘要:
    //     Height scale of this instance (compared to the prototype's size).
    public float heightScale;
    //
    // 摘要:
    //     Read-only. Rotation of the tree on X-Z plane (in radians).
    public float rotation;
    //
    // 摘要:
    //     Color of this instance.
    public MyColor color;
    //
    // 摘要:
    //     Lightmap color calculated for this instance.
    public MyColor lightmapColor;
    //
    // 摘要:
    //     Index of this instance in the TerrainData.treePrototypes array.
    public int prototypeIndex;
    public MyTreeInstance(TreeInstance instance)
    {
        this.color = new MyColor(instance.color);
        this.heightScale = instance.heightScale;
        this.position = new MyVec(instance.position);
        this.prototypeIndex = instance.prototypeIndex;
        this.lightmapColor = new MyColor(instance.lightmapColor);
        this.widthScale = instance.widthScale;
        this.rotation = instance.rotation;
    }

    internal TreeInstance ToTree()
    {
        TreeInstance tree = new TreeInstance();
        tree.color = this.color.ToColor();
        tree.heightScale = this.heightScale;
        tree.position = this.position.ToTree();
        tree.prototypeIndex = this.prototypeIndex;
        tree.lightmapColor = this.lightmapColor.ToColor();
        tree.widthScale = this.widthScale;
        tree.rotation = this.rotation;
        return tree;
    }
}
[Serializable]
public struct MyVec
{
    public float x;
    public float y;
    public float z;
    public MyVec(Vector3 v)
    {
        this.x = v.x;
        this.y = v.y;
        this.z = v.z;
    }
    public Vector3 ToTree()
    {
        return new Vector3(x, y, z);
    }

}
[Serializable]
public struct MyVec2
{
    public float x;
    public float y;
    public MyVec2(Vector2 v)
    {
        this.x = v.x;
        this.y = v.y;
    }
    public Vector2 ToV2()
    {
        return new Vector2(x, y);
    }

}
[Serializable]
public struct MyVec4
{
    public float x;
    public float y;
    public float z;
    public float w;
    public MyVec4(Vector4 v)
    {
        this.x = v.x;
        this.y = v.y;
        this.z = v.z;
        this.w = v.w;
    }
    public Vector4 ToV4()
    {
        return new Vector4(x, y, z, w);
    }

}
[Serializable]
public struct MyColor
{
    public byte r;
    public byte g;
    public byte b;
    public byte a;

    public MyColor(Color32 color)
    {
        this.r = color.r;
        this.g = color.g;
        this.b = color.b;
        this.a = color.a;
    }
    public Color32 ToColor()
    {
        return new Color32(r, g, b, a);
    }
}

[Serializable]
public class MyTreePrototype
{
    //
    // 摘要:
    //     Retrieves the actual GameObject used by the tree.
    public string prefab { get; set; }
    //
    // 摘要:
    //     Bend factor of the tree prototype.
    public float bendFactor { get; set; }
    public TreePrototype ToTree()
    {
        TreePrototype tree = new TreePrototype();
        tree.bendFactor = this.bendFactor;
        tree.prefab = AssetDatabase.LoadAssetAtPath<GameObject>(this.prefab);
        return tree;
    }
    public MyTreePrototype(TreePrototype tree)
    {
        this.bendFactor = tree.bendFactor;
        this.prefab = AssetDatabase.GetAssetPath(tree.prefab);
        //Debug.Log(this.prefab);
    }
}
[Serializable]
public class MyDetailPrototype
{
    //
    // 摘要:
    //     GameObject used by the DetailPrototype.
    public string prototype { get; set; }
    //
    // 摘要:
    //     Texture used by the DetailPrototype.
    public string prototypeTexture { get; set; }
    //
    // 摘要:
    //     Minimum width of the grass billboards (if render mode is GrassBillboard).
    public float minWidth { get; set; }
    //
    // 摘要:
    //     Maximum width of the grass billboards (if render mode is GrassBillboard).
    public float maxWidth { get; set; }
    //
    // 摘要:
    //     Minimum height of the grass billboards (if render mode is GrassBillboard).
    public float minHeight { get; set; }
    //
    // 摘要:
    //     Maximum height of the grass billboards (if render mode is GrassBillboard).
    public float maxHeight { get; set; }
    //
    // 摘要:
    //     How spread out is the noise for the DetailPrototype.
    public float noiseSpread { get; set; }
    //
    // 摘要:
    //     Bend factor of the detailPrototype.
    public float bendFactor { get; set; }
    //
    // 摘要:
    //     Color when the DetailPrototypes are "healthy".
    public MyColor healthyColor { get; set; }
    //
    // 摘要:
    //     Color when the DetailPrototypes are "dry".
    public MyColor dryColor { get; set; }
    //
    // 摘要:
    //     Render mode for the DetailPrototype.
    public int renderMode { get; set; }
    public bool usePrototypeMesh { get; set; }

    public MyDetailPrototype(DetailPrototype detail)
    {
        this.bendFactor = detail.bendFactor;
        this.dryColor = new MyColor(detail.dryColor);
        this.healthyColor = new MyColor(detail.healthyColor);
        this.maxHeight = detail.maxHeight;
        this.minWidth = detail.minWidth;
        this.minHeight = detail.minHeight;
        this.maxWidth = detail.maxWidth;
        this.noiseSpread = detail.noiseSpread;
        if (detail.prototype)
            this.prototype = AssetDatabase.GetAssetPath(detail.prototype);
        if (detail.prototypeTexture)
            this.prototypeTexture = AssetDatabase.GetAssetPath(detail.prototypeTexture);

        this.renderMode = (int)detail.renderMode;
        this.usePrototypeMesh = this.usePrototypeMesh;
    }

    public DetailPrototype ToDetail()
    {
        DetailPrototype detail = new DetailPrototype();
        detail.bendFactor = this.bendFactor;
        detail.dryColor = this.dryColor.ToColor();
        detail.healthyColor = this.healthyColor.ToColor();
        detail.maxHeight = this.maxHeight;
        detail.minWidth = this.minWidth;
        detail.minHeight = this.minHeight;
        detail.maxWidth = this.maxWidth;
        detail.noiseSpread = this.noiseSpread;
        if (!string.IsNullOrEmpty(this.prototype))
            detail.prototype = AssetDatabase.LoadAssetAtPath<GameObject>(this.prototype);
        if (!string.IsNullOrEmpty(this.prototypeTexture))
            detail.prototypeTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(this.prototypeTexture);
       // Debug.Log(this.prototypeTexture);
        detail.renderMode = (DetailRenderMode)this.renderMode;
        detail.usePrototypeMesh = detail.prototype;
        return detail;
    }
}
[Serializable] 
public class MyTerrainLayer
{
    //
    // 摘要:
    //     The diffuse texture used by the terrain layer.
    public string diffuseTexture { get; set; }
    // 摘要:
    //     Normal map texture used by the terrain layer.
    public string normalMapTexture { get; set; }
    //
    // 摘要:
    //     The mask map texture used by the terrain layer.
    public string maskMapTexture { get; set; }
    //
    // 摘要:
    //     UV Tiling size.
    public MyVec2 tileSize { get; set; }
    //
    // 摘要:
    //     UV tiling offset.
    public MyVec2 tileOffset { get; set; }
    //
    // 摘要:
    //     Specular color.
    public MyColor specular { get; set; }
    //
    // 摘要:
    //     Metallic factor used by the terrain layer.
    public float metallic { get; set; }
    //
    // 摘要:
    //     Smoothness of the specular reflection.
    public float smoothness { get; set; }
    //
    // 摘要:
    //     A float value that scales the normal vector. The minimum value is 0, the maximum
    //     value is 1.
    public float normalScale { get; set; }
    //
    // 摘要:
    //     A Vector4 value specifying the minimum RGBA value that the diffuse texture maps
    //     to when the value of the channel is 0.
    public MyVec4 diffuseRemapMin { get; set; }
    //
    // 摘要:
    //     A Vector4 value specifying the maximum RGBA value that the diffuse texture maps
    //     to when the value of the channel is 1.
    public MyVec4 diffuseRemapMax { get; set; }
    //
    // 摘要:
    //     A Vector4 value specifying the minimum RGBA value that the mask map texture maps
    //     to when the value of the channel is 0.
    public MyVec4 maskMapRemapMin { get; set; }
    //
    // 摘要:
    //     A Vector4 value specifying the maximum RGBA value that the mask map texture maps
    //     to when the value of the channel is 1.
    public MyVec4 maskMapRemapMax { get; set; }
    public MyTerrainLayer(TerrainLayer layer)
    {
        this.diffuseRemapMax = new MyVec4(layer.diffuseRemapMax);
        this.diffuseRemapMin = new MyVec4(layer.diffuseRemapMin);
        if (layer.diffuseTexture)
            this.diffuseTexture = AssetDatabase.GetAssetPath(layer.diffuseTexture);
        this.maskMapRemapMax = new MyVec4(layer.maskMapRemapMax);
        this.maskMapRemapMin = new MyVec4(layer.maskMapRemapMin);
        if (layer.maskMapTexture)
            this.maskMapTexture = AssetDatabase.GetAssetPath(layer.maskMapTexture);
        this.metallic = layer.metallic;
        if (layer.normalMapTexture)
            this.normalMapTexture = AssetDatabase.GetAssetPath(layer.normalMapTexture);
        this.normalScale = layer.normalScale;
        this.smoothness = layer.smoothness;
        this.specular = new MyColor(layer.specular);
        this.tileOffset = new MyVec2(layer.tileOffset);
        this.tileSize = new MyVec2(layer.tileSize);
    }
    public TerrainLayer ToLayer()
    {
        TerrainLayer layer = new TerrainLayer();
        layer.diffuseRemapMax = this.diffuseRemapMax.ToV4();
        layer.diffuseRemapMin = this.diffuseRemapMin.ToV4();
        if (!string.IsNullOrEmpty(this.diffuseTexture))
            layer.diffuseTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(this.diffuseTexture);
        layer.maskMapRemapMax = this.maskMapRemapMax.ToV4();
        layer.maskMapRemapMin = this.maskMapRemapMin.ToV4();
        if (!string.IsNullOrEmpty(this.maskMapTexture))
            layer.maskMapTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(this.maskMapTexture);
        if (!string.IsNullOrEmpty(this.normalMapTexture))
            layer.normalMapTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(this.normalMapTexture);
        layer.normalScale = this.metallic;
        layer.smoothness = this.smoothness;
        layer.specular = this.specular.ToColor();
        layer.tileOffset = this.tileOffset.ToV2();
        layer.tileSize = this.tileSize.ToV2();
        return layer;
    }
}