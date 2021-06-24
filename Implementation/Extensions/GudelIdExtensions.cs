using GudelIdService.Domain.Models;
using System;
using System.Collections.Generic;

namespace GudelIdService.Implementation.Extensions
{
    public static class GudelIdExtensions
    {
        public static List<Variance> GetDifferences(this GudelId oldGudelId, GudelId newGudelId)
        {
            var variances = new List<Variance>();

            var oType = oldGudelId.GetType();

            foreach (var oProperty in oType.GetProperties())
            {
                var oldValue = oProperty.GetValue(oldGudelId, null);
                var newValue = oProperty.GetValue(newGudelId, null);
                if (oProperty.Name == nameof(GudelId.ExtraFields) || oProperty.Name == nameof(GudelId.State) || oProperty.Name == nameof(GudelId.Activities) || oProperty.Name == nameof(GudelId.Type))
                {
                    continue;
                }

                if (!Equals(oldValue, newValue))
                {
                    variances.Add(new Variance()
                    {
                        Prop = oProperty.Name,
                        OldVal = oldValue != null ? oldValue.ToString() : null,
                        NewVal = newValue != null ? newValue.ToString() : null
                    });
                }
            }

            return variances;
        }
    }



}
