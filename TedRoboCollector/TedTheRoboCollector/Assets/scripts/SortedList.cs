﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A sorted list
/// </summary>
public class SortedList<T> where T:IComparable
{
    List<T> items = new List<T>();

    // used in Add method
    List<T> tempList = new List<T>();
	
    #region Constructors

    /// <summary>
    /// No argument constructor
    /// </summary>
    public SortedList()
    {
    }

    #endregion

    #region Properties

    /// <summary>
    /// Gets the number of items in the list
    /// </summary>
    /// <value>number of items in the list</value>
    public int Count
    {
        get { return items.Count; }
    }
	
    /// <summary>
    /// Gets the item in the array at the given index
    /// This property allows access using [ and ]
    /// </summary>
    /// <param name="index">index of item</param>
    /// <returns>item at the given index</returns>
    public T this[int index]
    {
        get { return items[index]; }
    }

    #endregion

    #region Methods

    /// <summary>
    /// Adds the given item to the list
    /// </summary>
    /// <param name="item">item</param>
    public void Add(T item)
    {
        // add your implementation below

        
            while ( items.Count != 0 && item.CompareTo(items[items.Count - 1]) < 0 )
            {
                tempList.Add(items[items.Count - 1]);
                items.Remove(items[items.Count - 1]);
            }
        
       

        items.Add(item);
        
        
            //items.AddRange(tempList);

            while (tempList.Count != 0)
            {
                items.Add(tempList[tempList.Count - 1]);
                tempList.Remove(tempList[tempList.Count - 1]);
            }

        

    }

    /// <summary>
    /// Removes the item at the given index from the list
    /// </summary>
    /// <param name="index">index</param>
    public void RemoveAt(int index)
    {
       
            
            if (index != items.Count - 1)
            {
                while (items.Count - 1 > index)
                {
                    tempList.Add(items[items.Count - 1]);
                    items.Remove(items[items.Count - 1]);
                }  
            }
            
            items.Remove(items[items.Count - 1]);

            if (index != items.Count - 1)
            {
                while (tempList.Count != 0)
                {
                    items.Add(tempList[tempList.Count - 1]);
                    tempList.Remove(tempList[tempList.Count - 1]);
                } 
            }
        
       
  
       
    }

    /// <summary>
    /// Determines the index of the given item using binary search
    /// </summary>
    /// <param name="item">the item to find</param>
    /// <returns>the index of the item or -1 if it's not found</returns>
    public int IndexOf(T item)
    {
        int lowerBound = 0;
        int upperBound = items.Count - 1;
        int location = -1;

        // loop until found value or exhausted array
        while ((location == -1) &&
            (lowerBound <= upperBound))
        {
            // find the middle
            int middleLocation = lowerBound + (upperBound - lowerBound) / 2;
            T middleValue = items[middleLocation];

            // check for match
            if (middleValue.CompareTo(item) == 0)
            {
                location = middleLocation;
            }
            else
            {
                // split data set to search appropriate side
                if (middleValue.CompareTo(item) > 0)
                {
                    upperBound = middleLocation - 1;
                }
                else
                {
                    lowerBound = middleLocation + 1;
                }
            }
        }
        return location;
    }

    /// <summary>
    /// Sorts the list
    /// </summary>
    public void Sort()
    {
        items.Sort();
    }

    #endregion
}
