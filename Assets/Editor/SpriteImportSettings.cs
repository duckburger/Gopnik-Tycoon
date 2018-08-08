using UnityEditor;
using UnityEngine;


public class SpriteImportSettings : AssetPostprocessor {


    void OnPreprocessTexture()
    {
        TextureImporter importer = (TextureImporter)assetImporter;
        importer.textureType = TextureImporterType.Sprite;
        importer.mipmapEnabled = false;
        importer.filterMode = FilterMode.Point;
        importer.textureCompression = TextureImporterCompression.Uncompressed;
    }
	
}
