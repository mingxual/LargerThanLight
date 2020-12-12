using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid2D : MonoBehaviour
{

    class Wall3DNode
    {
        public Wall3D m_Wall3D;
        public Wall3DNode m_LeftNode;
        public Wall3DNode m_RightNode;
        public Wall3DNode m_TopNode;
        public Wall3DNode m_BottomNode;
        public Vector2 m_Coordinate;

        public Wall3DNode()
        {
            m_Wall3D = null;
            m_LeftNode = null;
            m_RightNode = null;
            m_TopNode = null;
            m_BottomNode = null;
        }
    }

    Wall3DNode m_OriginNode;
    List<Wall3DNode> m_StoredNodeList = new List<Wall3DNode>(); // Keep track of stored nodes
    Vector2 m_CenterCoordinates = new Vector2(0, 300);

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreateGrid(Wall3D wall3D)
    {

        // Create origin node
        m_OriginNode = new Wall3DNode();
        m_OriginNode.m_Wall3D = wall3D;
        m_OriginNode.m_Coordinate = m_CenterCoordinates;
        m_StoredNodeList.Add(m_OriginNode);

        if(wall3D.m_LeftWall)
        {
            CreateGridHelper(wall3D.m_LeftWall, m_OriginNode.m_LeftNode);
        }
        if(wall3D.m_RightWall)
        {
            CreateGridHelper(wall3D.m_RightWall, m_OriginNode.m_RightNode);
        }
        if(wall3D.m_TopWall)
        {
            CreateGridHelper(wall3D.m_TopWall, m_OriginNode.m_TopNode);
        }
        if(wall3D.m_BottomWall)
        {
            CreateGridHelper(wall3D.m_BottomWall, m_OriginNode.m_BottomNode);
        }
    }

    void CreateGridHelper(Wall3D _wall3D, Wall3DNode currNode)
    {
        currNode = new Wall3DNode();
        currNode.m_Wall3D = _wall3D;
        m_StoredNodeList.Add(currNode);

        if (_wall3D.m_LeftWall)
        {
            Wall3DNode wall3DNode;
            bool leftNodeExists = NodeExists(_wall3D.m_LeftWall, out wall3DNode);
            if(!leftNodeExists)
            {
                CreateGridHelper(_wall3D.m_LeftWall, currNode.m_LeftNode);
            }
            else if(leftNodeExists && (currNode.m_LeftNode == null))
            {
                currNode.m_LeftNode = wall3DNode;
            }
        }
        if (_wall3D.m_RightWall)
        {
            Wall3DNode wall3DNode;
            bool rightNodeExists = NodeExists(_wall3D.m_RightWall);
            if (rightNodeExists)
            {
                CreateGridHelper(_wall3D.m_RightWall, currNode.m_RightNode, currNode);
            }
            else if (rightNodeExists && (currNode.m_RightNode == null))
            {
                currNode.m_RightNode = parentNode;
            }
        }
        if (_wall3D.m_TopWall)
        {
            Wall3DNode wall3DNode;
            bool topNodeExists = NodeExists(_wall3D.m_TopWall);
            if (topNodeExists)
            {
                CreateGridHelper(_wall3D.m_TopWall, currNode.m_TopNode, currNode);
            }
            else if (topNodeExists && (currNode.m_TopNode == null))
            {
                currNode.m_TopNode = parentNode;
            }
        }
        if (_wall3D.m_BottomWall)
        {
            Wall3DNode wall3DNode;
            bool bottomNodeExists = NodeExists(_wall3D.m_BottomWall);
            if (bottomNodeExists)
            {
                CreateGridHelper(_wall3D.m_BottomWall, currNode.m_BottomNode, currNode);
            }
            else if (bottomNodeExists && (currNode.m_BottomNode == null))
            {
                currNode.m_BottomNode = parentNode;
            }
        }
    }

    // Checks if node exists that contains this wall
    bool NodeExists(Wall3D _wall3D, out Wall3DNode node)
    {
        bool retval = false;
        node = null;
        for(int i = 0; i < m_StoredNodeList.Count; i++)
        {
            if(m_StoredNodeList[i].m_Wall3D == _wall3D)
            {
                retval = true;
                node = m_StoredNodeList[i];
            }
        }

        return retval;
    }
}
