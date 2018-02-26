namespace GSAWindowsParser
{
    public class Element
    {
        string name;
        string type;
        string category;
        string within;

        public Element(string name, string type, string category, string within)
        {
            Name = name;
            Type = type;
            Category = category;
            Within = within;
        }

        public string Name
        {
            get
            { return name; }
            private set
            { name = value; }
        }

        public string Type
        {
            get
            { return type; }
            private set
            { type = value; }
        }

        public string Category
        {
            get
            { return category; }
            private set
            { category = value; }
        }

        public string Within
        {
            get
            { return within; }
            private set
            { within = value; }
        }
    }
}