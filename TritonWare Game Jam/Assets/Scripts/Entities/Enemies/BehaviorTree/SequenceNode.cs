using System.Collections.Generic;

public class SequencerNode : Node
{
    private Node[] childNodes;

    public SequencerNode(Node[] childNodes_)
    {
        childNodes = childNodes_;
    }

    public override NodeStates Evaluate()
    {
        foreach (Node node in childNodes)
        {
            switch (node.Evaluate())
            {
                case NodeStates.Failure:
                    nodeState = NodeStates.Failure;
                    return nodeState;

                case NodeStates.Running:
                    nodeState = NodeStates.Running;
                    return nodeState;

                case NodeStates.Success:
                    continue;
            }
        }
        nodeState = NodeStates.Success;
        return nodeState;
    }
}