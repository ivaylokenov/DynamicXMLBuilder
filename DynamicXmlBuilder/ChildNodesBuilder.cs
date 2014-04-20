namespace DynamicXmlBuilder
{
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Text;

    public class ChildXmlNodesBuilder : DynamicObject
    {
        private IList<XmlNodeBuilder> children;
        private XmlNodeBuilder parent;

        internal ChildXmlNodesBuilder(XmlNodeBuilder parent = null)
        {
            this.parent = parent;
            this.children = new List<XmlNodeBuilder>();
        }

        public XmlNodeBuilder _d_
        {
            get
            {
                return parent;
            }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var nextNode = new XmlNodeBuilder(binder.Name, this.parent);
            this.AddNode(nextNode);
            result = nextNode;
            return true;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            var innerText = (string)args[0];
            Dictionary<string, string> attr = null;
            if (args.Length > 1)
            {
                attr = args[1].GetType().GetProperties().ToDictionary(pr => pr.Name, pr => pr.GetValue(args[1]).ToString());
            }
            var nextNode = new XmlNodeBuilder(binder.Name, this.parent, innerText, attr);
            this.AddNode(nextNode);
            result = nextNode;
            return true;
        }

        public string Build(int level)
        {
            var result = new StringBuilder();
            foreach (var child in this.children)
            {
                result.AppendLine(child.Build(level));
            }

            return result.ToString();
        }

        internal void AddNode(XmlNodeBuilder node)
        {
            this.children.Add(node);
        }

        internal bool Any()
        {
            return this.children.Any();
        }
    }
}
