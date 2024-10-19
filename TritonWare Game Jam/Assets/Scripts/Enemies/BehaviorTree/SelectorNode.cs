public class SelectorNode : Node
{
    private Node[] childNodes;

    public SelectorNode(Node[] childNodes_)
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
                    continue;
                case NodeStates.Running:
                    nodeState = NodeStates.Running;
                    return nodeState;
                case NodeStates.Success:
                    nodeState = NodeStates.Success;
                    return nodeState;
            }
        }
        nodeState = NodeStates.Failure;
        return nodeState;
    }
}