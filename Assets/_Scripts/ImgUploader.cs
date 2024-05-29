using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ImgUploader : MonoBehaviour
{
    // public SpriteRenderer img;
    public RawImage img;
    public string ServerURL;
    
    private void Update() {
        if(Input.GetKeyDown(KeyCode.Space)){
            UploadTexture();
        }
    }

    public void UploadTexture(){
        if(img.texture == null){
            Debug.LogError("The Image's Texture hasn't been set");
            return;
        }
        // StartCoroutine(UploadImage(DuplicateTexture(img.sprite.texture)));

        if(img.texture is Texture2D){
            Texture2D imgTex = img.texture as Texture2D;
            StartCoroutine(UploadImage(DuplicateTexture(imgTex)));

        }else{
            Texture2D imgTex = ConvertToTexture2D(img.texture);
            if(imgTex != null){
                StartCoroutine(UploadImage(DuplicateTexture(imgTex)));

            }else{
                Debug.LogError("[FAILED] Converting RawImg to Texture2D has failed");
            }
        }

    }

    private IEnumerator UploadImage(Texture2D texture){

        byte[] bytes = texture.EncodeToPNG();
        var form = new WWWForm();
        // form.AddField("id", "Image");

        // Param 1: form field name, the key for server
        // Param 2: content or the data of the file that will be uploaded, in this case the PNG file
        // Param 3: the name of the file, dont forget the extension
        // Param 4: MIME type, tell the server what kind of file is being uploaded
        // form.AddBinaryData("myimage", bytes, "Image.png", "image/png");
        form.AddBinaryData("p", bytes, "Image.png", "image/png");

        using(var unityWebRequest = UnityWebRequest.Post(ServerURL, form)){
            // unityWebRequest.SetRequestHeader("Authorization", "Token mytoken");

            yield return unityWebRequest.SendWebRequest();

            // if(unityWebRequest.result != UnityWebRequest.Result.Success){
            //     // Debug.Log("[Failed] Uploading Image: " + texture.name + ". Error: " + unityWebRequest.error);
            //     Debug.Log("[ERROR]: " + unityWebRequest.error);

            // }else{
            //     // Debug.Log("[Success] Uploading Image: " + texture.name);
            //     Debug.LogError(unityWebRequest.downloadHandler.text);

            // }

            if(unityWebRequest.responseCode == 200){
                // Debug.Log("[Failed] Uploading Image: " + texture.name + ". Error: " + unityWebRequest.error);

                // Remove the " from the response
                string dirtyResponse = unityWebRequest.downloadHandler.text;
                string cleanResponse = dirtyResponse.Trim('"');

                // Set the response to the Global String Singleton
                SingletonImgID.instance.SetImgID(cleanResponse);

                Debug.Log("[Success] Image ID: " + cleanResponse);
                // Debug.Log("[ERROR]: " + unityWebRequest.error);

            }else{
                // Debug.Log("[Success] Uploading Image: " + texture.name);
                Debug.LogError("[Error] Response Code: " + unityWebRequest.responseCode);

            }

        }

    }

    // Duplicate Texture, Prevent error unreadable Texture + uncompressed the Texture[by creating new texture(duplicate)]
    private Texture2D DuplicateTexture(Texture2D src){

        // Create Empty RendTexture
        RenderTexture rendTexture = RenderTexture.GetTemporary(
            src.width,
            src.height,
            0,
            RenderTextureFormat.Default,
            RenderTextureReadWrite.Linear
        );

        // Copy Texture Source to the Empty RendTex
        Graphics.Blit(src, rendTexture);

        // Save the Prev Active RendTex and set the Curr RendTex to active
        RenderTexture prev = RenderTexture.active;
        RenderTexture.active = rendTexture;

        // Make New Uncompressed Texture
        Texture2D newTexture = new Texture2D(src.width, src.height, TextureFormat.RGBA32, false);
        newTexture.ReadPixels(new Rect(0, 0, rendTexture.width, rendTexture.height), 0, 0);
        newTexture.Apply();

        // Restore the previous active RenderTexture and release the temporary RenderTexture
        RenderTexture.active = prev;
        RenderTexture.ReleaseTemporary(rendTexture);

        return newTexture;

    }

    private Texture2D ConvertToTexture2D(Texture tex){
        
        // Create a RenderTexture
        RenderTexture rendTex = RenderTexture.GetTemporary(
            tex.width,
            tex.height,
            0,
            RenderTextureFormat.Default,
            RenderTextureReadWrite.Linear);

        // Blit the texture to the RenderTexture
        Graphics.Blit(tex, rendTex);

        // Save the previous active RenderTexture and set the current RenderTexture as active
        RenderTexture prev = RenderTexture.active;
        RenderTexture.active = rendTex;

        // Create a new Texture2D and read the pixels from the RenderTexture
        Texture2D texture2D = new Texture2D(tex.width, tex.height, TextureFormat.RGBA32, false);
        texture2D.ReadPixels(new Rect(0, 0, rendTex.width, rendTex.height), 0, 0);
        texture2D.Apply();

        // Restore the previous active RenderTexture and release the temporary RenderTexture
        RenderTexture.active = prev;
        RenderTexture.ReleaseTemporary(rendTex);

        return texture2D;
    }

}
