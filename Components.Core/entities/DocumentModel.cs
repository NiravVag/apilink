using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace Components.Core.entities
{
    public class DocumentModel
    {
        public string Title { get; set;  }

        public string Subject { get; set;  }

        public string Author { get; set;  }

    }

    public class Header : Container
    {

    }

    public class Footer : Container
    {

    }

    public class Body : Container
    {

    }

    public class Container
    {
        public string FontName { get; set;  }
        public Alignement Alignement { get; set; }
        public string TabStop { get; set; }
    }

    public enum Alignement
    {
       Left= 1, 
       Right = 2, 
       Center = 3
    }

    public enum VerticalAlignement
    {
        Top = 1,
        Buttom = 2,
        Middle = 3
    }

    public class JsonRequestModel
    {
        public string JsonPath { get; set; }

        public object Model { get; set; }

    }

    public class ConfigurationSourceModel
    {
        public DataSet DataSource { get; set; }

        public IEnumerable<Variable> VariableList { get; set; }

        public ConfigurationFile FileCOnfiguration { get; set; }

        public string Path { get; set;  }

    }

}
