using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class MainCam : MonoBehaviour
{
    [SerializeField] GameObject player;
    Rigidbody2D rb;
    [SerializeField] float camSpeed;
   
    private void Start()
    {
        rb = player.GetComponent<Rigidbody2D>();
        StartCoroutine(CameraPosition());
    }

    IEnumerator CameraPosition()
    {
        while (true)
        {
            yield return new WaitUntil(() => Vector3.Distance(Camera.main.ViewportToScreenPoint(player.transform.position), transform.position) > 150);
            Debug.Log(Vector3.Distance(Camera.main.ViewportToScreenPoint(player.transform.position), transform.position));
            while (Vector3.Distance(Camera.main.ViewportToScreenPoint(player.transform.position), transform.position) > 150)
            {
                Vector2 _pos = Vector2.Lerp(transform.position, player.transform.position, Time.deltaTime * camSpeed);

                if (math.abs(rb.velocity.y) > math.abs(rb.velocity.x))
                {
                    transform.position = new Vector3(transform.position.x, _pos.y, -10);
                }
                else
                { 
                    transform.position = new Vector3(_pos.x, transform.position.y, -10);
                }
                yield return new WaitForEndOfFrame();
            }
        }
    }
}
