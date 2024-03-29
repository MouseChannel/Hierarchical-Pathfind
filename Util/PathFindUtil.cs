using System.Data;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Unity.Jobs;
using Unity.Mathematics;
using System;
using System.Linq;
using UnityEngine.Profiling;

public static class PathFindUtil
{
    public static List<Edge> CurrentPortalNodeList = new List<Edge>();
    public static bool MyContain(this List<Edge> edges, PortalNode node, out int index)
    {
        index = -1;
        for (int i = 0; i < edges.Count; i++)
        {
            if (edges[i].endNode == node)
            {
                index = i;
                return true;
            }
        }
        return false;
    }

    public static List<Edge> FindPath(int2 fromPos, int2 endPos)
    {
        Profiler.BeginSample("GetClusterAndGrid");
        GridSystem.GetClusterAndGrid(fromPos, out Cluster fromCluster, out Grid fromGrid);
        Profiler.EndSample();
        Profiler.BeginSample("(Connection");
        var fromPortalNode = fromCluster.ConnectGridToBorderNode(fromGrid);
        Profiler.EndSample();

        GridSystem.GetClusterAndGrid(endPos, out Cluster endCluster, out Grid endGrid);
        var endPortalNode = endCluster.ConnectGridToBorderNode(endGrid);
        
        Profiler.BeginSample("FindPathPortalNode");
        var res = FindPath(fromPortalNode, endPortalNode, true);
        Profiler.EndSample();
     
        Debug.Log("length = " + res.Count);
        return res;

    }
    /// <summary>
    /// Final Path Find
    /// </summary>
    /// <returns></returns>
    public static List<Edge> FindPath(PortalNode from, PortalNode end, bool firstFind = false)
    {
        if (firstFind)
            CurrentPortalNodeList.Clear();
        HashSet<PortalNode> visited = new HashSet<PortalNode>();
        //edge.fromNOde = camefromNode
        Dictionary<PortalNode, Edge> cameFromNode = new Dictionary<PortalNode, Edge>();
        Dictionary<PortalNode, int> GCost = new Dictionary<PortalNode, int>();



        ClassHeap<PortalNode> heap = new ClassHeap<PortalNode>();

        GCost[from] = 1;
        heap.Push(from, CalculateDistanceCost(from.pos, end.pos));


        var start = from;
        visited.Add(start);





        while (!heap.IsEmpty)
        {
            var front = heap.Pop();
            if (!firstFind)
            {
                if (CurrentPortalNodeList.MyContain(front, out int index))
                {
                    var tempList = BuildEdgePath(firstFind);
                    tempList.AddRange(CurrentPortalNodeList.GetRange(index, CurrentPortalNodeList.Count - index + 1));
                    return tempList;
                }

            }
            if (front.Equals(end))
            {
                return BuildEdgePath(firstFind);

            };
            visited.Add(front);

            foreach (var edge in front.edges)
            {
                var currentEndNode = edge.endNode;

                if (visited.Contains(currentEndNode))
                {
                    continue;
                }
                int tempGCost = GCost[front] + edge.weight;
                if (GCost.TryGetValue(currentEndNode, out int preGcost) && tempGCost >= preGcost)
                    continue;
                cameFromNode[currentEndNode] = edge;
                GCost[currentEndNode] = tempGCost;
                heap.Push(currentEndNode, tempGCost + CalculateDistanceCost(currentEndNode.pos, end.pos));


            }






        }


        return new List<Edge>();


        List<Edge> BuildEdgePath(bool firstFind)
        {
            List<Edge> res = new List<Edge>();
            PortalNode current = end;
            Edge e = null;

            while (cameFromNode.TryGetValue(current, out e))
            {
                res.Insert(0, e);
                current = e.fromNode;
            }
            if (firstFind)
                CurrentPortalNodeList = res;
            return res;
        }


    }

    private static int CalculateDistanceCost(int2 aPosition, int2 bPosition)
    {
        int xDistance = math.abs(aPosition.x - bPosition.x);
        int yDistance = math.abs(aPosition.y - bPosition.y);
        int remaining = math.abs(xDistance - yDistance);
        // return xDistance + yDistance;

        return 14 * math.min(xDistance, yDistance) + 10 * remaining;

    }


}
