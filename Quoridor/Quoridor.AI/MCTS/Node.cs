using System;
using System.Collections.Generic;

namespace Quoridor.AI.MCTS
{
    public class Node<TMove, TPlayer, TGame>
        where TGame : IMCTSGame<TGame, TMove, TPlayer>
    {
        public int Count { get; set; }
        public int Wins { get; set; }
        public bool Expandable { get; private set; }

        public Node<TMove, TPlayer, TGame> Parent { get; set; }
        public List<Node<TMove, TPlayer, TGame>> Children { get; set; } = new List<Node<TMove, TPlayer, TGame>>();
        public bool IsLeaf => Children.Count == 0;

        public TMove Move { get; set; }
        public TGame State { get; set; }

        public IEnumerator<TMove> ValidMoves { get; private set; }

        public Node(TGame State)
        {
            this.State = State;
            ValidMoves = State.GetValidMoves().GetEnumerator();
            //we need to call movenext before we can start accessing moves
            //using the IEnumerator.Current.
            Expandable = ValidMoves.MoveNext();
        }

        public Node<TMove, TPlayer, TGame> Expand()
        {
            if (ValidMoves.Current.Equals(default(TMove)))
            {
                throw new Exception($"Node has already been fully expanded.");
            }

            //get a move from the list of available moves
            Move = ValidMoves.Current;
            Expandable = ValidMoves.MoveNext();

            //copy the game state, apply the action
            var copy = State.DeepCopy();
            copy.Move(Move);

            //make a new node of the resulting game state
            var newNode = new Node<TMove, TPlayer, TGame>(copy) { Parent = this };
            Children.Add(newNode);
            return newNode;
        }
    }
}
