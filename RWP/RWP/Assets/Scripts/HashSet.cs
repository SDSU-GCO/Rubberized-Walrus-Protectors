using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HashSet<Value>
{
    private int maxCount;
    private readonly float ratioToFill;
    private int currentCount = 0;
    private List<ValueSlot> valueSlots = new List<ValueSlot>();

    private struct ValueSlot
    {
        public bool hasValue;
        public List<Value> values;
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

    public void AddValue(Value value)
    {
        currentCount++;
        if (currentCount < (maxCount * ratioToFill))
        {
            AddValue(valueSlots, value);
        }
        else
        {
            int newSize = maxCount * 2;
            List<ValueSlot> newList = GetNewListOfSlots(newSize);
            foreach (ValueSlot valueSlot in valueSlots)
            {
                if (valueSlot.hasValue == true)
                {
                    foreach (Value oldValue in valueSlot.values)
                    {
                        AddValue(newList, oldValue);
                    }
                }
            }
            maxCount = newSize;
            valueSlots = newList;
        }
    }

    private void AddValue(List<ValueSlot> valueSlots, Value value)
    {
        int UnresolvedHash = value.GetHashCode();

        int ResolvedHash = UnresolvedHash.GetHashCode() % valueSlots.Count;

        if (valueSlots[ResolvedHash].hasValue != true)
        {
            ValueSlot tmp = new ValueSlot
            {
                hasValue = true,
                values = new List<Value>()
            };
            tmp.values.Add(value);

            valueSlots[ResolvedHash] = tmp;
        }
        else
        {
            valueSlots[ResolvedHash].values.Add(value);
        }
    }

    public ResultantValue TryGetValue(Value value)
    {
        ResultantValue tmp = default;
        (bool result, Value myVal) = TryGetValue(valueSlots, value);

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
    public bool ContainsValue(Value value) => ContainsValue(valueSlots, value);
    public bool RemoveValue(Value value) => RemoveValue(valueSlots, value);

    private (bool, Value) TryGetValue(List<ValueSlot> valueSlots, Value value)
    {
        Value rtnVal = default;
        bool successful = false;
        int UnresolvedHash = value.GetHashCode();

        int ResolvedHash = UnresolvedHash.GetHashCode() % valueSlots.Count;

        if (valueSlots[ResolvedHash].hasValue == true)
        {
            foreach (Value tmpVal in valueSlots[ResolvedHash].values)
            {
                if (tmpVal.Equals(value))
                {
                    rtnVal = tmpVal;
                    successful = true;
                }
            }
        }

        return (successful, rtnVal);
    }

    private bool ContainsValue(List<ValueSlot> valueSlots, Value value)
    {
        bool rtnVal = false;
        int UnresolvedHash = value.GetHashCode();

        int ResolvedHash = UnresolvedHash.GetHashCode() % valueSlots.Count;

        if (valueSlots[ResolvedHash].hasValue == true)
        {
            rtnVal = valueSlots[ResolvedHash].values.Contains(value);
        }

        return rtnVal;
    }

    private bool RemoveValue(List<ValueSlot> valueSlots, Value value)
    {
        bool rtnVal = false;
        int UnresolvedHash = value.GetHashCode();

        int ResolvedHash = UnresolvedHash.GetHashCode() % valueSlots.Count;

        if (valueSlots[ResolvedHash].hasValue == true)
        {
            rtnVal = valueSlots[ResolvedHash].values.Remove(value);
        }
        return rtnVal;
    }

    public HashSet(float ratioToFill = 0.5f, int initialSize = 64)
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
}