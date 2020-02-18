using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slide : ActivatorListener
{
    public enum Direction {Up, Down, Left, Right, Depth};
    private bool open = false;
    private Vector3 stopPosition;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private SoundPlayer soundPlayer;
    private bool isMute = true;

    public Direction direction;
    [Range(0,10)]
    public float speed;
    public float distance;
    public AudioClip sound;

    public void Start()
    {
        soundPlayer = GetComponent<SoundPlayer>();
        startPosition = transform.position;
        targetPosition = startPosition;
        switch (direction)
        {
            case Direction.Up:
                stopPosition = startPosition + (Vector3.up * distance);
                break;
            case Direction.Down:
                stopPosition = startPosition - (Vector3.up * distance);
                break;
            case Direction.Left:
                stopPosition = startPosition + (Vector3.left * distance);
                break;
            case Direction.Right:
                stopPosition = startPosition - (Vector3.left * distance);
                break;
            case Direction.Depth:
                stopPosition = startPosition + (Vector3.forward * distance); 
                break;
        }
        isMute = false;
    }

    public override void OnActivate()
    {
        if (!open)
        {
            open = true;
            targetPosition = stopPosition;
            if (direction == Direction.Depth)
                GetComponent<Collider2D>().enabled = false;
            if (soundPlayer != null && !isMute)
                soundPlayer.PlaySoundAtLocation(sound, 1f);
        }
    }

    public override void OnDeactivate()
    {
        if (open)
        {
            open = false;
            targetPosition = startPosition;
            if (direction == Direction.Depth)
            {
                GetComponent<Collider2D>().enabled = true;
            }
            if (soundPlayer != null && !isMute)
                soundPlayer.PlaySoundAtLocation(sound, 1f);
        }
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(this.transform.position, targetPosition, Time.deltaTime * speed);
    }
}
