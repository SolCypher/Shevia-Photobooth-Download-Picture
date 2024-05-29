using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleFileBrowser;
using System.IO;
using UnityEngine.UI;

public class OpenFileExplorer : MonoBehaviour
{
    public RawImage previewIMG;

     public void OpenExplorer()
    {
        // Show a file browser window
        FileBrowser.SetFilters(true, new FileBrowser.Filter("Images", ".jpg", ".jpeg", ".png"));
        FileBrowser.SetDefaultFilter(".png");
        FileBrowser.AddQuickLink("Users", "C:\\Users", null);

        StartCoroutine(ShowLoadDialogCoroutine());
    }

    private IEnumerator ShowLoadDialogCoroutine()
    {
        yield return FileBrowser.WaitForLoadDialog(FileBrowser.PickMode.Files, false, "Select an Image", "Load");

        if (FileBrowser.Success)
        {
            OnFilesSelected(FileBrowser.Result);
        }
    }

    private void OnFilesSelected(string[] filePaths){
        // Print paths of the selected files
		for( int i = 0; i < filePaths.Length; i++ )
			Debug.Log( filePaths[i] );

		// Get the file path of the first selected file
		string filePath = filePaths[0];

		// Read the bytes of the first file via FileBrowserHelpers
		// Contrary to File.ReadAllBytes, this function works on Android 10+, as well
		byte[] bytes = FileBrowserHelpers.ReadBytesFromFile( filePath );

		// Or, copy the first file to persistentDataPath
		string destinationPath = Path.Combine( Application.persistentDataPath, FileBrowserHelpers.GetFilename( filePath ) );
		FileBrowserHelpers.CopyFile( filePath, destinationPath );
        
        LoadImage(destinationPath);
    }

    private void LoadImage(string path)
    {
        byte[] fileData = File.ReadAllBytes(path);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(fileData); // Load image data into the texture
        ApplyTexture(texture);
    }

    private void ApplyTexture(Texture2D texture)
    {
        // Renderer renderer = GetComponent<Renderer>();
        if (previewIMG != null)
        {
            previewIMG.texture = texture;
        }
    }

}
