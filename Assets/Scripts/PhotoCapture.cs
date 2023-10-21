using System;
using UnityEngine;
using UnityEngine.UI;

public class PhotoCapture : MonoBehaviour
{
    public Camera mainCamera;
    public LayerMask objectLayer;

    // sound  camera operations
    public AudioSource audioSource; 
    public AudioClip[] cameraSounds; 
    public AudioClip showPhotoSound; // polaroid sound played when a photo is shown

    // maximum distance to consider a captured object as valid
    public float maxCaptureDistance; 

    // UI settings to display the captured photo
    public Image photoDisplayUI;
    [SerializeField] private GameObject photoFrame; 
    private bool viewingPhoto; // flag to check if the photo is being viewed
    [SerializeField] private Animator fadingAnimation; // animation for photo fading

    private GameManager gameManager;
    private bool isRightMouseButtonHeld = false;

    private void Start()
    {
        // initialize GameManager and set photo frame to inactive
        gameManager = FindObjectOfType<GameManager>();
        photoFrame.SetActive(false);
    }

    // captures a photo and checks for objects within it
    public void CapturePhoto()
    {
        if (!isRightMouseButtonHeld) return;

        Texture2D photoTexture = CapturePhotoTexture();

        int sampleRate = 5; // rate to sample pixels in the captured photo

        // iterate over pixels in the photo
        for (int x = 0; x < photoTexture.width; x += sampleRate)
        {
            for (int y = 0; y < photoTexture.height; y += sampleRate)
            {
                Ray ray = mainCamera.ScreenPointToRay(new Vector3(x, y, 0));
                RaycastHit hit;
                
                if (Physics.Raycast(ray, out hit, Mathf.Infinity, objectLayer))
                {
                    if (gameManager.animalsToCapture.Contains(hit.collider.gameObject))
                    {
                        float distance = Vector3.Distance(mainCamera.transform.position, hit.collider.gameObject.transform.position);
                        if (distance <= maxCaptureDistance)
                        {
                            // if an object is in the photo and within range
                            Debug.Log(hit.collider.gameObject.name + " is in the frame and within capture distance!");
                            gameManager.MarkAnimalCaptured(hit.collider.gameObject);
                            PlayRandomCaptureSound();
                            ShowPhoto(photoTexture);
                            PlayShowPhotoSound();
                            viewingPhoto = true;
                        }
                        else
                        {
                            Debug.Log(hit.collider.gameObject.name + " is in the frame but too far away to count.");
                        }
                    }
                }
            }
        }
    }

    private void PlayRandomCaptureSound()
    {
        if (cameraSounds.Length > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, cameraSounds.Length);
            audioSource.clip = cameraSounds[randomIndex];
            audioSource.Play();
        }
    }
    
    private void PlayShowPhotoSound()
    {
        if (showPhotoSound != null)
        {
            audioSource.clip = showPhotoSound;
            audioSource.Play();
        }
    }

    // capture the current view as a Texture2D
    private Texture2D CapturePhotoTexture()
    {
        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        mainCamera.targetTexture = renderTexture;
        mainCamera.Render();

        RenderTexture.active = renderTexture;

        Texture2D photoTexture = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        photoTexture.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        photoTexture.Apply();

        mainCamera.targetTexture = null;
        RenderTexture.active = null;
        Destroy(renderTexture);

        return photoTexture;
    }

    // display the captured photo on the UI
    private void ShowPhoto(Texture2D photoTexture)
    {
        Sprite photoSprite = Sprite.Create(photoTexture, new Rect(0, 0, photoTexture.width, photoTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
        photoDisplayUI.sprite = photoSprite;

        viewingPhoto = true;
        UpdatePhotoFrameState();
        
        fadingAnimation.Play("PhotoFade");

        HideAfterDelay();
    }

    // remove the displayed photo after a delay
    private void HideAfterDelay()
    {
        Invoke("RemovePhoto", 3f); 
    }

    private void RemovePhoto() 
    {
        viewingPhoto = false;
        UpdatePhotoFrameState();
    }

    private void UpdatePhotoFrameState()
    {
        photoFrame.SetActive(viewingPhoto);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (!viewingPhoto)
            {
                isRightMouseButtonHeld = true;
            }
        }

        if (Input.GetMouseButtonUp(1))
        {
            isRightMouseButtonHeld = false;
        }

        // if left mouse button is clicked while right mouse button is held, capture a photo
        if (Input.GetKeyDown(KeyCode.Mouse0) && isRightMouseButtonHeld)
        {
            PlayRandomCaptureSound(); 
            CapturePhoto();
        }
    }
}
