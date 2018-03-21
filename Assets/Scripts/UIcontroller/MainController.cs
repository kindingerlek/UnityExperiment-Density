using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainController : MonoBehaviour {
    public Toggle toggleMyBlock;
    public Toggle toggleMaterial;
    public Dropdown dropdownMaterial;

    public Slider sliderMass;
    public InputField inputMass;

    public Slider sliderVolume;
    public InputField inputVolume;

    public Slider sliderDensity;
    public Text labelDensity;

    private float density;
    private float mass;
    private float volume;

    [SerializeField]
    private List<BouyanceObject> bouyanceObjs = new List<BouyanceObject>();

    void Start()
    {
        bouyanceObjs.AddRange(GameObject.FindObjectsOfType<BouyanceObject>());

        toggleMyBlock.onValueChanged.AddListener(delegate { OnMaterialToggleChange(); });
        toggleMaterial.onValueChanged.AddListener(delegate { OnMaterialToggleChange(); });

        dropdownMaterial.onValueChanged.AddListener(delegate { OnMaterialDropDownChange(); });

        sliderMass.onValueChanged.AddListener(delegate { OnMassSliderChange(); });
        inputMass.onEndEdit.AddListener(delegate { OnMassInputEnter(); });
        
        sliderVolume.onValueChanged.AddListener(delegate { OnVolumeSliderChange(); });
        inputVolume.onEndEdit.AddListener(delegate { OnVolumeInputEnter(); });

        mass = 1f;
        density = 1f;
        volume = 1f;

        UpdateUI();
    }

    void UpdateBouyanceObjects()
    {
        foreach(var obj in bouyanceObjs)
        {
            obj.SetMassVolume(mass, volume);
        }
    }

    void UpdateUI()
    {
        sliderDensity.value = density;
        sliderVolume.value = volume;
        sliderMass.value = mass;

        labelDensity.text = string.Format("{0:0.00 Kg/L}", density);
        inputMass.text = string.Format("{0:0.00}", mass);
        inputVolume.text = string.Format("{0:0.00}", volume);

        Color c = sliderMass.handleRect.GetComponent<Image>().color;

        c.a = (mass < sliderMass.minValue || mass > sliderMass.maxValue) ? 0.5f : 1f;
        sliderMass.handleRect.GetComponent<Image>().color = c;

        c.a = (volume < sliderVolume.minValue || volume > sliderVolume.maxValue) ? 0.5f : 1f;
        sliderVolume.handleRect.GetComponent<Image>().color = c;

        UpdateBouyanceObjects();

        dropdownMaterial.gameObject.SetActive(toggleMaterial.isOn);
    }

    void OnMaterialToggleChange()
    {
        dropdownMaterial.gameObject.SetActive(toggleMaterial.isOn);
        OnMaterialDropDownChange();
    }

    void OnMaterialDropDownChange()
    {
        float[] densityArray = { 0.15f, 0.40f, 0.92f, 2.00f, 2.70f };
        density = densityArray[dropdownMaterial.value];
        mass = calculateMass();


        UpdateUI();
    }

    void OnMassSliderChange()
    {
        mass = sliderMass.value;
        inputMass.text = string.Format("{0:0.00}", mass);

        if (toggleMaterial.isOn)
            volume = calculateVolume();
        else
            density = calculateDensity();

        UpdateUI();
    }

    void OnVolumeSliderChange()
    {
        volume = sliderVolume.value;

        if (toggleMaterial.isOn)
            mass = calculateMass();
        else
            density = calculateDensity();

        inputVolume.text = string.Format("{0:0.00}", volume);
        UpdateUI();
    }

    void OnMassInputEnter()
    {
        float f;

        if(float.TryParse(inputMass.text,out f))
        {
            f = Mathf.Clamp(f, 1f, 10f);

            volume = f;
            UpdateUI();
        }
    }

    void OnVolumeInputEnter()
    {
        float f;

        if (float.TryParse(inputVolume.text, out f))
        {
            f = Mathf.Clamp(f, 1f, 10f);

            volume = f;
            UpdateUI();
        }
    }

    private float calculateMass()
    {
        return density * volume;
    }

    private float calculateVolume()
    {
        return mass / density;
    }

    private float calculateDensity()
    {
        return mass / volume;
    }
}
