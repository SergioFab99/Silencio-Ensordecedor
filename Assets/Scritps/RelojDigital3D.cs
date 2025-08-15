using TMPro;
using UnityEngine;
using System;
using System.Collections;

[RequireComponent(typeof(TextMeshPro))]
public class RelojDigital3D : MonoBehaviour
{
    [Header("Configuración")]
    public Color colorTexto = Color.red;
    public bool formato24Horas = true;
    public bool actualizarContinuo = true;

    private TextMeshPro _texto3D;
    private DateTime _horaActual;

    void Start()
    {
        _texto3D = GetComponent<TextMeshPro>();
        _texto3D.color = colorTexto;
        _texto3D.alignment = TextAlignmentOptions.Center;

        if (actualizarContinuo)
            StartCoroutine(ActualizarReloj());
        else
            ActualizarHoraUnaVez();
    }

    IEnumerator ActualizarReloj()
    {
        while (true)
        {
            _horaActual = DateTime.Now;
            _texto3D.text = formato24Horas 
                ? _horaActual.ToString("HH:mm:ss") 
                : _horaActual.ToString("hh:mm:ss tt");
            
            yield return new WaitForSeconds(1f);
        }
    }

    void ActualizarHoraUnaVez()
    {
        _texto3D.text = formato24Horas 
            ? DateTime.Now.ToString("HH:mm:ss") 
            : DateTime.Now.ToString("hh:mm:ss tt");
    }
}