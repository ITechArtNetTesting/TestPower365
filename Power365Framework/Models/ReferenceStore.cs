using System;
using System.Collections.Generic;

namespace BinaryTree.Power365.AutomationFramework
{

    public abstract class ReferenceStore
    {
        private Dictionary<string, object> _referenceLookup = new Dictionary<string, object>();

        public abstract void BuildReferences();

        internal void AddReference(Referential referential)
        {
            if (referential == null || referential.Reference == null)
                return;

            if (_referenceLookup.ContainsKey(referential.Reference))
                throw new Exception(string.Format("Reference '{0}' already exists.", referential.Reference));
            _referenceLookup.Add(referential.Reference, referential);
        }

        public T GetByReference<T>(string reference)
            where T : Referential
        {
            if (!_referenceLookup.ContainsKey(reference))
                throw new Exception(string.Format("Reference '{0}' not found.", reference));
            return (T)_referenceLookup[reference];
        }
    }
}