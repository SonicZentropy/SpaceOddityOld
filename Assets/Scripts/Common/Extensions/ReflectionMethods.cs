// /** 
//  * ReflectionMethods.cs
//  * Dylan Bailey
//  * 20161209
// */

#pragma warning disable 0414, 0219, 649, 169, 618, 1570

namespace Zenobit.Common.Extensions
{
    #region Dependencies

    using System;
    using System.Collections;

    #endregion

    public static class ReflectionMethods
    {
        public static void Clone<T>(T objectToCloneTo, T objectToCloneFrom)
        {
            //get the array of fields for the new type instance.
            var fields = objectToCloneTo.GetType().GetFields();

            var i = 0;

            foreach (var fi in objectToCloneTo.GetType().GetFields())
            {
                //We query if the fiels support the ICloneable interface.
                var cloneType = fi.FieldType.
                    GetInterface("ICloneable", true);

                if (cloneType != null)
                {
                    //Getting the ICloneable interface from the object.
                    var clone = (ICloneable) fi.GetValue(objectToCloneFrom);

                    //We use the clone method to set the new value to the field.
                    fields[i].SetValue(objectToCloneTo, clone.Clone());
                }
                else
                {
                    // If the field doesn't support the ICloneable 
                    // interface then just set it.
                    fields[i].SetValue(objectToCloneTo, fi.GetValue(objectToCloneFrom));
                }

                //Now we check if the object support the 
                //IEnumerable interface, so if it does
                //we need to enumerate all its items and check if 
                //they support the ICloneable interface.
                var enumerableType = fi.FieldType.GetInterface
                    ("IEnumerable", true);
                if (enumerableType != null)
                {
                    //Get the IEnumerable interface from the field.
                    var IEnum = (IEnumerable) fi.GetValue(objectToCloneFrom);

                    //objectToCloneTo version support the IList and the 
                    //IDictionary interfaces to iterate on collections.
                    var listType = fields[i].FieldType.GetInterface
                        ("IList", true);
                    var dictType = fields[i].FieldType.GetInterface
                        ("IDictionary", true);

                    var j = 0;
                    if (listType != null)
                    {
                        //Getting the IList interface.
                        var list = (IList) fields[i].GetValue(objectToCloneTo);

                        foreach (var obj in IEnum)
                        {
                            //Checking to see if the current item 
                            //support the ICloneable interface.
                            cloneType = obj.GetType().
                                GetInterface("ICloneable", true);

                            if (cloneType != null)
                            {
                                //If it does support the ICloneable interface, 
                                //we use it to set the clone of
                                //the object in the list.
                                var clone = (ICloneable) obj;

                                list[j] = clone.Clone();
                            }

                            //NOTE: If the item in the list is not 
                            //support the ICloneable interface then in the 
                            //cloned list objectToCloneTo item will be the same 
                            //item as in the original list
                            //(as long as objectToCloneTo type is a reference type).

                            j++;
                        }
                    }
                    else if (dictType != null)
                    {
                        //Getting the dictionary interface.
                        var dic = (IDictionary) fields[i].
                            GetValue(objectToCloneTo);
                        j = 0;

                        foreach (DictionaryEntry de in IEnum)
                        {
                            //Checking to see if the item 
                            //support the ICloneable interface.
                            cloneType = de.Value.GetType().
                                GetInterface("ICloneable", true);

                            if (cloneType != null)
                            {
                                var clone = (ICloneable) de.Value;

                                dic[de.Key] = clone.Clone();
                            }
                            j++;
                        }
                    }
                }
                i++;
            }
        }
    }
}