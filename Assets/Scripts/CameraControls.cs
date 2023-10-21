using UnityEngine;

public class CameraControls : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private float _targetFOV = 45.0f;
    [SerializeField] private float _lerpSpeed = 5.0f;

    [Header("UI Settings")]
    [SerializeField] private float _uiFadeSpeed = 500f;
    [SerializeField] private GameObject _cameraUI;
    [SerializeField] private GameObject _gameUI;

    private Camera _camera;
    private float _originalFOV;
    private bool _isUIVisible = false;

    private CanvasGroup _uiCanvasGroup;
    private float _targetAlpha = 0.0f; // target alpha for UI fade

    void Start()
    {
        _camera = GetComponent<Camera>();
        _originalFOV = _camera.fieldOfView;
        _uiCanvasGroup = _cameraUI.GetComponent<CanvasGroup>();
        _uiCanvasGroup.alpha = 0.0f; // set initial alpha to be transparent
        _cameraUI.SetActive(false);
    }

    void Update()
    {
        // handle camera zoom and UI activation based on right mouse button input
        if (Input.GetMouseButton(1))
        {
            _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, _targetFOV, Time.deltaTime * _lerpSpeed);

            if (!_isUIVisible)
            {
                _targetAlpha = 1.0f;
                _gameUI.SetActive(false);
                _cameraUI.SetActive(true);
                _isUIVisible = true;
            }
        }
        else
        {
            _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, _originalFOV, Time.deltaTime * _lerpSpeed);

            if (_isUIVisible)
            {
                _targetAlpha = 0.0f;
                _isUIVisible = false;
            }
        }

        // UI fading
        _uiCanvasGroup.alpha = Mathf.Lerp(_uiCanvasGroup.alpha, _targetAlpha, Time.deltaTime * _uiFadeSpeed);

        // deactivate the UI when it's fully hidden
        if (Mathf.Approximately(_uiCanvasGroup.alpha, 0f))
        {
            _uiCanvasGroup.alpha = 0.0f;
            _gameUI.SetActive(true);
            _cameraUI.SetActive(false);
        }
    }
}
