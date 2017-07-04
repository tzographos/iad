using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using AnomalyDetection.Core.Models;

namespace AnomalyDetection.Core
{
    public class PersonCollection :  ICollection<Person>
    {
        private readonly ICollection<Person> _items = new Collection<Person>();
        private readonly IDictionary<int, Person> _personById = new Dictionary<int, Person>();

        public Person this[int personId] => this.Contains(personId) ? _personById[personId] : null;

        public void Add(Person person)
        {
            //guard
            if (person == null) throw new ArgumentNullException(nameof(person));

            if (!this.Contains(person))
            {
                _personById.Add(person.Id, person);
                _items.Add(person);
            }
        }

        public bool Contains(Person person) => this.Contains(person.Id);
        public bool Contains(int personId) => _personById.ContainsKey(personId);
        public void CopyTo(Person[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);

        public bool Remove(Person item) => throw new NotImplementedException();
        public void Clear() => throw new NotImplementedException();


        public int Count => _items.Count;
        public bool IsReadOnly => _items.IsReadOnly;
        public IEnumerator<Person> GetEnumerator() => _items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _items.GetEnumerator();
    }
}
