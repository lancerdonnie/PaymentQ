using System;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace Payment.Api.Attributes
{
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false)]
    public class EnsureOneItemAttribute : ValidationAttribute
    {
        private readonly int Min;
        private readonly int Max;
        public EnsureOneItemAttribute(int min = 0, int max = int.MaxValue)
        {
            Min = min;
            Max = max;
        }

        public override bool IsValid(object value)
        {
            if (!(value is IList list))
                return false;

            return list.Count >= Min && list.Count <= Max;
        }
    }
}