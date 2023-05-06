using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class BulletCasing : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private AudioClip shellSound;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        audioSource.Play();
        StartCoroutine("DestroyCasing");
    }

    IEnumerator DestroyCasing()
    {
        yield return new WaitForSeconds(30f);
        Destroy(gameObject);
    }
}
