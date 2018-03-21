using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BouyanceObject : MonoBehaviour {

    public enum CalculateType
    {
        None,
        Mass,
        Density,
        Volume
    }

    [SerializeField]
    private CalculateType autoCalculate = CalculateType.None;

    [SerializeField]
    private float density = 0.75f;
    
    private new Rigidbody rigidbody;


    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();

        CalculateAttributes();
    }

    public void CalculateAttributes()
    {
        if (autoCalculate == CalculateType.None)
            return;


        float volume = GetComponent<MeshFilter>().mesh.Volume() * transform.localScale.Volume();

        switch (autoCalculate)
        {
            case CalculateType.Density:
                density = rigidbody.mass / volume;
                break;

            case CalculateType.Mass:
                rigidbody.mass = density * volume;
                break;

            case CalculateType.Volume:
                transform.localScale = Vector3.one * Mathf.Pow( (rigidbody.mass / density) , 1f/3f);
                break;
        }
    }

    public void SetMassVolume(float mass, float volume)
    {
        autoCalculate = CalculateType.Density;

        rigidbody = GetComponent<Rigidbody>();

        rigidbody.mass = mass;
        transform.localScale = Vector3.one * Mathf.Pow(volume, 1f / 3f);
    }

    public void SetMassDensity(float mass, float density)
    {
        autoCalculate = CalculateType.Volume;
        rigidbody = GetComponent<Rigidbody>();

        this.rigidbody.mass = mass;
        this.density = density;
    }

    public void SetVolumeDensity(float volume, float density)
    {
        autoCalculate = CalculateType.Mass;

        transform.localScale = Vector3.one * Mathf.Pow(volume, 1f / 3f);
        this.density = density;
    }
}
