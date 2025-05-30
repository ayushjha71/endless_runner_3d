using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PictureUploaderHandler : MonoBehaviour
{
    [SerializeField] private RawImage displayImage;
    [SerializeField] private Button uploadButton;
    [SerializeField] private Button cameraButton;

    private Texture2D selectedTexture;
    private string savedImagePath;

    public RawImage DisPlayImage => displayImage;

    private void Start()
    {
        // Set the path where we'll save the image
        savedImagePath = Path.Combine(Application.persistentDataPath, "savedImage.png");

        // Load saved image if it exists
        LoadSavedImage();

        // Set up button listeners
        uploadButton.onClick.AddListener(UploadImage);
    }

    private void UploadImage()
    {
        // Check if we're running on Android
        if (Application.platform == RuntimePlatform.Android)
        {
            // Check permission first - returns bool in some versions
            bool hasPermission = NativeGallery.CheckPermission(
                NativeGallery.PermissionType.Read,
                NativeGallery.MediaType.Image
            );

            if (hasPermission)
            {
                // Permission granted, proceed
                NativeGallery.GetImageFromGallery((path) =>
                {
                    if (path != null)
                    {
                        // Create Texture from selected image
                        Texture2D texture = NativeGallery.LoadImageAtPath(path, -1, false);
                        if (texture == null)
                        {
                            Debug.Log("Couldn't load texture from " + path);
                            return;
                        }

                        ProcessSelectedImage(texture);
                    }
                }, "Select a PNG image", "image/png");
            }
            else
            {
                Debug.LogError("Permission denied for accessing gallery");
                // Optionally show a message to the user or request permission
            }
        }
        else
        {
            Debug.LogWarning("Image picking only works on Android");
        }
    }

    private void ProcessSelectedImage(Texture2D texture)
    {
        // Free memory from previous texture if it exists
        if (selectedTexture != null)
        {
            Destroy(selectedTexture);
        }

        selectedTexture = texture;
        DisPlayImage.texture = selectedTexture;

        // Save the image
        SaveImage(texture);
    }

    private void SaveImage(Texture2D texture)
    {
        try
        {
            // Convert texture to PNG bytes
            byte[] bytes = texture.EncodeToPNG();

            // Save to persistent data path
            File.WriteAllBytes(savedImagePath, bytes);

            Debug.Log("Image saved to: " + savedImagePath);
        }
        catch (Exception e)
        {
            Debug.LogError("Error saving image: " + e.Message);
        }
    }

    private void LoadSavedImage()
    {
        if (File.Exists(savedImagePath))
        {
            try
            {
                // Load the image data
                byte[] bytes = File.ReadAllBytes(savedImagePath);

                // Create new texture
                Texture2D loadedTexture = new Texture2D(2, 2);
                loadedTexture.LoadImage(bytes);

                // Display it
                ProcessSelectedImage(loadedTexture);
            }
            catch (Exception e)
            {
                Debug.LogError("Error loading saved image: " + e.Message);
            }
        }
        else
        {
            Debug.Log("No saved image found at " + savedImagePath);
        }
    }

    private void OnDestroy()
    {
        // Clean up
        if (selectedTexture != null)
        {
            Destroy(selectedTexture);
        }
    }
}