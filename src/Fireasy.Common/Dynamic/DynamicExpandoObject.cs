﻿// -----------------------------------------------------------------------
// <copyright company="Fireasy"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
#if !NET35
using System;
using System.Collections.Generic;
using System.Dynamic;
using Fireasy.Common.Extensions;

namespace Fireasy.Common.Dynamic
{
    /// <summary>
    /// 实现 <see cref="ExpandoObject"/> 类似功能的动态类型。
    /// </summary>
    public class DynamicExpandoObject : DynamicObject, IDictionary<string, object>
    {
        private Dictionary<string, object> values = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public static implicit operator DynamicExpandoObject (KeyValuePair<string, object>[] sourceDic)
        {
            var result = new DynamicExpandoObject();
            var dictionary = (IDictionary<string, object>)result;

            foreach (var kvp in sourceDic)
            {
                dictionary.Add(kvp.Key, kvp.Value);
            }

            return result;
        }

        /// <summary>
        /// 返回成员名称的枚举。
        /// </summary>
        /// <returns></returns>
        public override IEnumerable<string> GetDynamicMemberNames()
        {
            return values.Keys;
        }

        /// <summary>
        /// 尝试获取成员的值。
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            return values.TryGetValue(binder.Name, out result);
        }

        /// <summary>
        /// 尝试设置成员的值。
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            return values.TryAdd(binder.Name, value);
        }

        /// <summary>
        /// 尝试调用成员的操作。
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="args"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
        {
            if (values.ContainsKey(binder.Name))
            {
                result = values[binder.Name];
                var dydel = result as DynamicDelegate;
                if (dydel != null)
                {
                    result = dydel.Invoke(this, args);
                    return true;
                }
            }

            return base.TryInvokeMember(binder, args, out result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="indexes"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public override bool TrySetIndex(SetIndexBinder binder, object[] indexes, object value)
        {
            var key = "__" + string.Join("-", indexes);
            return values.TryAdd(key, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="binder"></param>
        /// <param name="indexes"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public override bool TryGetIndex(GetIndexBinder binder, object[] indexes, out object result)
        {
            var key = "__" + string.Join("-", indexes);
            return values.TryGetValue(key, out result);
        }

        #region IDictionary<string, object>成员
        void IDictionary<string, object>.Add(string key, object value)
        {
            values.Add(key, value);
        }

        bool IDictionary<string, object>.ContainsKey(string key)
        {
            return values.ContainsKey(key);
        }

        ICollection<string> IDictionary<string, object>.Keys
        {
            get { return values.Keys; }
        }

        bool IDictionary<string, object>.Remove(string key)
        {
            return values.Remove(key);
        }

        bool IDictionary<string, object>.TryGetValue(string key, out object value)
        {
            return values.TryGetValue(key, out value);
        }

        ICollection<object> IDictionary<string, object>.Values
        {
            get { return values.Values; }
        }

        object IDictionary<string, object>.this[string key]
        {
            get
            {
                return values[key];
            }
            set
            {
                values[key] = value;
            }
        }

        void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item)
        {
            throw new NotImplementedException();
        }

        void ICollection<KeyValuePair<string, object>>.Clear()
        {
            values.Clear();
        }

        bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item)
        {
            throw new NotImplementedException();
        }

        void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        int ICollection<KeyValuePair<string, object>>.Count
        {
            get { return values.Count; }
        }

        bool ICollection<KeyValuePair<string, object>>.IsReadOnly
        {
            get { return false; }
        }

        bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
        {
            throw new NotImplementedException();
        }

        IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
        {
            return values.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return values.GetEnumerator();
        }
        #endregion
    }
}
#endif

