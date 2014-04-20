namespace DynamicXmlBuilder
{
    using System.Collections.Generic;
    using System.Dynamic;
    using System.Linq;
    using System.Text;

    public class XmlNodeBuilder : DynamicObject
    {
        private ChildXmlNodesBuilder children;
        private XmlNodeBuilder parent;
        private string name;
        private string innerText;
        private IDictionary<string, string> attributes;

        internal XmlNodeBuilder(string tagName, XmlNodeBuilder parent, string innerText = null, IDictionary<string, string> attributes = null)
        {
            this.children = new ChildXmlNodesBuilder();
            this.parent = parent;
            this.name = tagName;
            this.innerText = innerText;
            this.attributes = attributes;   
        }

        public ChildXmlNodesBuilder _b_
        {
            get
            {
                this.children = new ChildXmlNodesBuilder(this);
                return this.children;
            }
        }

        public XmlNodeBuilder _d_
        {
            get
            {
                return this.parent;
            }
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            var nextNode = new XmlNodeBuilder(binder.Name, this.parent);
            this.parent.AddNode(nextNode);
            result = nextNode;
            return true;
        }

        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            var innerText = (string)args[0];
            Dictionary<string, string> attr = null;
            if (args.Length > 1)
            {
                attr = (Dictionary<string, string>)args[1];
            }
            var nextNode = new XmlNodeBuilder(binder.Name, this.parent, innerText, attr);
            this.parent.AddNode(nextNode);
            result = nextNode;
            return true;
        }

        internal void AddNode(XmlNodeBuilder node)
        {
            this.children.AddNode(node);
        }

        public string Build(int level = 0)
        {
            var result = new StringBuilder();
            var whiteSpace = new string(' ', level * 4);
            result.Append(whiteSpace + "<" + this.name);
            if (this.attributes != null && this.attributes.Any())
            {
                var attrAsString = new StringBuilder(" ");
                foreach (var attr in this.attributes)
                {
                    attrAsString.Append(attr.Key + "=\"" + attr.Value + "\" ");
                }
                result.Append(attrAsString.ToString().TrimEnd());
            }
            result.Append(">" + this.innerText);
            if (this.children.Any())
            {
                result.AppendLine(whiteSpace).Append(this.children.Build(level + 1));
                result.Append(whiteSpace + "</" + this.name + ">" + whiteSpace);
            }
            else
            {
                result.Append("</" + this.name + ">");
            }
            
            return result.ToString();
        }
    }
}
