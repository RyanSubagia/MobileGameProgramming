using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentGenerator : MonoBehaviour
{
    public GameObject[] segment;
    public int maxActiveSegments = 5;

    [SerializeField] int zPos = 50;
    [SerializeField] bool creatingSegment = false;

    private Queue<GameObject> activeSegments = new Queue<GameObject>();

    void Update()
    {
        if(creatingSegment == false)
        {
            creatingSegment = true;
            StartCoroutine(SegmentGen());
        }
    }

    IEnumerator SegmentGen()
    {
        int segmentNum = Random.Range(0, segment.Length);
        GameObject newSegment = Instantiate(segment[segmentNum], new Vector3(0, 0, zPos), Quaternion.identity);
        activeSegments.Enqueue(newSegment);
        if (activeSegments.Count > maxActiveSegments)
        {
            Destroy(activeSegments.Dequeue());
        }
        zPos += 50;
        yield return new WaitForSeconds(10);
        creatingSegment = false;
    }
}
