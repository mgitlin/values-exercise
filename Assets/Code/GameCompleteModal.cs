/**
 * GameCompleteModal.cs
 * by Matthew Gitlin
 * Functionality and information for game complete modal and its buttons
 * 
**/

using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;

public class GameCompleteModal : MonoBehaviour {


    [DllImport("__Internal")]
    private static extern void openWindow(string url);

    [DllImport("__Internal")]
    private static extern void openLink(string url);

    public Transform cardArea;
    [Header("Buttons")]
    public Button screenshot;
    public Button close;


    public void ReturnToHomepage()
    {
        StartCoroutine(HomeReroute());
    }

    IEnumerator HomeReroute()
    {
        yield return new WaitForEndOfFrame();
        openLink("http://www.solution-focusedcoaching.com/");
    }

    public void Screenshot()
    {
        StartCoroutine(UploadPNG());
        //Debug.log (encodedText);
    }

    IEnumerator UploadPNG()
    {
        // We should only read the screen after all rendering is complete
        yield return new WaitForEndOfFrame();

        // Create a texture the size of the screen, RGB24 format
        int width = Screen.width;
        int height = Screen.height;
        var tex = new Texture2D(width, height, TextureFormat.RGB24, false);

        // Read screen contents into the texture
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        // Encode texture into PNG
        byte[] bytes = tex.EncodeToPNG();
        Destroy(tex);

        //string ToBase64String byte[]
        string encodedText = System.Convert.ToBase64String(bytes);

        var image_url = "data:image/png;base64," + encodedText;

        Debug.Log(image_url);

        openWindow(image_url);
    }

    public void Close()
    {
        GetComponent<Animator>().Play("CloseModal");
    }
}
