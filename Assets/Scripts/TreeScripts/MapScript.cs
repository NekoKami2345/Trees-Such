/*  Programmer: Tyler Heald
 *  tyler@waveofdisdain.com
 *  
 *  This script contains all the functions and data to create and handle a world map for _______
 *  
 *  It works kind of like Kruskal's alg. Instead of adding edges to a priority queue based on weight,
 *  all edges will randomly be added to a normal queue, and the top will be popped, and added to the tree
 *  if either point it contains isn't already in a tree.
 *  
 *  Classes:
 *      Edge:
 *          Contains Node point1, Node point2
 *      Node:
 *          contains int color
 *              color will be used to determine if an edge would join two trees
 *              0 is no color, and anything above is some arbitrary color
 *  
 *  Functions:
 *      MapGen():
 *          Adds all possible edges to a queue in a random order, then repeatedly pops the top,
 *          adding the edge to the spanning tree if either point the edge contains is not yet in
 *          a tree
 *          
 *  Variables/Data:
 *      bool[width*length] inTree:
 *          A boolean array to tell if a given point is in a tree
 *      
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapScript : MonoBehaviour {

    //Map node item
    public Object mapNode;
    //Map edge item
    public Object mapEdge;

    //Testing variables
    public int length = 5;
    public int width = 5;

    //Variables/Data
    //UnionFind structure to show which nodes have a path to the endVert.
    //When a path is found, all elements will be compressed, and -1 will represent
    //they have a path
    int[] unionFind;

    //List of all nodes, so that all references to nodes refer to the same set, rather than
    //making new data
    List<Node> nodeList;
    //List of edges, for printing it out in a visual form
    List<Edge> edgeList;

	// Use this for initialization
	void Start () {
        //Initializing the unionFind and the Node list
        unionFind = new int[length * width];
        nodeList = new List<Node>(length*width);
        //Initializing the edgeList
        edgeList = new List<Edge>(length * width - 1);
        
        //Filling the nodeList so each Node has its ID as its index
        //Fill unionFind with all 1
        for(int i = 0; i < length*width; i ++)
        {
            unionFind[i] = 1;
            nodeList.Add(new Node(i));
        }

        int endVertex = MapGen();
        Debug.Log("Returned endVert is: " + endVertex);
        PrintMap(endVertex);
	}

    /****   FUNCTIONS       ****/
    int MapGen()
    {
        //Get a random end vertex
        int endVert = (int)Random.Range(0, (length * width));
        //DEBUG
        Debug.Log("endVert: " + endVert);
        //Adjusting unionFind so this vert is the end
        unionFind[endVert] = -1;
        //DEBUG
        Debug.Log("unionFind[endVert]: " + unionFind[endVert]);
        //Getting the x and y of the end vertex
        int endX = endVert % length;
        int endY = endVert / length;

        //Running until all elements have some path to the endVert
        while (System.Array.Exists<int>(unionFind, element => element != -1))
        {
            //Finding the first node not connected to the endVert
            int firstNoPath = System.Array.FindIndex<int>(unionFind, element => element != -1);
            //DEBUG
            Debug.Log("firstNoPath: " + firstNoPath);
            //Getting the node for this item
            Node firstNode = nodeList[firstNoPath];
            //Setting a reference for the node it came from
            Node prevNode;

            //Getting the x and y of this node for finding next node purposes
            int noPathX = firstNoPath % length;
            int noPathY = firstNoPath / length;
            //DEBUG
            Debug.Log("firstNoPathX: " + noPathX + "\nfirstNoPathY: " + noPathY);

            //Choosing a random direction to go
            //up = 1
            //down = 2
            //left = 3
            //right = 4
            int direction = (int)Random.Range(1, 5);
            //DEBUG
            Debug.Log("Direction for firstNoPath: ");
            switch(direction)
            {
                case 1:
                    Debug.Log("up");
                    break;
                case 2:
                    Debug.Log("down");
                    break;
                case 3:
                    Debug.Log("left");
                    break;
                case 4:
                    Debug.Log("right");
                    break;
            }
            //Just gonna swap direction if it can't go that way
            if (noPathX == 0 && direction == 3)
            {
                direction = 4;
            }
            else if (noPathX == length - 1 && direction == 4)
            {
                direction = 3;
            }
            if (noPathY == 0 && direction == 1)
            {
                direction = 2;
            }
            else if (noPathY == width - 1 && direction == 2)
            {
                direction = 1;
            }
            //Performing the first path addition
            //Checking the next node on the path
            //Graph edge cases have already been checked, so no need to worry about non-existence(probably)
            if (direction == 1)
            {
                firstNode.NextNode = nodeList[firstNoPath - length];
                //Checking if there is a path through there already
                if (unionFind[firstNoPath - length] == -1)
                {
                    //Adding the edge to the edgeList
                    Debug.Log("Adding edge");
                    edgeList.Add(new Edge(firstNode.NodeID, firstNode.NextNode.NodeID));

                    //DEBUG
                    Debug.Log("Continuing1");
                    //if there is, set this one to reflect that, and continue to next loop
                    unionFind[firstNoPath] = -1;
                    continue;
                }
            }
            else if (direction == 2)
            {
                firstNode.NextNode = nodeList[firstNoPath + length];
                //Checking if there is a path through there already
                if (unionFind[firstNoPath + length] == -1)
                {
                    //Adding the edge to the edgeList
                    Debug.Log("Adding edge");
                    edgeList.Add(new Edge(firstNode.NodeID, firstNode.NextNode.NodeID));

                    Debug.Log("Continuing2");
                    //if there is, set this one to reflect that, and continue to next loop
                    unionFind[firstNoPath] = -1;
                    continue;
                }
            }
            else if (direction == 3)
            {
                firstNode.NextNode = nodeList[firstNoPath - 1];
                //Checking if there is a path through there already
                if (unionFind[firstNoPath - 1] == -1)
                {
                    //Adding the edge to the edgeList
                    Debug.Log("Adding edge");
                    edgeList.Add(new Edge(firstNode.NodeID, firstNode.NextNode.NodeID));

                    Debug.Log("Continuing3");
                    //if there is, set this one to reflect that, and continue to next loop
                    unionFind[firstNoPath] = -1;
                    continue;
                }
            }
            else if (direction == 4)
            {
                firstNode.NextNode = nodeList[firstNoPath + 1];
                //Checking if there is a path through there already
                if (unionFind[firstNoPath + 1] == -1)
                {
                    //Adding the edge to the edgeList
                    Debug.Log("Adding edge");
                    edgeList.Add(new Edge(firstNode.NodeID, firstNode.NextNode.NodeID));

                    Debug.Log("Continuing4");
                    //if there is, set this one to reflect that, and continue to next loop
                    unionFind[firstNoPath] = -1;
                    continue;
                }
            }

            //Setting the first node as the prevNode
            prevNode = firstNode;
            //Setting a temporary Node and entering a while loop to keep running the same kind of thing until
            //a path is found
            Node temp = firstNode.NextNode;
            //DEBUG
            Debug.Log("Second node in path: " + temp.NodeID);

            //Operating until firstNode has found a path to endVert
            while (unionFind[temp.NodeID] != -1)
            {
                //Getting a new random direction
                direction = (int)Random.Range(1, 5);
                //DEBUG
                Debug.Log("Temp direction: ");
                switch (direction)
                {
                    case 1:
                        Debug.Log("up");
                        break;
                    case 2:
                        Debug.Log("down");
                        break;
                    case 3:
                        Debug.Log("left");
                        break;
                    case 4:
                        Debug.Log("right");
                        break;
                }
                //Finding the x and y of the temp node
                int tempX = temp.NodeID % length;
                int tempY = temp.NodeID / length;
                //*******************Might want to change these for short circuiting purposes**********//
                //Just gonna swap direction if it can't go that way
                if (tempX == 0 && direction == 3)
                {
                    direction = 4;
                }
                else if (tempX == length - 1 && direction == 4)
                {
                    direction = 3;
                }
                if (tempY == 0 && direction == 1)
                {
                    direction = 2;
                }
                else if (tempY == width - 1 && direction == 2)
                {
                    direction = 1;
                }
                //Note that the direction may be backwards on the path. Each if statement below will check that,
                //and use continue to reset the loop if that is the case, so a new direction can be found

                //Checking the next node on the path
                //Graph edge cases have already been checked, so no need to worry about non-existence(probably)
                if (direction == 1)
                {
                    //Checking if the node is attempting to go backwards
                    if(nodeList[temp.NodeID - length].Equals(prevNode)) { continue; }

                    temp.NextNode = nodeList[temp.NodeID - length];
                    //Checking if there is a path through there already
                    if(unionFind[temp.NodeID-length] == -1)
                    {
                        //Adding the edge to the edgeList
                        Debug.Log("Adding edge");
                        edgeList.Add(new Edge(temp.NodeID, temp.NextNode.NodeID));

                        unionFind[temp.NodeID] = -1;
                        continue;
                    }
                }
                else if(direction == 2)
                {
                    if (nodeList[temp.NodeID + length].Equals(prevNode)) { continue; }

                    temp.NextNode = nodeList[temp.NodeID + length];
                    //Checking if there is a path through there already
                    if (unionFind[temp.NodeID + length] == -1)
                    {
                        //Adding the edge to the edgeList
                        Debug.Log("Adding edge");
                        edgeList.Add(new Edge(temp.NodeID, temp.NextNode.NodeID));

                        unionFind[temp.NodeID] = -1;
                        continue;
                    }
                }
                else if(direction == 3)
                {
                    if (nodeList[temp.NodeID - 1].Equals(prevNode)) { continue; }

                    temp.NextNode = nodeList[temp.NodeID -1];
                    //Checking if there is a path through there already
                    if (unionFind[temp.NodeID -1] == -1)
                    {
                        //Adding the edge to the edgeList
                        Debug.Log("Adding edge");
                        edgeList.Add(new Edge(temp.NodeID, temp.NextNode.NodeID));

                        unionFind[temp.NodeID] = -1;
                        continue;
                    }

                }
                else if(direction == 4)
                {
                    if (nodeList[temp.NodeID + 1].Equals(prevNode)) { continue; }

                    temp.NextNode = nodeList[temp.NodeID + 1];
                    //Checking if there is a path through there already
                    if (unionFind[temp.NodeID + 1] == -1)
                    {
                        //Adding the edge to the edgeList
                        Debug.Log("Adding edge");
                        edgeList.Add(new Edge(temp.NodeID, temp.NextNode.NodeID));

                        unionFind[temp.NodeID] = -1;
                        continue;
                    }
                }

                //At this point, a nextNode should have been found on the path. Set that to be the new temp,
                //and run again
                //DEBUG
                Debug.Log("Temp next: " + temp.NextNode.NodeID);
                //Set prevNode
                prevNode = temp;
                temp = temp.NextNode;
                //DEBUG
                //return;
            }

            //Now, there should be a path that has been found, so we can start from firstNode and adjust the union
            //find table for all elements on the path
            temp = firstNode;
            while(unionFind[temp.NodeID] != -1)
            {
                //Adding the edge to the edgeList
                Debug.Log("Adding edge");
                edgeList.Add(new Edge(temp.NodeID, temp.NextNode.NodeID));

                unionFind[temp.NodeID] = -1;
                temp = temp.NextNode;
            }
            //I think that's it...
        }
        //Returning the endVert
        return endVert;
    }

    //function to print the map
    void PrintMap(int endVert)
    {
        /*
        //Just gonna print the paths from each node to the endVert
        foreach(Node element in nodeList)
        {
            Debug.Log("Path for Node " + element.NodeID + " is: ");
            Node temp = element.NextNode;
            while(temp != null)
            {
                Debug.Log(temp.NodeID + " ");
                temp = temp.NextNode;
            }
        }*/

        //Creating a visual representation of the graph
        for(int k = 0; k < width; k ++)
        {
            for(int i = 0; i < length; i ++)
            {
                //Debug.Log(i + (k * length));
                Vector2 nodePos = new Vector2((float)(i * .5), (float)(k * .5));
                if((i+(k*length)) == endVert)
                {
                    GameObject endVertex = (GameObject)Instantiate(mapNode, nodePos, Quaternion.identity);
                    endVertex.GetComponent<SpriteRenderer>().color = new Color(.5f, .8f, .2f);
                }
                else
                {
                    Instantiate(mapNode, nodePos, Quaternion.identity);
                }
            }
        }
        
        int edgeX;
        int edgeY;
        //Printing out the edges of the tree
        for(int i = 0; i < edgeList.Count; i++)
        {
            Debug.Log("Edge goes from: " + edgeList[i].P1 + " to: " + edgeList[i].P2);
            if(edgeList[i].P1 > edgeList[i].P2)
            {
                if(edgeList[i].P1 == edgeList[i].P2 + length)
                {
                    edgeX = edgeList[i].P2 % length;
                    edgeY = edgeList[i].P2 / length;

                    Instantiate(mapEdge, new Vector3((float)(edgeX * .5), (float)(edgeY * .5 + .25)), Quaternion.identity);
                }
                else if(edgeList[i].P1 == edgeList[i].P2 + 1)
                {
                    edgeX = edgeList[i].P2 % length;
                    edgeY = edgeList[i].P2 / length;

                    Instantiate(mapEdge, new Vector3((float)(edgeX * .5 + .25), (float)(edgeY * .5)), Quaternion.identity);
                }
            }
            else if(edgeList[i].P1 < edgeList[i].P2)
            {
                if(edgeList[i].P1 == edgeList[i].P2 - length)
                {
                    edgeX = edgeList[i].P1 % length;
                    edgeY = edgeList[i].P1 / length;

                    Instantiate(mapEdge, new Vector3((float)(edgeX * .5), (float)(edgeY * .5 + .25)), Quaternion.identity);
                }
                else if(edgeList[i].P1 == edgeList[i].P2 -1)
                {
                    edgeX = edgeList[i].P1 % length;
                    edgeY = edgeList[i].P1 / length;

                    Instantiate(mapEdge, new Vector3((float)(edgeX * .5 + .25), (float)(edgeY * .5)), Quaternion.identity);
                }
            }
        }
    }
}

//Helper class to represent individual nodes. Contains their ID and a reference to the
//next node on the path to the endVert
class Node
{
    //Variables
    //ID of the node
    int ID;
    public int NodeID
    {
        get { return ID; }
    }
    //Reference to the next node on the path to the endVert
    Node nextNode;
    public Node NextNode
    {
        get { return nextNode; }
        set { nextNode = value; }
    }

    /****   CONSTRUCTORS    *****/
    public Node(int id, Node next)
    {
        ID = id;
        nextNode = next;
    }
    public Node(int id)
    {
        ID = id;
    }
}

//Small helper class for edge stuff. Just contains the point IDs it connects
class Edge
{
    int point1;
    public int P1
    {
        get { return point1; }
        set { point1 = value; }
    }
    int point2;
    public int P2
    {
        get { return point2; }
        set { point2 = value; }
    }

    /****   CONSTRUCTORS    ****/
    public Edge(int p1, int p2)
    {
        point1 = p1;
        point2 = p2;
    }
}