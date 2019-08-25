using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class HashTable<Key, Value> where Key : IEquatable<Key>
{
    private int maxCount;
    private readonly float ratioToFill;
    private int currentCount = 0;

    [SerializeField]
    private List<ValueSlot> valueSlots = new List<ValueSlot>();

    [System.Serializable]
    private struct ValueSlot
    {
        public bool hasValue;
        public List<SerializableKeyValuePair<Key, Value>> values;
    }

    [Serializable]
    public struct SerializableKeyValuePair<TKey, TValue>
    {
        public SerializableKeyValuePair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        public TKey Key
        {
            get; set;
        }
        public TValue Value
        {
            get; set;
        }
    }

    public struct ResultantValue
    {
        public bool hasValue;
        private Value value;
        public Value Value
        {
            get
            {
                if (hasValue)
                {
                    return value;
                }
                else
                {
                    throw new NullReferenceException("Null reference Ecxeption trying to use a value from hash table!");
                }
            }
            set
            {
                hasValue = true;
                this.value = value;
            }
        }
    }

    public HashTable(float ratioToFill = 0.5f, int initialSize = 64)
    {
        Debug.Assert(initialSize > 0, "HashTables must have an initial size >= 1");
        maxCount = initialSize;
        Debug.Assert(ratioToFill > 0f && ratioToFill < 1f, "HashTables must have a ratio to keep full between 0f & 1f");
        this.ratioToFill = ratioToFill;

        valueSlots = GetNewListOfSlots(maxCount);
    }

    private List<ValueSlot> GetNewListOfSlots(int sizeOfList)
    {
        List<ValueSlot> valueSlots = new List<ValueSlot>();
        for (int i = 0; i < sizeOfList; i++)
        {
            ValueSlot tmp = new ValueSlot
            {
                hasValue = false,
                values = null,
            };

            valueSlots.Add(tmp);
        }

        return valueSlots;
    }

    public void AddValue(Key key, Value value)
    {
        currentCount++;
        if (currentCount < (maxCount * ratioToFill))
        {
            AddValue(valueSlots, key, value);
        }
        else
        {
            int newSize = maxCount * 2;
            List<ValueSlot> newList = GetNewListOfSlots(newSize);
            foreach (ValueSlot valueSlot in valueSlots)
            {
                if (valueSlot.hasValue == true)
                {
                    foreach (SerializableKeyValuePair<Key, Value> kvp in valueSlot.values)
                    {
                        AddValue(newList, kvp.Key, kvp.Value);
                    }
                }
            }
            maxCount = newSize;
            valueSlots = newList;
        }
    }

    private bool AddValue(List<ValueSlot> valueSlots, Key key, Value value)
    {
        bool rtnVal = true;

        int UnresolvedHash = key.GetHashCode();

        int ResolvedHash = UnresolvedHash.GetHashCode() % valueSlots.Count;

        if (valueSlots[ResolvedHash].hasValue != true)
        {
            ValueSlot tmp = new ValueSlot
            {
                hasValue = true,
                values = new List<SerializableKeyValuePair<Key, Value>>()
            };
            SerializableKeyValuePair<Key, Value> keyValuePair = default;
            keyValuePair.Key = key;
            keyValuePair.Value = value;

            tmp.values.Add(keyValuePair);

            valueSlots[ResolvedHash] = tmp;
        }
        else
        {
            SerializableKeyValuePair<Key, Value> keyValuePair = default;
            keyValuePair.Key = key;
            keyValuePair.Value = value;

            rtnVal = false;
            foreach (SerializableKeyValuePair<Key, Value> kvp in valueSlots[ResolvedHash].values.TakeWhile(x => rtnVal == false))
            {
                bool evaluation;
                if (typeof(K))
                {
                    if (kvp.Key == keyValuePair.Key)
                    {
                        rtnVal = true;
                    }
                }
            }
            if (rtnVal == true)
            {
                valueSlots[ResolvedHash].values.Add(keyValuePair);
            }
        }
        return rtnVal;
    }

    public ResultantValue TryGetValue(Key key)
    {
        ResultantValue tmp = default;
        (bool result, Value myVal) = TryGetValue(valueSlots, key);

        if (result)
        {
            tmp.Value = myVal;
        }
        else
        {
            tmp.hasValue = false;
        }

        return tmp;
    }

    public bool ContainsValue(Key key) => ContainsValue(valueSlots, key);
    public bool RemoveValue(Key key) => RemoveValue(valueSlots, key);

    private (bool, Value) TryGetValue(List<ValueSlot> valueSlots, Key key)
    {
        Value rtnVal = default;
        bool successful = false;
        int UnresolvedHash = key.GetHashCode();

        int ResolvedHash = UnresolvedHash.GetHashCode() % valueSlots.Count;

        if (valueSlots[ResolvedHash].hasValue == true)
        {
            foreach (SerializableKeyValuePair<Key, Value> kvp in valueSlots[ResolvedHash].values.TakeWhile(x => successful == false))
            {
                if (kvp.Key == key)
                {
                    rtnVal = kvp.Value;
                    successful = true;
                }
            }
        }

        return (successful, rtnVal);
    }

    private bool ContainsValue(List<ValueSlot> valueSlots, Key key)
    {
        bool rtnVal = false;
        int UnresolvedHash = key.GetHashCode();

        int ResolvedHash = UnresolvedHash.GetHashCode() % valueSlots.Count;

        if (valueSlots[ResolvedHash].hasValue == true)
        {
            foreach (SerializableKeyValuePair<Key, Value> kvp in valueSlots[ResolvedHash].values.TakeWhile(x => rtnVal == false))
            {
                if (kvp.Key == key)
                {
                    rtnVal = true;
                }
            }
        }

        return rtnVal;
    }

    private bool RemoveValue(List<ValueSlot> valueSlots, Key key)
    {
        bool rtnVal = false;
        int UnresolvedHash = key.GetHashCode();

        int ResolvedHash = UnresolvedHash.GetHashCode() % valueSlots.Count;

        if (valueSlots[ResolvedHash].hasValue == true)
        {
            SerializableKeyValuePair<Key, Value> kvpToRemove = default;
            foreach (SerializableKeyValuePair<Key, Value> kvp in valueSlots[ResolvedHash].values.TakeWhile(x => rtnVal == false))
            {
                if (kvp.Key == key)
                {
                    kvpToRemove = kvp;
                    rtnVal = true;
                }
            }

            if (rtnVal == true)
            {
                rtnVal = valueSlots[ResolvedHash].values.Remove(kvpToRemove);
                if (rtnVal == true)
                {
                    currentCount--;
                }
            }
        }
        return rtnVal;
    }
}