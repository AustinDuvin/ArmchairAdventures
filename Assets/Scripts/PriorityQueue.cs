using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PriorityQueue : MonoBehaviour
{
    private List<GameObject> priority = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        //priority = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Adds new data to the priority queue
    public void Enqueue(GameObject data)
    {
        //Node vert = data.GetComponent<Node>();

        priority.Add(data);

        int index = (priority.Count - 1);

        while (priority[index].GetComponent<Node>().H < priority[(index - 1) / 2].GetComponent<Node>().H)
        {
            int parentIndex = (index - 1) / 2;

            GameObject temp = priority[index];

            priority[index] = priority[parentIndex];

            priority[parentIndex] = temp;

            index = parentIndex;
        }
    }

    //Removes data from the priority queue
    public GameObject Dequeue()
    {
        GameObject removed = priority[0];

        priority[0] = priority[priority.Count - 1];

        int index = 0;

        priority.RemoveAt(priority.Count - 1);

        bool bigger = true;

        while (bigger == true)
        {

            int leftChild;

            int rightChild;

            leftChild = (2 * index) + 1;

            rightChild = (2 * index) + 2;

            if (leftChild < priority.Count && rightChild < priority.Count)
            {
                if (priority[index].GetComponent<Node>().H > priority[leftChild].GetComponent<Node>().H || priority[index].GetComponent<Node>().H > priority[rightChild].GetComponent<Node>().H)
                {
                    if (priority[leftChild].GetComponent<Node>().H <= priority[rightChild].GetComponent<Node>().H)
                    {
                        GameObject temp = priority[index];

                        priority[index] = priority[leftChild];

                        priority[leftChild] = temp;

                        index = leftChild;
                    }

                    else
                    {
                        GameObject temp = priority[index];

                        priority[index] = priority[rightChild];

                        priority[rightChild] = temp;

                        index = rightChild;
                    }
                }

                else
                {
                    bigger = false;
                }
            }

            else if (leftChild < priority.Count && rightChild >= priority.Count)
            {
                if (priority[index].GetComponent<Node>().H > priority[leftChild].GetComponent<Node>().H)
                {
                    GameObject temp = priority[index];

                    priority[index] = priority[leftChild];

                    priority[leftChild] = temp;

                    index = leftChild;
                }

                else
                {
                    bigger = false;
                }
            }

            else
            {
                bigger = false;
            }
        }

        return removed;
    }

    //Checks the first piece of data
    //in the priority queue
    public GameObject Peek()
    {
        return priority[0];
    }

    //Checks to see if the
    //priority queue is empty
    bool IsEmpty()
    {
        if (priority.Count == 0)
        {
            return true;
        }

        else
        {
            return false;
        }
    }
}
