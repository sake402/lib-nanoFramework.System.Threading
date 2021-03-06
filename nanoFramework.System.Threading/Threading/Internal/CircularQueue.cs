﻿//
// Copyright (c) .NET Foundation and Contributors
// Portions Copyright (c) Microsoft Corporation.  All rights reserved.
// See LICENSE file in the project root for full license information.
//

using System;
using System.Diagnostics;

namespace System.Threading.Internal
{
    internal struct CircularQueue<T>
    {
        int put;
        int get;
        int size;
        int depth;
        T[] data;

        public CircularQueue(int depth)
        {
            put = 0;
            get = 0;
            this.depth = depth;
            data = new T[depth];
            size = 0;
        }

        public bool Dequeue(ref T data)
        {
            if (size == 0)
                return false;
            data = this.data[get];
            get++;
            size--;
            if (get >= depth)
                get = 0;
            return true;
        }

        public bool Peek(ref T data)
        {
            if (size == 0)
                return false;
            data = this.data[get];
            return true;
        }

        public bool Enqueue(T data)
        {
            if (size >= depth)
                return false;
            this.data[put] = data;
            put++;
            size++;
            if (put >= depth)
                put = 0;
            return true;
        }

        public bool Peek(ref T data, int index)
        {
            if (size <= index)
                return false;
            if (index >= depth)
                index -= depth;
            data = this.data[index];
            return true;
        }

        public T this[int index]
        {
            get
            {
                T t = default;
                if (Peek(ref t, index))
                    return t;
                throw new Exception("Invalid Index");
            }
        }

        public bool Contains(T data)
        {
            if (size == 0)
                return false;
            T dat = default;
            for (int i = 0; i < size; i++)
            {
                if (Peek(ref dat, i))
                {
                    if (dat.Equals(data))
                        return true;
                }
            }
            return false;
        }

        public int Count => size;
        public int Space => depth - size;
        public bool IsFull => Space == 0;

        public void Clear()
        {
            put = 0;
            get = 0;
            size = 0;
        }
    }
}
