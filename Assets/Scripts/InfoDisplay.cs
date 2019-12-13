using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

// Pulls images from Asset/InfoDisplay every 20 minutes and displays them at the bottom

// Attached to: Overlay/UI/Misc/InfoDisplay

public class InfoDisplay : MonoBehaviour 
{
    public Transform ListPrefab;
    public Transform SlidePrefab;

	void Start () 
    {
        InvokeRepeating("GetImage", 1f, 1200f);
    }

    private void GetImage()
    {
        ListPrefab.gameObject.SetActive(true);
        SlidePrefab.gameObject.SetActive(true);

        if (transform.childCount > 2) // Delete old entries
        {
            Destroy(transform.GetChild(2).gameObject);
        }

        Transform list = Instantiate(ListPrefab);
        list.SetParent(transform, false); // Keep original position

        Transform SlideContainer = list.GetChild(0);

        foreach (string file in Directory.GetFiles("Assets/InfoDisplay"))
        {
            if (!file.Contains(".meta"))
            {
                Transform picture = Instantiate(SlidePrefab);
                picture.SetParent(SlideContainer);

                Texture2D image = LoadPNG(file);

                Sprite sprite_photo = Sprite.Create(
                    image, new Rect(0f, 0f, image.width, image.height), new Vector2(0.5f, 0.5f), 100.0f);
                    
                picture.GetComponent<Image>().sprite = sprite_photo;
            }
        }

        list.GetComponent<UI_InfiniteScroll>().Init();
        ListPrefab.gameObject.SetActive(false);
        SlidePrefab.gameObject.SetActive(false);
    }

    public static Texture2D LoadPNG(string filePath)
    {
        Texture2D tex = null;
        byte[] fileData;

        if (File.Exists(filePath))
        {
            fileData = File.ReadAllBytes(filePath);
            tex = new Texture2D(2, 2);
            tex.LoadImage(fileData); //this will auto-resize the texture dimensions.
        }
        return tex;
    }
}
