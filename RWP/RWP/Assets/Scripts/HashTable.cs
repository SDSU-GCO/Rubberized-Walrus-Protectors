using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HashTable<Key, Value>
{
    private int maxCount;
    private readonly float ratioToFill;
    private int currentCount = 0;
    private List<ValueSlot> valueSlots = new List<ValueSlot>();

    private struct ValueSlot
    {
        public bool hasValue;
        public List<KeyValuePair<Key, Value>> values;
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
                    foreach (KeyValuePair<Key, Value> kvp in valueSlot.values)
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
                values = new List<KeyValuePair<Key, Value>>()
            };
            KeyValuePair<Key, Value> keyValuePair = new KeyValuePair<Key, Value>(key, value);

            tmp.values.Add(keyValuePair);

            valueSlots[ResolvedHash] = tmp;
        }
        else
        {
            KeyValuePair<Key, Value> keyValuePair = new KeyValuePair<Key, Value>(key, value);

            rtnVal = false;
            foreach (KeyValuePair<Key, Value> kvp in valueSlots[ResolvedHash].values.TakeWhile(x => rtnVal == false))
            {
                if (kvp.Key.Equals(keyValuePair.Key))
                {
                    rtnVal = true;
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
            foreach (KeyValuePair<Key, Value> kvp in valueSlots[ResolvedHash].values.TakeWhile(x => successful == false))
            {
                if (kvp.Key.Equals(key))
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
            foreach (KeyValuePair<Key, Value> kvp in valueSlots[ResolvedHash].values.TakeWhile(x => rtnVal == false))
            {
                if (kvp.Key.Equals(key))
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
            KeyValuePair<Key, Value> kvpToRemove;
            foreach (KeyValuePair<Key, Value> kvp in valueSlots[ResolvedHash].values.TakeWhile(x => rtnVal == false))
            {
                if (kvp.Key.Equals(key))
                {
                    rtnVal = true;
                    kvpToRemove = kvp;
                }
            }

            if (rtnVal == true)
            {
                rtnVal = valueSlots[ResolvedHash].values.Remove(kvpToRemove);
            }
        }
        return rtnVal;
    }
}