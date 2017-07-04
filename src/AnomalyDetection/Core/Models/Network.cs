using System;
using System.Collections.Generic;
using System.Text;

namespace AnomalyDetection.Core.Models
{
    public class Network
    {
        public PersonCollection Persons { get; set; }


        public Network()
        {
            this.Persons = new PersonCollection();
        }

    }
}

