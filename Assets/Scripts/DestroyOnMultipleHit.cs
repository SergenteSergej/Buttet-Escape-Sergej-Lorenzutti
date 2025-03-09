using UnityEngine;

public class Destroy_On_Multiple_Hit : MonoBehaviour
{
    [SerializeField] int maxHitCount = 10;
    [SerializeField] private bool randomHitCount = true;

    Material _material;
    private float _destroyStepsPercent = 1;

    GameMenager _gameMenager;
    public GameMenager GameMenager { set => _gameMenager = value; }

    private AudioSource _audioSource;

    void Start()
    {
        _material = GetComponent<MeshRenderer>().material;
        _audioSource = GetComponent<AudioSource>();

        if (randomHitCount)
        {
            maxHitCount = Random.Range(1, maxHitCount);
        }

    }

    private void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.GetComponent<Mouve_Bullets>()) return;

        maxHitCount -= 1;

        _destroyStepsPercent = Mathf.Clamp( 1-(1f / maxHitCount),0 ,1);

        _material.color = new Color(1, 1, 1, _destroyStepsPercent);

        //Debug.Log($"{_destroyStepsPercent} -> alpha = {_material.color.a}");

        if (maxHitCount == 0)
        {
            if (_audioSource.clip)
            {
                _audioSource.Play();
                Invoke(nameof(DestroyMe), _audioSource.clip.length);
            }
            else
            {
                Destroy(gameObject);
            }
            _gameMenager.DidDestroyWall();
        }
    }

    private void DestroyMe()
    {
        _gameMenager = null;
        Destroy(gameObject);
    }

}