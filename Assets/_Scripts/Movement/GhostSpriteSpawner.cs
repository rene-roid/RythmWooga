using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostSpriteSpawner : MonoBehaviour
{
    [Header("Ghost Sprites")]
    public GameObject SpritePrefab;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        
    }
    
    public void SpawnGhost()
    {
        GameObject newGhost = Instantiate(SpritePrefab, transform.position, Quaternion.identity);

        newGhost.GetComponent<SpriteRenderer>().sprite = _spriteRenderer.sprite;
        newGhost.GetComponent<SpriteRenderer>().flipX = _spriteRenderer.flipX;

        Destroy(newGhost, newGhost.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length);
    }
}