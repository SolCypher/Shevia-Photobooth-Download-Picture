using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZXing;
using ZXing.QrCode;
using UnityEngine.UI;
using JetBrains.Annotations;
using System.IO;
using System;
using TMPro;

public class QRGenerator : MonoBehaviour
{
    [Header("QR Code Info")]
    public RawImage outputImgQR;
    // public TMP_InputField contentQR;
    public TMP_InputField imgName;
    private int widthQR = 256;
    private int heightQR = 256;
    // Web path to get Uploaded Img
    private string webPath = "https://shevia.id/api/webhook/photo/";

    [Header("QR Code Save Location")]
    // [Tooltip("Leave empty for default (Downloads Folder)")]
    // public string path;

    private Texture2D encodedTex;
    private int generateCount = 0;

    public void GenerateQR(){
        SetQRContent();
    }

    public void SaveQR(){
        if(outputImgQR.texture == null){
            Debug.LogError("QR hasn't been generated!");
            return;
        }

        SaveToFile(encodedTex, imgName.text);

    }

    private Color32[] Encode(string content, int width, int height){
        BarcodeWriter writer = new BarcodeWriter{
            Format = BarcodeFormat.QR_CODE,
            Options = new QrCodeEncodingOptions{
                Width = width,
                Height = height,
            }
        };

        return writer.Write(content);

    }

    private void SetQRContent(){
        // Create Empty Texture
        encodedTex = new Texture2D(widthQR, heightQR);
        
        // The returned Uploaded Img ID from Server
        string imageID = SingletonImgID.instance.ReturnedImgID;

        // Check if the content is empty/not
        if(string.IsNullOrEmpty(imageID)){ // Set to contentQR.text for debugging, dont forget to un-comment the contentQR
            Debug.LogError("Content for QR is empty");
            return;
        }

        // Generate the QR (Encode the content from Pixel > Texture)
        Color32[] OutputQR = Encode(webPath + imageID, encodedTex.width, encodedTex.height); // Set to contentQR.text for debugging, dont forget to un-comment the contentQR
        encodedTex.SetPixels32(OutputQR);;
        encodedTex.Apply();

        // Set the QR Texture to Raw Image
        outputImgQR.texture = encodedTex;
        generateCount++;

    }

    private void SaveToFile(Texture2D texture, string name, string path = null){
        byte[] bytes = texture.EncodeToPNG();
        string fileName = "";

        if(string.IsNullOrEmpty(name)){
            fileName = "QR Code " + generateCount + ".png";
        }else{
            fileName = name + ".png";
        }

        if(string.IsNullOrEmpty(path)){
            string defaultPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
            defaultPath = Path.Combine(defaultPath, fileName);

            File.WriteAllBytes(defaultPath, bytes);
            Debug.Log("QR Code saved to " + defaultPath);

        }else{
            string finalPath = Path.Combine(path, fileName);

            File.WriteAllBytes(finalPath, bytes);
            Debug.Log("QR Code saved to " + finalPath);

        }

    }

}
