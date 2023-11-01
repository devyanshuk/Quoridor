using System;
using System.Linq;
using System.Collections.Generic;
using Quoridor.AI.Interfaces;

namespace Quoridor.AI.AStarAlgorithm
{
    public class AStar<TVector2D, TMaze, TPlayer> : AIAgent<TVector2D, TMaze, TPlayer>
        where TPlayer : class, IAStarPlayer<TVector2D>, IEquatable<TPlayer>
        where TVector2D : class, IVector2D
        where TMaze : class, INeighbors<TVector2D>
    {
        public override string Name => nameof(AStarAlgorithm);

        public override TVector2D BestMove(TMaze maze, TPlayer player)
        {
            ValidateParams(maze, player);

            var start = new Node<TVector2D> { CurrPos = player.CurrentPos };
            var openSet = new HashSet<Node<TVector2D>>() { start  };
            var closedSet = new HashSet<Node<TVector2D>>();

            //set the initial node as the current node
            var currNode = start;

            while (openSet.Count() > 0)
            {
                //get the node from the opoen set with the lowest f-score value
                var nodeWithLowestFscore = openSet.Aggregate((min, x) => x.FValue < min.FValue ? x : min);
                currNode = nodeWithLowestFscore;

                //put this node in closed set and remove it from open set
                closedSet.Add(nodeWithLowestFscore);
                openSet.Remove(nodeWithLowestFscore);

                //if the closed set contains a goal node, we're done
                if (closedSet.Any(node => player.IsGoalCell(node.CurrPos)))
                    return ConstructPath(currNode);

                foreach (var neighbor in maze.Neighbors(currNode.CurrPos))
                {
                    var neighborNode = new Node<TVector2D> { CurrPos = neighbor };
                    //if it's in the closed list, skip it
                    if (closedSet.Contains(neighborNode))
                        continue;

                    var gScore = currNode.GValue + 1;

                    //if the neighbor is not in the open set or if the neighbor's G score is
                    //lower than the calculated g-score, update the g score and set the neighbor
                    //as current node's parent.
                    if (!openSet.Contains(neighborNode) || neighborNode.GValue < gScore)
                    {
                        neighborNode.GValue = gScore;
                        neighborNode.HValue = player.ManhattanHeuristicFn(neighbor);
                        neighborNode.FValue = gScore + neighborNode.HValue;
                        neighborNode.Parent = currNode;
                        //if neighbor was already present, it won't re-add.
                        openSet.Add(neighborNode);
                    }
                }
            }
            return null;
        }

        private static TVector2D ConstructPath(Node<TVector2D> currNode)
        {
            //we only need the next position so we skip the rest
            while (currNode.Parent.Parent != null)
                currNode = currNode.Parent;

            return currNode.CurrPos;
        }

        private static void ValidateParams(
            TMaze maze, TPlayer player)
        {
            GuardAgainstNullValue(maze, nameof(maze));
            GuardAgainstNullValue(player, nameof(player));
        }

        private static void GuardAgainstNullValue<T>(T param, string paramName)
        {
            if (param == null)
                throw new ArgumentNullException($"{paramName} : {typeof(T).Name}");
        }

        
    }
}
