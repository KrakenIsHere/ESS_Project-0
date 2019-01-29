using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuOptions : MonoBehaviour {

	private ConfigGameReader configReader;

	[Header("Options Panels")]
	public GameObject TabGeneral;
	public GameObject TabGraphic;
	public GameObject TabControls;

	[Header("General")]
	public Slider volumeSlider;
	public Slider sensitivitySlider;

	[Header("Graphic")]
	public Dropdown resolution;
	public Dropdown quality;
	public Dropdown antialiasing;
	public Dropdown anisotropic;
	public Dropdown textureQuality;
	public Dropdown blendWeight;
	public Dropdown vSync;
	public Dropdown shadowResolution;
	public Dropdown shadowCascades;
	public Dropdown shadowProjecion;
	public Slider shadowDistance;
	public Toggle fullscreen;
	public Text shadowDistanceText;

	private int qualityLevel;
	private int currQuality;

	private int resolutionLevel;
	private int antiaLevel;
	private int anisoLevel;
	private int textureQualityLevel;
	private int blendWeightLevel;
	private int vSyncLevel;
	private int shadowResLevel;
	private int shadowCasLevel;
	private int shadowProjLevel;

	private bool m_resolution;
	private bool m_antialiasing;
	private bool m_anisotropic;
	private bool m_textureQuality;
	private bool m_blendWeight;
	private bool m_vSync;
	private bool m_shadowResolution;
	private bool m_shadowCascades;
	private bool m_shadowProjecion;
	private bool m_fullscreen;
	private bool m_shadowDistanceText;

	void Start()
	{
		configReader = GameObject.Find ("GAMEMANAGER").GetComponent<ConfigGameReader> ();
		resolution.onValueChanged.AddListener (delegate{ OnResolutionChange (resolution);});
		quality.onValueChanged.AddListener (delegate{ OnQualityChange (quality);});
		antialiasing.onValueChanged.AddListener (delegate{ OnAntialiasingChange (antialiasing);});
		anisotropic.onValueChanged.AddListener (delegate{ OnAnisotropicChange (anisotropic);});
		textureQuality.onValueChanged.AddListener (delegate{ OnTextureQualityChange (textureQuality);});
		blendWeight.onValueChanged.AddListener (delegate{ OnBlendWeightChange (blendWeight);});
		vSync.onValueChanged.AddListener (delegate{ OnVSyncChange (vSync);});
		shadowResolution.onValueChanged.AddListener (delegate{ OnShadowResolutionChange (shadowResolution);});
		shadowCascades.onValueChanged.AddListener (delegate{ OnShadowCascadesChange (shadowCascades);});
		shadowProjecion.onValueChanged.AddListener (delegate{ OnShadowProjectionChange (shadowProjecion);});

		UpdateSettings ();
	}

	void UpdateSettings()
	{
		string res = Screen.currentResolution.ToString();
		string[] resSplit = res.Split (new char[] { 'x', '@' });
		string height = resSplit [0].Trim ();
		string width = resSplit [1].Trim ();
		string currentResolution = (height + "x" + width);

		switch (currentResolution) {
		case "1920x1080":
			resolution.value = 0;
			break;
		case "1600x900":
			resolution.value = 1;
			break;
		case "1366x768":
			resolution.value = 2;
			break;
		case "1280x720":
			resolution.value = 3;
			break;
		case "1024x768":
			resolution.value = 4;
			break;
		case "800x600":
			resolution.value = 5;
			break;
		}
		switch ((int)QualitySettings.antiAliasing) {
		case 0:
			antialiasing.value = 0;
			break;
		case 2:
			antialiasing.value = 1;
			break;
		case 4:
			antialiasing.value = 2;
			break;
		case 8:
			antialiasing.value = 3;
			break;
		}
		switch ((int)QualitySettings.shadowCascades) {
		case 0:
			shadowCascades.value = 0;
			break;
		case 2:
			shadowCascades.value = 1;
			break;
		case 4:
			shadowCascades.value = 2;
			break;
		}

		if (quality.value != 3) {
			quality.value = QualitySettings.GetQualityLevel ();
		}
		currQuality = QualitySettings.GetQualityLevel ();
		anisotropic.value =  (int)QualitySettings.anisotropicFiltering;
		textureQuality.value = (int)QualitySettings.masterTextureLimit;
		blendWeight.value = (int)QualitySettings.blendWeights;
		vSync.value = (int)QualitySettings.vSyncCount;
		shadowResolution.value = (int)QualitySettings.shadowResolution;
		shadowProjecion.value = (int)QualitySettings.shadowProjection;
		shadowDistance.value = QualitySettings.shadowDistance;
		fullscreen.isOn = Screen.fullScreen;
		if (configReader && configReader.ContainsSection ("Game")) {
			volumeSlider.value = float.Parse (configReader.Deserialize ("Game", "Volume"));
			sensitivitySlider.value = float.Parse (configReader.Deserialize ("Game", "Sensitivity"));
		}
	}

	void Update()
	{
		shadowDistanceText.text = shadowDistance.value.ToString ();
	}

	public void OnResolutionChange(Dropdown dd)
	{
		resolutionLevel = dd.value;
		m_resolution = true;
	}

	public void OnQualityChange(Dropdown dd)
	{
		qualityLevel = dd.value;
	}

	public void OnAntialiasingChange(Dropdown dd)
	{
		switch (dd.value) {
		case 0:
			antiaLevel = 0;
			break;
		case 1:
			antiaLevel = 2;
			break;
		case 2:
			antiaLevel = 4;
			break;
		case 3:
			antiaLevel = 8;
			break;
		}
		m_antialiasing = true;
	}

	public void OnAnisotropicChange(Dropdown dd)
	{
		anisoLevel = dd.value;
		m_anisotropic = true;
	}

	public void OnTextureQualityChange(Dropdown dd)
	{
		textureQualityLevel = dd.value;
		m_textureQuality = true;
	}

	public void OnBlendWeightChange(Dropdown dd)
	{
		blendWeightLevel = dd.value;
		m_blendWeight = true;
	}

	public void OnVSyncChange(Dropdown dd)
	{
		vSyncLevel = dd.value;
		m_vSync = true;
	}

	public void OnShadowResolutionChange(Dropdown dd)
	{
		shadowResLevel = dd.value;
		m_shadowResolution = true;
	}

	public void OnShadowCascadesChange(Dropdown dd)
	{
		switch (dd.value) {
		case 0:
			shadowCasLevel = 0;
			break;
		case 1:
			shadowCasLevel = 2;
			break;
		case 2:
			shadowCasLevel = 4;
			break;
		}
		m_shadowCascades = true;
	}

	public void OnShadowProjectionChange(Dropdown dd)
	{
		shadowProjLevel = dd.value;
		m_shadowProjecion = true;
	}

	void OnDestroy()
	{
		resolution.onValueChanged.RemoveAllListeners();
		quality.onValueChanged.RemoveAllListeners();
		antialiasing.onValueChanged.RemoveAllListeners();
		anisotropic.onValueChanged.RemoveAllListeners();
		textureQuality.onValueChanged.RemoveAllListeners();
		blendWeight.onValueChanged.RemoveAllListeners();
		vSync.onValueChanged.RemoveAllListeners();
		shadowResolution.onValueChanged.RemoveAllListeners();
		shadowCascades.onValueChanged.RemoveAllListeners();
		shadowProjecion.onValueChanged.RemoveAllListeners();
	}

	public void ApplyAllSettings () {
		if (TabGraphic.activeSelf && !TabGeneral.activeSelf && !TabControls.activeSelf) {
			if (currQuality == qualityLevel) {
				if (m_resolution) {
					switch (resolutionLevel) {
					case 0:
						Screen.SetResolution (1920, 1080, fullscreen.isOn);
						break;
					case 1:
						Screen.SetResolution (1600, 900, fullscreen.isOn);
						break;
					case 2:
						Screen.SetResolution (1366, 768, fullscreen.isOn);
						break;
					case 3:
						Screen.SetResolution (1280, 720, fullscreen.isOn);
						break;
					case 4:
						Screen.SetResolution (1024, 768, fullscreen.isOn);
						break;
					case 5:
						Screen.SetResolution (800, 600, fullscreen.isOn);
						break;
					}
					m_resolution = false;
				}
				if (m_anisotropic) {
					switch (anisoLevel) {
					case 0:
						QualitySettings.anisotropicFiltering = AnisotropicFiltering.ForceEnable;
						break;
					case 1:
						QualitySettings.anisotropicFiltering = AnisotropicFiltering.Enable;
						break;
					case 2:
						QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable;
						break;
					}
					m_anisotropic = false;
				}
				if (m_blendWeight) {
					switch (blendWeightLevel) {
					case 0:
						QualitySettings.blendWeights = BlendWeights.OneBone;
						break;
					case 1:
						QualitySettings.blendWeights = BlendWeights.TwoBones;
						break;
					case 2:
						QualitySettings.blendWeights = BlendWeights.FourBones;
						break;
					}
					m_blendWeight = false;
				}
				if (m_shadowResolution) {
					switch (shadowResLevel) {
					case 0:
						QualitySettings.shadowResolution = ShadowResolution.Low;
						break;
					case 1:
						QualitySettings.shadowResolution = ShadowResolution.Medium;
						break;
					case 2:
						QualitySettings.shadowResolution = ShadowResolution.High;
						break;
					case 3:
						QualitySettings.shadowResolution = ShadowResolution.VeryHigh;
						break;
					}
					m_shadowResolution = false;
				}
				if (m_shadowProjecion) {
					switch (shadowProjLevel) {
					case 0:
						QualitySettings.shadowProjection = ShadowProjection.CloseFit;
						break;
					case 1:
						QualitySettings.shadowProjection = ShadowProjection.StableFit;
						break;
					}
					m_shadowProjecion = false;
				}
				if (m_antialiasing) {
					QualitySettings.antiAliasing = antiaLevel;
					m_antialiasing = false;
				}
				if (m_textureQuality) {
					QualitySettings.masterTextureLimit = textureQualityLevel;
					m_textureQuality = false;
				}
				if (m_vSync) {
					QualitySettings.vSyncCount = vSyncLevel;
					m_vSync = false;
				}
				if (m_shadowCascades) {
					QualitySettings.shadowCascades = shadowCasLevel;
					m_shadowCascades = false;
				}

				QualitySettings.shadowDistance = shadowDistance.value;
				quality.value = 3;
			} else {
				QualitySettings.SetQualityLevel (qualityLevel, fullscreen.isOn);
				quality.value = qualityLevel;
				currQuality = qualityLevel;
			}

			StartCoroutine (ApplySettings ());
		}
		if (configReader && !TabGraphic.activeSelf && TabGeneral.activeSelf && !TabControls.activeSelf) {
			configReader.Serialize ("Game", "Volume", volumeSlider.value.ToString());
			configReader.Serialize ("Game", "Sensitivity", sensitivitySlider.value.ToString());
			configReader.Refresh ();
		}

		if (configReader && !configReader.isMainMenu && !TabGraphic.activeSelf && !TabGeneral.activeSelf && TabControls.activeSelf) {
			configReader.gameObject.GetComponent<InputManager>().RefreshInputs ();
		}
	}

	IEnumerator ApplySettings()
	{
		yield return new WaitForSeconds (0.5f);
		UpdateSettings ();
	}
}