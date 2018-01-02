﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasloController : MonoBehaviour {

    public float minValue = 10f;
    public float maxValue = 100f;

    public float minimum_time_spawned = 3f;
    public float maximum_time_spawned = 10f;

    private float _value;
    private float _alive_time;

    private AudioSource audio;

    private void Start()
    {
        _alive_time = Random.Range(minimum_time_spawned, maximum_time_spawned);
        _value = Random.Range(minValue, maxValue);

        audio = GetComponent<AudioSource>();

        Destroy(gameObject, _alive_time);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player") {
            audio.Play();
            Destroy(gameObject);
            CurrencyController.Instance.IncreaseValue(_value);
        }
    }
}
