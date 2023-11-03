using System;
using System.Linq;
using System.Collections.Generic;

using Quoridor.AI.Interfaces;

namespace Quoridor.AI.AStarAlgorithm
{
    public class AStar<TMove, TMaze, TPlayer> : AIStrategy<TMove, TMaze, TPlayer>
        where TPlayer : IAStarPlayer<TMove>
        where TMove : Movement
        where TMaze : INeighbors<TMove>
    {
        public override string Name => nameof(AStarAlgorithm);

        public override AIStrategyResult<TMove> BestMove(TMaze maze, TPlayer player)
        {
            ValidateParams(maze, player);

            var start = new Node<TMove> { CurrMove = player.GetCurrentMove() };
            var openSet = new HashSet<Node<TMove>>() { start  };
            var closedSet = new HashSet<Node<TMove>>();

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
                if (closedSet.Any(node => player.IsGoal(node.CurrMove)))
                    return ConstructPath(currNode);

                foreach (var neighbor in maze.Neighbors(currNode.CurrMove))
                {
                    var neighborNode = new Node<TMove> { CurrMove = neighbor };
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
                        neighborNode.HValue = player.CalculateHeuristic(neighbor);
                        neighborNode.FValue = gScore + neighborNode.HValue;
                        neighborNode.Parent = currNode;
                        //if neighbor was already present, it won't re-add.
                        openSet.Add(neighborNode);
                    }
                }
            }
            return null;
        }

        private static AIStrategyResult<TMove> ConstructPath(Node<TMove> currNode)
        {
            //we only need the next position so we skip the rest
            if (currNode.Parent == null)
                return new AIStrategyResult<TMove> { Value = 0, BestMove = currNode.CurrMove };

            var pathLen = 1;
            while (currNode.Parent.Parent != null)
            {
                pathLen++;
                currNode = currNode.Parent;
            }

            return new AIStrategyResult<TMove> { Value = pathLen, BestMove = currNode.CurrMove };
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
