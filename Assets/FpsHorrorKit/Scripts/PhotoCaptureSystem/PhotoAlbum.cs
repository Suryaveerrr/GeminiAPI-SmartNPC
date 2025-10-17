namespace FpsHorrorKit
{
    using System.Collections.Generic;
    using UnityEngine;

    [CreateAssetMenu(fileName = "PhotoAlbum", menuName = "ScriptableObjects/PhotoAlbum")]
    public class PhotoAlbum : ScriptableObject
    {
        public List<Sprite> photos = new List<Sprite>();

        // Adds and saves a photo
        public void AddPhoto(Sprite photo)
        {
            photos.Add(photo);
#if UNITY_EDITOR
            // Marks the ScriptableObject as dirty (modified)
            UnityEditor.EditorUtility.SetDirty(this);
            // Saves the changes
            UnityEditor.AssetDatabase.SaveAssets();
#endif
        }
    }
}